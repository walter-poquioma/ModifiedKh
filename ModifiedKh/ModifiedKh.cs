using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;



using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Basics;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.Modeling;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.PropertyModeling;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;


namespace ModifiedKh
{
    /// <summary>
    /// This class contains all the methods and subclasses of the ModifiedKh.
    /// Worksteps are displayed in the workflow editor.
    /// </summary>
    class ModifiedKh : Workstep<ModifiedKh.Arguments>, IExecutorSource, IAppearance, IDescriptionSource
    {
        #region Overridden Workstep methods

        /// <summary>
        /// Creates an empty Argument instance
        /// </summary>
        /// <returns>New Argument instance.</returns>

        protected override ModifiedKh.Arguments CreateArgumentPackageCore(IDataSourceManager dataSourceManager)
        {
            return new Arguments(dataSourceManager);
        }
        /// <summary>
        /// Copies the Arguments instance.
        /// </summary>
        /// <param name="fromArgumentPackage">the source Arguments instance</param>
        /// <param name="toArgumentPackage">the target Arguments instance</param>
        protected override void CopyArgumentPackageCore(Arguments fromArgumentPackage, Arguments toArgumentPackage)
        {
            DescribedArgumentsHelper.Copy(fromArgumentPackage, toArgumentPackage);
        }

        /// <summary>
        /// Gets the unique identifier for this Workstep.
        /// </summary>
        protected override string UniqueIdCore
        {
            get
            {
                return "e5ce9eb3-0f59-4d54-b6e3-4b6b372e3599";
            }
        }
        #endregion

        #region IExecutorSource Members and Executor class

        /// <summary>
        /// Creates the Executor instance for this workstep. This class will do the work of the Workstep.
        /// </summary>
        /// <param name="argumentPackage">the argumentpackage to pass to the Executor</param>
        /// <param name="workflowRuntimeContext">the context to pass to the Executor</param>
        /// <returns>The Executor instance.</returns>
        public Slb.Ocean.Petrel.Workflow.Executor GetExecutor(object argumentPackage, WorkflowRuntimeContext workflowRuntimeContext)
        {
            return new Executor(argumentPackage as Arguments, workflowRuntimeContext);
        }

        public class Executor : Slb.Ocean.Petrel.Workflow.Executor
        {
            Arguments arguments;
            WorkflowRuntimeContext context;

            public Executor(Arguments arguments, WorkflowRuntimeContext context)
            {
                this.arguments = arguments;
                this.context = context;
            }

            public override void ExecuteSimple()
            {

                #region Creating and Setting the ratio Kh_wt/ SumKh as a Property

                double Kh_ave; double Kh_wt = 0;
                double x_ave;
                double y_ave;
                int TotalCellsCounter; int CellsPerZoneCounter;
                double MD1x; double MD1y; double MD1z; double MD1;
                double MD2x; double MD2y; double MD2z; double MD2;
                Borehole SelectedWell = null;
                double MiddlePointx; double MiddlePointy; double MiddlePointz;
                Dictionary<Index3, double> DictionaryOfRatios = new Dictionary<Index3,double>();

                int max_i = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.I;
                int max_j = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.J;
                int max_k = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.K;
                int ave_i; int ave_j; 
                List<Index3> ListOfCellsWithProperty = new List<Index3>();
                List<PerforatedCell> ListOfPerforatedCells = new List<PerforatedCell>();
                double KhRatio;
     
               // double[] arrayOfProperty = new double[max_i * max_j* max_k];
                List<double> ListOfProperty = Enumerable.Repeat(0.0/0.0, max_i * max_j * max_k).ToList();
                Template TemplateOfProperty = PetrelProject.WellKnownTemplates.PetrophysicalGroup.Permeability;
               
                


                Point3 MiddlePoint;
                Point3 IntersectingPoint;
                Index3 MiddleCell;
                

                //List<CellSide> Side;
                IPillarGridIntersectionService pgiservice = CoreSystem.GetService<IPillarGridIntersectionService>();
                PropertyCollection pc1 = this.arguments.OneLayerPerZoneGrid.PropertyCollection;
                PropertyCollection pc2 = this.arguments.PermeabilityFromModel.Grid.PropertyCollection;



                using (ITransaction trans = DataManager.NewTransaction())
                {
                    trans.Lock(pc1);
                    Property p = pc1.CreateProperty(TemplateOfProperty);
                    p.Name = "Kh_ratio";

                    foreach (Dictionary<int, List<CellData>> dict in this.arguments.ListOfCellDataDictionaries)  //Looping through all Dictionaries that contain the information associated to a particular well test.
                {                                                       
                    Kh_ave = 0.0;
                    TotalCellsCounter = 0;
                     

                    foreach (int ind in dict.Keys) //Looping through all Zones belonging to that particular well test
                    {
                        //x_ave = 0.0;
                        //y_ave = 0.0;
                        //MD1 = -1;
                        //MD2 = -2;
                        ave_i = 0;
                        ave_j = 0;
                        CellsPerZoneCounter = 0;


                        foreach (CellData cell in dict[ind])  //Looping through all the CellData objects in a particular zone.
                        { 
                            CellsPerZoneCounter = CellsPerZoneCounter + 1;
                            Kh_ave = cell.Perm * cell.Height + Kh_ave;
                            ave_i = ave_i + cell.CellIndex.I;
                            ave_j = ave_j + cell.CellIndex.J;
                            PerforatedCell PerforatedCellObj =  new PerforatedCell(); 
                            PerforatedCellObj.CellIndex.I = cell.CellIndex.I;
                            PerforatedCellObj.CellIndex.J = cell.CellIndex.J;
                            PerforatedCellObj.CellIndex.K = cell.CellIndex.K;

                            ListOfPerforatedCells.Add(PerforatedCellObj);
                           // cell_center = this.arguments.PermeabilityFromModel.Grid.GetCellCenter(cell.CellIndex);
                            
                            

                            //if(counter ==1)
                            //{
                            //    SelectedWell = cell.Well;
                            //    Kh_wt = cell.Kh_wt;
                            //    IntersectingPoint = KandaIntersectionService.GetIntersectingPoint(pgiservice, this.arguments.PermeabilityFromModel.Grid, 
                            //                                                                            cell.Well, cell.CellIndex, cell.PerforatedZonesOnly);
                            //    MD1 = cell.Well.Transform(Domain.ELEVATION_DEPTH, IntersectingPoint.Z, Domain.MD);


                            //}
                            //else if(counter == dict[ind].Count)
                            //{
                            //    IntersectingPoint = KandaIntersectionService.GetIntersectingPoint(pgiservice, this.arguments.PermeabilityFromModel.Grid,
                            //                                                                                cell.Well, cell.CellIndex, cell.PerforatedZonesOnly);
                            //      MD2 = cell.Well.Transform(Domain.ELEVATION_DEPTH, IntersectingPoint.Z, Domain.MD);
                            //}
                            Kh_wt = cell.Kh_wt;
                        }
                        ave_i = ave_i / CellsPerZoneCounter;
                        ave_j = ave_j / CellsPerZoneCounter;

                        ListOfCellsWithProperty.Add(new Index3(ave_i, ave_j, ind));

                        TotalCellsCounter = CellsPerZoneCounter + TotalCellsCounter;
                       
                        //if (MD1 >= 0 && MD2 >= 0 && SelectedWell !=null)
                        //{
                        //    MiddlePointx = SelectedWell.Transform(Domain.MD, (MD1 + MD2) / 2, Domain.X);
                        //    MiddlePointy = SelectedWell.Transform(Domain.MD, (MD1 + MD2) / 2, Domain.Y);
                        //    MiddlePointz = SelectedWell.Transform(Domain.MD, (MD1 + MD2) / 2, Domain.ELEVATION_DEPTH);
                        //    MiddlePoint = new Point3(MiddlePointx, MiddlePointy, MiddlePointz);
                            
                        //     MiddleCell = this.arguments.PermeabilityFromModel.Grid.GetCellAtPoint(MiddlePoint);
     
                        //}
                       
                    }
                   
                    Kh_ave = Kh_ave / TotalCellsCounter; //Average of all Kh of all cells corresponding to the same kh_wt
                    KhRatio = Kh_wt / Kh_ave;

                    foreach (Index3 index in ListOfCellsWithProperty) //Looping through all cells which are assigned a KhRatio property in the one layer per zone grid.
                    {
                        p[index] = (float)KhRatio; //Assigning the KhRatio property value

                    //#if Debug
                        if (Kh_wt > 0)
                        {
                            PetrelLogger.InfoOutputWindow("The ratio of the selected zones is " + System.Convert.ToString(KhRatio));
                            PetrelLogger.InfoOutputWindow("The corresponding index is: " + System.Convert.ToString(index.I) + ", " +
                                                          System.Convert.ToString(index.J) + ", " + System.Convert.ToString(index.K));
                            //  ListOfProperty[MiddleCell.I + MiddleCell.J * max_i + MiddleCell.K * max_i * max_j] = Kh_wt / Kh_ave;
                        }
                        else
                        {
                            MessageBox.Show("The Kh of Well Testing corresponding to Well " + SelectedWell.Name + " has not been set to a valid value.");
                            return;
                        }
                     //#endif
                    }
                    p.SetCellsUpscaled(ListOfCellsWithProperty);

                    //Saving all the cell indices corresponding to the wells belong to the same Kh_wt and their corresponding KhRatio value so that they can be used when
                    // the KhRatio property is set in the grid that has multiple layers per zone.
                    for (int i = ListOfPerforatedCells.Count - TotalCellsCounter; i < ListOfPerforatedCells.Count; i++) { ListOfPerforatedCells[i].KhRatio= KhRatio; }
                    


                }


               
                //Setting the property value. In this case the property is the ratio kh_wt/Kh_ave.
                //using (ITransaction trans = DataManager.NewTransaction())
                //{
                //    trans.Lock(pc2);
                //    Property p2 = pc2.CreateProperty(TemplateOfProperty);
                //    p.Name = "Kh_ratio";

                //    for (int i = 0; i < max_i; i++)
                //    {
                //        for (int j = 0; j < max_j; j++)
                //        {
                //            for (int k = 0; k < max_k; k++)
                //            {
                //                p2[i, j, k] = (float)ListOfProperty[i + j * max_i + k * max_i * max_j];
                //                Index3 DummyInd = new Index3(i,j,k);
                //                DummyInd =  ModelingUnitSystem.ConvertIndexToUI(this.arguments.PermeabilityFromModel.Grid,DummyInd);
                //                if (!double.IsNaN(p2[i, j, k]))
                //                {
                //                PetrelLogger.InfoOutputWindow("The grid cell " + System.Convert.ToString(DummyInd.I) + ", " +
                //                                                  System.Convert.ToString(DummyInd.J) + ", " + System.Convert.ToString(DummyInd.K) + ", " +
                //                                                   "has a Kh ratio value of " + System.Convert.ToString(p[i, j, k]));
                //                }
                //            }
                //        }
                //    }

                //    trans.Commit();
                //}


                #endregion

                #region Kriging 

                //Getting the maximum vertical range in order to ensure that it exists a high vertical correlation of the cells within the same zone
                ModelVariogramArguments VariogramArgs = new ModelVariogramArguments();
                // ModelVariogramArguments VariogramArgs = new ModelVariogramArguments(this.arguments.VariogramType, this.arguments.Nugget, 
                //      this.arguments.MajorRange, this.arguments.MinorRange,GridHeight * 10) ;
                KrigingArguments KrigingArgs = new KrigingArguments(VariogramArgs);


                this.arguments.ListOfZones = KandaPropertyCreator.GetAllLowLevelZones(this.arguments.OneLayerPerZoneGrid.Zones);
                //KrigingAlgorithm KriggingAlg = new KrigingAlgorithm();
                //    foreach(Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone z in this.arguments.ListOfZones)
                //    {
                //        KriggingAlg.Invoke(p, z, KrigingArgs);
              
                //    }
                    trans.Commit();
                }
               
                



                #endregion
            }
        }

        #endregion

        /// <summary>
        /// ArgumentPackage class for ModifiedKh.
        /// Each public property is an argument in the package.  The name, type and
        /// input/output role are taken from the property and modified by any
        /// attributes applied.
        /// </summary>
        public class Arguments : DescribedArgumentsByReflection
        {
            public Arguments()
                : this(DataManager.DataSourceManager)
            {                
            }

            public Arguments(IDataSourceManager dataSourceManager)
            {
            }

           // private Property permeabilityFromModel;
           // private List<Borehole> wellsSelected;
            private List<WellKh> listOfWellKh;
            public List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> ListOfZones = new List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone>();
            public List<Dictionary<Index3, List<double>>> listOfKhDictionaries;
            private List<double> Kh_ave;
            private List<double> Kh_wt;
            private List<Dictionary<int, List<CellData>>> listOfCellDataDictionaries = new List<Dictionary<int, List<CellData>>>();
            private Property permeabilityFromModel;
            private Grid oneLayerPerZoneGrid;
            public  ModelVariogramType VariogramType = ModelVariogramType.Spherical;
            public double Nugget = 2;
            public double MajorRange = 10;
           
            //private Dictionary<string, List<CellData>> dictionaryOfCellDataOfSelectedWells = new Dictionary<string, List<CellData>>();
            [Description("PermeabilityFromModel", "The permeability from the 3D model")]
            public Property PermeabilityFromModel
            {
                internal get { return this.permeabilityFromModel; }
                set { this.permeabilityFromModel = value; }
            }

            //[Description("wellsSelected", "A list of the selected wells")]
            //public List<Borehole> WellsSelected
            //{
            //    internal get { return this.wellsSelected; }
            //    set { this.wellsSelected = value; }
            //}

            [Description("listOfWellKh", "A list of the WellKh Objects")]
            public List<WellKh> ListOfWellKh
            {
                internal get { return this.listOfWellKh; }
                set { this.listOfWellKh = value; }
            }

            [Description("listOfCellDataDictionaries", "A list of the WellKh Objects")]
            public List<Dictionary<int, List<CellData>>> ListOfCellDataDictionaries
            { 
                get { return this.listOfCellDataDictionaries; }
                set { this.listOfCellDataDictionaries = value; }
            }

            [Description("oneLayerPerZoneGrid", "The grid dropped by the user that has one layer per zone only")]
            public Grid OneLayerPerZoneGrid
            {
                get { return this.oneLayerPerZoneGrid; }
                set { this.oneLayerPerZoneGrid = value; }
            }

            //[Description("dictionaryOfCellDataOfSelectedWells", "A dictionary with UWI for keys and list of cell data objects with information about the selected cells")]
            //Dictionary<string, List<CellData>> DictionaryOfCellDataOfSelectedWells
            //{
            //    get { return this.dictionaryOfCellDataOfSelectedWells; }
            //    set { this.dictionaryOfCellDataOfSelectedWells = value; }
            //}

        }
    
        #region IAppearance Members
        public event EventHandler<TextChangedEventArgs> TextChanged;
        protected void RaiseTextChanged()
        {
            if (this.TextChanged != null)
                this.TextChanged(this, new TextChangedEventArgs(this));
        }

        public string Text
        {
            get { return Description.Name; }
            private set 
            {
                // TODO: implement set
                this.RaiseTextChanged();
            }
        }

        public event EventHandler<ImageChangedEventArgs> ImageChanged;
        protected void RaiseImageChanged()
        {
            if (this.ImageChanged != null)
                this.ImageChanged(this, new ImageChangedEventArgs(this));
        }

        public System.Drawing.Bitmap Image
        {
            get { return PetrelImages.Modules; }
            private set 
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }
        #endregion

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the ModifiedKh
        /// </summary>
        public IDescription Description
        {
            get { return ModifiedKhDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the ModifiedKh.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class ModifiedKhDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private  static ModifiedKhDescription instance = new ModifiedKhDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static ModifiedKhDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of ModifiedKh
            /// </summary>
            public string Name
            {
                get { return "ModifiedKh"; }
            }
            /// <summary>
            /// Gets the short description of ModifiedKh
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of ModifiedKh
            /// </summary>
            public string Description
            {
                get { return ""; }
            }

            #endregion
        }
        #endregion

        public class UIFactory : WorkflowEditorUIFactory
        {
            /// <summary>
            /// This method creates the dialog UI for the given workstep, arguments
            /// and context.
            /// </summary>
            /// <param name="workstep">the workstep instance</param>
            /// <param name="argumentPackage">the arguments to pass to the UI</param>
            /// <param name="context">the underlying context in which the UI is being used</param>
            /// <returns>a Windows.Forms.Control to edit the argument package with</returns>
            protected override System.Windows.Forms.Control CreateDialogUICore(Workstep workstep, object argumentPackage, WorkflowContext context)
            {
                return new ModifiedKhUI((ModifiedKh)workstep, (Arguments)argumentPackage, context);
            }
        }
    }
    
    public class PerforatedCell
    {
      public PerforatedCell() 
        {
            CellIndex = new Index3();
            KhRatio = 0.0 / 0.0;
        }
       public Index3 CellIndex;
       public double KhRatio;

    
    }
}
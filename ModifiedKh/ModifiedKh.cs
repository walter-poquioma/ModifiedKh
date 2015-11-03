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
                double counter;
                double MD1x; double MD1y; double MD1z; double MD1;
                double MD2x; double MD2y; double MD2z; double MD2;
                Borehole SelectedWell = null;
                double MiddlePointx; double MiddlePointy; double MiddlePointz;

                int max_i = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.I;
                int max_j = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.J;
                int max_k = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.K;
               // double[] arrayOfProperty = new double[max_i * max_j* max_k];
                List<double> ListOfProperty = Enumerable.Repeat(0.0/0.0, max_i * max_j * max_k).ToList();
                Template TemplateOfProperty = PetrelProject.WellKnownTemplates.PetrophysicalGroup.Permeability;
                Point3 MiddlePoint;
                Point3 IntersectingPoint;
                Index3 MiddleCell;
                

                //List<CellSide> Side;
                IPillarGridIntersectionService pgiservice = CoreSystem.GetService<IPillarGridIntersectionService>();
                PropertyCollection pc = this.arguments.PermeabilityFromModel.Grid.PropertyCollection;

                
                
                

                foreach (Dictionary<int, List<CellData>> dict in this.arguments.ListOfCellDataDictionaries)
                {
                    Kh_ave = 0.0;
                    foreach (int ind in dict.Keys)
                    {
                        x_ave = 0.0;
                        y_ave = 0.0;
                        counter = 0;
                        MD1 = -1;
                        MD2 = -2;

                        foreach (CellData cell in dict[ind])
                        {
                            counter = counter + 1;
                            Kh_ave = cell.Perm * cell.Height + Kh_ave;
                           // cell_center = this.arguments.PermeabilityFromModel.Grid.GetCellCenter(cell.CellIndex);
                            
                            

                            if(counter ==1)
                            {
                                SelectedWell = cell.Well;
                                Kh_wt = cell.Kh_wt;
                                IntersectingPoint = KandaIntersectionService.GetIntersectingPoint(pgiservice, this.arguments.PermeabilityFromModel.Grid, 
                                                                                                        cell.Well, cell.CellIndex, cell.PerforatedZonesOnly);
                                // MD1x = cell.Well.Transform(Domain.X, IntersectingPoint.X, Domain.MD);
                                // MD1y = cell.Well.Transform(Domain.Y, IntersectingPoint.Y, Domain.MD);
                                MD1 = cell.Well.Transform(Domain.ELEVATION_DEPTH, IntersectingPoint.Z, Domain.MD);
                              

                                //if( Math.Abs(MD1x- MD1y)<0.01 && Math.Abs(MD1x- MD1z)<0.01)
                                //{
                                //    MD1 = MD1x;
                                //}

                            }
                            else if(counter == dict[ind].Count)
                            {
                                IntersectingPoint = KandaIntersectionService.GetIntersectingPoint(pgiservice, this.arguments.PermeabilityFromModel.Grid,
                                                                                                            cell.Well, cell.CellIndex, cell.PerforatedZonesOnly);
                                //MD2x = cell.Well.Transform(Domain.X, IntersectingPoint.X, Domain.MD);
                                //MD2y = cell.Well.Transform(Domain.Y, IntersectingPoint.Y, Domain.MD);
                                MD2 = cell.Well.Transform(Domain.ELEVATION_DEPTH, IntersectingPoint.Z, Domain.MD);

                                //if (Math.Abs(MD2x - MD2y) < 0.01 && Math.Abs(MD2x - MD2z) < 0.01)
                                //{
                                //    MD2 = MD2x;
                                //}
                            }
                            


                           // x_ave = x_ave + cell_center.X;
                           // y_ave = y_ave + cell_center.Y;
                        }

                        //x_ave = x_ave / counter;
                        //y_ave = y_ave / counter;
                        if (MD1 >= 0 && MD2 >= 0 && SelectedWell !=null)
                        {
                            MiddlePointx = SelectedWell.Transform(Domain.MD, (MD1 + MD2) / 2, Domain.X);
                            MiddlePointy = SelectedWell.Transform(Domain.MD, (MD1 + MD2) / 2, Domain.Y);
                            MiddlePointz = SelectedWell.Transform(Domain.MD, (MD1 + MD2) / 2, Domain.ELEVATION_DEPTH);
                            MiddlePoint = new Point3(MiddlePointx, MiddlePointy, MiddlePointz);
                            
                             MiddleCell = this.arguments.PermeabilityFromModel.Grid.GetCellAtPoint(MiddlePoint);


                              Kh_ave = Kh_ave / counter;

                            if(Kh_wt>0)
                            {
                                PetrelLogger.InfoOutputWindow("The ratio of the selected zones is " + System.Convert.ToString(Kh_wt / Kh_ave));
                                ListOfProperty[MiddleCell.I + MiddleCell.J * max_i + MiddleCell.K * max_i * max_j] = Kh_wt / Kh_ave;
                             }
                            else
                            {
                                MessageBox.Show("The Kh of Well Testing corresponding to Well " + SelectedWell.Name + " has not been set to a valid value.");
                                return;
                            }

                            
                        }

                      
                        

                    }
                }

                //Setting the property value. In this case the property is the ratio kh_wt/Kh_ave.
                using (ITransaction trans = DataManager.NewTransaction())
                {
                    trans.Lock(pc);
                    Property p = pc.CreateProperty(TemplateOfProperty);
                    p.Name = "Kh_ratio";

                    for (int i = 0; i < max_i; i++)
                    {
                        for (int j = 0; j < max_j; j++)
                        {
                            for (int k = 0; k < max_k; k++)
                            {
                                p[i, j, k] = (float)ListOfProperty[i + j * max_i + k * max_i * max_j];
                                Index3 DummyInd = new Index3(i,j,k);
                                DummyInd =  ModelingUnitSystem.ConvertIndexToUI(this.arguments.PermeabilityFromModel.Grid,DummyInd);
                                if (!double.IsNaN(p[i, j, k]))
                                {
                                PetrelLogger.InfoOutputWindow("The grid cell " + System.Convert.ToString(DummyInd.I) + ", " +
                                                                  System.Convert.ToString(DummyInd.J) + ", " + System.Convert.ToString(DummyInd.K) + ", " +
                                                                   "has a Kh ratio value of " + System.Convert.ToString(p[i, j, k]));
                                }
                            }
                        }
                    }

                    trans.Commit();
                }


                #endregion

                #region Kriging 

                //Getting the maximum vertical range in order to ensure that it exists a high vertical correlation of the cells within the same zone
                double GridHeight = this.arguments.PermeabilityFromModel.Grid.BoundingBox.Height;
               // ModelVariogramArguments VariogramArgs = new ModelVariogramArguments(this.arguments.VariogramType, this.arguments.Nugget, 
                                                                             //      this.arguments.MajorRange, this.arguments.MinorRange,GridHeight * 10) ;

                
               
                



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
            public List<Dictionary<Index3, List<double>>> listOfKhDictionaries;
            private List<double> Kh_ave;
            private List<double> Kh_wt;
            private List<Dictionary<int, List<CellData>>> listOfCellDataDictionaries = new List<Dictionary<int, List<CellData>>>();
            private Property permeabilityFromModel;
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
}
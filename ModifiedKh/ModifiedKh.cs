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
    /// 
    

    class PermMatching : Workstep<PermMatching.Arguments>, IExecutorSource, IAppearance, IDescriptionSource
    {
        internal static readonly Droid ArgsDroid = new Droid(CONNECTModifiedKhDataSourceFactory.DataSourceId, "ModifiedKh.SaveableArguments");

        private SaveableArguments saveableSettings;

        public SaveableArguments SaveableSettings 
        {
            get 
            {
                saveableSettings = (SaveableArguments) DataManager.Resolve(ArgsDroid);

                if (saveableSettings == null)
                {
                    saveableSettings = new SaveableArguments();
                }
                
                return saveableSettings;
            }
             set    
            {
                saveableSettings = value;
            }
        }


        #region Overridden Workstep methods

        /// <summary>
        /// Creates an empty Argument instance
        /// </summary>
        /// <returns>New Argument instance.</returns>

        protected override PermMatching.Arguments CreateArgumentPackageCore(IDataSourceManager dataSourceManager)
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
            private KrigingAlgorithm kgf;

            public Executor(Arguments arguments, WorkflowRuntimeContext context)
            {
                this.arguments = arguments;
                this.context = context;
            }

            public override void ExecuteSimple()
            {
                try
                {
                    //Cursor c = Cursors.WaitCursor;
                    //this.arguments.PBar = PetrelLogger.NewProgress(0, 100, ProgressType.Cancelable, c);


                    //this.arguments.PBar.ProgressStatus = 0;
                    //if (this.arguments.PBar.IsCanceled == true)
                    //{
                    //    return;
                    //}

                    #region Get All "Included" KhTableRowInfoContainer Object References into one list

                    List<KhTableRowInfoContainer> ListOfIncludedModellingData = new List<KhTableRowInfoContainer>();

                    foreach (KhTableRowInfoContainer ri in arguments.ListOfModellingData)
                    {
                        if (ri.Include)
	                    {
                            ListOfIncludedModellingData.Add(ri);
	                    }

                    }


                    #endregion

                    #region Normal Score Transformation of kh ratio data

                    arguments.ListOfRatios.Clear();
                    arguments.mean = 0;
                    arguments.std = 0;

                    //getting the mean
                    foreach (KhTableRowInfoContainer ri in ListOfIncludedModellingData)
                    {

                            arguments.ListOfRatios.Add(ri.Ratio);
                            arguments.mean = ri.Ratio + arguments.mean;

                     
                    }

                    arguments.mean = arguments.mean / arguments.ListOfRatios.Count;

                    //getting the standard deviation
                    foreach (double value in arguments.ListOfRatios)
                    {
                        arguments.std = Math.Pow(value - arguments.mean, 2) + arguments.std;
                    }

                    arguments.std = Math.Sqrt(arguments.std / arguments.ListOfRatios.Count);

                    //Normal transformation of data
                    foreach (double ra in arguments.ListOfRatios)
                    {
                        arguments.ListOfNormalTransformData.Add(Normal_Transform(arguments.mean, arguments.std, ra));
                    }

                    #endregion



                    double Kh_ave; double Kh_wt = 0;
                    double x_ave;
                    double y_ave;
                    int TotalCellsCounter; int CellsPerZoneCounter;
                    double MD1x; double MD1y; double MD1z; double MD1;
                    double MD2x; double MD2y; double MD2z; double MD2;
                    Borehole SelectedWell = null;
                    double MiddlePointx; double MiddlePointy; double MiddlePointz;
                    Dictionary<Index3, double> DictionaryOfRatios = new Dictionary<Index3, double>();

                    int max_i = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.I;
                    int max_j = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.J;
                    int max_k = this.arguments.PermeabilityFromModel.Grid.NumCellsIJK.K;
                    int ave_i; int ave_j;
                    List<Index3> ListOfCellsWithProperty = new List<Index3>();
                    List<PerforatedCell> ListOfPerforatedCells = new List<PerforatedCell>();
                    double KhRatio;

                    // double[] arrayOfProperty = new double[max_i * max_j* max_k];
                    List<double> ListOfProperty = Enumerable.Repeat(0.0 / 0.0, max_i * max_j * max_k).ToList();
                    Template TemplateOfProperty = PetrelProject.WellKnownTemplates.PetrophysicalGroup.Permeability;




                    Point3 MiddlePoint;
                    Point3 IntersectingPoint;
                    Index3 MiddleCell;


                    //List<CellSide> Side;
                    IPillarGridIntersectionService pgiservice = CoreSystem.GetService<IPillarGridIntersectionService>();
                    PropertyCollection pc1 = this.arguments.OneLayerPerZoneGrid.PropertyCollection;
                    PropertyCollection pc2 = this.arguments.PermeabilityFromModel.Grid.PropertyCollection;
                    List<Index3> ListOfSelectedCellsInOneLayerGrid = new List<Index3>();

                    //Creating a new transaction


                     //this.arguments.PBar.ProgressStatus = 20;
                     //if (this.arguments.PBar.IsCanceled == true)
                     //{
                     //    return;
                     //}
                    using (ITransaction trans = DataManager.NewTransaction())
                    {
                        //trans.Lock(PetrelProject.PrimaryProject);
                        //List<PropertyCollection> ListOfLockingObj = new List<PropertyCollection>();
                        //ListOfLockingObj.Add(pc1);
                        //ListOfLockingObj.Add(pc2);

                        //trans.LockCollection(ListOfLockingObj);
                        trans.Lock(pc1);

                        #region Creating and Setting the Kh ratio as a Property
                        Property p = pc1.CreateProperty(PetrelProject.WellKnownTemplates.LogTypes2DGroup.RelativePermeability);
                        p.Name = "Kh_ratio Of Single Layer Model";

                        //Assigning normal-transformed kh ratio values to their corresponding cells in the One Layer per Grid property "p".
                        for (int i = 0; i < arguments.ListOfRatios.Count; i++)
                        {
                            p[ListOfIncludedModellingData[i].AvgIJK] = (float)arguments.ListOfNormalTransformData[i];

                            ListOfSelectedCellsInOneLayerGrid.Add(ListOfIncludedModellingData[i].AvgIJK);


                            PetrelLogger.InfoOutputWindow("The ratio of the selected zones is " + System.Convert.ToString(arguments.ListOfRatios[i]) + " and its normalized value is " +
                                                               System.Convert.ToString(arguments.ListOfNormalTransformData[i]));
                            PetrelLogger.InfoOutputWindow("The corresponding index is: " + System.Convert.ToString(ListOfIncludedModellingData[i].AvgIJK.I) + ", " +
                                                          System.Convert.ToString(ListOfIncludedModellingData[i].AvgIJK.J) + ", " + System.Convert.ToString(ListOfIncludedModellingData[i].AvgIJK.K));

                        }

                        p.SetCellsUpscaled(ListOfSelectedCellsInOneLayerGrid);



                        #endregion

                        //this.arguments.PBar.ProgressStatus = 50;
                        //if (this.arguments.PBar.IsCanceled == true)
                        //{
                        //    return;
                        //}

                        #region Kriging the normalized property kh ratio

                        //Setting of the Model Variogram parameters and Kriging Arguments. The values are obtained from the arguments variable set in the ModifiedKhUI section.
                        KrigingArguments KrigingArgs = null;
                        kgf = WellKnownPetrophysicalAlgorithms.Kriging;
                        KrigingArgs = kgf.CreateArgumentPackage(p);
                        KrigingArgs.ModelVariogram.MajorRange = arguments.VarArg.MajorRange;
                        KrigingArgs.ModelVariogram.MinorRange = arguments.VarArg.MinorRange;
                        KrigingArgs.ModelVariogram.Nugget = arguments.VarArg.Nugget;
                        KrigingArgs.ModelVariogram.ModelVariogramType = arguments.VarArg.ModelVariogramType;
                        KrigingArgs.ModelVariogram.Dip = new Angle(0);
                        KrigingArgs.ModelVariogram.MajorDirection = arguments.VarArg.MajorDirection;
                        KrigingArgs.Expert.KrigingType = arguments.KrigType;
                        ModelVariogramArguments argval = new ModelVariogramArguments();

                        arguments.ListOfZonesOfOneLayerPerZoneGrid = KandaPropertyCreator.GetAllLowLevelZones(arguments.OneLayerPerZoneGrid.Zones);

                        //Kriging the property by zones. First we check for those zones that have Kh ratio values and then we krig that zone using the previously defined Kriging Arguments.
                        foreach (Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone zone in arguments.ListOfZonesOfOneLayerPerZoneGrid)
                        {
                            if (arguments.ListOfPenetratedZoneNames.Contains(zone.Name))
                            {
                                kgf.Invoke(p, zone, KrigingArgs);
                                PetrelLogger.InfoOutputWindow("The " + zone.Name + " zone of the " + p.Name + " property with grid " + p.Grid.Name + " was kriged.");
                            }
                        }

                        #endregion

                        arguments.CopyOfp = p;

                        trans.Commit();
                        //this.arguments.PBar.ProgressStatus = 80;
                        //if (this.arguments.PBar.IsCanceled == true)
                        //{
                        //    return;
                        //}
                    }

                    using (ITransaction trans = DataManager.NewTransaction())
                    {

                        trans.Lock(pc2);
                        #region Multiplying the Permeability property of Original Grid with the krigged ratio of Kh and storing in a newly created Property.

                        //Creating New K property
                        Property p2 = pc2.CreateProperty(TemplateOfProperty);
                        p2.Name = "Updated K";

                        //Multiplying the previous K values contained in arguments.PermeabilityFromModel by their corresponding unnormalized Kh ratio.      
                        int counter = 0;
                        int counter2 = 0;
                        int counter3 = 0;
                        int top_k;
                        int base_k;

                        //TODO: put an if statement to check for stairstep grid
                        foreach (Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone zone in arguments.ListOfAllZones)
                        {
                            if (zone.BaseK < zone.TopK)
                            {
                                base_k = zone.BaseK;
                                top_k = zone.TopK;
                            }
                            else
                            {
                                top_k = zone.BaseK;
                                base_k = zone.TopK;
                            }
                            if (arguments.ListOfPenetratedZoneNames.Contains(zone.Name)) //If it is a kriged zone then multiply the unnormalized ratio property with the original K property
                            {

                                for (int i = 0; i < max_i; i++)
                                {
                                    for (int j = 0; j < max_j; j++)
                                    {
                                        for (int k = base_k; k <= top_k; k++)
                                        {
                                            if (!float.IsNaN(arguments.PermeabilityFromModel[i, j, k]))
                                            {
                                                p2[i, j, k] = (float)Normal_Transform_Reverse(arguments.mean, arguments.std, (double)arguments.CopyOfp[i, j, counter]) * arguments.PermeabilityFromModel[i, j, k];
                                                counter2 = counter2 + 1;
                                            }

                                        }
                                    }
                                }
                            }
                            else       //If it is not a kriged zone then just make sure that the k value of the original K property is given to p2
                            {
                                for (int i = 0; i < max_i; i++)
                                {
                                    for (int j = 0; j < max_j; j++)
                                    {
                                        for (int k = base_k; k <= top_k; k++)
                                        {
                                            if (!float.IsNaN(arguments.PermeabilityFromModel[i, j, k]))
                                            {
                                                p2[i, j, k] = arguments.PermeabilityFromModel[i, j, k];
                                                counter3 = counter3 + 1;

                                            }

                                        }
                                    }
                                }
                            }
                            counter = counter + 1;
                        }

                        //Multiplying the cells that were penetrated by a well by their corresponding Kh ratio. 
                        //We use the original k (contained in arguments.PermeabilityFromModel) and multiply it by the ratio corresponding to their corresponding
                        //hard data kh ratio.

                        for (int i = 0; i < arguments.ListOfRatios.Count; i++)
                        {
                            foreach (Index3 ind in ListOfIncludedModellingData[i].ListOfCellInd)
                            {
                                //p2[ind] = arguments.PermeabilityFromModel[ind] * (float)arguments.ListOfRatios[i]; 
                                p2[ind] = arguments.PermeabilityFromModel[ind] * (float)ListOfIncludedModellingData[i].Ratio;
                            }
                        }

                        #endregion

                        trans.Commit();
                        //this.arguments.PBar.ProgressStatus = 100;
                        //this.arguments.PBar.Dispose();
                    }

                    arguments.Successful = true;
                }
                catch 
                {

                    arguments.Successful = false;
                }
             

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
            public List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> ListOfAllZones = new List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone>();
            public List<Dictionary<Index3, List<double>>> listOfKhDictionaries;
            private List<double> Kh_ave;
            private List<double> Kh_wt;
            private List<Dictionary<int, List<CellData>>> listOfCellDataDictionaries = new List<Dictionary<int, List<CellData>>>();
            private Property permeabilityFromModel;
            private Grid oneLayerPerZoneGrid;
            public ModelVariogramArguments VarArg = new ModelVariogramArguments();
            public  ModelVariogramType VariogramType;
            public List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> ListOfZonesOfOneLayerPerZoneGrid = new List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone>();
            private IProgress pBar;
            public KrigingType KrigType;

            public double VerticalRange;
            public List<String> ListOfPenetratedZoneNames = new List<string>();
            public Property CopyOfp;
            public bool Successful = false;


            public List<KhTableRowInfoContainer> ListOfModellingData = new List<KhTableRowInfoContainer>();
            public List<int> ListOfModellingKhwtIndices = new List<int>();
            public double mean;
            public double std;
            public List<double> ListOfNormalTransformData = new List<double>();
            public List<double> ListOfRatios = new List<double>();

            
           
            //private Dictionary<string, List<CellData>> dictionaryOfCellDataOfSelectedWells = new Dictionary<string, List<CellData>>();
            public IProgress PBar
            {
                get { return pBar; }
                set { pBar = value; }
            }

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
                get { return "CONNECT-PermMatch"; }
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
                return new ModifiedKhUI((PermMatching)workstep, (Arguments)argumentPackage, context);
            }
        }

        public static List<Borehole> GetAllBoreholesInProject(BoreholeCollection TopBhCollection)
        {

            List<Borehole> ListOfBoreholes = new List<Borehole>();

            foreach (Borehole bh in TopBhCollection)
            {
                ListOfBoreholes.Add(bh);
            }

            foreach (BoreholeCollection BhCollection in TopBhCollection.BoreholeCollections)
            {
                    ListOfBoreholes.AddRange(GetAllBoreholesInProject(BhCollection));
            }
            return ListOfBoreholes;

        
        }

        public static double Normal_Transform(double mean, double std, double OriginalData)
        {
            double TransformedData = -999.0;

            if (std >0) 
            {
                TransformedData = (OriginalData - mean) / (std);

            }
            else if (std == 0)
            {
                TransformedData = 0;
            }
            return TransformedData;

        }

        public static double Normal_Transform_Reverse(double mean, double std, double OriginalData)
        {
           double BackTransformedData = -999.0;

              if (std >=0)
             {
               BackTransformedData = (OriginalData * std) + mean; 
             }
                 
                return BackTransformedData;

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
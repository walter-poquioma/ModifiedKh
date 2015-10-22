using System;
using System.Collections.Generic;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.Modeling;
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
                // TODO: Implement the workstep logic here.
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

           // [Description("PermeabilityFromModel", "The permeability from the 3D model")]
            //public Property PermeabilityFromModel
            //{
            //    internal get { return this.permeabilityFromModel; }
            //    set { this.permeabilityFromModel = value; }
            //}

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
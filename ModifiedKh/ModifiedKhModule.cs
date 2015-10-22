using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace ModifiedKh
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    public class ModifiedKhModule : IModule
    {
        #region Private Variables
        private Process m_modifiedkhInstance;
        #endregion
        public ModifiedKhModule()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IModule Members

        /// <summary>
        /// This method runs once in the Module life; when it loaded into the petrel.
        /// This method called first.
        /// </summary>
        public void Initialize()
        {
            System.Diagnostics.Debugger.Launch();
            // TODO:  Add ModifiedKhModule.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
            
            // TODO:  Add ModifiedKhModule.Integrate implementation
            
            // Register ModifiedKh
            ModifiedKh modifiedkhInstance = new ModifiedKh();
            PetrelSystem.WorkflowEditor.AddUIFactory<ModifiedKh.Arguments>(new ModifiedKh.UIFactory());
            PetrelSystem.WorkflowEditor.Add(modifiedkhInstance);
            m_modifiedkhInstance = new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(modifiedkhInstance);
            PetrelSystem.ProcessDiagram.Add(m_modifiedkhInstance, "Plug-ins");
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {

            // TODO:  Add ModifiedKhModule.IntegratePresentation implementation
        }

        /// <summary>
        /// This method called once in the life of the module; 
        /// right before the module is unloaded. 
        /// It is usually when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
            // TODO:  Add ModifiedKhModule.Disintegrate implementation
            // Unregister ModifiedKh
            PetrelSystem.WorkflowEditor.RemoveUIFactory<ModifiedKh.Arguments>();
            PetrelSystem.ProcessDiagram.Remove(m_modifiedkhInstance);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add ModifiedKhModule.Dispose implementation
        }

        #endregion

    }


}
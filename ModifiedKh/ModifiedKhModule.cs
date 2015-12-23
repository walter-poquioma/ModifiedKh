using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.Commands;
using System.Windows.Forms;
using Slb.Ocean.Petrel.Contexts;
using Slb.Ocean.Petrel.Configuration;

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
            //System.Diagnostics.Debugger.Launch();
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
            PermMatching modifiedkhInstance = new PermMatching();
            PetrelSystem.WorkflowEditor.AddUIFactory<PermMatching.Arguments>(new PermMatching.UIFactory());
            //PetrelSystem.WorkflowEditor.Add(modifiedkhInstance);
            m_modifiedkhInstance = new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(modifiedkhInstance);
            PetrelSystem.ProcessDiagram.Add(m_modifiedkhInstance, "Property modeling");
            PetrelSystem.AddDataSourceFactory( new CONNECTModifiedKhDataSourceFactory());
            PetrelSystem.CommandManager.CreateCommand(PermMatchCommandHandler.Id, new PermMatchCommandHandler());


          //  PetrelSystem.CommandManager.CreateCommand("Slb.Ocean.Example.MyFirstPluginCommand",  new DoSomethingCommandHandler());

        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {

            // TODO:  Add ModifiedKhModule.IntegratePresentation implementation
            PetrelSystem.ConfigurationService.AddConfiguration(ResourcePermMatch.PermMatchConfig);
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
            PetrelSystem.WorkflowEditor.RemoveUIFactory<PermMatching.Arguments>();
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

    class PermMatchCommandHandler : SimpleCommandHandler
    {
        public static string Id = "ModifiedKh.PermMatchCommandHandler";

        public override bool CanExecute(Context contextt)
        {
 
        

            Slb.Ocean.Petrel.Basics.IProjectInfo pi = PetrelProject.GetProjectInfo(DataManager.DataSourceManager);
            if (pi.ProjectFile != null)
                return true;
            else
            {
                MessageBox.Show(
                        "This module can only be executed after a PETREL project has been opened.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
   
            

        }



       // public override void Execute(Slb.Ocean.Petrel.Contexts.Context context)
        public override void Execute(Context context)    
    { 
           // PermMatching PermMatchObj = new PermMatching();
           //// Process m_modifiedkhInstance = new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(PermMatchObj);
           // Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper.Context PermMatchContext =  new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper.Context();
           // PermMatching.Arguments PermArgs = new PermMatching.Arguments(DataManager.DataSourceManager);


           // ModifiedKhUI ModifiedKh = new ModifiedKhUI(PermMatchObj, PermArgs, PermMatchContext);
            PetrelLogger.InfoOutputWindow("CONNECT-PermMatch plug-in has been launched.");


        }

    }



}
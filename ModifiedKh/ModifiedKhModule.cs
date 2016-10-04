using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.Commands;
using System.Windows.Forms;
using Slb.Ocean.Petrel.Contexts;
using Slb.Ocean.Petrel.Configuration;
using Slb.Ocean.Petrel.Basics;

namespace ModifiedKh
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    /// 
    [ModuleAppearance(typeof(ModifiedKhModuleAppearance))]

    [FlexFeature("OCEAN_KAA_CONNECT-PERMMATCH", 0.0f)]
    public class ModifiedKhModule : IModule
    {
        #region Private Variables
        private Process m_modifiedkhInstance;
        #endregion
        private bool m_HasLicense;
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



            m_HasLicense = PetrelSystem.LicensingService.HasModuleLicense(typeof(ModifiedKhModule));
            if (m_HasLicense == true)
            {
                CoreLogger.Info(FeatureName + ", " + FeatureVersion + " is licensed with key : " + m_HasLicense.ToString());

            }
            else
            {
                CoreLogger.Info("License is not valid for " + FeatureName + ", " + FeatureVersion);
                PetrelLogger.InfoOutputWindow("CONNECT-PermMatch license is not available or expired");
            }
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
            if (m_HasLicense == true)
           {
                PermMatching modifiedkhInstance = new PermMatching();
                PetrelSystem.WorkflowEditor.AddUIFactory<PermMatching.Arguments>(new PermMatching.UIFactory());
                //PetrelSystem.WorkflowEditor.Add(modifiedkhInstance);
                m_modifiedkhInstance = new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(modifiedkhInstance);
                PetrelSystem.ProcessDiagram.Add(m_modifiedkhInstance, "Property modeling");
                PetrelSystem.AddDataSourceFactory(new CONNECTModifiedKhDataSourceFactory());
                PetrelSystem.CommandManager.CreateCommand(PermMatchCommandHandler.Id, new PermMatchCommandHandler());

              
           }

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
           if (m_HasLicense == true)
           {
                PetrelSystem.ConfigurationService.AddConfiguration(ModifiedKh.Properties.Resources.PermMatchConfig);
           }


      
            //Register help content via PetrelSystem.HelpService
            string helpDirectory1 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            HelpService helpService1 = PetrelSystem.HelpService;
            PluginHelpManifest helpContent3 = new PluginHelpManifest(System.IO.Path.Combine(helpDirectory1, @"HelpFiles\CONNECT PermMatch_CL.chm"))
            {
                Text = "CONNECT-PermMatch",
                HelpTargets = HelpTargets.Classic
            };
            helpService1.Add(helpContent3);
            PluginHelpManifest helpContent4 = new PluginHelpManifest(System.IO.Path.Combine(helpDirectory1, @"HelpFiles\CONNECT PermMatch.chm"))
            {
                Text = "CONNECT-PermMatch",
                HelpTargets = HelpTargets.Default
            };
            helpService1.Add(helpContent4);
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

        #region ISlbFeature Members

        public string FeatureName
        {
            get { return "OCEAN_KAA_CONNECT-PERMMATCH"; }
        }

        public float FeatureVersion
        {
            get { return 0.0f; }
        }

        #endregion

        #region ModuleAppearance Class

        /// <summary>
        /// Appearance (or branding) for a Slb.Ocean.Core.IModule.
        /// This is associated with a module using Slb.Ocean.Core.ModuleAppearanceAttribute.
        /// </summary>
        internal class ModifiedKhModuleAppearance : IModuleAppearance
        {
            /// <summary>
            /// Description of the module.
            /// </summary>
            public string Description
            {
                get { return "CONNECT-PermMatch"; }
            }

            /// <summary>
            /// Display name for the module.
            /// </summary>
            public string DisplayName
            {
                get { return "CONNECT-PermMatch"; }
            }

            /// <summary>
            /// Returns the name of a image resource.
            /// </summary>
            public string ImageResourceName
            {
                get { return null; }//"Upscaling.ResourceConnect.connect_logo_symbol"
            }

            /// <summary>
            /// A link to the publisher or null.
            /// </summary>
            public Uri ModuleUri
            {
                get { return null; }//(new Uri("ttp://www.kelkar-and-assoc.com"))
            }
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
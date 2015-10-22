using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel;
using Slb.Ocean.Core;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.Well;
using Slb.Ocean.Petrel.DomainObject.ColorTables;

namespace ModifiedKh
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class ModifiedKhUI : UserControl
    {
        private ModifiedKh workstep;
        /// <summary>
        /// The argument package instance being edited by the UI.
        /// </summary>
        private ModifiedKh.Arguments args;
        /// <summary>
        /// Contains the actual underlaying context.
        /// </summary>
        private WorkflowContext context;

        List<string> ListOfNamesOfIntersectedZones = new List<string>();
        List<string> ListOfSelectedWellZones = new List<string>();
        WellKh WellKhObj = new WellKh();

       

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedKhUI"/> class.
        /// </summary>
        /// <param name="workstep">the workstep instance</param>
        /// <param name="args">the arguments</param>
        /// <param name="context">the underlying context in which this UI is being used</param>
        public ModifiedKhUI(ModifiedKh workstep, ModifiedKh.Arguments args, WorkflowContext context)
        {
            InitializeComponent();

            this.workstep = workstep;
            this.args = args;
            this.context = context;
        }


        private void UpdateArgs()
        {
            this.args.ListOfWellKh.Add(WellKhObj);
           // this.args.PermeabilityFromModel = DroppedPermeability;
           // this.args.WellsSelected.Add(DroppedBorehole);
        }

     

        private void PermeabilityDropTarget_DragDrop(object sender, DragEventArgs e)
        {
            
            WellKhObj.Permeability = e.Data.GetData(typeof(Property)) as Property;
            if (WellKhObj.Permeability.Template.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.Perm))
            {

                e.Effect = DragDropEffects.All;
                PermeabilityPresentationBox.Text = WellKhObj.Permeability.Name;

            }
            else
            {
                e.Effect = DragDropEffects.None;
                MessageBox.Show("Property is not the one expected");
            }
        }

        private void WellDropTarget_DragDrop(object sender, DragEventArgs e)
        {
            WellKhObj.Well = e.Data.GetData(typeof(Borehole)) as Borehole;
           
            if (WellKhObj.GetListOfNamesOfIntersectedZones(true))
            {
                e.Effect = DragDropEffects.All;
                WellPresentationBox.Text = WellKhObj.Well.Name;
                ZonesListBox.DataSource = null;
                ZonesListBox.DataSource = WellKhObj.ListOfNamesOfIntersectedZones.Distinct().ToList();
            }

            else 
            {
                e.Effect = DragDropEffects.None;
                MessageBox.Show("Please verify that a Permeability property and a Zone Index property have been dropped.");
            }
          
        }

        private void ZoneIndexDropTarget_DragDrop(object sender, DragEventArgs e)
        {
            WellKhObj.ZoneIndex = e.Data.GetData(typeof(DictionaryProperty)) as DictionaryProperty;

            if (WellKhObj.ZoneIndex.DictionaryTemplate.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.ZonesSubHierarchy) ||
                 (WellKhObj.ZoneIndex.DictionaryTemplate.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.ZonesMain)) ||
                (WellKhObj.ZoneIndex.DictionaryTemplate.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.ZonesSub)) ||
                 (WellKhObj.ZoneIndex.DictionaryTemplate.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.ZonesAllK)))           
            {

                e.Effect = DragDropEffects.All;
                ZoneIndexPresentationBox.Text = WellKhObj.ZoneIndex.Name;
             
            }
            else
            {
                e.Effect = DragDropEffects.None;
              
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (ZonesListBox.SelectedItems != null && WellKhObj.KhWellTesting > 0)
            {   
                 //foreach (DataRowView objDataRowView in ZonesListBox.SelectedItems)
                foreach (string ZoneName in ZonesListBox.SelectedItems)
                {     
                       ListOfSelectedWellZones.Add(ZoneName);
                       PetrelLogger.InfoOutputWindow(ZoneName);
                       //ListOfSelectedWellZones.Add(objDataRowView[0].ToString());
                       //PetrelLogger.InfoOutputWindow(objDataRowView[0].ToString());
                   }

                 if (!WellKhObj.VerticalContinuity(ListOfSelectedWellZones)) 
                 {
                     MessageBox.Show("The selected Zone intervals are not vertically continous");
                  }

            }
            else
            {
                MessageBox.Show("Please verify that a Permeability property, a Zone Index property and a well have been dropped." + "\r\n" +
                                 "If all these items have been dropped you may select the zones intersected by the well which contribute towards the typed well testing Kh");
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (WellKhObj.GetListOfNamesOfIntersectedZones(false))
                {
                    ZonesListBox.DataSource = null;
                    ZonesListBox.DataSource = WellKhObj.ListOfNamesOfIntersectedZones.Distinct().ToList();
                }
                else 
                {
                    MessageBox.Show("Please verify that a Permeability property and a Zone Index property have been dropped.");
                }


            }

            else
            {

                if (WellKhObj.GetListOfNamesOfIntersectedZones(true))
                {
                    ZonesListBox.DataSource = null;
                    ZonesListBox.DataSource = WellKhObj.ListOfNamesOfIntersectedZones.Distinct().ToList();
                }
                else
                {
                    MessageBox.Show("Please verify that a Permeability property and a Zone Index property have been dropped.");
                }
          
            }
        
        }

        private void ZonesListBox_DataSourceChanged(object sender, EventArgs e)
        {
            
			/*
			*The following code is required to remove 
			*existing items from the Items collection
			*when the DataSource is set to null.
			*/ 
			
			ListBox ctlLIST = (ListBox) sender;
			if (ctlLIST.DataSource == null)
			ctlLIST.Items.Clear();
		
			
        }

        private void WellTestTextBox_TextChanged(object sender, EventArgs e)
        {
            
            try
            {
                WellKhObj.KhWellTesting = Convert.ToDouble(WellTestTextBox.Text)*WellKh.FactorToConvert_mdft_To_m3;
            }

            catch
            {
                WellKhObj.KhWellTesting = -1;
                MessageBox.Show("Please input a valid entry (any number between 1.7E +/- 308 )");
            }
           
        }

       
    }
}

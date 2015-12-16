using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.ObjectModel;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Core;
using Slb.Ocean.Units;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.Well;
using Slb.Ocean.Petrel.DomainObject.ColorTables;
using Slb.Ocean.Petrel.UI.Controls;
using System.Text.RegularExpressions;
using Infragistics.Win.UltraWinChart;
using Infragistics.UltraChart.Shared.Styles;

namespace ModifiedKh
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class ModifiedKhUI : UserControl
    {
        private PermMatching workstep;
        /// <summary>
        /// The argument package instance being edited by the UI.
        /// </summary>
        private PermMatching.Arguments args;
        /// <summary>
        /// Contains the actual underlaying context.
        /// </summary>
        private WorkflowContext context;


        List<string> ListOfNamesOfIntersectedZones = new List<string>();
        List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone> ListOfZones = new List<Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone>();
        List<string> ListOfSelectedWellZones = new List<string>();
        List<Borehole> ListOfDroppedWells = new List<Borehole>();
        List<Borehole> ListOfBoreholes = new List<Borehole>();
        WellKh WellKhObj = new WellKh();
        bool Depth_or_Zones = false;
        bool PerforatedZonesOnly = true;
        List<KhTableRowInfoContainer> ListOfRowInfo = new List<KhTableRowInfoContainer>();
        List<KhTableRowInfoContainer> ListOfOriginalRowDataInfo = new List<KhTableRowInfoContainer>();
        List<double> ListOfRatios = new List<double>();
        List<double> ListOfLogRatios = new List<double>();
        ObservableCollection<int> ListOfFilledKhwtIndices = new ObservableCollection<int>();
        List<int> ListOfOriginalFilledKhwtIndices = new List<int>();
        bool FieldUnitsFlag;
        int SignificantDigits = 8;
        bool FirstTimeEditingRatio = true;
        double SumOfRatios = 0;
        int OldIndex;
        double MaxRatio = 0; double MinRatio = 0;    
        bool FirstTimeTruncating = true;
        Cursor c = Cursors.WaitCursor;
        public IProgress PBar1;

        private SaveableArguments SaveArgs;
       
        //DictionaryProperty ZoneIndexing;
       
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedKhUI"/> class.
        /// </summary>
        /// <param name="workstep">the workstep instance</param>
        /// <param name="args">the arguments</param>
        /// <param name="context">the underlying context in which this UI is being used</param>
        public ModifiedKhUI(PermMatching workstep, PermMatching.Arguments args, WorkflowContext context)
        {
            InitializeComponent();

            this.workstep = workstep;
            this.args = args;
            this.context = context;
 
        }

 

        private void UpdateArgs(WellKh WellKhObj)
        { 
           // this.args.ListOfWellKh.Add(WellKhObj);
            this.args.ListOfCellDataDictionaries.Add(WellKhObj.GetKhDictionaryOfSelectedGridCells(Depth_or_Zones, PerforatedZonesOnly, true));
           // this.args.PermeabilityFromModel = DroppedPermeability;
           // this.args.WellsSelected.Add(DroppedBorehole);
        }


        private void UpdateAllRows() 
        {
            int counter = 0;


            foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
            {Button B1 = new Button();
             // B1.Text  = "Original";

                if(counter < WellKhDataGridView.RowCount - 1)
                {   foreach(DataGridViewCell cell in WellKhDataGridView.Rows[counter].Cells)
                    {
                          switch(cell.ColumnIndex)
                          {   case 0:
                               cell.Value = ri.WellName;
                               break;
                            
                              case 1:
                               cell.Value = ri.ZoneName;
                               break;
                             
                              case 2:
                               if (FieldUnitsFlag)
                                   cell.Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_sim * (1 / WellKh.FactorToConvert_mdft_To_m3), SignificantDigits));
                               else
                                   cell.Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_sim * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits));
                               break;
                              
                              case 3:
                               if (ri.Kh_wt > 0)
                                   if (FieldUnitsFlag)
                                       cell.Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_wt * (1 / WellKh.FactorToConvert_mdft_To_m3), SignificantDigits));
                                   else
                                       cell.Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_wt * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits));
                               else
                                   cell.Value = String.Empty;
                               break;
                             
                              case 4:                               
                               cell.Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Ratio,SignificantDigits));
                               break;
                                    
                              case 5:                               
                               cell.Value = false;
                               break;

                              case 6:
                               cell.Value = B1;
                               break;

                              case 7:
                               cell.Value = true;
                               break;

                          }
                     

                     }
                WellKhDataGridView.Rows[counter].Tag = ri;
                }
                else
                {
                    if (ri.Kh_wt > 0)
                    {
                        if (FieldUnitsFlag)
                            WellKhDataGridView.Rows.Add(ri.WellName, ri.ZoneName, System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_sim * (1 / WellKh.FactorToConvert_mdft_To_m3),SignificantDigits)),
                                                    System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_wt * (1 / WellKh.FactorToConvert_mdft_To_m3),SignificantDigits)), 
                                                    System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Ratio,SignificantDigits)),false, B1,true);
                        else
                            WellKhDataGridView.Rows.Add(ri.WellName, ri.ZoneName, System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_sim * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits)),
                                                     System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_wt * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits)),
                                                     System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Ratio, SignificantDigits)), false, B1,true);
                    }
                    else
                    {
                        if (FieldUnitsFlag)
                            WellKhDataGridView.Rows.Add(ri.WellName, ri.ZoneName, System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_sim * (1 / WellKh.FactorToConvert_mdft_To_m3),SignificantDigits)),
                                                     String.Empty, System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Ratio, SignificantDigits)), false, B1,true);
                        else
                            WellKhDataGridView.Rows.Add(ri.WellName, ri.ZoneName, System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Kh_sim * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits)),
                                                     String.Empty, System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ri.Ratio, SignificantDigits)), false, B1,true);
                    }
                    WellKhDataGridView.Rows[counter].Tag = ri;
                }
                counter++; 
            }

        }

        private void PermeabilityDropTarget_DragDrop(object sender, DragEventArgs e)
        {
           
            WellKhObj.Permeability = e.Data.GetData(typeof(Property)) as Property;


            if (WellKhObj.Permeability == null)
            {
                MessageBox.Show("A Permeability Property needs to be dropped");
                return;
            }


            if (UpdateArgsWithNewPerm())
            {
              e.Effect = DragDropEffects.All;  
            } 
            else
	        {
                e.Effect = DragDropEffects.None; 
	        }
           
 
        }

        private bool UpdateArgsWithNewPerm()
        {
            bool Success = false;
            if (WellKhObj.Permeability.Template.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.Perm))
            {


                //PBar1 = PetrelLogger.NewProgress(0, 100, ProgressType.Cancelable, c);
                //PBar1.SetProgressText("Loading Permeability...");
                //PBar1.ProgressStatus = 1;
                //if (PBar1.IsCanceled == true)
                //{
                //    return false;
                //}
                #region Enabling and Disabling certain UI objects
                
                SelectedWellsCheckBox.Enabled = false;
                HistogramButton.Enabled = true;
                SaveOriginalData.Enabled = true;
                //Missing2Mean.Enabled = true;
                Truncate2NormalDist.Enabled = true;
                UseOriginalData.Enabled = true;
                KeepMissingRatio1.Enabled = true;

                WellKhDataGridView.Columns[5].ReadOnly = false;
                WellKhDataGridView.Columns[3].ReadOnly = false;
                WellKhDataGridView.Columns[4].ReadOnly = false;
                WellKhDataGridView.Columns[7].ReadOnly = false;
                WellKhDataGridView.Columns[6].ReadOnly = false;

                MajorRangeTextBox.ReadOnly = false;
                MinorRangeTextBox.ReadOnly = false;
                MajorDirectionTextBox.ReadOnly = false;
                NuggetTextBox.ReadOnly = false;
                VariogramTypeComboBox.Enabled = true;
                KrigingAlgComboBox.Enabled = true;
                MaximumRatioValue.ReadOnly = false;
                MinimumRatioValue.ReadOnly = false;
               
                #endregion

                #region Setting the default Variogram parameters
                args.VarArg.MajorRange = CalculateDefaultRange(WellKhObj.Permeability.Grid);
                args.VarArg.MinorRange = args.VarArg.MajorRange;

                MajorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MajorRange, SignificantDigits));
                MinorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MinorRange, SignificantDigits));

                args.VarArg.Nugget = 0.0;
                NuggetTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.Nugget, SignificantDigits));

                args.VariogramType = ModelVariogramType.Spherical;
                VariogramTypeComboBox.SelectedIndex = 0;

                args.VarArg.MajorDirection = Angle.CreateFromCompassAngle(0, false);
                MajorDirectionTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(System.Convert.ToDouble(args.VarArg.MajorDirection.CompassDegrees), SignificantDigits));

                args.VarArg.ModelVariogramType = ModelVariogramType.Spherical;
                VariogramTypeComboBox.SelectedIndex = 0;

                args.KrigType = Slb.Ocean.Petrel.PropertyModeling.KrigingType.Ordinary;
                KrigingAlgComboBox.SelectedIndex = 0;

                UpdateSillTextBox();

                //PBar1.ProgressStatus = 10;
                //if (PBar1.IsCanceled == true)
                //{
                //    return false;
                //}
                #endregion

                #region Assigning the dropped permeability to argument object parameters
               // e.Effect = DragDropEffects.All;
                Success = true;
                PermeabilityPresentationBox.Text = WellKhObj.Permeability.Name;
                this.args.PermeabilityFromModel = WellKhObj.Permeability;
                #endregion

                #region Obtaining all the zones in the grid
                ListOfZones = KandaPropertyCreator.GetAllLowLevelZones(WellKhObj.Permeability.Grid.Zones);
                args.ListOfAllZones = ListOfZones;
         

                ListOfSelectedWellZones.Clear();
                foreach (Slb.Ocean.Petrel.DomainObject.PillarGrid.Zone lzone in ListOfZones)
                {
                    //PetrelLogger.InfoOutputWindow(lzone.Name);
                    ListOfSelectedWellZones.Add(lzone.Name);
                }
                //PBar1.ProgressStatus = 20;
                //if (PBar1.IsCanceled == true)
                //{
                //    return false;
                //}
                WellKhObj.ZoneIndex = KandaPropertyCreator.CreateZoneIndex(WellKhObj.Permeability.Grid);
                //PBar1.ProgressStatus = 30;
                //if (PBar1.IsCanceled == true)
                //{
                //    return false;
                //}
                #endregion
                //WellKhObj.VerticalContinuity(this.ListOfNamesOfIntersectedZones);

                #region Updating the DataGridView and ListOfRowInfo object
                WellKhDataGridView.AllowUserToAddRows = true;
                UpdateRowObjectsWithNewWells(ListOfBoreholes);
                //PBar1.ProgressStatus = 75;
                //if (PBar1.IsCanceled == true)
                //{
                //    return false;
                //}
                UpdateAllRows();
                //PBar1.ProgressStatus = 90;
                //if (PBar1.IsCanceled == true)
                //{
                //    return false;
                //}
                WellKhDataGridView.AllowUserToAddRows = false;
                #endregion

                WellKhDataGridView.Columns[6].ReadOnly = true;
               // PBar1.ProgressStatus = 100;
                //PBar1.Dispose();
            }
            else
            {
                //e.Effect = DragDropEffects.None;
                Success = false;
                MessageBox.Show("Property is not the one expected");
            }
            return Success;
        }
        private void WellDropTarget_DragDrop(object sender, DragEventArgs e)
        {
            
                WellKhObj.Well = e.Data.GetData(typeof(Borehole)) as Borehole;
           
            if(WellKhObj.Well ==null)
            {
                MessageBox.Show("Please make sure that you are dropping a well");
                return;
            }

            if (WellKhObj.SetListOfNamesOfIntersectedZones(true))
            {    
                if (!ListOfDroppedWells.Contains(WellKhObj.Well))
                {
                    ListOfDroppedWells.Add(WellKhObj.Well);
                    e.Effect = DragDropEffects.All;
                    WellPresentationBox.Text = WellKhObj.Well.Name;
                    //ZonesListBox.DataSource = null;
                    //ZonesListBox.DataSource = WellKhObj.ListOfNamesOfIntersectedZones.Distinct().ToList();
                    ZonesListBox.Items.Clear();
                    foreach (String ZoneName in WellKhObj.ListOfNamesOfIntersectedZones.Distinct().ToList())
                    { ZonesListBox.Items.Add(ZoneName); }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    MessageBox.Show("This well has been dropped previously.");
                }
            }

            else 
            {
                e.Effect = DragDropEffects.None;
                MessageBox.Show("Please verify that a Permeability property and a Zone Index property have been dropped.");
            }
          
        }

        //private void ZoneIndexDropTarget_DragDrop(object sender, DragEventArgs e)
        //{
        //       WellKhObj.ZoneIndex = e.Data.GetData(typeof(DictionaryProperty)) as DictionaryProperty;


        //       if (WellKhObj.ZoneIndex == null)
        //        {
        //            MessageBox.Show("A Zone Index property needs to be dropped");
        //            return;
        //        }

        //    if (WellKhObj.ZoneIndex.DictionaryTemplate.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.ZonesSubHierarchy) ||
        //         (WellKhObj.ZoneIndex.DictionaryTemplate.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.ZonesMain)) ||
        //        (WellKhObj.ZoneIndex.DictionaryTemplate.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.ZonesSub)) ||
        //         (WellKhObj.ZoneIndex.DictionaryTemplate.TemplateType.Equals(Slb.Ocean.Petrel.DomainObject.Basics.TemplateType.ZonesAllK)))           
        //    {

        //        e.Effect = DragDropEffects.All;
        //        ZoneIndexPresentationBox.Text = WellKhObj.ZoneIndex.Name;

        //        /////
        //        for (int i = 0; i < WellKhObj.Permeability.Grid.NumCellsIJK.I; i++)
        //        {
        //            for (int j = 0; j < WellKhObj.Permeability.Grid.NumCellsIJK.J; j++)
        //            {
        //                for (int k = 0; k < WellKhObj.Permeability.Grid.NumCellsIJK.K; k++)
        //                {
        //                    if (WellKhObj.ZoneIndex[i, j, k] != ZoneIndexing[i, j, k] && WellKhObj.ZoneIndex[i, j, k] == WellKhObj.ZoneIndex[i, j, k])
        //                    {
        //                        if (WellKhObj.ZoneIndex[i, j, k] != ZoneIndexing[i, j, k] + 1 && WellKhObj.ZoneIndex[i, j, k]  != ZoneIndexing[i, j, k] + 2)
        //                        { PetrelLogger.InfoOutputWindow("The following cell does not match Zone Index: " + 
        //                                               System.Convert.ToString(i) + ", " +  System.Convert.ToString(j) + ", " +
        //                                                 System.Convert.ToString(k) + "the two indeces are: " + System.Convert.ToString(WellKhObj.ZoneIndex[i, j, k])+
        //                                                "and " + ZoneIndexing[i, j, k]);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        ////
             
        //    }
        //    else
        //    {
        //        e.Effect = DragDropEffects.None;
              
        //    }
        //}

        private void Add_Click(object sender, EventArgs e)
        {
            if (ZonesListBox.SelectedItems != null && WellKhObj.KhWellTesting > 0)
            {   
                 //foreach (DataRowView objDataRowView in ZonesListBox.SelectedItems)
                foreach (string ZoneName in ZonesListBox.SelectedItems)
                {     
                       ListOfSelectedWellZones.Add(ZoneName);
                       
                       PetrelLogger.InfoOutputWindow(ZoneName);
                   }
               

                if (!WellKhObj.VerticalContinuity(ListOfSelectedWellZones))
                {
                    MessageBox.Show("The selected Zone intervals are not vertically continous");
                    ListOfSelectedWellZones.Clear();
                }
                else 
                {  
                    UpdateArgs(WellKhObj);
                    
                    foreach(Dictionary<int,List<CellData>> dict in this.args.ListOfCellDataDictionaries)
                     {
                         foreach (int ind in dict.Keys)
                         {
                             PetrelLogger.InfoOutputWindow("Cell Info Corresponding to Zone Index " + System.Convert.ToString(ind) + " :");
                             foreach (CellData cell in dict[ind]) 
                             {    Index3 ind_UI =  ModelingUnitSystem.ConvertIndexToUI(WellKhObj.Permeability.Grid, cell.CellIndex); 
                                 PetrelLogger.InfoOutputWindow( System.Convert.ToString(ind_UI.I) + "," + System.Convert.ToString(ind_UI.J)+ ","+
                                                                System.Convert.ToString(ind_UI.K));
                                 PetrelLogger.InfoOutputWindow(System.Convert.ToString(cell.Height));
                                 PetrelLogger.InfoOutputWindow(System.Convert.ToString(cell.Kh_wt));
                                 PetrelLogger.InfoOutputWindow(System.Convert.ToString(cell.Perm));
                             }
                         }
                     }
                    for (int x = ZonesListBox.SelectedItems.Count - 1; x >= 0; x--)
                    {
                        int idx = ZonesListBox.SelectedIndices[x];
                        ZonesListBox.Items.RemoveAt(idx);
                    }
                    ListOfSelectedWellZones.Clear();
                }

                //List<string> ListOfCurrentlyDisplayedZones = ZonesListBox;

                


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
                PerforatedZonesOnly = false;
            }

            else
            {
                PerforatedZonesOnly = true;
            }

            if (WellKhObj.SetListOfNamesOfIntersectedZones(PerforatedZonesOnly))
            {
                //ZonesListBox.DataSource = null;
                //ZonesListBox.DataSource = WellKhObj.ListOfNamesOfIntersectedZones.Distinct().ToList();
                ZonesListBox.Items.Clear();
                foreach (String ZoneName in WellKhObj.ListOfNamesOfIntersectedZones.Distinct().ToList())
                { ZonesListBox.Items.Add(ZoneName); }
            }
            else
            {
                MessageBox.Show("Please verify that a Permeability property and a Zone Index property have been dropped.");
            }
        
        }

        private void ZonesListBox_DataSourceChanged(object sender, EventArgs e)
        {
            
			/*
			*The following code is required to remove 
			*existing items from the Items collection
			*when the DataSource is set to null.
			*/

            System.Windows.Forms.ListBox ctlLIST = (System.Windows.Forms.ListBox)sender;
			if (ctlLIST.DataSource == null)
			ctlLIST.Items.Clear();
		
			
        }

        private void WellTestTextBox_TextChanged(object sender, EventArgs e)
        {
            
            try
            {
                if (FieldUnitsFlag)
                    WellKhObj.KhWellTesting = Convert.ToDouble(WellTestTextBox.Text) * WellKh.FactorToConvert_mdft_To_m3;
                else
                    WellKhObj.KhWellTesting = Convert.ToDouble(WellTestTextBox.Text) * WellKh.FactorToConvert_mdm_To_m3;
            }

            catch
            {
                WellKhObj.KhWellTesting = -1;
                MessageBox.Show("Please input a valid entry (any number between 1.7E +/- 308 )");
            }
           
        }

        private void WellTestingIntervalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (WellTestingIntervalCheckBox.Checked)
            {
                Depth_or_Zones = true;
                Top.Visible = true;
                Base.Visible = true;
            }
            else
            {
                Depth_or_Zones = false;
                Top.Visible = false;
                Base.Visible = false;
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {


            if (context is WorkstepProcessWrapper.Context)
            {
                if ( TestUserInput())
                {
                   
                    //HistogramButton.PerformClick();
                    SetListOfModellingDataAndDataGridView();
                    UpdateSaveableArgs();
                    Executor executor = workstep.GetExecutor(args, null);
                    args.Successful = false;
                    executor.ExecuteSimple();

                    if (args.Successful)
                    {
                        PetrelLogger.InfoOutputWindow("The program finished running. \n" +
                            "The new Permeability property can be found in the same Properties Folder from where the original Permeability was taken. \n" +
                             "The new Permeability property is named Updated K. \n" +
                              "A property called Kh_ratio Of Single Layer Model was created. \n" +
                            "The new property can be found in the Properties folder of the one layer per zone grid.");
                    }
                    else
                    {
                        PetrelLogger.InfoOutputWindow("The Plug-In did not perform the operation correctly. \n" + "Please make sure that all the required inputs are provided.");
                    } 
                }
                else
                {
                    PetrelLogger.InfoOutputWindow("The Plug-In did not perform the operation correctly. \n" + "Please make sure that all the required inputs are provided.");
                }
              
            }
        }

        private void OneLayerGridDropTarget_DragDrop(object sender, DragEventArgs e)
        {
            this.args.OneLayerPerZoneGrid = e.Data.GetData(typeof(Grid)) as Grid;


            if (this.args.OneLayerPerZoneGrid == null)
            {
                MessageBox.Show("A Grid needs to be dropped");
                return;
            }
            else
            {
                OneLayerGridPresentationBox.Text = this.args.OneLayerPerZoneGrid.Name;
            }
        }

        private void WellKhDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;
            if (e.ColumnIndex == 5 && e.RowIndex >=0)
            {
            Global_Individual_Check(ref senderGrid,(bool)senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue, e.RowIndex, e.ColumnIndex);
            }
            else if (e.ColumnIndex == 7 && e.RowIndex >=0)
            {
                Include_Check(ref senderGrid, (bool)senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue, e.RowIndex, e.ColumnIndex);
            }
           
        }

        private void Include_Check(ref DataGridView senderGrid, bool Included, int RowIndex, int ColumnIndex)
        {
            if (!Included)
            {
                senderGrid.Rows[RowIndex].ReadOnly = true;
                senderGrid.Rows[RowIndex].Cells[ColumnIndex].ReadOnly = false;
                ExcludeKhFromTotalRatio(ref senderGrid, RowIndex);
                ListOfRowInfo[RowIndex].Include = false;
 
            }
            else
            {
                senderGrid.Rows[RowIndex].ReadOnly = false;
                ListOfRowInfo[RowIndex].Include = true;
                ListOfFilledKhwtIndices.Add(RowIndex);



            }
        }

        private void Global_Individual_Check(ref DataGridView senderGrid, bool Global, int RowIndex, int ColumnIndex)
        { 
        
         double KhTotal = 0;
            double RatioTotal = 0;
            double KhwtIn;
            List<int> ListOfIndeces = new List<int>();
            int counter = 0;
            double DoubleValue;


            if (senderGrid.Columns[ColumnIndex] is DataGridViewCheckBoxColumn && RowIndex >= 0)
            {
                //PetrelLogger.InfoOutputWindow("The cell" + e.ColumnIndex.ToString() + " was clicked");
                string Entry = System.Convert.ToString(WellKhDataGridView.Rows[RowIndex].Cells[3].Value);


                if (Global)
                {
                    if (!Entry.Equals(String.Empty) && Double.TryParse(Entry, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue))
                    {
                        if (DoubleValue>0) //If Value of Kh_wt is positive
                        {
                            if (FieldUnitsFlag)
                            { KhwtIn = System.Convert.ToDouble(Entry) * WellKh.FactorToConvert_mdft_To_m3; }
                            else
                            { KhwtIn = System.Convert.ToDouble(Entry) * WellKh.FactorToConvert_mdm_To_m3; }

                            //if (FirstTimeEditingRatio)
                            //{  
                            //    foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
                            //    {  //TODO: Add the condition to check for Included flag.
                            //        if (ri.WellName.Equals(ListOfRowInfo[RowIndex].WellName))
                            //        {
                            //            KhTotal = KhTotal + ri.Kh_sim;
                            //            ListOfIndeces.Add(counter);
                            //        }
                            //        counter = counter + 1;
                            //    }



                            //    RatioTotal = KhwtIn / KhTotal;
                            //}
                            //else
                            //{
                            //    RatioTotal = ListOfRowInfo[RowIndex].Ratio;
                            //}

                                foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
                                {  //TODO: Add the condition to check for Included flag.
                                    if (ri.WellName.Equals(ListOfRowInfo[RowIndex].WellName) && ri.Include)
                                    {
                                        KhTotal = KhTotal + ri.Kh_sim;
                                        ListOfIndeces.Add(counter);
                                    }
                                    counter = counter + 1;
                                }



                                RatioTotal = KhwtIn / KhTotal;


                            foreach (int index in ListOfIndeces)
                            {

                                if (ListOfFilledKhwtIndices.Any(ind => ind == index))
                                {
                                    var itemToRemove = ListOfFilledKhwtIndices.Single(r => r == index);
                                    ListOfFilledKhwtIndices.Remove(itemToRemove);
                                }
                             
                                senderGrid.Rows[index].Cells[ColumnIndex - 2].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(System.Convert.ToDouble(Entry), SignificantDigits));
                                ListOfRowInfo[index].Kh_wt = KhwtIn;
                                ListOfRowInfo[index].Ratio = RatioTotal;
                                senderGrid.Rows[index].Cells[ColumnIndex - 1].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[index].Ratio, SignificantDigits));
                                senderGrid.Rows[index].Cells[ColumnIndex - 2].ReadOnly = true;

                                ListOfFilledKhwtIndices.Add(index);
                                WellKhDataGridView.CellValueChanged -= WellKhDataGridView_CellValueChanged;

                                    senderGrid.Rows[index].Cells[ColumnIndex].Value = true;
                                    ListOfRowInfo[index].Global = true;
                      
                                
                                WellKhDataGridView.CellValueChanged += WellKhDataGridView_CellValueChanged;

                               
                                // senderGrid.RefreshEdit();
                            }  
                        }
                        else
                        {
                            MessageBox.Show("Please enter a positive number in the Kh well testing column before checking the Global/Individiual check box.");
                        }
                        


                    }
                    else
                    {
                        MessageBox.Show("Please enter a number or a valid entry in the Kh well testing column before checking the Global/Individiual check box.");
                    }
                }

                else
                {
                    //foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
                    //{
                    //    if (ri.WellName.Equals(ListOfRowInfo[RowIndex].WellName))
                    //    {
                    //        UpdateSpecificRowInfoObject(counter, ColumnIndex ,System.Convert.ToString(senderGrid.Rows[counter].Cells[ColumnIndex - 2].Value));
                    //        senderGrid.Rows[counter].Cells[ColumnIndex - 2].ReadOnly = false;

                    //        WellKhDataGridView.CellValueChanged -= WellKhDataGridView_CellValueChanged;

                    //            senderGrid.Rows[counter].Cells[ColumnIndex].Value = false;
                    //            ri.Global = false;

                                
                    //        WellKhDataGridView.CellValueChanged += WellKhDataGridView_CellValueChanged;
                    //        //  senderGrid.RefreshEdit();

                    //    }
                    //    counter = counter + 1;
                    //}

                    ExcludeKhFromTotalRatio(ref senderGrid, RowIndex);

                    ListOfFilledKhwtIndices.Add(RowIndex);
                    senderGrid.Rows[RowIndex].Cells[ColumnIndex - 2].ReadOnly = false;


                           

                }


            }
        }

        private void WellKhDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
                string Entry = System.Convert.ToString(WellKhDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                UpdateSpecificRowInfoObject(e.RowIndex, e.ColumnIndex,Entry);

        }

        private bool UpdateSpecificRowInfoObject(int RowInd,int ColInd,string Entry)
        {   double DoubleValue;
        OldIndex = RowInd;

          if (ColInd == 3)
          {
            if (Double.TryParse(Entry, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue)) //
            {
                if (DoubleValue >0)
                {
                    if (ListOfFilledKhwtIndices.Any(ind => ind == RowInd))
                    {
                        var itemToRemove = ListOfFilledKhwtIndices.Single(r => r == RowInd);
                        ListOfFilledKhwtIndices.Remove(itemToRemove);
                    }

                    if (FieldUnitsFlag)
                    { ListOfRowInfo[RowInd].Kh_wt = DoubleValue * WellKh.FactorToConvert_mdft_To_m3; }
                    else
                    { ListOfRowInfo[RowInd].Kh_wt = DoubleValue * WellKh.FactorToConvert_mdm_To_m3; }

                    WellKhDataGridView.Rows[RowInd].Cells[4].Value = RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits);
                    ListOfFilledKhwtIndices.Add(RowInd);
           
                    return true;  
                }
                else
                {
                    MessageBox.Show("Invalid Input: Please make sure that the input is a positive number and it does not contain any commas");
                    return false;
                }

               
            }
            else
            {
                if (Entry.Equals(String.Empty))
                {
                   
                    try
                    {
                        var itemToRemove = ListOfFilledKhwtIndices.Single(r => r == RowInd);
                        ListOfFilledKhwtIndices.Remove(itemToRemove);
                    }
                    catch
                    { 
                    
                    }
                    ListOfRowInfo[RowInd].Kh_wt = -1;
                   // WellKhDataGridView.Rows[RowInd].Cells[4].Value = RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits);
                    return true;

                }
                else
                {
                    if (ListOfRowInfo[RowInd].Kh_wt > 0)
                    {
                        if (FieldUnitsFlag)
                            WellKhDataGridView.Rows[RowInd].Cells[3].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Kh_wt * (1 / WellKh.FactorToConvert_mdft_To_m3), SignificantDigits));
                        else
                            WellKhDataGridView.Rows[RowInd].Cells[3].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Kh_wt * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits));
                    }
                    else
                    {
                        WellKhDataGridView.Rows[RowInd].Cells[3].Value = String.Empty;
                    }
                    MessageBox.Show("Invalid Input: Please make sure that the input is a positive number and it does not contain any commas");
                    return false;
                }
            }
        }
          else if (ColInd == 4)
          {
              //Editing the ratio column


              //if (FirstTimeEditingRatio)
              //{
              //    if (MessageBox.Show("Do you want the current data to be saved as the Orginal data and start editing ratios?\n If not then input all the well testing data first.",
              //         "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              //    {
              //        // user clicked yes
              //        FirstTimeEditingRatio = false;
              //        ListOfOriginalRowDataInfo.Clear();
              //        foreach (KhTableRowInfoContainer ri in  ListOfRowInfo)
              //        {
              //            ListOfOriginalRowDataInfo.Add(ri.CreateCopy());
              //        }
  
              //        ListOfOriginalFilledKhwtIndices.Clear();
              //        foreach (int ind in ListOfFilledKhwtIndices.ToList())
              //        {
              //            ListOfOriginalFilledKhwtIndices.Add(ind);
              //        }

              //        WellKhDataGridView.Columns[6].ReadOnly = false;

  

              //    }
              //    else
              //    {

              //        WellKhDataGridView.Rows[RowInd].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits));
              //        return false;
              //    }

              //}

              //if (!FirstTimeEditingRatio)
              //{

                  if (Double.TryParse(Entry, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue)) //
                  {
                      if (DoubleValue>0)
                      {
                          ExcludeKhFromTotalRatio(ref WellKhDataGridView, RowInd);

                          ListOfRowInfo[RowInd].Ratio = DoubleValue;

                          if (ListOfFilledKhwtIndices.Any(ind => ind == RowInd))
                          {
                              ListOfFilledKhwtIndices.Add(RowInd);
                          }
                         


                          WellKhDataGridView.Rows[RowInd].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits));
                          return true; 
                      }
                      else
                      {
                          WellKhDataGridView.Rows[RowInd].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits));
                          MessageBox.Show("Invalid Input: Please make sure that the input is a positive number and it does not contain any commas");
                          return false;
                      }
                     
                  }
                  else
                  {
                      WellKhDataGridView.Rows[RowInd].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits));
                      MessageBox.Show("Invalid Input: Please make sure that the input is a positive number and it does not contain any commas");
                      return false;
                  }

              //}
              //else
              //{ return false; }
          }
          else
          {
              return false;
          }

        }

        //private bool UpdateSpecificRowInfoObject(int RowInd, int ColInd, string Entry)
        //{
        //    double DoubleValue;
        //    OldIndex = RowInd;

        //    if (ColInd == 3)
        //    {
        //        if (Double.TryParse(Entry, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue)) //
        //        {
        //            if (DoubleValue > 0)
        //            {
        //                if (ListOfFilledKhwtIndices.Any(ind => ind == RowInd))
        //                {
        //                    var itemToRemove = ListOfFilledKhwtIndices.Single(r => r == RowInd);
        //                    ListOfFilledKhwtIndices.Remove(itemToRemove);
        //                }

        //                if (FieldUnitsFlag)
        //                { ListOfRowInfo[RowInd].Kh_wt = DoubleValue * WellKh.FactorToConvert_mdft_To_m3; }
        //                else
        //                { ListOfRowInfo[RowInd].Kh_wt = DoubleValue * WellKh.FactorToConvert_mdm_To_m3; }

        //                WellKhDataGridView.Rows[RowInd].Cells[4].Value = RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits);
        //                ListOfFilledKhwtIndices.Add(RowInd);

        //                return true;
        //            }
        //            else
        //            {
        //                MessageBox.Show("Invalid Input: Please make sure that the input is a positive number and it does not contain any commas");
        //                return false;
        //            }


        //        }
        //        else
        //        {
        //            if (Entry.Equals(String.Empty))
        //            {

        //                try
        //                {
        //                    var itemToRemove = ListOfFilledKhwtIndices.Single(r => r == RowInd);
        //                    ListOfFilledKhwtIndices.Remove(itemToRemove);
        //                }
        //                catch
        //                {

        //                }
        //                ListOfRowInfo[RowInd].Kh_wt = -1;
        //                // WellKhDataGridView.Rows[RowInd].Cells[4].Value = RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits);
        //                return true;

        //            }
        //            else
        //            {
        //                if (ListOfRowInfo[RowInd].Kh_wt > 0)
        //                {
        //                    if (FieldUnitsFlag)
        //                        WellKhDataGridView.Rows[RowInd].Cells[3].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Kh_wt * (1 / WellKh.FactorToConvert_mdft_To_m3), SignificantDigits));
        //                    else
        //                        WellKhDataGridView.Rows[RowInd].Cells[3].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Kh_wt * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits));
        //                }
        //                else
        //                {
        //                    WellKhDataGridView.Rows[RowInd].Cells[3].Value = String.Empty;
        //                }
        //                MessageBox.Show("Invalid Input: Please make sure that the input is a positive number and it does not contain any commas");
        //                return false;
        //            }
        //        }
        //    }
        //    else if (ColInd == 4)
        //    {
        //        //Editing the ratio column


        //        if (FirstTimeEditingRatio)
        //        {
        //            if (MessageBox.Show("Do you want the current data to be saved as the Orginal data and start editing ratios?\n If not then input all the well testing data first.",
        //                 "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //            {
        //                // user clicked yes
        //                FirstTimeEditingRatio = false;
        //                ListOfOriginalRowDataInfo.Clear();
        //                foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
        //                {
        //                    ListOfOriginalRowDataInfo.Add(ri.CreateCopy());
        //                }

        //                ListOfOriginalFilledKhwtIndices.Clear();
        //                foreach (int ind in ListOfFilledKhwtIndices.ToList())
        //                {
        //                    ListOfOriginalFilledKhwtIndices.Add(ind);
        //                }

        //                WellKhDataGridView.Columns[6].ReadOnly = false;



        //            }
        //            else
        //            {

        //                WellKhDataGridView.Rows[RowInd].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits));
        //                return false;
        //            }

        //        }

        //        if (!FirstTimeEditingRatio)
        //        {

        //            if (Double.TryParse(Entry, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue)) //
        //            {
        //                if (DoubleValue > 0)
        //                {
        //                    ExcludeKhFromTotalRatio(ref WellKhDataGridView, RowInd);

        //                    ListOfRowInfo[RowInd].Ratio = DoubleValue;

        //                    if (ListOfFilledKhwtIndices.Any(ind => ind == RowInd))
        //                    {
        //                        ListOfFilledKhwtIndices.Add(RowInd);
        //                    }



        //                    WellKhDataGridView.Rows[RowInd].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits));
        //                    return true;
        //                }
        //                else
        //                {
        //                    WellKhDataGridView.Rows[RowInd].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits));
        //                    MessageBox.Show("Invalid Input: Please make sure that the input is a positive number and it does not contain any commas");
        //                    return false;
        //                }

        //            }
        //            else
        //            {
        //                WellKhDataGridView.Rows[RowInd].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowInd].Ratio, SignificantDigits));
        //                MessageBox.Show("Invalid Input: Please make sure that the input is a positive number and it does not contain any commas");
        //                return false;
        //            }

        //        }
        //        else
        //        { return false; }
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        private void ModifiedKhUI_Load(object sender, EventArgs e)
        {  
            Project proj = PetrelProject.PrimaryProject;
            WellRoot wr = WellRoot.Get(proj);

            BoreholeCollection BhCollection = wr.BoreholeCollection;
            ListOfBoreholes = PermMatching.GetAllBoreholesInProject(BhCollection);

            UpdateRowsInDataGridWithNewWells(ListOfBoreholes); 

            WellKhDataGridView.AllowUserToAddRows = false;
           
            IUnitServiceSettings uss;
            uss = CoreSystem.GetService<IUnitServiceSettings>();
            IUnitSystem ui = uss.CurrentUISystem;
            //IUnitCatalog catalog = uss.CurrentCatalog;
            //IUnitMeasurement measurement = catalog.GetUnitMeasurement("Length");

            //Template permeabilityT = PetrelProject.WellKnownTemplates.PetrophysicalGroup.Permeability;
            ListOfFilledKhwtIndices.CollectionChanged += FilledKhwtIndices_CollectionChanged;
           
            
            if (ui.Name == "Petrel ProjField_0")
            { FieldUnitsFlag = true;}
            else if (ui.Name == "Petrel ProjMetric_0")
            {
                FieldUnitsFlag = false;
                foreach (DataGridViewColumn col in WellKhDataGridView.Columns)
                {
                    if (col.Name.Equals("KhSim"))
                    {
                        col.HeaderText = "Kh(sim) " + "md-m";
                    }
                    else if (col.Name.Equals("Khwt"))
                    {
                        col.HeaderText = "Kh(wt) " + "md-m";
                    }
                }
            }
            else 
            {
                FieldUnitsFlag = true;
                MessageBox.Show("The project units were not identified. The units for the flow capacity values shown here will be displayed in Field units (md-ft)");
            }
            HistogramChart1.Visible = false;
            HistogramChart2.Visible = false;
            UpdateUIWithSaveableArgs();
        }

        private void FilledKhwtIndices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        { bool Remove = false;
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                SumOfRatios = SumOfRatios + ListOfRowInfo[ListOfFilledKhwtIndices[e.NewStartingIndex]].Ratio; 
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                //SumOfRatios = SumOfRatios - ListOfRowInfo[OldIndex].Ratio;
                SumOfRatios = SumOfRatios - ListOfRowInfo[(int)e.OldItems[0] ].Ratio;
                Remove = true;
               // SumOfRatios = SumOfRatios - System.Convert.ToDouble(WellKhDataGridView.Rows[e.OldStartingIndex].Cells[4].Value);
            }

            for (int i = 0; i <WellKhDataGridView.Rows.Count; i++)
            {

                if (String.IsNullOrEmpty(WellKhDataGridView.Rows[i].Cells[3].Value as String) || (Remove && i == (int)e.OldItems[0]))
                {
                    if (!KeepMissingRatio1.Checked && ListOfFilledKhwtIndices.Count() > 0)
                    {
                        ListOfRowInfo[i].Ratio = SumOfRatios / ListOfFilledKhwtIndices.Count();
                    }
                    else
                    {
                        ListOfRowInfo[i].Ratio = 1.0;  
                    }
                  WellKhDataGridView.Rows[i].Cells[4].Value = RoundingClass.RoundToSignificantDigits(ListOfRowInfo[i].Ratio, SignificantDigits);
                }
            }


        }

        private void UpdateRowsInDataGridWithNewWells(List<Borehole> ListOfBoreholes) 
        {
            ListOfBoreholes = SortListOfBoreholesByName(ListOfBoreholes);
            WellKhDataGridView.Rows.Clear();
            int counter = 0;

            foreach (Borehole bh in ListOfBoreholes)
            {

                if (counter < WellKhDataGridView.RowCount - 1)
                {
                    WellKhDataGridView.Rows[counter].Cells[0].Value = bh.Name;
                    //PetrelLogger.InfoOutputWindow(bh.Name);
                    WellKhDataGridView.Rows[counter].Cells[0].Tag = bh;
                }
                else
                {
                    Button B1 = new Button();
                    WellKhDataGridView.Rows.Add(bh.Name, "", "", "", "", false,B1,true);
                    WellKhDataGridView.Rows[counter].Cells[0].Tag = bh;
                }
                counter++;
            }
        }

        private void UpdateRowObjectsWithNewWells(List<Borehole> ListOfWells)
        {
            ListOfRowInfo.Clear();
            List<String> ListOfIntersectedZonesNames = new List<string>();
            ListOfWells = SortListOfBoreholesByName(ListOfWells);

            foreach (Borehole bh in ListOfWells)
            {
                WellKhObj.Well = bh;


                if (WellKhObj.SetListOfNamesOfIntersectedZones(true))
                {
                    ListOfIntersectedZonesNames = WellKhObj.ListOfNamesOfIntersectedZones.Distinct().ToList();

                    WellKhObj.VerticalContinuity(ListOfIntersectedZonesNames);
                    #region Getting all the names of penetrated zones into one list
                    if (args.ListOfPenetratedZoneNames != null)
                    {
                        foreach (String name in ListOfIntersectedZonesNames)
                        {
                            if (!args.ListOfPenetratedZoneNames.Contains(name))
                            {
                                args.ListOfPenetratedZoneNames.Add(name);
                            }
                        }
                    }
                    else
                    {
                        args.ListOfPenetratedZoneNames.AddRange(ListOfIntersectedZonesNames);
                    }
                    #endregion

                    #region Creating new KhTableRowInfoContainer objects to contain all relevant information about zones, well,cells and kh
                    Dictionary<int, List<CellData>> Dict = WellKhObj.GetKhDictionaryOfSelectedGridCells(Depth_or_Zones, PerforatedZonesOnly, true);

                   // int counter = 0;
                    foreach (int ind in Dict.Keys)
                    {
                      //  Droid ArgsDroid = new Droid(CONNECTModifiedKhDataSourceFactory.DataSourceId, "ModifiedKh.KhTableRowInfoContainer_" + System.Convert.ToString(counter));
                        KhTableRowInfoContainer RowInfoObj = new KhTableRowInfoContainer(Dict, ind, ListOfZones[ind]);
                        ListOfRowInfo.Add(RowInfoObj);
                       // counter = counter + 1;

                    }
                    #endregion
                }

                else
                {
                    MessageBox.Show("Please verify that a Permeability property and a Zone Index property have been dropped.");
                }


            }

        }

        private List<Borehole> SortListOfBoreholesByName(List<Borehole> ListOfBoreholes)
        {
            List<Borehole> SortedList = ListOfBoreholes.OrderBy(o => o.Name).ToList(); ;
            return SortedList;
        }

        private void WellKhDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0 && ListOfOriginalRowDataInfo.Count > 0)
            {
                if (e.ColumnIndex==6 && ListOfRowInfo[e.RowIndex].Include)
                {
                    if ((bool)senderGrid.Rows[e.RowIndex].Cells[5].Value)
                    {

                        ExcludeKhFromTotalRatio(ref WellKhDataGridView, e.RowIndex);
                      
                        ListOfRowInfo[e.RowIndex].Global = false;
                        WellKhDataGridView.CellValueChanged -= WellKhDataGridView_CellValueChanged;
                        WellKhDataGridView.Rows[e.RowIndex].Cells[5].Value = false;
                        WellKhDataGridView.CellValueChanged += WellKhDataGridView_CellValueChanged;
                        
                        
                    }

                    if (ListOfFilledKhwtIndices.Any(ind => ind == e.RowIndex))
                    {
                        var itemToRemove = ListOfFilledKhwtIndices.Single(r => r == e.RowIndex);
                        ListOfFilledKhwtIndices.Remove(itemToRemove);
                    }

                    ListOfRowInfo[e.RowIndex] = ListOfOriginalRowDataInfo[e.RowIndex].CreateCopy();
                    ListOfFilledKhwtIndices.Add(e.RowIndex);

                    WellKhDataGridView.Rows[e.RowIndex].Cells[4].Value = ListOfRowInfo[e.RowIndex].Ratio;

                    if (FieldUnitsFlag)
                    { WellKhDataGridView.Rows[e.RowIndex].Cells[3].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[e.RowIndex].Kh_wt * (1 / WellKh.FactorToConvert_mdft_To_m3), SignificantDigits));}
                    else
                    { WellKhDataGridView.Rows[e.RowIndex].Cells[3].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[e.RowIndex].Kh_wt * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits)); }
                    

                }
                    

            }
        }

        private void ExcludeKhFromTotalRatio(ref DataGridView senderGrid, int RowIndex)
        {
            double NewRatio = 1.0 / (1.0 /(ListOfRowInfo[RowIndex].Ratio) - ListOfRowInfo[RowIndex].Kh_sim / ListOfRowInfo[RowIndex].Kh_wt);

            for (int i = 0; i < senderGrid.Rows.Count; i++)
            {
                if (ListOfRowInfo[RowIndex].WellName.Equals(ListOfRowInfo[i].WellName) && RowIndex != i && (bool)senderGrid.Rows[i].Cells[5].Value)
                {
                    if (ListOfFilledKhwtIndices.Any(ind => ind == i))
                    {
                        var itemToRemove = ListOfFilledKhwtIndices.Single(r => r == i);
                        ListOfFilledKhwtIndices.Remove(itemToRemove);
                    }
                    ListOfRowInfo[i].Ratio = NewRatio;
                    senderGrid.Rows[i].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(NewRatio, SignificantDigits));
                    ListOfFilledKhwtIndices.Add(i);
                }

            }

            //Making sure that the previous ratio is removed from the List of Filled Khwt so that the calculation of the total average ratio of the datagridview is done without that number
            //If the new ratio (kh_wt/kh_sim) needs to be taken into account for the calculation of the new average then you need to use ListofFilledKhwtIndices.Add() outside of the method.
            if (ListOfFilledKhwtIndices.Any(ind => ind == RowIndex))
            {
                var itemToRemove = ListOfFilledKhwtIndices.Single(r => r == RowIndex);
                ListOfFilledKhwtIndices.Remove(itemToRemove);
            }

            //Updating DatagridView and ListOfRowInfo
            senderGrid.CellValueChanged -= WellKhDataGridView_CellValueChanged;
            senderGrid.Rows[RowIndex].Cells[5].Value = false;
            ListOfRowInfo[RowIndex].Global = false;
            senderGrid.CellValueChanged += WellKhDataGridView_CellValueChanged;
            ListOfRowInfo[RowIndex].Ratio = ListOfRowInfo[RowIndex].Kh_wt / ListOfRowInfo[RowIndex].Kh_sim; //New ratio (kh_wt/kh_sim)
            senderGrid.Rows[RowIndex].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[RowIndex].Ratio, SignificantDigits));

        }


        private void WellKhDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                WellKhDataGridView.EndEdit();

            }
        }

        private void SelectedWellsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SelectedWellsCheckBoxUpdateArgs(SelectedWellsCheckBox.Checked);
        }

        private void SelectedWellsCheckBoxUpdateArgs(bool CheckBoxState)
        {
            ListOfBoreholes.Clear();
            if (CheckBoxState == true)
            {
                //ListOfBoreholes = PetrelProject.Inputs.GetSelected<Borehole>().ToList();
                //ListOfBoreholes = PetrelProject.ToggleWindows.GetSelected<Borehole>().ToList();
                ToggleWindow twindow = (ToggleWindow)PetrelProject.ToggleWindows.Active;
                foreach (object item in twindow.VisibleObjects)
                {
                    if (item is Borehole)
                    {
                        ListOfBoreholes.Add((Borehole)item);

                    }
                }
                if (ListOfBoreholes != null)
                {
                    UpdateRowsInDataGridWithNewWells(ListOfBoreholes);
                }
                else
                    MessageBox.Show("Please select the wells in the Input Tab that you would like to use");
            }
            else
            {
                Project proj = PetrelProject.PrimaryProject;
                WellRoot wr = WellRoot.Get(proj);

                BoreholeCollection BhCollection = wr.BoreholeCollection;
                ListOfBoreholes = PermMatching.GetAllBoreholesInProject(BhCollection);
                if (ListOfBoreholes != null)
                {
                    UpdateRowsInDataGridWithNewWells(ListOfBoreholes);
                }
                else
                    MessageBox.Show("Please create some wells in the Input Tab");
            }
        }
        private void CreateHistograms(List<double> ListOfRatios)
        {
            try
            {
                using (ITransaction trans = DataManager.NewTransaction())
                {
                    string NameOfCollection;
                    string NameOfPointSet;

                    string NameOfCollectionOriginal = "Original Ratio(Kh_well_testing/kh_sim)";
                    string NameOfPointSetOriginal = "Histogram of Original Ratios (Kh_well_testing/kh_sim)";


                    //if (!DistributionCheckBox.Checked)
                    //{
                        NameOfCollection = "Ratio(Kh_well_testing/kh_sim)";
                        NameOfPointSet = "Histogram of Modelling Ratios (Kh_well_testing/kh_sim)";

                    //}
                    //else 
                    //{
                    //    NameOfCollection = "Log of Ratio(Kh_well_testing/kh_sim)";
                    //    NameOfPointSet = "Histogram of Modelling Log of Ratio(Kh_well_testing/kh_sim)";
                    //}
                    trans.Lock(PetrelProject.PrimaryProject);
                    Collection col = PetrelProject.PrimaryProject.CreateCollection(NameOfCollection);
                    Collection colOriginal = PetrelProject.PrimaryProject.CreateCollection(NameOfCollectionOriginal);

                    List<Point3> pt3list = new List<Point3>();
                    List<Point3> pt3listOriginal = new List<Point3>();
                    pt3list.Clear(); 
                    pt3listOriginal.Clear();

                    for (int i = 0; i < ListOfRatios.Count; i++)
                    {
                        pt3list.Add(new Point3((double)(i + 1), (double)(i + 1), (double)(ListOfRatios[i])));
                    }

                    int ii = 0;
                    foreach (KhTableRowInfoContainer ri in ListOfOriginalRowDataInfo)
                    {
                         pt3listOriginal.Add( new Point3((double)(ii + 1), (double)(ii + 1), (double)(ri.Ratio)));
                         ii = ii +1;
                    }
                    PointSet ptsset = col.CreatePointSet(NameOfPointSet);
                    PointSet ptssetOriginal = colOriginal.CreatePointSet(NameOfPointSetOriginal);

                    ptsset.Points = new Point3Set(pt3list);
                    ptssetOriginal.Points = new Point3Set(pt3listOriginal);
                    trans.Commit();
                }

                

                double[] arrayOfRatios =  new double[ListOfRatios.Count];
                  for (int i = 0; i < ListOfRatios.Count; i++)
                    {
                        arrayOfRatios[i] = RoundingClass.RoundToSignificantDigits(ListOfRatios[i],4);
                    }
                double[] arrayOfOriginalRatios =  new double [ListOfOriginalRowDataInfo.Count];
                  for (int i = 0; i < ListOfOriginalRowDataInfo.Count; i++)
                    {
                        arrayOfOriginalRatios[i] = RoundingClass.RoundToSignificantDigits(ListOfOriginalRowDataInfo[i].Ratio,4);
                    } 

                 int numberOfBins = 10;
                 try
                 {
                     generateHistogramUI(arrayOfRatios, arrayOfOriginalRatios, numberOfBins);
                 }
                 catch (NullReferenceException)
                 {
                     PetrelLogger.InfoOutputWindow("An error ocurred while trying to plot the histograms. Please use the histogram data created in the Input pane in order to visualize the histograms.");
                 } 
                  //HistogramChart1.Data.DataSource = arrayOfRatios;
                  //HistogramChart1.Data.DataBind();
                  //HistogramChart1.Axis.X.RangeMax = arrayOfRatios.Max();
                  //HistogramChart1.Axis.X.RangeMin = arrayOfRatios.Min();
                  //HistogramChart1.Axis.X.TickmarkInterval = (arrayOfRatios.Max() - arrayOfRatios.Min()) / 10;
                  

            }
            catch (NullReferenceException)
            {
                throw;
            }
        }

        private void generateHistogramUI(double[] arrayOfRatios, double[] arrayOfOriginalRatios, int numberOfBins)
        {
            try
            {
                UltraChart HistogramChart1 = new UltraChart();
                this.HistogramChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.HistogramChart;

                this.HistogramChart1.HistogramChart.ColumnAppearance.StringAxis = false;
                this.HistogramChart1.HistogramChart.ColumnAppearance.ShowInLegend = true;

                this.HistogramChart1.Axis.X.NumericAxisType = Infragistics.UltraChart.Shared.Styles.NumericAxisType.Linear;
                this.HistogramChart1.Axis.X.RangeMin = arrayOfRatios.Min();
                this.HistogramChart1.Axis.X.RangeMax = arrayOfRatios.Max();
                this.HistogramChart1.Axis.X.RangeType = AxisRangeType.Custom;
                this.HistogramChart1.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval;
                //this.ultraChart2.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Percentage;
                this.HistogramChart1.Axis.X.TickmarkInterval = (arrayOfRatios.Max() - arrayOfRatios.Min()) / numberOfBins; ;// Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
                //this.ultraChart2.Axis.X.LogBase = 2.71;
                this.HistogramChart1.HistogramChart.ColumnAppearance.ColumnSpacing = .1;
                //this.ultraChart2.Axis.Y.NumericAxisType = Infragistics.UltraChart.Shared.Styles.NumericAxisType.Logarithmic;
                this.HistogramChart1.Axis.Y2.Visible = false;
               

                this.HistogramChart1.Axis.X.Labels.ItemFormat = AxisItemLabelFormat.Custom;
                this.HistogramChart1.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
                this.HistogramChart1.Axis.X.Labels.SeriesLabels.Visible = false;
                //this.ultraChart2.Axis.X.Labels.ItemFormatString = "Math.Pow(2.71,<DATA_VALUE:00.##>)";
                //this.ultraChart2.Axis.X.Labels.SeriesLabels.Format = Infragistics.UltraChart.Shared.Styles.AxisSeriesLabelFormat.Custom;
                //this.ultraChart2.Axis.X.Labels.SeriesLabels.FormatString = "Math.Pow(<ITEM_LABEL>,2.71)";//<DATA_VALUE:0#>
                this.HistogramChart1.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
                this.HistogramChart1.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
                this.HistogramChart1.Axis.X.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
                this.HistogramChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
                this.HistogramChart1.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
                this.HistogramChart1.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.Black;
                this.HistogramChart1.Axis.X.Labels.SeriesLabels.FormatString = "";
                this.HistogramChart1.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
                this.HistogramChart1.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
                this.HistogramChart1.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
                this.HistogramChart1.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;

                this.HistogramChart1.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;

                this.HistogramChart1.Legend.Visible = true;
                this.HistogramChart1.Legend.BackgroundColor = System.Drawing.Color.Transparent;
                this.HistogramChart1.Legend.BorderColor = System.Drawing.Color.Black;
                this.HistogramChart1.Legend.Margins.Bottom = 50;
                this.HistogramChart1.Legend.Margins.Left = 2;
                this.HistogramChart1.Legend.Margins.Right = 2;
                this.HistogramChart1.Legend.Margins.Top = 2;
                this.HistogramChart1.Legend.Visible = false;

                //new int[] { 1, 2, 4, 5, 6, 7, 8, 9, 9, 9, 12, 13, 14, 15, 16 };


                this.HistogramChart1.Visible = true;
                //this.ultraChart2.Axis.X.ScrollScale.Visible = true;
                //this.ultraChart2.Axis.Y.ScrollScale.Visible = true;
                this.HistogramChart1.HistogramChart.LineAppearance.Visible = false;
                this.HistogramChart1.ColorModel.AlphaLevel = 255;
                this.HistogramChart1.ColorModel.Grayscale = false;
                this.HistogramChart1.Data.DataSource = arrayOfRatios;



                this.HistogramChart1.Data.DataBind();
                this.HistogramChart1.Refresh();

                UltraChart HistogramChart2 = new UltraChart();
                this.HistogramChart2.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.HistogramChart;

                this.HistogramChart2.HistogramChart.ColumnAppearance.StringAxis = false;
                this.HistogramChart2.HistogramChart.ColumnAppearance.ShowInLegend = true;

                this.HistogramChart2.Axis.X.NumericAxisType = Infragistics.UltraChart.Shared.Styles.NumericAxisType.Linear;
                this.HistogramChart2.Axis.X.RangeMin = arrayOfOriginalRatios.Min();
                this.HistogramChart2.Axis.X.RangeMax = arrayOfOriginalRatios.Max();
                this.HistogramChart2.Axis.X.RangeType = AxisRangeType.Custom;
                this.HistogramChart2.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval;
                //this.ultraChart2.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Percentage;
                this.HistogramChart2.Axis.X.TickmarkInterval = (arrayOfOriginalRatios.Max() - arrayOfOriginalRatios.Min()) / numberOfBins;// Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
                //this.ultraChart2.Axis.X.LogBase = 2.71;
                this.HistogramChart2.HistogramChart.ColumnAppearance.ColumnSpacing = .1;
                //this.ultraChart2.Axis.Y.NumericAxisType = Infragistics.UltraChart.Shared.Styles.NumericAxisType.Logarithmic;
                this.HistogramChart2.Axis.Y2.Visible = false;

                this.HistogramChart2.Axis.X.Labels.ItemFormat = AxisItemLabelFormat.Custom;
                this.HistogramChart2.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
                this.HistogramChart2.Axis.X.Labels.SeriesLabels.Visible = false;
                //this.ultraChart2.Axis.X.Labels.ItemFormatString = "Math.Pow(2.71,<DATA_VALUE:00.##>)";
                //this.ultraChart2.Axis.X.Labels.SeriesLabels.Format = Infragistics.UltraChart.Shared.Styles.AxisSeriesLabelFormat.Custom;
                //this.ultraChart2.Axis.X.Labels.SeriesLabels.FormatString = "Math.Pow(<ITEM_LABEL>,2.71)";//<DATA_VALUE:0#>
                this.HistogramChart2.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
                this.HistogramChart2.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
                this.HistogramChart2.Axis.X.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
                this.HistogramChart2.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
                this.HistogramChart2.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
                this.HistogramChart2.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.Black;
                this.HistogramChart2.Axis.X.Labels.SeriesLabels.FormatString = "";
                this.HistogramChart2.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
                this.HistogramChart2.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
                this.HistogramChart2.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
                this.HistogramChart2.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;

                this.HistogramChart2.Axis.X.Labels.Orientation = TextOrientation.VerticalLeftFacing;

                this.HistogramChart2.Legend.Visible = true;
                this.HistogramChart2.Legend.BackgroundColor = System.Drawing.Color.Transparent;
                this.HistogramChart2.Legend.BorderColor = System.Drawing.Color.Black;
                this.HistogramChart2.Legend.Margins.Bottom = 50;
                this.HistogramChart2.Legend.Margins.Left = 2;
                this.HistogramChart2.Legend.Margins.Right = 2;
                this.HistogramChart2.Legend.Margins.Top = 2;
                this.HistogramChart2.Legend.Visible = false;

                //new int[] { 1, 2, 4, 5, 6, 7, 8, 9, 9, 9, 12, 13, 14, 15, 16 };


                this.HistogramChart2.Visible = true;
                //this.ultraChart2.Axis.X.ScrollScale.Visible = true;
                //this.ultraChart2.Axis.Y.ScrollScale.Visible = true;
                this.HistogramChart2.HistogramChart.LineAppearance.Visible = false;
                this.HistogramChart2.ColorModel.AlphaLevel = 255;
                this.HistogramChart2.ColorModel.Grayscale = false;
                this.HistogramChart2.Data.DataSource = arrayOfOriginalRatios;



                this.HistogramChart2.Data.DataBind();
                this.HistogramChart2.Refresh();
              

            }
            catch (Exception e)
            {
                PetrelLogger.InfoBox(e.Message + "\n" + e.StackTrace);
            }
        }

        private void HistogramButton_Click(object sender, EventArgs e)
        {
            SetListOfModellingDataAndDataGridView();
                    ListOfRatios.Clear();
                try
                {
                   
                    foreach (KhTableRowInfoContainer ri in args.ListOfModellingData)
                    {
                        ListOfRatios.Add(ri.Ratio);
                    }

                    CreateHistograms(ListOfRatios);
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Please ensure that the permeability property has been dropped.");
                }
           // }
            //else 
            //{
            //    ListOfLogRatios.Clear();

            //    try
            //    {
            //        foreach (KhTableRowInfoContainer ri in args.ListOfModellingData)
            //        {
            //            ListOfLogRatios.Add(Math.Log(ri.Ratio));
            //        }
            //        CreateHistograms(ListOfLogRatios);
            //    }
            //    catch (NullReferenceException)
            //    {
            //        MessageBox.Show("Please ensure that the permeability property has been dropped.");
            //    }
            //}
        }


        public void SetListOfModellingDataAndDataGridView()
        {

            double mean = 0; double std = 0;
            args.ListOfModellingData.Clear();
            args.ListOfModellingKhwtIndices.Clear();

            if (FirstTimeEditingRatio)
            {

                ListOfOriginalRowDataInfo.Clear();
                foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
                {
                    ListOfOriginalRowDataInfo.Add(ri.CreateCopy());
                    args.ListOfModellingData.Add(ri.CreateCopy());
                }

                ListOfOriginalFilledKhwtIndices.Clear();
                foreach (int ind in ListOfFilledKhwtIndices.ToList())
                {
                    ListOfOriginalFilledKhwtIndices.Add(ind);
                    args.ListOfModellingKhwtIndices.Add(ind);
                }

            }
            else
            {
                if (UseOriginalData.Checked)
                {

                    foreach (KhTableRowInfoContainer ri in ListOfOriginalRowDataInfo)
                     {
                         args.ListOfModellingData.Add(ri.CreateCopy());
                     }


                    foreach (int ind in ListOfOriginalFilledKhwtIndices.ToList())
                      {
                          args.ListOfModellingKhwtIndices.Add(ind);
                      }

                }
                else
                {
                    foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
                    {
                        args.ListOfModellingData.Add(ri.CreateCopy());
                    }

                 
                    foreach (int ind in ListOfFilledKhwtIndices.ToList())
                    {
                        args.ListOfModellingKhwtIndices.Add(ind);
                    }
                }
            }


            if (Truncate2NormalDist.Checked)
            {
                if (!Truncate2NormalDistData(args.ListOfModellingData, args.ListOfModellingKhwtIndices, ref mean, ref std, ref  MaxRatio, ref  MinRatio))
                {
                    MessageBox.Show("The data  could not be truncated by using a normal distribution because no data was input in the Kh well testing column. \n" +
                             "If you do not wish to truncate the data using a normal distribution please uncheck the Truncate to Normal Distribution checkbox.");
                    return;
                }
                TruncateData(ref WellKhDataGridView, ref args.ListOfModellingData, MaxRatio, MinRatio);
            }
            else if (!Truncate2NormalDist.Checked && !UseOriginalData.Checked)
            {
                if (MaxRatio > 0)
                {
                    TruncateData(ref WellKhDataGridView, ref args.ListOfModellingData, MaxRatio, MinRatio);
                }
                else
                {
                    MessageBox.Show("Please input a value bigger than zero for the Maximum Ratio");
                    return;
                }
            }
        }
        //private void Missing2Mean_CheckedChanged(object sender, EventArgs e)
        //{
        //    List<double> ListOfFilledRatios = new List<double>();
        //    List<int> ListOfEmptyKhwtIndices = new List<int>();
            
        //    double average;
        //    int counter = 0;

        //    if (Missing2Mean.Checked == true)
        //    {
        //        try
        //        {
        //            foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
        //            {
        //                if (!System.Convert.ToString(WellKhDataGridView.Rows[counter].Cells[3].Value).Equals(String.Empty))
        //                { ListOfFilledRatios.Add(ri.Ratio); }
        //                else
        //                { ListOfEmptyKhwtIndices.Add(counter); }

        //                counter = counter + 1;
        //            }

        //        }
        //        catch (NullReferenceException)
        //        {
        //            MessageBox.Show("Please ensure that the permeability property has been dropped.");
        //            return;
        //        }

        //        average = ListOfRatios.Sum()/ListOfRatios.Count;

        //        foreach (int index in ListOfEmptyKhwtIndices)
        //        {
        //            ListOfRowInfo[index].Ratio = average;
        //            WellKhDataGridView.Rows[counter].Cells[4].Value = System.Convert.ToString(
        //                             RoundingClass.RoundToSignificantDigits(ListOfRowInfo[index].Ratio, SignificantDigits));
        //        }


        //    }
        //    else 
        //    {
        //        try
        //        {
        //            foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
        //            {
        //                if (System.Convert.ToString(WellKhDataGridView.Rows[counter].Cells[4].Value).Equals(String.Empty))
        //                {
        //                    ri.Ratio = 1.0;
        //                    WellKhDataGridView.Rows[counter].Cells[5].Value =  System.Convert.ToString(ri.Ratio);
        //                }

        //            }
        //        }
        //        catch(NullReferenceException)
        //        {
        //            MessageBox.Show("Please ensure that the permeability property has been dropped.");
        //            return;
        //        }
        //    }


        //}

        private void KeepMissingRatio1_CheckedChanged(object sender, EventArgs e)
        {
            if (ListOfRowInfo.Count>0)
            {
                for (int i = 0; i < WellKhDataGridView.Rows.Count; i++)
                {
                    if (String.IsNullOrEmpty(WellKhDataGridView.Rows[i].Cells[3].Value as String))
                    {
                        if (!KeepMissingRatio1.Checked && ListOfFilledKhwtIndices.Count() > 0)
                        {
                            ListOfRowInfo[i].Ratio = SumOfRatios / ListOfFilledKhwtIndices.Count();
                        }
                        else
                        {
                            ListOfRowInfo[i].Ratio = 1.0;
                        }
                        WellKhDataGridView.Rows[i].Cells[4].Value = RoundingClass.RoundToSignificantDigits(ListOfRowInfo[i].Ratio, SignificantDigits);
                    }
                }   
            }
            

        }

        private void MinimumRatioValue_TextChanged(object sender, EventArgs e)
        {   
            double DoubleValue;

            Truncate2NormalDist.Checked = false;
            UseOriginalData.Checked = false;
          
            if (Double.TryParse(MinimumRatioValue.Text, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue))
	         {
                 if (DoubleValue>=0)
                 {
                     MinRatio = DoubleValue;
                 }
                 else
                 {
                     MinimumRatioValue.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(MinRatio, SignificantDigits));
                     MessageBox.Show("Please make sure that you input a positive number without any commas");
                 }
                 
	         }
                else
            {
                MinimumRatioValue.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(MinRatio, SignificantDigits));
                    MessageBox.Show("Please make sure that you input a positive number without any commas");
               
	         }
                  

        }

        private void MaximumRatioValue_TextChanged(object sender, EventArgs e)
        {

            double DoubleValue;

            Truncate2NormalDist.Checked = false;
            UseOriginalData.Checked = false;

            if (Double.TryParse(MaximumRatioValue.Text, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue))
            {
                if (DoubleValue>=0)
                {
                    MaxRatio = DoubleValue;  
                }
                else
                {
                    MaximumRatioValue.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(MaxRatio,SignificantDigits));
                    MessageBox.Show("Please make sure that you input a positive number without any commas");
                }
                
            }
            else
            {
                MaximumRatioValue.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(MaxRatio, SignificantDigits));
                MessageBox.Show("Please make sure that you input a positive number without any commas");

            }
        }

        private void Truncate2NormalDist_CheckedChanged(object sender, EventArgs e)
        {

            //MinimumRatioValue.Text = String.Empty;
            //MaximumRatioValue.Text = String.Empty;

            if (Truncate2NormalDist.Checked && UseOriginalData.Checked)
            {
                UseOriginalData.Checked = false;
            }

        }


        private bool Truncate2NormalDistData(List<KhTableRowInfoContainer> ListOfRowInfoData, List<int> ListOfIndices, ref double mean,
            ref double std,ref double MaxVal, ref double MinVal)
        {

            //Calculate the mean and std
            if (ListOfIndices.Count>0)
            {
                mean = 0.0;
                std = 0.0;

                foreach (int i in ListOfIndices)
                {
                    mean = mean + ListOfRowInfoData[i].Ratio;
                }

                mean = mean / ListOfIndices.Count;

                foreach (int i in ListOfIndices)
                {
                    std = std + Math.Pow(ListOfRowInfoData[i].Ratio - mean, 2);
                }

                std = Math.Sqrt(std / ListOfIndices.Count);

                MaxVal = mean + 3 * std;
                MinVal = mean - 3 * std;
               

                return true;
            }
            else
            {
                return false;
            }

        }

        private void TruncateData(ref DataGridView dgv,ref List<KhTableRowInfoContainer> ListOfRowInfoData, double MaxVal, double MinVal)
        {
           int counter = 0;
           String MaxValStr = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(MaxVal,SignificantDigits));
           String MinValStr = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(MinVal, SignificantDigits));

            foreach (KhTableRowInfoContainer ric in ListOfRowInfoData)
            {
                if (ric.Ratio > MaxVal)
                {
                    ric.Ratio = MaxVal;
                 //   dgv.Rows[counter].Cells[4].Value = MaxValStr;
                }
                else if (ric.Ratio < MinVal)
                {
                    ric.Ratio = MinVal;
                //    dgv.Rows[counter].Cells[4].Value = MinValStr;
                }
                counter = counter +1;
            }

        }

        private void UseOriginalData_CheckedChanged(object sender, EventArgs e)
        {
            if (Truncate2NormalDist.Checked && UseOriginalData.Checked)
            {
                Truncate2NormalDist.Checked = false;
            }
        }

        private double CalculateDefaultRange(Grid grid) 
        { double Range;
            //Get the lenght of the diagonal in top face of the box and then half that lenght.
           Range = Math.Sqrt(Math.Pow(grid.BoundingBox.Length, 2) + Math.Pow(grid.BoundingBox.Width, 2)) / 2.0;
           return Range;

        }

        private void MajorRangeTextBox_TextChanged(object sender, EventArgs e)
        {
            double DoubleValue;

            if (Double.TryParse(MajorRangeTextBox.Text, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue))
            {
                if (DoubleValue>0)
                {
                    args.VarArg.MajorRange = DoubleValue;
                    MajorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MajorRange, SignificantDigits));
                    UpdateSillTextBox();
                }
                else
                {
                    MajorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MajorRange, SignificantDigits));
                    MessageBox.Show("Please make sure that you input a positive number without any commas");
                }
                
            }
            else
            {
                MajorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MajorRange,SignificantDigits));
                MessageBox.Show("Please make sure that you input a positive number without any commas");

            }
        }

        private void MinorRangeTextBox_TextChanged(object sender, EventArgs e)
        {
            double DoubleValue;

            if (Double.TryParse(MinorRangeTextBox.Text, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue))
            {
                if (DoubleValue>0)
                {
                    args.VarArg.MinorRange = DoubleValue;
                    MinorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MinorRange, SignificantDigits));
                     UpdateSillTextBox();
                }
                else
                {
                    MinorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MinorRange, SignificantDigits));
                    MessageBox.Show("Please make sure that you input a positive number without any commas");

                }
                
            }
            else
            {
                MinorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MinorRange, SignificantDigits));
                MessageBox.Show("Please make sure that you input a positive number without any commas");

            }
        }

        private void NuggetTextBox_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void VariogramTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (VariogramTypeComboBox.SelectedIndex ==0 )
            {
                args.VarArg.ModelVariogramType = ModelVariogramType.Spherical;
            }
            else if (VariogramTypeComboBox.SelectedIndex ==1)
            {
                args.VarArg.ModelVariogramType = ModelVariogramType.Gaussian;
            }
            else if (VariogramTypeComboBox.SelectedIndex == 2)
	        {
                args.VarArg.ModelVariogramType = ModelVariogramType.Exponential;
	        }
            UpdateSillTextBox();
        }
      
        private void KrigingAlgComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (KrigingAlgComboBox.SelectedIndex == 0)
            {
                args.KrigType = Slb.Ocean.Petrel.PropertyModeling.KrigingType.Ordinary;
            }
            else 
            {
                args.KrigType = Slb.Ocean.Petrel.PropertyModeling.KrigingType.Simple;
            }

            UpdateSillTextBox();
        }

        private void MajorDirectionTextBox_Leave(object sender, EventArgs e)
        {
            double DoubleValue;
            double CosValue;
            double SinValue;
            double Alpha = 0;

            if (Double.TryParse(MajorDirectionTextBox.Text, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue))
            {
                CosValue = Math.Cos((Math.PI / 180) * DoubleValue);
                SinValue = Math.Sin((Math.PI / 180) * DoubleValue);

                if (CosValue >= 0 && SinValue >= 0)
                {
                    Alpha = Math.Acos(CosValue)*(180/Math.PI);
                }
                else if (CosValue <= 0 && SinValue > 0)
                {
                    Alpha = -180 + Math.Acos(CosValue) * (180 / Math.PI);
                }
                else if (CosValue < 0 && SinValue <= 0)
                {
                    Alpha = 180 - Math.Acos(CosValue) * (180 / Math.PI);
                }
                else if (CosValue >= 0 && SinValue < 0)
                {
                    Alpha = -Math.Acos(CosValue) * (180 / Math.PI);
                }

                args.VarArg.MajorDirection = Angle.CreateFromCompassAngle(Alpha, false);
                MajorDirectionTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(System.Convert.ToDouble(args.VarArg.MajorDirection.CompassDegrees), SignificantDigits));
                UpdateSillTextBox();
            }
            else
            {
                MajorDirectionTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(System.Convert.ToDouble(args.VarArg.MajorDirection.CompassDegrees), SignificantDigits));
                MessageBox.Show("Please make sure that you input a number without any commas");

            }
        }

        private void UpdateSillTextBox() 
        {
            SillTextBox.ReadOnly = false;
            SillTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.Sill, SignificantDigits));
            SillTextBox.ReadOnly = true;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Apply_Click(sender, e);
            if (args.Successful)
            {
                 this.FindForm().Close();
            }

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();      
        }

        private void UpdateSaveableArgs()
        {
            SaveArgs.Truncate2NormalDist = Truncate2NormalDist.Checked;
           
            if (args.PermeabilityFromModel!=null)
            {
                SaveArgs.PermDroid = args.PermeabilityFromModel.Droid; 
            }

            if (args.OneLayerPerZoneGrid!= null)
            {
                SaveArgs.GridDroid = args.OneLayerPerZoneGrid.Droid;
            }
                     
            SaveArgs.SelectedWellsCheck = SelectedWellsCheckBox.Checked;
            SaveArgs.UseOriginalData = UseOriginalData.Checked;
            SaveArgs.FirstTimeEditingRatio = FirstTimeEditingRatio;
            if (ListOfRowInfo.Count!=0)
            {
                SaveArgs.ListOfRowInfo = ListOfRowInfo;
               
            }
            if (ListOfOriginalRowDataInfo.Count != 0)
            {
                SaveArgs.ListOfOriginalData = ListOfOriginalRowDataInfo;
            }
            if (ListOfFilledKhwtIndices.Count != 0)
            {
                SaveArgs.ListOfKhwtIndices = ListOfFilledKhwtIndices;
            }
            if (ListOfOriginalFilledKhwtIndices.Count != 0)
            {
                SaveArgs.ListOfOriginalKhwtIndices = ListOfOriginalFilledKhwtIndices;
            }
            
            SaveArgs.MaxRatio = MaxRatio;
            SaveArgs.MinRatio = MinRatio;
            SaveArgs.KeepMissingAs1 = KeepMissingRatio1.Checked;
            SaveArgs.MajorRange = args.VarArg.MajorRange;
            SaveArgs.MinorRange = args.VarArg.MinorRange;
            SaveArgs.Nugget = args.VarArg.Nugget;
            SaveArgs.VariogramTypeIndex = VariogramTypeComboBox.SelectedIndex;
            SaveArgs.KrigingAlgIndex = KrigingAlgComboBox.SelectedIndex;
            SaveArgs.MajorDirection = Angle.CreateFromCompassAngle(args.VarArg.MajorDirection.CompassDegrees, false);
            //SaveArgs.ModelDataGridView = WellKhDataGridView;
            
        }

        private void UpdateUIWithSaveableArgs() 
        {
            SaveArgs = workstep.SaveableSettings;

            if (SaveArgs.SelectedWellsCheck)
            {
                SelectedWellsCheckBox.Checked = SaveArgs.SelectedWellsCheck;
            }

            //Perm Droid
            if (SaveArgs.PermDroid != null)
            {
                WellKhObj.Permeability = PropertyDroidDrop(SaveArgs.PermDroid, PermeabilityPresentationBox);
                if (WellKhObj.Permeability !=null )
                {
                    UpdateArgsWithNewPerm();
                }
                else
                {
                    PermeabilityPresentationBox.Text = "Permeability Property Deleted";
                }
            }

            if (SaveArgs.GridDroid != null)
            {

                    this.args.OneLayerPerZoneGrid = DataManager.Resolve(SaveArgs.GridDroid) as Grid;
                    if (this.args.OneLayerPerZoneGrid != null)
                    {
                        OneLayerGridPresentationBox.Text = this.args.OneLayerPerZoneGrid.Name;
                    }

            }

            MaxRatio = SaveArgs.MaxRatio;

            MinRatio = SaveArgs.MinRatio;

            Truncate2NormalDist.Checked = SaveArgs.Truncate2NormalDist;
            UseOriginalData.Checked = SaveArgs.UseOriginalData;
            FirstTimeEditingRatio = SaveArgs.FirstTimeEditingRatio;
            KeepMissingRatio1.CheckedChanged -= KeepMissingRatio1_CheckedChanged;
            KeepMissingRatio1.Checked = SaveArgs.KeepMissingAs1;
            KeepMissingRatio1.CheckedChanged += KeepMissingRatio1_CheckedChanged;


            if (!UseOriginalData.Checked && !Truncate2NormalDist.Checked)
            {
                MaximumRatioValue.Text= System.Convert.ToString(RoundingClass.RoundToSignificantDigits(MaxRatio, SignificantDigits));
                MinimumRatioValue.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(MinRatio, SignificantDigits));
            }

            if (SaveArgs.ListOfRowInfo.Count > 0)
            {
                ListOfRowInfo.Clear();
                foreach (KhTableRowInfoContainer ri in SaveArgs.ListOfRowInfo)
                {
                    ListOfRowInfo.Add(ri.CreateCopy());
                }

                ListOfOriginalRowDataInfo.Clear();
                foreach (KhTableRowInfoContainer ri in SaveArgs.ListOfOriginalData)
                {
                    ListOfOriginalRowDataInfo.Add(ri.CreateCopy());
                }

               ListOfOriginalFilledKhwtIndices = SaveArgs.ListOfOriginalKhwtIndices;
                ListOfFilledKhwtIndices = SaveArgs.ListOfKhwtIndices;
                UpdateDataGridViewWithListOfRowInfoObj(ListOfRowInfo, ref WellKhDataGridView);
    
            }
            if (SaveArgs.MajorRange>0.0)
            {
                args.VarArg.MajorRange = SaveArgs.MajorRange;
                MajorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MajorRange, SignificantDigits)); 
            }

            if (SaveArgs.MinorRange>0.0)
            {
                args.VarArg.MinorRange = SaveArgs.MinorRange;
                MinorRangeTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.MinorRange, SignificantDigits));   
            }
            
            args.VarArg.Nugget = SaveArgs.Nugget;
            NuggetTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.Nugget, SignificantDigits));
            if (SaveArgs.MajorDirection!=null)
            {
                args.VarArg.MajorDirection = Angle.CreateFromCompassAngle(SaveArgs.MajorDirection.CompassDegrees, false);
                MajorDirectionTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(System.Convert.ToDouble(args.VarArg.MajorDirection.CompassDegrees), SignificantDigits));
            }

            if (SaveArgs.KrigingAlgIndex >=0)
            {
               KrigingAlgComboBox.SelectedIndexChanged -= KrigingAlgComboBox_SelectedIndexChanged;
               KrigingAlgComboBox.SelectedIndex = SaveArgs.KrigingAlgIndex;
               KrigingAlgComboBox.SelectedIndexChanged += KrigingAlgComboBox_SelectedIndexChanged;

                  if (KrigingAlgComboBox.SelectedIndex == 0)
                  {
                     args.KrigType = Slb.Ocean.Petrel.PropertyModeling.KrigingType.Ordinary;
                  }
                  else 
                 {
                     args.KrigType = Slb.Ocean.Petrel.PropertyModeling.KrigingType.Simple;
                 }
            }

            if (SaveArgs.VariogramTypeIndex >= 0)
            {
                VariogramTypeComboBox.SelectedIndexChanged -= VariogramTypeComboBox_SelectedIndexChanged;
                VariogramTypeComboBox.SelectedIndex = SaveArgs.VariogramTypeIndex;
                VariogramTypeComboBox.SelectedIndexChanged += VariogramTypeComboBox_SelectedIndexChanged;

                if (VariogramTypeComboBox.SelectedIndex == 0)
                {
                    args.VarArg.ModelVariogramType = ModelVariogramType.Spherical;
                }
                else if (VariogramTypeComboBox.SelectedIndex == 1)
                {
                    args.VarArg.ModelVariogramType = ModelVariogramType.Gaussian;
                }
                else if (VariogramTypeComboBox.SelectedIndex == 2)
                {
                    args.VarArg.ModelVariogramType = ModelVariogramType.Exponential;
                }
            }
            UpdateSillTextBox();
        }

        private bool TestUserInput()
        {
            return true;
        }

        private Property PropertyDroidDrop(Droid droidOfProperty, PresentationBox presboxToPutPropertyIn)
        {
            Property prop = DataManager.Resolve(droidOfProperty) as Property;
            if (prop!=null)
            {
                presboxToPutPropertyIn.Text = prop.Name;
            }
            return prop;
        }

        private void UpdateDataGridViewWithListOfRowInfoObj(List<KhTableRowInfoContainer> ListOfRowInfo, ref DataGridView dgv)
        {
            if (ListOfRowInfo.Count == dgv.Rows.Count)
            {
                for (int col = 3; col <= 5; col++)
                {
                    switch (col)
                    {
                        case 3:
                            dgv.CellEndEdit -= WellKhDataGridView_CellEndEdit;
                            for (int i = 0; i < dgv.Rows.Count; i++)
                            {
                                if (ListOfRowInfo[i].Kh_wt>=0)
                                {
                                    if (FieldUnitsFlag)
                                        dgv.Rows[i].Cells[3].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[i].Kh_wt * (1 / WellKh.FactorToConvert_mdft_To_m3), SignificantDigits));
                                    else
                                        dgv.Rows[i].Cells[3].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[i].Kh_wt * (1 / WellKh.FactorToConvert_mdm_To_m3), SignificantDigits));  
                                }
                                
                            }
                            dgv.CellEndEdit += WellKhDataGridView_CellEndEdit;
                            break;

                        case 4:
                            dgv.CellEndEdit -= WellKhDataGridView_CellEndEdit;
                            for (int i = 0; i < dgv.Rows.Count; i++)
                            {
                                if (ListOfRowInfo[i].Ratio>=0)
                                {
                                    dgv.Rows[i].Cells[4].Value = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(ListOfRowInfo[i].Ratio, SignificantDigits));
                                }
                                   
                                
                            }
                            dgv.CellEndEdit += WellKhDataGridView_CellEndEdit;
                            break;

                        case 5:
                            dgv.CellValueChanged -= WellKhDataGridView_CellValueChanged;
                            for (int i = 0; i < dgv.Rows.Count; i++)
                            {
                               dgv.Rows[i].Cells[5].Value  = ListOfRowInfo[i].Global;
                               if (ListOfRowInfo[i].Global)
                               {
                                   dgv.Rows[i].ReadOnly = true;
                                   dgv.Rows[i].Cells[5].ReadOnly = false;
                               }
                            }
                            dgv.CellValueChanged += WellKhDataGridView_CellValueChanged;
                            break;

                    }


                }  
            }
            else
            {
                MessageBox.Show("There was a problem loading the saved data into the Table. \n" +
                    "The number of rows of the current Table does not match the number of rows of the previously saved Table.");
            }

        }

        private void NuggetTextBox_Leave(object sender, EventArgs e)
        {
            double DoubleValue;

            if (Double.TryParse(NuggetTextBox.Text, System.Globalization.NumberStyles.Float, new CultureInfo("en-US"), out DoubleValue))
            {
                if (DoubleValue <= 1 && DoubleValue >= 0)
                {
                    args.VarArg.Nugget = DoubleValue;
                    NuggetTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.Nugget, SignificantDigits));
                    UpdateSillTextBox();
                }
                else
                {
                    NuggetTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.Nugget, SignificantDigits));
                    MessageBox.Show("The Nugget can not have a value bigger than 1.");
                }

            }
            else
            {
                NuggetTextBox.Text = System.Convert.ToString(RoundingClass.RoundToSignificantDigits(args.VarArg.Nugget, SignificantDigits));
                MessageBox.Show("Please make sure that you input a positive number without any commas");

            }
        }



         private void LoadClipBoardData(ref DataGridView senderGrid)
        {
            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {  
                int rowOfInterest = senderGrid.CurrentCell.RowIndex;
                int leftMostColumn = senderGrid.CurrentCell.ColumnIndex;

                string[] selectedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");

                if (selectedRows == null || selectedRows.Length == 0)
                    return;

                if (selectedRows.Length > (senderGrid.Rows.Count - (rowOfInterest - 1)))
                {
                    MessageBox.Show("The number of rows of the data in the clipboard is bigger than the available amount of rows. \n" +
                        "Please make sure that the selected row in the table is correct.");
                    return;
                }
                if (leftMostColumn!= 3 && leftMostColumn!= 4 )
                {
                    MessageBox.Show("The selected cell does not belong to the Kh (wt) or Ratio column. \n Please make sure that the selected column is correct");
                    return;
                    
                }
                else if (leftMostColumn== 3)
                {
                    try
                    {
                        foreach (string row in selectedRows)
                        {
                            string[] cols = Regex.Split(row, "\t");

                            if (cols.Length > 2)
                            {
                                MessageBox.Show("The number of columns that you are trying to paste is bigger than 2. \n Please note that you can only paste in the Kh(wt) and Ratio columns.");
                                return;
                            }
                        }

                        foreach (string row in selectedRows)
                        {
                            if (rowOfInterest >= senderGrid.Rows.Count)
                                break;

                            try
                            {
                                string[] data = Regex.Split(row, "\t");
                                int col = senderGrid.CurrentCell.ColumnIndex;

                                foreach (string ob in data)
                                {
                                    if (col >= senderGrid.Columns.Count)
                                        break;
                                    if (ob != null)
                                    { //senderGrid[col, rowOfInterest].Value = Convert.ChangeType(ob, senderGrid[col, rowOfInterest].ValueType); 
                                        senderGrid[col, rowOfInterest].Value = ob;
                                        UpdateSpecificRowInfoObject(rowOfInterest, col, ob);
                                    }
                                    col++;
                                }
                            }
                            catch (Exception e)
                            {
                                //do something here 
                            }
                            rowOfInterest++;
                        }
                    }
                    catch (Exception e)
                    {                       
                    }
                   
                }
                else if (leftMostColumn == 4)
                {
                    try
                    {
                        foreach (string row in selectedRows)
                        {
                            string[] cols = Regex.Split(row, "\t");

                            if (cols.Length > 1)
                            {
                                MessageBox.Show("The number of columns that you are trying to paste is bigger than 1. \n Please note that you can only paste in the Kh(wt) and Ratio columns.");
                                return;
                            }
                        }


                        foreach (string row in selectedRows)
                        {
                            if (rowOfInterest >= senderGrid.Rows.Count)
                                break;

                            try
                            {
                                string[] data = Regex.Split(row, "\t");
                                int col = senderGrid.CurrentCell.ColumnIndex;

                                foreach (string ob in data)
                                {
                                    if (col >= senderGrid.Columns.Count)
                                        break;
                                    if (ob != null)
                                    { //senderGrid[col, rowOfInterest].Value = Convert.ChangeType(ob, senderGrid[col, rowOfInterest].ValueType); 
                                        senderGrid[col, rowOfInterest].Value = ob;
                                        UpdateSpecificRowInfoObject(rowOfInterest, col, ob);
                                    }
                                    col++;
                                }
                            }
                            catch (Exception e)
                            {
                                //do something here 
                            }
                            rowOfInterest++;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                  
                }
               
            }
        }

         private void WellKhDataGridView_KeyDown(object sender, KeyEventArgs e)
         {
             DataGridView senderGrid = sender as DataGridView;

             if (e.Control && e.KeyCode == Keys.V)
             {
                 if (args.PermeabilityFromModel != null)
	            {
		             LoadClipBoardData(ref senderGrid);
	            }
                 else
                 {
                     MessageBox.Show("Please drop a peremability model before copying any data into the table");
                 }
                
             }

         }

         private void SaveOriginalData_Click(object sender, EventArgs e)
         {
             if ( FirstTimeEditingRatio)
             {
                  FirstTimeEditingRatio = false;
             }
             if (ListOfRowInfo.Count >0)
             {
                 ListOfOriginalRowDataInfo.Clear();
                 foreach (KhTableRowInfoContainer ri in ListOfRowInfo)
                 {
                     ListOfOriginalRowDataInfo.Add(ri.CreateCopy());
                 }

                 ListOfOriginalFilledKhwtIndices.Clear();
                 foreach (int ind in ListOfFilledKhwtIndices.ToList())
                 {
                     ListOfOriginalFilledKhwtIndices.Add(ind);
                 }
             }
                    
             
         }
    }

    static class RoundingClass
    {//Note that this method only works for positive numbers. A negative input will return a Nan.
        public static double RoundToSignificantDigits(this double d, int digits)
        {
            if (d == 0)
                return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return scale * Math.Round(d / scale, digits);
        }
    }
}

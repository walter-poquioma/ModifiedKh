namespace ModifiedKh
{
    partial class ModifiedKhUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.PermeabilityDropTarget = new Slb.Ocean.Petrel.UI.DropTarget();
            this.WellDropTarget = new Slb.Ocean.Petrel.UI.DropTarget();
            this.PermeabilityPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.WellPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.ZonesListBox = new System.Windows.Forms.ListBox();
            this.WellTestTextBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.Add = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.Top = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.Base = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.WellTestingIntervalCheckBox = new System.Windows.Forms.CheckBox();
            this.Apply = new System.Windows.Forms.Button();
            this.OneLayerGridDropTarget = new Slb.Ocean.Petrel.UI.DropTarget();
            this.OneLayerGridPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.WellKhDataGridView = new System.Windows.Forms.DataGridView();
            this.ColumnWell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnZones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KhSim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Khwt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ratio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Global = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Estimate = new System.Windows.Forms.DataGridViewButtonColumn();
            this.IncludeRow = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SelectedWellsCheckBox = new System.Windows.Forms.CheckBox();
            this.HistogramButton = new System.Windows.Forms.Button();
            this.DistributionCheckBox = new System.Windows.Forms.CheckBox();
            this.KeepMissingRatio1 = new System.Windows.Forms.CheckBox();
            this.Truncate2NormalDist = new System.Windows.Forms.CheckBox();
            this.MaximumRatioValue = new System.Windows.Forms.TextBox();
            this.MinimumRatioValue = new System.Windows.Forms.TextBox();
            this.UseOriginalData = new System.Windows.Forms.CheckBox();
            this.MajorRangeTextBox = new System.Windows.Forms.TextBox();
            this.MinorRangeTextBox = new System.Windows.Forms.TextBox();
            this.NuggetTextBox = new System.Windows.Forms.TextBox();
            this.VariogramTypeComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.KhRatioTab = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.KrigingParametersTab = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.KrigingAlgComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SillTextBox = new System.Windows.Forms.TextBox();
            this.MajorDirectionTextBox = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.WellKhDataGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.KhRatioTab.SuspendLayout();
            this.KrigingParametersTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // PermeabilityDropTarget
            // 
            this.PermeabilityDropTarget.AllowDrop = true;
            this.PermeabilityDropTarget.Location = new System.Drawing.Point(155, 15);
            this.PermeabilityDropTarget.Name = "PermeabilityDropTarget";
            this.PermeabilityDropTarget.Size = new System.Drawing.Size(26, 23);
            this.PermeabilityDropTarget.TabIndex = 0;
            this.PermeabilityDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.PermeabilityDropTarget_DragDrop);
            // 
            // WellDropTarget
            // 
            this.WellDropTarget.AllowDrop = true;
            this.WellDropTarget.Location = new System.Drawing.Point(176, 220);
            this.WellDropTarget.Name = "WellDropTarget";
            this.WellDropTarget.Size = new System.Drawing.Size(26, 23);
            this.WellDropTarget.TabIndex = 1;
            this.WellDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.WellDropTarget_DragDrop);
            // 
            // PermeabilityPresentationBox
            // 
            this.PermeabilityPresentationBox.Location = new System.Drawing.Point(187, 16);
            this.PermeabilityPresentationBox.Name = "PermeabilityPresentationBox";
            this.PermeabilityPresentationBox.Size = new System.Drawing.Size(156, 22);
            this.PermeabilityPresentationBox.TabIndex = 2;
            // 
            // WellPresentationBox
            // 
            this.WellPresentationBox.Location = new System.Drawing.Point(237, 224);
            this.WellPresentationBox.Name = "WellPresentationBox";
            this.WellPresentationBox.Size = new System.Drawing.Size(156, 22);
            this.WellPresentationBox.TabIndex = 3;
            // 
            // ZonesListBox
            // 
            this.ZonesListBox.FormattingEnabled = true;
            this.ZonesListBox.Location = new System.Drawing.Point(237, 342);
            this.ZonesListBox.Name = "ZonesListBox";
            this.ZonesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ZonesListBox.Size = new System.Drawing.Size(156, 134);
            this.ZonesListBox.TabIndex = 4;
            this.ZonesListBox.DataSourceChanged += new System.EventHandler(this.ZonesListBox_DataSourceChanged);
            // 
            // WellTestTextBox
            // 
            this.WellTestTextBox.Location = new System.Drawing.Point(42, 316);
            this.WellTestTextBox.Name = "WellTestTextBox";
            this.WellTestTextBox.Size = new System.Drawing.Size(108, 20);
            this.WellTestTextBox.TabIndex = 5;
            this.WellTestTextBox.TextChanged += new System.EventHandler(this.WellTestTextBox_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(166, 318);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(36, 20);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "md-ft";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(34, 223);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(108, 20);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "Selected Borehole";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(42, 290);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 9;
            this.textBox5.Text = "Well Test Kh";
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(281, 482);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 13;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(237, 318);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(156, 17);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Show All Intersected Zones";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Top
            // 
            this.Top.Location = new System.Drawing.Point(76, 387);
            this.Top.Name = "Top";
            this.Top.Size = new System.Drawing.Size(100, 20);
            this.Top.TabIndex = 16;
            this.Top.Visible = false;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(182, 387);
            this.textBox7.Name = "textBox7";
            this.textBox7.ReadOnly = true;
            this.textBox7.Size = new System.Drawing.Size(20, 20);
            this.textBox7.TabIndex = 17;
            this.textBox7.Text = "ft";
            // 
            // Base
            // 
            this.Base.Location = new System.Drawing.Point(76, 413);
            this.Base.Name = "Base";
            this.Base.Size = new System.Drawing.Size(100, 20);
            this.Base.TabIndex = 18;
            this.Base.Visible = false;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(182, 413);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(20, 20);
            this.textBox8.TabIndex = 19;
            this.textBox8.Text = "ft";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(26, 387);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(33, 20);
            this.textBox6.TabIndex = 20;
            this.textBox6.Text = "Top";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(28, 413);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(33, 20);
            this.textBox9.TabIndex = 21;
            this.textBox9.Text = "Base";
            // 
            // WellTestingIntervalCheckBox
            // 
            this.WellTestingIntervalCheckBox.AutoSize = true;
            this.WellTestingIntervalCheckBox.Location = new System.Drawing.Point(26, 355);
            this.WellTestingIntervalCheckBox.Name = "WellTestingIntervalCheckBox";
            this.WellTestingIntervalCheckBox.Size = new System.Drawing.Size(123, 17);
            this.WellTestingIntervalCheckBox.TabIndex = 22;
            this.WellTestingIntervalCheckBox.Text = "Well Testing Interval";
            this.WellTestingIntervalCheckBox.UseVisualStyleBackColor = true;
            this.WellTestingIntervalCheckBox.CheckedChanged += new System.EventHandler(this.WellTestingIntervalCheckBox_CheckedChanged);
            // 
            // Apply
            // 
            this.Apply.Location = new System.Drawing.Point(412, 545);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(75, 23);
            this.Apply.TabIndex = 23;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // OneLayerGridDropTarget
            // 
            this.OneLayerGridDropTarget.AllowDrop = true;
            this.OneLayerGridDropTarget.Location = new System.Drawing.Point(155, 67);
            this.OneLayerGridDropTarget.Name = "OneLayerGridDropTarget";
            this.OneLayerGridDropTarget.Size = new System.Drawing.Size(26, 23);
            this.OneLayerGridDropTarget.TabIndex = 24;
            this.OneLayerGridDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.OneLayerGridDropTarget_DragDrop);
            // 
            // OneLayerGridPresentationBox
            // 
            this.OneLayerGridPresentationBox.Location = new System.Drawing.Point(187, 67);
            this.OneLayerGridPresentationBox.Name = "OneLayerGridPresentationBox";
            this.OneLayerGridPresentationBox.Size = new System.Drawing.Size(156, 22);
            this.OneLayerGridPresentationBox.TabIndex = 25;
            // 
            // WellKhDataGridView
            // 
            this.WellKhDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.WellKhDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnWell,
            this.ColumnZones,
            this.KhSim,
            this.Khwt,
            this.Ratio,
            this.Global,
            this.Estimate,
            this.IncludeRow});
            this.WellKhDataGridView.Location = new System.Drawing.Point(30, 145);
            this.WellKhDataGridView.Name = "WellKhDataGridView";
            this.WellKhDataGridView.Size = new System.Drawing.Size(867, 325);
            this.WellKhDataGridView.TabIndex = 26;
            this.WellKhDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.WellKhDataGridView_CellContentClick);
            this.WellKhDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.WellKhDataGridView_CellEndEdit);
            this.WellKhDataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.WellKhDataGridView_CellMouseUp);
            this.WellKhDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.WellKhDataGridView_CellValueChanged);
            this.WellKhDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WellKhDataGridView_KeyDown);
            // 
            // ColumnWell
            // 
            this.ColumnWell.HeaderText = "Well";
            this.ColumnWell.Name = "ColumnWell";
            this.ColumnWell.ReadOnly = true;
            // 
            // ColumnZones
            // 
            this.ColumnZones.HeaderText = "Zones";
            this.ColumnZones.Name = "ColumnZones";
            this.ColumnZones.ReadOnly = true;
            // 
            // KhSim
            // 
            this.KhSim.HeaderText = "Kh(sim) md-ft";
            this.KhSim.Name = "KhSim";
            this.KhSim.ReadOnly = true;
            // 
            // Khwt
            // 
            this.Khwt.HeaderText = "Kh(wt) md-ft";
            this.Khwt.Name = "Khwt";
            this.Khwt.ReadOnly = true;
            // 
            // Ratio
            // 
            this.Ratio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Ratio.HeaderText = "Ratio: Kh(wt)/Kh(sim)";
            this.Ratio.Name = "Ratio";
            this.Ratio.ReadOnly = true;
            // 
            // Global
            // 
            this.Global.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Global.HeaderText = "Global/Individual Kh";
            this.Global.Name = "Global";
            this.Global.ReadOnly = true;
            // 
            // Estimate
            // 
            this.Estimate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Estimate.HeaderText = "Estimate";
            this.Estimate.Name = "Estimate";
            this.Estimate.ReadOnly = true;
            this.Estimate.Text = "";
            this.Estimate.UseColumnTextForButtonValue = true;
            // 
            // IncludeRow
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "True";
            this.IncludeRow.DefaultCellStyle = dataGridViewCellStyle1;
            this.IncludeRow.HeaderText = "Include";
            this.IncludeRow.Name = "IncludeRow";
            this.IncludeRow.ReadOnly = true;
            this.IncludeRow.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IncludeRow.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // SelectedWellsCheckBox
            // 
            this.SelectedWellsCheckBox.AutoSize = true;
            this.SelectedWellsCheckBox.Location = new System.Drawing.Point(435, 110);
            this.SelectedWellsCheckBox.Name = "SelectedWellsCheckBox";
            this.SelectedWellsCheckBox.Size = new System.Drawing.Size(97, 17);
            this.SelectedWellsCheckBox.TabIndex = 27;
            this.SelectedWellsCheckBox.Text = "Selected Wells";
            this.SelectedWellsCheckBox.UseVisualStyleBackColor = true;
            this.SelectedWellsCheckBox.CheckedChanged += new System.EventHandler(this.SelectedWellsCheckBox_CheckedChanged);
            // 
            // HistogramButton
            // 
            this.HistogramButton.Enabled = false;
            this.HistogramButton.Location = new System.Drawing.Point(218, 104);
            this.HistogramButton.Name = "HistogramButton";
            this.HistogramButton.Size = new System.Drawing.Size(124, 23);
            this.HistogramButton.TabIndex = 28;
            this.HistogramButton.Text = "Create Histogram Data";
            this.HistogramButton.UseVisualStyleBackColor = true;
            this.HistogramButton.Click += new System.EventHandler(this.HistogramButton_Click);
            // 
            // DistributionCheckBox
            // 
            this.DistributionCheckBox.AutoSize = true;
            this.DistributionCheckBox.Location = new System.Drawing.Point(759, 522);
            this.DistributionCheckBox.Name = "DistributionCheckBox";
            this.DistributionCheckBox.Size = new System.Drawing.Size(117, 17);
            this.DistributionCheckBox.TabIndex = 29;
            this.DistributionCheckBox.Text = "Log(kh_wt/kh_sim)";
            this.DistributionCheckBox.UseVisualStyleBackColor = true;
            // 
            // KeepMissingRatio1
            // 
            this.KeepMissingRatio1.AutoSize = true;
            this.KeepMissingRatio1.Location = new System.Drawing.Point(657, 20);
            this.KeepMissingRatio1.Name = "KeepMissingRatio1";
            this.KeepMissingRatio1.Size = new System.Drawing.Size(139, 17);
            this.KeepMissingRatio1.TabIndex = 31;
            this.KeepMissingRatio1.Text = "Keep missing ratios as 1";
            this.KeepMissingRatio1.UseVisualStyleBackColor = true;
            this.KeepMissingRatio1.CheckedChanged += new System.EventHandler(this.KeepMissingRatio1_CheckedChanged);
            // 
            // Truncate2NormalDist
            // 
            this.Truncate2NormalDist.AutoSize = true;
            this.Truncate2NormalDist.Checked = true;
            this.Truncate2NormalDist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Truncate2NormalDist.Enabled = false;
            this.Truncate2NormalDist.Location = new System.Drawing.Point(840, 21);
            this.Truncate2NormalDist.Name = "Truncate2NormalDist";
            this.Truncate2NormalDist.Size = new System.Drawing.Size(172, 17);
            this.Truncate2NormalDist.TabIndex = 32;
            this.Truncate2NormalDist.Text = "Truncate to Normal Distribution";
            this.Truncate2NormalDist.UseVisualStyleBackColor = true;
            this.Truncate2NormalDist.CheckedChanged += new System.EventHandler(this.Truncate2NormalDist_CheckedChanged);
            // 
            // MaximumRatioValue
            // 
            this.MaximumRatioValue.Location = new System.Drawing.Point(517, 18);
            this.MaximumRatioValue.Name = "MaximumRatioValue";
            this.MaximumRatioValue.ReadOnly = true;
            this.MaximumRatioValue.Size = new System.Drawing.Size(100, 20);
            this.MaximumRatioValue.TabIndex = 33;
            this.MaximumRatioValue.TextChanged += new System.EventHandler(this.MaximumRatioValue_TextChanged);
            // 
            // MinimumRatioValue
            // 
            this.MinimumRatioValue.Location = new System.Drawing.Point(517, 50);
            this.MinimumRatioValue.Name = "MinimumRatioValue";
            this.MinimumRatioValue.ReadOnly = true;
            this.MinimumRatioValue.Size = new System.Drawing.Size(100, 20);
            this.MinimumRatioValue.TabIndex = 35;
            this.MinimumRatioValue.TextChanged += new System.EventHandler(this.MinimumRatioValue_TextChanged);
            // 
            // UseOriginalData
            // 
            this.UseOriginalData.AutoSize = true;
            this.UseOriginalData.Location = new System.Drawing.Point(657, 50);
            this.UseOriginalData.Name = "UseOriginalData";
            this.UseOriginalData.Size = new System.Drawing.Size(172, 17);
            this.UseOriginalData.TabIndex = 37;
            this.UseOriginalData.Text = "Use Original Data for Modelling";
            this.UseOriginalData.UseVisualStyleBackColor = true;
            this.UseOriginalData.CheckedChanged += new System.EventHandler(this.UseOriginalData_CheckedChanged);
            // 
            // MajorRangeTextBox
            // 
            this.MajorRangeTextBox.Location = new System.Drawing.Point(170, 28);
            this.MajorRangeTextBox.Name = "MajorRangeTextBox";
            this.MajorRangeTextBox.ReadOnly = true;
            this.MajorRangeTextBox.Size = new System.Drawing.Size(100, 20);
            this.MajorRangeTextBox.TabIndex = 38;
            this.MajorRangeTextBox.TextChanged += new System.EventHandler(this.MajorRangeTextBox_TextChanged);
            // 
            // MinorRangeTextBox
            // 
            this.MinorRangeTextBox.Location = new System.Drawing.Point(170, 67);
            this.MinorRangeTextBox.Name = "MinorRangeTextBox";
            this.MinorRangeTextBox.ReadOnly = true;
            this.MinorRangeTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinorRangeTextBox.TabIndex = 40;
            this.MinorRangeTextBox.TextChanged += new System.EventHandler(this.MinorRangeTextBox_TextChanged);
            // 
            // NuggetTextBox
            // 
            this.NuggetTextBox.Location = new System.Drawing.Point(362, 29);
            this.NuggetTextBox.Name = "NuggetTextBox";
            this.NuggetTextBox.ReadOnly = true;
            this.NuggetTextBox.Size = new System.Drawing.Size(100, 20);
            this.NuggetTextBox.TabIndex = 42;
            this.NuggetTextBox.Leave += new System.EventHandler(this.NuggetTextBox_Leave);
            // 
            // VariogramTypeComboBox
            // 
            this.VariogramTypeComboBox.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.VariogramTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VariogramTypeComboBox.Enabled = false;
            this.VariogramTypeComboBox.FormattingEnabled = true;
            this.VariogramTypeComboBox.Items.AddRange(new object[] {
            "Spherical",
            "Gaussian\t",
            "Exponential"});
            this.VariogramTypeComboBox.Location = new System.Drawing.Point(600, 28);
            this.VariogramTypeComboBox.MaxDropDownItems = 3;
            this.VariogramTypeComboBox.Name = "VariogramTypeComboBox";
            this.VariogramTypeComboBox.Size = new System.Drawing.Size(100, 21);
            this.VariogramTypeComboBox.TabIndex = 44;
            this.VariogramTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.VariogramTypeComboBox_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.KhRatioTab);
            this.tabControl1.Controls.Add(this.KrigingParametersTab);
            this.tabControl1.Location = new System.Drawing.Point(3, 37);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1040, 502);
            this.tabControl1.TabIndex = 46;
            // 
            // KhRatioTab
            // 
            this.KhRatioTab.Controls.Add(this.label4);
            this.KhRatioTab.Controls.Add(this.label3);
            this.KhRatioTab.Controls.Add(this.label2);
            this.KhRatioTab.Controls.Add(this.label1);
            this.KhRatioTab.Controls.Add(this.PermeabilityDropTarget);
            this.KhRatioTab.Controls.Add(this.PermeabilityPresentationBox);
            this.KhRatioTab.Controls.Add(this.OneLayerGridDropTarget);
            this.KhRatioTab.Controls.Add(this.OneLayerGridPresentationBox);
            this.KhRatioTab.Controls.Add(this.HistogramButton);
            this.KhRatioTab.Controls.Add(this.MaximumRatioValue);
            this.KhRatioTab.Controls.Add(this.WellKhDataGridView);
            this.KhRatioTab.Controls.Add(this.Truncate2NormalDist);
            this.KhRatioTab.Controls.Add(this.SelectedWellsCheckBox);
            this.KhRatioTab.Controls.Add(this.UseOriginalData);
            this.KhRatioTab.Controls.Add(this.MinimumRatioValue);
            this.KhRatioTab.Controls.Add(this.KeepMissingRatio1);
            this.KhRatioTab.Location = new System.Drawing.Point(4, 22);
            this.KhRatioTab.Name = "KhRatioTab";
            this.KhRatioTab.Padding = new System.Windows.Forms.Padding(3);
            this.KhRatioTab.Size = new System.Drawing.Size(1032, 476);
            this.KhRatioTab.TabIndex = 0;
            this.KhRatioTab.Text = "Kh Ratio";
            this.KhRatioTab.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(435, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Minimum Ratio";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(432, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Maximum Ratio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "One Layer Per Zone Grid :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "Selected Permeability :";
            // 
            // KrigingParametersTab
            // 
            this.KrigingParametersTab.Controls.Add(this.label12);
            this.KrigingParametersTab.Controls.Add(this.KrigingAlgComboBox);
            this.KrigingParametersTab.Controls.Add(this.label11);
            this.KrigingParametersTab.Controls.Add(this.label10);
            this.KrigingParametersTab.Controls.Add(this.label9);
            this.KrigingParametersTab.Controls.Add(this.label8);
            this.KrigingParametersTab.Controls.Add(this.label7);
            this.KrigingParametersTab.Controls.Add(this.label6);
            this.KrigingParametersTab.Controls.Add(this.label5);
            this.KrigingParametersTab.Controls.Add(this.SillTextBox);
            this.KrigingParametersTab.Controls.Add(this.MajorDirectionTextBox);
            this.KrigingParametersTab.Controls.Add(this.VariogramTypeComboBox);
            this.KrigingParametersTab.Controls.Add(this.MajorRangeTextBox);
            this.KrigingParametersTab.Controls.Add(this.NuggetTextBox);
            this.KrigingParametersTab.Controls.Add(this.MinorRangeTextBox);
            this.KrigingParametersTab.Location = new System.Drawing.Point(4, 22);
            this.KrigingParametersTab.Name = "KrigingParametersTab";
            this.KrigingParametersTab.Padding = new System.Windows.Forms.Padding(3);
            this.KrigingParametersTab.Size = new System.Drawing.Size(1032, 476);
            this.KrigingParametersTab.TabIndex = 1;
            this.KrigingParametersTab.Text = "Kriging Parameters";
            this.KrigingParametersTab.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(483, 180);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 13);
            this.label12.TabIndex = 60;
            this.label12.Text = "Kriging Algorithm Type";
            // 
            // KrigingAlgComboBox
            // 
            this.KrigingAlgComboBox.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.KrigingAlgComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KrigingAlgComboBox.Enabled = false;
            this.KrigingAlgComboBox.FormattingEnabled = true;
            this.KrigingAlgComboBox.Items.AddRange(new object[] {
            "Ordinary",
            "Simple"});
            this.KrigingAlgComboBox.Location = new System.Drawing.Point(600, 176);
            this.KrigingAlgComboBox.MaxDropDownItems = 3;
            this.KrigingAlgComboBox.Name = "KrigingAlgComboBox";
            this.KrigingAlgComboBox.Size = new System.Drawing.Size(100, 21);
            this.KrigingAlgComboBox.TabIndex = 59;
            this.KrigingAlgComboBox.SelectedIndexChanged += new System.EventHandler(this.KrigingAlgComboBox_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(119, 180);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.label11.TabIndex = 58;
            this.label11.Text = "Azimuth";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(119, 154);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(132, 13);
            this.label10.TabIndex = 57;
            this.label10.Text = "Major Direction Orientation";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(514, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 56;
            this.label9.Text = "Variogram Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(336, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 13);
            this.label8.TabIndex = 55;
            this.label8.Text = "Sill";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(314, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 54;
            this.label7.Text = "Nugget";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(96, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "Minor Range";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(96, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 52;
            this.label5.Text = "Major Range";
            // 
            // SillTextBox
            // 
            this.SillTextBox.Location = new System.Drawing.Point(362, 67);
            this.SillTextBox.Name = "SillTextBox";
            this.SillTextBox.ReadOnly = true;
            this.SillTextBox.Size = new System.Drawing.Size(100, 20);
            this.SillTextBox.TabIndex = 51;
            // 
            // MajorDirectionTextBox
            // 
            this.MajorDirectionTextBox.Location = new System.Drawing.Point(170, 176);
            this.MajorDirectionTextBox.Name = "MajorDirectionTextBox";
            this.MajorDirectionTextBox.ReadOnly = true;
            this.MajorDirectionTextBox.Size = new System.Drawing.Size(100, 20);
            this.MajorDirectionTextBox.TabIndex = 48;
            this.MajorDirectionTextBox.Leave += new System.EventHandler(this.MajorDirectionTextBox_Leave);
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(524, 545);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 47;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(639, 545);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 48;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ModifiedKhUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.WellTestingIntervalCheckBox);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.Base);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.Top);
            this.Controls.Add(this.DistributionCheckBox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.WellTestTextBox);
            this.Controls.Add(this.ZonesListBox);
            this.Controls.Add(this.WellPresentationBox);
            this.Controls.Add(this.WellDropTarget);
            this.Name = "ModifiedKhUI";
            this.Size = new System.Drawing.Size(1053, 598);
            this.Load += new System.EventHandler(this.ModifiedKhUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.WellKhDataGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.KhRatioTab.ResumeLayout(false);
            this.KhRatioTab.PerformLayout();
            this.KrigingParametersTab.ResumeLayout(false);
            this.KrigingParametersTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Slb.Ocean.Petrel.UI.DropTarget PermeabilityDropTarget;
        private Slb.Ocean.Petrel.UI.DropTarget WellDropTarget;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox PermeabilityPresentationBox;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox WellPresentationBox;
        private System.Windows.Forms.ListBox ZonesListBox;
        private System.Windows.Forms.TextBox WellTestTextBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox Top;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox Base;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.CheckBox WellTestingIntervalCheckBox;
        private System.Windows.Forms.Button Apply;
        private Slb.Ocean.Petrel.UI.DropTarget OneLayerGridDropTarget;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox OneLayerGridPresentationBox;
        private System.Windows.Forms.DataGridView WellKhDataGridView;
        private System.Windows.Forms.CheckBox SelectedWellsCheckBox;
        private System.Windows.Forms.Button HistogramButton;
        private System.Windows.Forms.CheckBox DistributionCheckBox;
        private System.Windows.Forms.CheckBox KeepMissingRatio1;
        private System.Windows.Forms.CheckBox Truncate2NormalDist;
        private System.Windows.Forms.TextBox MaximumRatioValue;
        private System.Windows.Forms.TextBox MinimumRatioValue;
        private System.Windows.Forms.CheckBox UseOriginalData;
        private System.Windows.Forms.TextBox MajorRangeTextBox;
        private System.Windows.Forms.TextBox MinorRangeTextBox;
        private System.Windows.Forms.TextBox NuggetTextBox;
        private System.Windows.Forms.ComboBox VariogramTypeComboBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage KhRatioTab;
        private System.Windows.Forms.TabPage KrigingParametersTab;
        private System.Windows.Forms.TextBox MajorDirectionTextBox;
        private System.Windows.Forms.TextBox SillTextBox;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox KrigingAlgComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWell;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnZones;
        private System.Windows.Forms.DataGridViewTextBoxColumn KhSim;
        private System.Windows.Forms.DataGridViewTextBoxColumn Khwt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ratio;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Global;
        private System.Windows.Forms.DataGridViewButtonColumn Estimate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IncludeRow;
    }
}

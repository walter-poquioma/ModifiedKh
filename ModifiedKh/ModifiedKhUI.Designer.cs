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
            this.PermeabilityDropTarget = new Slb.Ocean.Petrel.UI.DropTarget();
            this.WellDropTarget = new Slb.Ocean.Petrel.UI.DropTarget();
            this.PermeabilityPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.WellPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.ZonesListBox = new System.Windows.Forms.ListBox();
            this.WellTestTextBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.ZoneIndexDropTarget = new Slb.Ocean.Petrel.UI.DropTarget();
            this.ZoneIndexPresentationBox = new Slb.Ocean.Petrel.UI.Controls.PresentationBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            this.SuspendLayout();
            // 
            // PermeabilityDropTarget
            // 
            this.PermeabilityDropTarget.AllowDrop = true;
            this.PermeabilityDropTarget.Location = new System.Drawing.Point(166, 48);
            this.PermeabilityDropTarget.Name = "PermeabilityDropTarget";
            this.PermeabilityDropTarget.Size = new System.Drawing.Size(26, 23);
            this.PermeabilityDropTarget.TabIndex = 0;
            this.PermeabilityDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.PermeabilityDropTarget_DragDrop);
            // 
            // WellDropTarget
            // 
            this.WellDropTarget.AllowDrop = true;
            this.WellDropTarget.Location = new System.Drawing.Point(166, 223);
            this.WellDropTarget.Name = "WellDropTarget";
            this.WellDropTarget.Size = new System.Drawing.Size(26, 23);
            this.WellDropTarget.TabIndex = 1;
            this.WellDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.WellDropTarget_DragDrop);
            // 
            // PermeabilityPresentationBox
            // 
            this.PermeabilityPresentationBox.Location = new System.Drawing.Point(237, 49);
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
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(26, 49);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(108, 20);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "Selected Permeability";
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
            // ZoneIndexDropTarget
            // 
            this.ZoneIndexDropTarget.AllowDrop = true;
            this.ZoneIndexDropTarget.Location = new System.Drawing.Point(166, 133);
            this.ZoneIndexDropTarget.Name = "ZoneIndexDropTarget";
            this.ZoneIndexDropTarget.Size = new System.Drawing.Size(26, 23);
            this.ZoneIndexDropTarget.TabIndex = 10;
            this.ZoneIndexDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.ZoneIndexDropTarget_DragDrop);
            // 
            // ZoneIndexPresentationBox
            // 
            this.ZoneIndexPresentationBox.Location = new System.Drawing.Point(237, 136);
            this.ZoneIndexPresentationBox.Name = "ZoneIndexPresentationBox";
            this.ZoneIndexPresentationBox.Size = new System.Drawing.Size(156, 22);
            this.ZoneIndexPresentationBox.TabIndex = 11;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(34, 133);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "Zone Index";
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
            this.Apply.Location = new System.Drawing.Point(101, 555);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(75, 23);
            this.Apply.TabIndex = 23;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // ModifiedKhUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.WellTestingIntervalCheckBox);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.Base);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.Top);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ZoneIndexPresentationBox);
            this.Controls.Add(this.ZoneIndexDropTarget);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.WellTestTextBox);
            this.Controls.Add(this.ZonesListBox);
            this.Controls.Add(this.WellPresentationBox);
            this.Controls.Add(this.PermeabilityPresentationBox);
            this.Controls.Add(this.WellDropTarget);
            this.Controls.Add(this.PermeabilityDropTarget);
            this.Name = "ModifiedKhUI";
            this.Size = new System.Drawing.Size(477, 611);
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
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private Slb.Ocean.Petrel.UI.DropTarget ZoneIndexDropTarget;
        private Slb.Ocean.Petrel.UI.Controls.PresentationBox ZoneIndexPresentationBox;
        private System.Windows.Forms.TextBox textBox1;
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
    }
}

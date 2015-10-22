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
            this.SuspendLayout();
            // 
            // PermeabilityDropTarget
            // 
            this.PermeabilityDropTarget.AllowDrop = true;
            this.PermeabilityDropTarget.Location = new System.Drawing.Point(141, 47);
            this.PermeabilityDropTarget.Name = "PermeabilityDropTarget";
            this.PermeabilityDropTarget.Size = new System.Drawing.Size(26, 23);
            this.PermeabilityDropTarget.TabIndex = 0;
            this.PermeabilityDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.PermeabilityDropTarget_DragDrop);
            // 
            // WellDropTarget
            // 
            this.WellDropTarget.AllowDrop = true;
            this.WellDropTarget.Location = new System.Drawing.Point(141, 223);
            this.WellDropTarget.Name = "WellDropTarget";
            this.WellDropTarget.Size = new System.Drawing.Size(26, 23);
            this.WellDropTarget.TabIndex = 1;
            this.WellDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.WellDropTarget_DragDrop);
            // 
            // PermeabilityPresentationBox
            // 
            this.PermeabilityPresentationBox.Location = new System.Drawing.Point(200, 48);
            this.PermeabilityPresentationBox.Name = "PermeabilityPresentationBox";
            this.PermeabilityPresentationBox.Size = new System.Drawing.Size(156, 22);
            this.PermeabilityPresentationBox.TabIndex = 2;
            // 
            // WellPresentationBox
            // 
            this.WellPresentationBox.Location = new System.Drawing.Point(200, 224);
            this.WellPresentationBox.Name = "WellPresentationBox";
            this.WellPresentationBox.Size = new System.Drawing.Size(156, 22);
            this.WellPresentationBox.TabIndex = 3;
            // 
            // ZonesListBox
            // 
            this.ZonesListBox.FormattingEnabled = true;
            this.ZonesListBox.Location = new System.Drawing.Point(200, 342);
            this.ZonesListBox.Name = "ZonesListBox";
            this.ZonesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ZonesListBox.Size = new System.Drawing.Size(156, 134);
            this.ZonesListBox.TabIndex = 4;
            this.ZonesListBox.DataSourceChanged += new System.EventHandler(this.ZonesListBox_DataSourceChanged);
            // 
            // WellTestTextBox
            // 
            this.WellTestTextBox.Location = new System.Drawing.Point(15, 318);
            this.WellTestTextBox.Name = "WellTestTextBox";
            this.WellTestTextBox.Size = new System.Drawing.Size(108, 20);
            this.WellTestTextBox.TabIndex = 5;
            this.WellTestTextBox.TextChanged += new System.EventHandler(this.WellTestTextBox_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(131, 318);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(36, 20);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "md-ft";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(15, 223);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(108, 20);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "Selected Borehole";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(15, 49);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(108, 20);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "Selected Permeability";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(15, 292);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 9;
            this.textBox5.Text = "Well Test Kh";
            // 
            // ZoneIndexDropTarget
            // 
            this.ZoneIndexDropTarget.AllowDrop = true;
            this.ZoneIndexDropTarget.Location = new System.Drawing.Point(141, 133);
            this.ZoneIndexDropTarget.Name = "ZoneIndexDropTarget";
            this.ZoneIndexDropTarget.Size = new System.Drawing.Size(26, 23);
            this.ZoneIndexDropTarget.TabIndex = 10;
            this.ZoneIndexDropTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.ZoneIndexDropTarget_DragDrop);
            // 
            // ZoneIndexPresentationBox
            // 
            this.ZoneIndexPresentationBox.Location = new System.Drawing.Point(200, 134);
            this.ZoneIndexPresentationBox.Name = "ZoneIndexPresentationBox";
            this.ZoneIndexPresentationBox.Size = new System.Drawing.Size(156, 22);
            this.ZoneIndexPresentationBox.TabIndex = 11;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 136);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "Zone Index";
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(237, 482);
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
            this.checkBox1.Location = new System.Drawing.Point(200, 318);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(156, 17);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Show All Intersected Zones";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ModifiedKhUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Size = new System.Drawing.Size(392, 599);
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
    }
}

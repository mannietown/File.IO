namespace Stock_Manager
{
    partial class frmChooseSite
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grbNotSelected = new System.Windows.Forms.GroupBox();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lsbDeselected = new System.Windows.Forms.ListBox();
            this.lsbSelected = new System.Windows.Forms.ListBox();
            this.btnSwitchSides = new System.Windows.Forms.Button();
            this.grbNotSelected.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbNotSelected
            // 
            this.grbNotSelected.Controls.Add(this.lsbDeselected);
            this.grbNotSelected.Location = new System.Drawing.Point(13, 13);
            this.grbNotSelected.Name = "grbNotSelected";
            this.grbNotSelected.Size = new System.Drawing.Size(138, 202);
            this.grbNotSelected.TabIndex = 0;
            this.grbNotSelected.TabStop = false;
            this.grbNotSelected.Text = "Available Sites";
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Location = new System.Drawing.Point(88, 221);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(75, 28);
            this.btnDeselectAll.TabIndex = 1;
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lsbSelected);
            this.groupBox1.Location = new System.Drawing.Point(184, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(134, 202);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Sites";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(169, 221);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 28);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            // 
            // lsbDeselected
            // 
            this.lsbDeselected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsbDeselected.FormattingEnabled = true;
            this.lsbDeselected.Location = new System.Drawing.Point(3, 16);
            this.lsbDeselected.Name = "lsbDeselected";
            this.lsbDeselected.Size = new System.Drawing.Size(132, 183);
            this.lsbDeselected.TabIndex = 0;
            this.lsbDeselected.Enter += new System.EventHandler(this.lsbDeselected_Enter);
            // 
            // lsbSelected
            // 
            this.lsbSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsbSelected.FormattingEnabled = true;
            this.lsbSelected.Location = new System.Drawing.Point(3, 16);
            this.lsbSelected.Name = "lsbSelected";
            this.lsbSelected.Size = new System.Drawing.Size(128, 183);
            this.lsbSelected.TabIndex = 1;
            this.lsbSelected.Enter += new System.EventHandler(this.lsbSelected_Enter);
            // 
            // btnSwitchSides
            // 
            this.btnSwitchSides.Location = new System.Drawing.Point(157, 98);
            this.btnSwitchSides.Name = "btnSwitchSides";
            this.btnSwitchSides.Size = new System.Drawing.Size(21, 23);
            this.btnSwitchSides.TabIndex = 3;
            this.btnSwitchSides.Text = ">";
            this.btnSwitchSides.UseVisualStyleBackColor = true;
            this.btnSwitchSides.Click += new System.EventHandler(this.btnSwitchSides_Click);
            // 
            // frmChooseSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 261);
            this.Controls.Add(this.btnSwitchSides);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDeselectAll);
            this.Controls.Add(this.grbNotSelected);
            this.Name = "frmChooseSite";
            this.Text = "frmChooseSite";
            this.grbNotSelected.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbNotSelected;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.ListBox lsbDeselected;
        private System.Windows.Forms.ListBox lsbSelected;
        private System.Windows.Forms.Button btnSwitchSides;
    }
}
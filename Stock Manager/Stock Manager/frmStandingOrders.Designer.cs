namespace Stock_Manager
{
    partial class frmStandingOrders
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
            this.grpIncoming = new System.Windows.Forms.GroupBox();
            this.grpOutgoing = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.grpIncoming.SuspendLayout();
            this.grpOutgoing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // grpIncoming
            // 
            this.grpIncoming.Controls.Add(this.dataGridView1);
            this.grpIncoming.Location = new System.Drawing.Point(13, 13);
            this.grpIncoming.Name = "grpIncoming";
            this.grpIncoming.Size = new System.Drawing.Size(443, 121);
            this.grpIncoming.TabIndex = 0;
            this.grpIncoming.TabStop = false;
            this.grpIncoming.Text = "Incoming (Delivery To You)";
            // 
            // grpOutgoing
            // 
            this.grpOutgoing.Controls.Add(this.dataGridView2);
            this.grpOutgoing.Location = new System.Drawing.Point(13, 140);
            this.grpOutgoing.Name = "grpOutgoing";
            this.grpOutgoing.Size = new System.Drawing.Size(443, 121);
            this.grpOutgoing.TabIndex = 1;
            this.grpOutgoing.TabStop = false;
            this.grpOutgoing.Text = "Outgoing (Delivery To Customer)";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(437, 102);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 16);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(437, 102);
            this.dataGridView2.TabIndex = 1;
            // 
            // frmStandingOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 391);
            this.Controls.Add(this.grpOutgoing);
            this.Controls.Add(this.grpIncoming);
            this.Name = "frmStandingOrders";
            this.Text = "Standing Orders";
            this.grpIncoming.ResumeLayout(false);
            this.grpOutgoing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpIncoming;
        private System.Windows.Forms.GroupBox grpOutgoing;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}
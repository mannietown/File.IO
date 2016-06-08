namespace Stock_Manager
{
    partial class frmHome
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
            this.components = new System.ComponentModel.Container();
            this.btnAdminPanel = new System.Windows.Forms.Button();
            this.btnCheckout = new System.Windows.Forms.Button();
            this.btnStockManager = new System.Windows.Forms.Button();
            this.btnLogOff = new System.Windows.Forms.Button();
            this.lblWelcomeMessage = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.tmrDateTime = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAdminPanel
            // 
            this.btnAdminPanel.Location = new System.Drawing.Point(13, 46);
            this.btnAdminPanel.Name = "btnAdminPanel";
            this.btnAdminPanel.Size = new System.Drawing.Size(103, 47);
            this.btnAdminPanel.TabIndex = 0;
            this.btnAdminPanel.Text = "Admin Panel";
            this.btnAdminPanel.UseVisualStyleBackColor = true;
            // 
            // btnCheckout
            // 
            this.btnCheckout.Location = new System.Drawing.Point(122, 46);
            this.btnCheckout.Name = "btnCheckout";
            this.btnCheckout.Size = new System.Drawing.Size(103, 47);
            this.btnCheckout.TabIndex = 1;
            this.btnCheckout.Text = "Checkout";
            this.btnCheckout.UseVisualStyleBackColor = true;
            // 
            // btnStockManager
            // 
            this.btnStockManager.Location = new System.Drawing.Point(340, 46);
            this.btnStockManager.Name = "btnStockManager";
            this.btnStockManager.Size = new System.Drawing.Size(103, 47);
            this.btnStockManager.TabIndex = 2;
            this.btnStockManager.Text = "Stock Manager";
            this.btnStockManager.UseVisualStyleBackColor = true;
            // 
            // btnLogOff
            // 
            this.btnLogOff.Location = new System.Drawing.Point(340, 12);
            this.btnLogOff.Name = "btnLogOff";
            this.btnLogOff.Size = new System.Drawing.Size(103, 28);
            this.btnLogOff.TabIndex = 3;
            this.btnLogOff.Text = "Log Off";
            this.btnLogOff.UseVisualStyleBackColor = true;
            this.btnLogOff.Click += new System.EventHandler(this.btnLogOff_Click);
            // 
            // lblWelcomeMessage
            // 
            this.lblWelcomeMessage.Location = new System.Drawing.Point(12, 12);
            this.lblWelcomeMessage.Name = "lblWelcomeMessage";
            this.lblWelcomeMessage.Size = new System.Drawing.Size(213, 28);
            this.lblWelcomeMessage.TabIndex = 4;
            this.lblWelcomeMessage.Text = "Welcome, ";
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(231, 12);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(103, 28);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "Time And Date:";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tmrDateTime
            // 
            this.tmrDateTime.Enabled = true;
            this.tmrDateTime.Interval = 200;
            this.tmrDateTime.Tick += new System.EventHandler(this.tmrDateTime_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(231, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 47);
            this.button1.TabIndex = 6;
            this.button1.Text = "Checkout";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 105);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblWelcomeMessage);
            this.Controls.Add(this.btnLogOff);
            this.Controls.Add(this.btnStockManager);
            this.Controls.Add(this.btnCheckout);
            this.Controls.Add(this.btnAdminPanel);
            this.Name = "frmHome";
            this.Opacity = 0D;
            this.Text = "Where Next?";
            this.Load += new System.EventHandler(this.frmHome_Load);
            this.Enter += new System.EventHandler(this.frmHome_Enter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAdminPanel;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.Button btnStockManager;
        private System.Windows.Forms.Button btnLogOff;
        private System.Windows.Forms.Label lblWelcomeMessage;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer tmrDateTime;
        private System.Windows.Forms.Button button1;
    }
}
namespace Stock_Manager
{
    partial class frmCheckout
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
            this.grbCart = new System.Windows.Forms.GroupBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClearCart = new System.Windows.Forms.Button();
            this.lblTotalDiscount = new System.Windows.Forms.Label();
            this.btnApplyCoupon = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.grbCart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.SuspendLayout();
            // 
            // grbCart
            // 
            this.grbCart.Controls.Add(this.dgvCart);
            this.grbCart.Location = new System.Drawing.Point(12, 12);
            this.grbCart.Name = "grbCart";
            this.grbCart.Size = new System.Drawing.Size(444, 261);
            this.grbCart.TabIndex = 0;
            this.grbCart.TabStop = false;
            this.grbCart.Text = "Shopping Cart";
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(343, 276);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(113, 38);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(343, 320);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(113, 29);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "That\'s Everything!";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnClearCart
            // 
            this.btnClearCart.Location = new System.Drawing.Point(12, 320);
            this.btnClearCart.Name = "btnClearCart";
            this.btnClearCart.Size = new System.Drawing.Size(113, 29);
            this.btnClearCart.TabIndex = 3;
            this.btnClearCart.Text = "Clear All";
            this.btnClearCart.UseVisualStyleBackColor = true;
            // 
            // lblTotalDiscount
            // 
            this.lblTotalDiscount.Location = new System.Drawing.Point(12, 276);
            this.lblTotalDiscount.Name = "lblTotalDiscount";
            this.lblTotalDiscount.Size = new System.Drawing.Size(113, 38);
            this.lblTotalDiscount.TabIndex = 4;
            this.lblTotalDiscount.Text = "Discount Total:";
            // 
            // btnApplyCoupon
            // 
            this.btnApplyCoupon.Location = new System.Drawing.Point(178, 320);
            this.btnApplyCoupon.Name = "btnApplyCoupon";
            this.btnApplyCoupon.Size = new System.Drawing.Size(113, 29);
            this.btnApplyCoupon.TabIndex = 5;
            this.btnApplyCoupon.Text = "Apply Coupon";
            this.btnApplyCoupon.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(178, 294);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(113, 20);
            this.textBox1.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(178, 276);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Coupon Code:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dgvCart
            // 
            this.dgvCart.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCart.Location = new System.Drawing.Point(3, 16);
            this.dgvCart.Name = "dgvCart";
            this.dgvCart.Size = new System.Drawing.Size(438, 242);
            this.dgvCart.TabIndex = 0;
            // 
            // frmCheckout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 361);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnApplyCoupon);
            this.Controls.Add(this.lblTotalDiscount);
            this.Controls.Add(this.btnClearCart);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.grbCart);
            this.Name = "frmCheckout";
            this.Text = "Checkout";
            this.grbCart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbCart;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClearCart;
        private System.Windows.Forms.Label lblTotalDiscount;
        private System.Windows.Forms.Button btnApplyCoupon;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvCart;
    }
}


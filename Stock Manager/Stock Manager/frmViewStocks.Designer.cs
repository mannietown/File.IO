namespace Stock_Manager
{
    partial class frmViewStocks
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOrderStock = new System.Windows.Forms.Button();
            this.dgvStocks = new System.Windows.Forms.DataGridView();
            this.btnViewScheduledOrders = new System.Windows.Forms.Button();
            this.stockItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.itemIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberInStockDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStocks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockItemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.Location = new System.Drawing.Point(12, 362);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(110, 33);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnOrderStock
            // 
            this.btnOrderStock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrderStock.Location = new System.Drawing.Point(581, 362);
            this.btnOrderStock.Name = "btnOrderStock";
            this.btnOrderStock.Size = new System.Drawing.Size(110, 33);
            this.btnOrderStock.TabIndex = 4;
            this.btnOrderStock.Text = "Order New Stock";
            this.btnOrderStock.UseVisualStyleBackColor = true;
            // 
            // dgvStocks
            // 
            this.dgvStocks.AllowUserToAddRows = false;
            this.dgvStocks.AllowUserToDeleteRows = false;
            this.dgvStocks.AllowUserToOrderColumns = true;
            this.dgvStocks.AutoGenerateColumns = false;
            this.dgvStocks.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvStocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStocks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemIDDataGridViewTextBoxColumn,
            this.ItemDescription,
            this.numberInStockDataGridViewTextBoxColumn,
            this.itemValueDataGridViewTextBoxColumn});
            this.dgvStocks.DataSource = this.stockItemBindingSource;
            this.dgvStocks.Location = new System.Drawing.Point(12, 12);
            this.dgvStocks.Name = "dgvStocks";
            this.dgvStocks.Size = new System.Drawing.Size(679, 344);
            this.dgvStocks.TabIndex = 5;
            this.dgvStocks.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStocks_CellEndEdit);
            this.dgvStocks.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvStocks_UserAddedRow);
            this.dgvStocks.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvStocks_UserDeletedRow);
            this.dgvStocks.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvStocks_UserDeletingRow);
            // 
            // btnViewScheduledOrders
            // 
            this.btnViewScheduledOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewScheduledOrders.Location = new System.Drawing.Point(286, 362);
            this.btnViewScheduledOrders.Name = "btnViewScheduledOrders";
            this.btnViewScheduledOrders.Size = new System.Drawing.Size(128, 33);
            this.btnViewScheduledOrders.TabIndex = 6;
            this.btnViewScheduledOrders.Text = "View Standing Orders";
            this.btnViewScheduledOrders.UseVisualStyleBackColor = true;
            // 
            // stockItemBindingSource
            // 
            this.stockItemBindingSource.DataSource = typeof(Stock_Manager.StockItem);
            // 
            // itemIDDataGridViewTextBoxColumn
            // 
            this.itemIDDataGridViewTextBoxColumn.DataPropertyName = "ItemID";
            this.itemIDDataGridViewTextBoxColumn.HeaderText = "Unique ID";
            this.itemIDDataGridViewTextBoxColumn.Name = "itemIDDataGridViewTextBoxColumn";
            // 
            // ItemDescription
            // 
            this.ItemDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDescription.DataPropertyName = "ItemDescription";
            this.ItemDescription.HeaderText = "Description";
            this.ItemDescription.Name = "ItemDescription";
            // 
            // numberInStockDataGridViewTextBoxColumn
            // 
            this.numberInStockDataGridViewTextBoxColumn.DataPropertyName = "NumberInStock";
            this.numberInStockDataGridViewTextBoxColumn.HeaderText = "Stock Count";
            this.numberInStockDataGridViewTextBoxColumn.Name = "numberInStockDataGridViewTextBoxColumn";
            // 
            // itemValueDataGridViewTextBoxColumn
            // 
            this.itemValueDataGridViewTextBoxColumn.DataPropertyName = "ItemValue";
            this.itemValueDataGridViewTextBoxColumn.HeaderText = "Cost Per Item";
            this.itemValueDataGridViewTextBoxColumn.Name = "itemValueDataGridViewTextBoxColumn";
            // 
            // frmViewStocks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 407);
            this.Controls.Add(this.btnViewScheduledOrders);
            this.Controls.Add(this.dgvStocks);
            this.Controls.Add(this.btnOrderStock);
            this.Controls.Add(this.btnExit);
            this.Name = "frmViewStocks";
            this.Text = "Current Stock";
            this.Load += new System.EventHandler(this.frmViewStocks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stockItemBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOrderStock;
        private System.Windows.Forms.BindingSource stockItemBindingSource;
        private System.Windows.Forms.DataGridView dgvStocks;
        private System.Windows.Forms.Button btnViewScheduledOrders;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberInStockDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemValueDataGridViewTextBoxColumn;
    }
}
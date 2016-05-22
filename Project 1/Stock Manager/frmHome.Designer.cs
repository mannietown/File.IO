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
            this.dgvMain = new System.Windows.Forms.DataGridView();
            this.objectDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastStockAddedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentStockDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsDatabase = new System.Windows.Forms.BindingSource(this.components);
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.numStock = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMain
            // 
            this.dgvMain.AutoGenerateColumns = false;
            this.dgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.objectDescriptionDataGridViewTextBoxColumn,
            this.lastStockAddedDataGridViewTextBoxColumn,
            this.currentStockDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn});
            this.dgvMain.DataSource = this.bsDatabase;
            this.dgvMain.Location = new System.Drawing.Point(13, 12);
            this.dgvMain.Name = "dgvMain";
            this.dgvMain.Size = new System.Drawing.Size(589, 368);
            this.dgvMain.TabIndex = 0;
            // 
            // objectDescriptionDataGridViewTextBoxColumn
            // 
            this.objectDescriptionDataGridViewTextBoxColumn.DataPropertyName = "ObjectDescription";
            this.objectDescriptionDataGridViewTextBoxColumn.HeaderText = "ObjectDescription";
            this.objectDescriptionDataGridViewTextBoxColumn.Name = "objectDescriptionDataGridViewTextBoxColumn";
            // 
            // lastStockAddedDataGridViewTextBoxColumn
            // 
            this.lastStockAddedDataGridViewTextBoxColumn.DataPropertyName = "LastStockAdded";
            this.lastStockAddedDataGridViewTextBoxColumn.HeaderText = "LastStockAdded";
            this.lastStockAddedDataGridViewTextBoxColumn.Name = "lastStockAddedDataGridViewTextBoxColumn";
            this.lastStockAddedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currentStockDataGridViewTextBoxColumn
            // 
            this.currentStockDataGridViewTextBoxColumn.DataPropertyName = "CurrentStock";
            this.currentStockDataGridViewTextBoxColumn.HeaderText = "CurrentStock";
            this.currentStockDataGridViewTextBoxColumn.Name = "currentStockDataGridViewTextBoxColumn";
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "Price";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            // 
            // bsDatabase
            // 
            this.bsDatabase.DataSource = typeof(Stock_Manager.DataEntry);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(527, 386);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 45);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(13, 391);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(255, 31);
            this.txtDescription.TabIndex = 2;
            // 
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numPrice.Location = new System.Drawing.Point(274, 392);
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(122, 31);
            this.numPrice.TabIndex = 3;
            // 
            // numStock
            // 
            this.numStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numStock.Location = new System.Drawing.Point(402, 391);
            this.numStock.Name = "numStock";
            this.numStock.Size = new System.Drawing.Size(119, 31);
            this.numStock.TabIndex = 4;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 443);
            this.Controls.Add(this.numStock);
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvMain);
            this.Name = "frmHome";
            this.Text = "Home";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.BindingSource bsDatabase;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.NumericUpDown numPrice;
        private System.Windows.Forms.NumericUpDown numStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn objectDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastStockAddedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentStockDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
    }
}


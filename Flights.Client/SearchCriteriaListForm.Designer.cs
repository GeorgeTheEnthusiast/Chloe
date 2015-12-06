namespace Flights.Client
{
    partial class SearchCriteriaListForm
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
            this.searchCriteriaViewBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flightsDataSet = new Flights.Client.FlightsDataSet();
            this.searchCriteria_ViewTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.SearchCriteria_ViewTableAdapter();
            this.searchCriteriaViewBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewSearchCriterias = new System.Windows.Forms.DataGridView();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.flightsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flightsTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.FlightsTableAdapter();
            this.searchCriteriasBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchCriteriasTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.SearchCriteriasTableAdapter();
            this.Nazwa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataWylotu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Od = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Do = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Strona = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaViewBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaViewBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearchCriterias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriasBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // searchCriteriaViewBindingSource
            // 
            this.searchCriteriaViewBindingSource.DataMember = "SearchCriteria_View";
            this.searchCriteriaViewBindingSource.DataSource = this.flightsDataSet;
            // 
            // flightsDataSet
            // 
            this.flightsDataSet.DataSetName = "FlightsDataSet";
            this.flightsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // searchCriteria_ViewTableAdapter
            // 
            this.searchCriteria_ViewTableAdapter.ClearBeforeFill = true;
            // 
            // searchCriteriaViewBindingSource1
            // 
            this.searchCriteriaViewBindingSource1.DataMember = "SearchCriteria_View";
            this.searchCriteriaViewBindingSource1.DataSource = this.flightsDataSet;
            // 
            // dataGridViewSearchCriterias
            // 
            this.dataGridViewSearchCriterias.AllowUserToAddRows = false;
            this.dataGridViewSearchCriterias.AllowUserToDeleteRows = false;
            this.dataGridViewSearchCriterias.AutoGenerateColumns = false;
            this.dataGridViewSearchCriterias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewSearchCriterias.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewSearchCriterias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSearchCriterias.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nazwa,
            this.DataWylotu,
            this.Od,
            this.Do,
            this.Strona});
            this.dataGridViewSearchCriterias.DataSource = this.searchCriteriaViewBindingSource;
            this.dataGridViewSearchCriterias.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewSearchCriterias.Name = "dataGridViewSearchCriterias";
            this.dataGridViewSearchCriterias.ReadOnly = true;
            this.dataGridViewSearchCriterias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSearchCriterias.Size = new System.Drawing.Size(743, 156);
            this.dataGridViewSearchCriterias.TabIndex = 0;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(12, 174);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 1;
            this.buttonAdd.Text = "Dodaj";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(181, 174);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Usuń";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // flightsBindingSource
            // 
            this.flightsBindingSource.DataMember = "Flights";
            this.flightsBindingSource.DataSource = this.flightsDataSet;
            // 
            // flightsTableAdapter
            // 
            this.flightsTableAdapter.ClearBeforeFill = true;
            // 
            // searchCriteriasBindingSource
            // 
            this.searchCriteriasBindingSource.DataMember = "SearchCriterias";
            this.searchCriteriasBindingSource.DataSource = this.flightsDataSet;
            // 
            // searchCriteriasTableAdapter
            // 
            this.searchCriteriasTableAdapter.ClearBeforeFill = true;
            // 
            // Nazwa
            // 
            this.Nazwa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Nazwa.DataPropertyName = "Nazwa";
            this.Nazwa.HeaderText = "Nazwa";
            this.Nazwa.Name = "Nazwa";
            this.Nazwa.ReadOnly = true;
            this.Nazwa.Width = 65;
            // 
            // DataWylotu
            // 
            this.DataWylotu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DataWylotu.DataPropertyName = "DataWylotu";
            this.DataWylotu.HeaderText = "DataWylotu";
            this.DataWylotu.Name = "DataWylotu";
            this.DataWylotu.ReadOnly = true;
            this.DataWylotu.Width = 88;
            // 
            // Od
            // 
            this.Od.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Od.DataPropertyName = "Od";
            this.Od.HeaderText = "Od";
            this.Od.Name = "Od";
            this.Od.ReadOnly = true;
            this.Od.Width = 46;
            // 
            // Do
            // 
            this.Do.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Do.DataPropertyName = "Do";
            this.Do.HeaderText = "Do";
            this.Do.Name = "Do";
            this.Do.ReadOnly = true;
            this.Do.Width = 46;
            // 
            // Strona
            // 
            this.Strona.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Strona.DataPropertyName = "Strona";
            this.Strona.HeaderText = "Strona";
            this.Strona.Name = "Strona";
            this.Strona.ReadOnly = true;
            this.Strona.Width = 63;
            // 
            // SearchCriteriaListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 319);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.dataGridViewSearchCriterias);
            this.Name = "SearchCriteriaListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lista kryteriów";
            this.Load += new System.EventHandler(this.SearchCriteriaListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaViewBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaViewBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearchCriterias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriasBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FlightsDataSet flightsDataSet;
        private System.Windows.Forms.BindingSource searchCriteriaViewBindingSource;
        private FlightsDataSetTableAdapters.SearchCriteria_ViewTableAdapter searchCriteria_ViewTableAdapter;
        private System.Windows.Forms.BindingSource searchCriteriaViewBindingSource1;
        private System.Windows.Forms.DataGridView dataGridViewSearchCriterias;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.BindingSource flightsBindingSource;
        private FlightsDataSetTableAdapters.FlightsTableAdapter flightsTableAdapter;
        private System.Windows.Forms.BindingSource searchCriteriasBindingSource;
        private FlightsDataSetTableAdapters.SearchCriteriasTableAdapter searchCriteriasTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nazwa;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataWylotu;
        private System.Windows.Forms.DataGridViewTextBoxColumn Od;
        private System.Windows.Forms.DataGridViewTextBoxColumn Do;
        private System.Windows.Forms.DataGridViewTextBoxColumn Strona;
    }
}
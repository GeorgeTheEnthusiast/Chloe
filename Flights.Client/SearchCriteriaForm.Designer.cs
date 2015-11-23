namespace Flights.Client
{
    partial class SearchCriteriaForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.searchCriteriaDataGridView = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nazwaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataWylotuDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.doDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.odDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.przewoźnikDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.searchCriteriaDataTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flightsDataSet = new Flights.Client.FlightsDataSet();
            this.fillByToolStrip = new System.Windows.Forms.ToolStrip();
            this.fillByToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.fKFlightSearchCriteriaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchCriteriasBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flightsDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchCriteriasTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.SearchCriteriasTableAdapter();
            this.flightsTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.FlightsTableAdapter();
            this.searchCriteriaDataTableTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.SearchCriteriaDataTableTableAdapter();
            this.searchCriteriaDataTableBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.buttonAddSearchCriteria = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaDataTableBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSet)).BeginInit();
            this.fillByToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fKFlightSearchCriteriaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriasBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaDataTableBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.searchCriteriaDataGridView);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(649, 453);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kryteria wyszukiwania";
            // 
            // searchCriteriaDataGridView
            // 
            this.searchCriteriaDataGridView.AllowUserToAddRows = false;
            this.searchCriteriaDataGridView.AllowUserToDeleteRows = false;
            this.searchCriteriaDataGridView.AllowUserToResizeRows = false;
            this.searchCriteriaDataGridView.AutoGenerateColumns = false;
            this.searchCriteriaDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.searchCriteriaDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nazwaDataGridViewTextBoxColumn,
            this.dataWylotuDataGridViewTextBoxColumn,
            this.doDataGridViewTextBoxColumn,
            this.odDataGridViewTextBoxColumn,
            this.przewoźnikDataGridViewTextBoxColumn});
            this.searchCriteriaDataGridView.DataSource = this.searchCriteriaDataTableBindingSource;
            this.searchCriteriaDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchCriteriaDataGridView.Location = new System.Drawing.Point(3, 16);
            this.searchCriteriaDataGridView.Name = "searchCriteriaDataGridView";
            this.searchCriteriaDataGridView.ReadOnly = true;
            this.searchCriteriaDataGridView.Size = new System.Drawing.Size(643, 434);
            this.searchCriteriaDataGridView.TabIndex = 0;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nazwaDataGridViewTextBoxColumn
            // 
            this.nazwaDataGridViewTextBoxColumn.DataPropertyName = "Nazwa";
            this.nazwaDataGridViewTextBoxColumn.HeaderText = "Nazwa";
            this.nazwaDataGridViewTextBoxColumn.Name = "nazwaDataGridViewTextBoxColumn";
            this.nazwaDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataWylotuDataGridViewTextBoxColumn
            // 
            this.dataWylotuDataGridViewTextBoxColumn.DataPropertyName = "Data wylotu";
            this.dataWylotuDataGridViewTextBoxColumn.HeaderText = "Data wylotu";
            this.dataWylotuDataGridViewTextBoxColumn.Name = "dataWylotuDataGridViewTextBoxColumn";
            this.dataWylotuDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // doDataGridViewTextBoxColumn
            // 
            this.doDataGridViewTextBoxColumn.DataPropertyName = "Do";
            this.doDataGridViewTextBoxColumn.HeaderText = "Do";
            this.doDataGridViewTextBoxColumn.Name = "doDataGridViewTextBoxColumn";
            this.doDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // odDataGridViewTextBoxColumn
            // 
            this.odDataGridViewTextBoxColumn.DataPropertyName = "Od";
            this.odDataGridViewTextBoxColumn.HeaderText = "Od";
            this.odDataGridViewTextBoxColumn.Name = "odDataGridViewTextBoxColumn";
            this.odDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // przewoźnikDataGridViewTextBoxColumn
            // 
            this.przewoźnikDataGridViewTextBoxColumn.DataPropertyName = "Przewoźnik";
            this.przewoźnikDataGridViewTextBoxColumn.HeaderText = "Przewoźnik";
            this.przewoźnikDataGridViewTextBoxColumn.Name = "przewoźnikDataGridViewTextBoxColumn";
            this.przewoźnikDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // searchCriteriaDataTableBindingSource
            // 
            this.searchCriteriaDataTableBindingSource.DataMember = "SearchCriteriaDataTable";
            this.searchCriteriaDataTableBindingSource.DataSource = this.flightsDataSet;
            // 
            // flightsDataSet
            // 
            this.flightsDataSet.DataSetName = "FlightsDataSet";
            this.flightsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // fillByToolStrip
            // 
            this.fillByToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillByToolStripButton});
            this.fillByToolStrip.Location = new System.Drawing.Point(0, 0);
            this.fillByToolStrip.Name = "fillByToolStrip";
            this.fillByToolStrip.Size = new System.Drawing.Size(756, 25);
            this.fillByToolStrip.TabIndex = 1;
            this.fillByToolStrip.Text = "fillByToolStrip";
            // 
            // fillByToolStripButton
            // 
            this.fillByToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fillByToolStripButton.Name = "fillByToolStripButton";
            this.fillByToolStripButton.Size = new System.Drawing.Size(159, 22);
            this.fillByToolStripButton.Text = "Kryteria wyszukiwania lotów";
            this.fillByToolStripButton.Click += new System.EventHandler(this.fillByToolStripButton_Click);
            // 
            // fKFlightSearchCriteriaBindingSource
            // 
            this.fKFlightSearchCriteriaBindingSource.DataMember = "FK_Flight_SearchCriteria";
            this.fKFlightSearchCriteriaBindingSource.DataSource = this.searchCriteriasBindingSource;
            // 
            // searchCriteriasBindingSource
            // 
            this.searchCriteriasBindingSource.DataMember = "SearchCriterias";
            this.searchCriteriasBindingSource.DataSource = this.flightsDataSet;
            // 
            // flightsDataSetBindingSource
            // 
            this.flightsDataSetBindingSource.DataSource = this.flightsDataSet;
            this.flightsDataSetBindingSource.Position = 0;
            // 
            // searchCriteriasTableAdapter
            // 
            this.searchCriteriasTableAdapter.ClearBeforeFill = true;
            // 
            // flightsTableAdapter
            // 
            this.flightsTableAdapter.ClearBeforeFill = true;
            // 
            // searchCriteriaDataTableTableAdapter
            // 
            this.searchCriteriaDataTableTableAdapter.ClearBeforeFill = true;
            // 
            // searchCriteriaDataTableBindingSource1
            // 
            this.searchCriteriaDataTableBindingSource1.DataMember = "SearchCriteriaDataTable";
            this.searchCriteriaDataTableBindingSource1.DataSource = this.flightsDataSetBindingSource;
            // 
            // buttonAddSearchCriteria
            // 
            this.buttonAddSearchCriteria.Location = new System.Drawing.Point(668, 29);
            this.buttonAddSearchCriteria.Name = "buttonAddSearchCriteria";
            this.buttonAddSearchCriteria.Size = new System.Drawing.Size(75, 23);
            this.buttonAddSearchCriteria.TabIndex = 2;
            this.buttonAddSearchCriteria.Text = "Dodaj";
            this.buttonAddSearchCriteria.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(668, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Usuń";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SearchCriteriaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 477);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonAddSearchCriteria);
            this.Controls.Add(this.fillByToolStrip);
            this.Controls.Add(this.groupBox1);
            this.Name = "SearchCriteriaForm";
            this.Text = "Flights.Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaDataTableBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSet)).EndInit();
            this.fillByToolStrip.ResumeLayout(false);
            this.fillByToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fKFlightSearchCriteriaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriasBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriaDataTableBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private FlightsDataSet flightsDataSet;
        private System.Windows.Forms.BindingSource searchCriteriasBindingSource;
        private FlightsDataSetTableAdapters.SearchCriteriasTableAdapter searchCriteriasTableAdapter;
        private System.Windows.Forms.BindingSource flightsDataSetBindingSource;
        private System.Windows.Forms.ToolStrip fillByToolStrip;
        private System.Windows.Forms.ToolStripButton fillByToolStripButton;
        private System.Windows.Forms.BindingSource fKFlightSearchCriteriaBindingSource;
        private FlightsDataSetTableAdapters.FlightsTableAdapter flightsTableAdapter;
        private System.Windows.Forms.DataGridView searchCriteriaDataGridView;
        private System.Windows.Forms.BindingSource searchCriteriaDataTableBindingSource;
        private FlightsDataSetTableAdapters.SearchCriteriaDataTableTableAdapter searchCriteriaDataTableTableAdapter;
        private System.Windows.Forms.BindingSource searchCriteriaDataTableBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nazwaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataWylotuDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn doDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn odDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn przewoźnikDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button buttonAddSearchCriteria;
        private System.Windows.Forms.Button button1;
    }
}


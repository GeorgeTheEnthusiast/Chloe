namespace Flights.Client
{
    partial class GroupsForm
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
            this.flightsDataSet = new Flights.Client.FlightsDataSet();
            this.receiverGroupsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.receiverGroupsTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.ReceiverGroupsTableAdapter();
            this.dataGridViewGroups = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiverGroupsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.dataGridViewReceivers = new System.Windows.Forms.DataGridView();
            this.emailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notificationReceiversBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.notificationReceiversTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.NotificationReceiversTableAdapter();
            this.dataGridViewJoined = new System.Windows.Forms.DataGridView();
            this.ColumnGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReceiver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notificationReceiversGroupsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flightsDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.currenciesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.currenciesTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.CurrenciesTableAdapter();
            this.notificationReceiversGroupsTableAdapter = new Flights.Client.FlightsDataSetTableAdapters.NotificationReceiversGroupsTableAdapter();
            this.buttonAddEmailToGroup = new System.Windows.Forms.Button();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.receiverGroupsIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notificationReceiversIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.receiverGroupsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.receiverGroupsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReceivers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notificationReceiversBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJoined)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notificationReceiversGroupsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // flightsDataSet
            // 
            this.flightsDataSet.DataSetName = "FlightsDataSet";
            this.flightsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // receiverGroupsBindingSource
            // 
            this.receiverGroupsBindingSource.DataMember = "ReceiverGroups";
            this.receiverGroupsBindingSource.DataSource = this.flightsDataSet;
            // 
            // receiverGroupsTableAdapter
            // 
            this.receiverGroupsTableAdapter.ClearBeforeFill = true;
            // 
            // dataGridViewGroups
            // 
            this.dataGridViewGroups.AutoGenerateColumns = false;
            this.dataGridViewGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn});
            this.dataGridViewGroups.DataSource = this.receiverGroupsBindingSource1;
            this.dataGridViewGroups.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewGroups.MultiSelect = false;
            this.dataGridViewGroups.Name = "dataGridViewGroups";
            this.dataGridViewGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGroups.Size = new System.Drawing.Size(263, 192);
            this.dataGridViewGroups.TabIndex = 2;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Nazwa grupy";
            this.nameDataGridViewTextBoxColumn.MaxInputLength = 50;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // receiverGroupsBindingSource1
            // 
            this.receiverGroupsBindingSource1.DataMember = "ReceiverGroups";
            this.receiverGroupsBindingSource1.DataSource = this.flightsDataSet;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(242, 458);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // dataGridViewReceivers
            // 
            this.dataGridViewReceivers.AutoGenerateColumns = false;
            this.dataGridViewReceivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReceivers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.emailDataGridViewTextBoxColumn});
            this.dataGridViewReceivers.DataSource = this.notificationReceiversBindingSource;
            this.dataGridViewReceivers.Location = new System.Drawing.Point(281, 12);
            this.dataGridViewReceivers.MultiSelect = false;
            this.dataGridViewReceivers.Name = "dataGridViewReceivers";
            this.dataGridViewReceivers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewReceivers.Size = new System.Drawing.Size(272, 192);
            this.dataGridViewReceivers.TabIndex = 4;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            this.emailDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            this.emailDataGridViewTextBoxColumn.HeaderText = "Email";
            this.emailDataGridViewTextBoxColumn.MaxInputLength = 255;
            this.emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            // 
            // notificationReceiversBindingSource
            // 
            this.notificationReceiversBindingSource.DataMember = "NotificationReceivers";
            this.notificationReceiversBindingSource.DataSource = this.flightsDataSet;
            // 
            // notificationReceiversTableAdapter
            // 
            this.notificationReceiversTableAdapter.ClearBeforeFill = true;
            // 
            // dataGridViewJoined
            // 
            this.dataGridViewJoined.AutoGenerateColumns = false;
            this.dataGridViewJoined.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewJoined.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnGroup,
            this.ColumnReceiver,
            this.idDataGridViewTextBoxColumn,
            this.receiverGroupsIdDataGridViewTextBoxColumn,
            this.notificationReceiversIdDataGridViewTextBoxColumn,
            this.createdDataGridViewTextBoxColumn});
            this.dataGridViewJoined.DataSource = this.notificationReceiversGroupsBindingSource;
            this.dataGridViewJoined.Location = new System.Drawing.Point(13, 239);
            this.dataGridViewJoined.MultiSelect = false;
            this.dataGridViewJoined.Name = "dataGridViewJoined";
            this.dataGridViewJoined.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewJoined.Size = new System.Drawing.Size(540, 213);
            this.dataGridViewJoined.TabIndex = 5;
            // 
            // ColumnGroup
            // 
            this.ColumnGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnGroup.DataPropertyName = "GroupName";
            this.ColumnGroup.HeaderText = "Grupa";
            this.ColumnGroup.Name = "ColumnGroup";
            this.ColumnGroup.Width = 61;
            // 
            // ColumnReceiver
            // 
            this.ColumnReceiver.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnReceiver.DataPropertyName = "ReceiverName";
            this.ColumnReceiver.HeaderText = "Email";
            this.ColumnReceiver.Name = "ColumnReceiver";
            this.ColumnReceiver.Width = 57;
            // 
            // notificationReceiversGroupsBindingSource
            // 
            this.notificationReceiversGroupsBindingSource.DataMember = "NotificationReceiversGroups";
            this.notificationReceiversGroupsBindingSource.DataSource = this.flightsDataSetBindingSource;
            // 
            // flightsDataSetBindingSource
            // 
            this.flightsDataSetBindingSource.DataSource = this.flightsDataSet;
            this.flightsDataSetBindingSource.Position = 0;
            // 
            // currenciesBindingSource
            // 
            this.currenciesBindingSource.DataMember = "Currencies";
            this.currenciesBindingSource.DataSource = this.flightsDataSetBindingSource;
            // 
            // currenciesTableAdapter
            // 
            this.currenciesTableAdapter.ClearBeforeFill = true;
            // 
            // notificationReceiversGroupsTableAdapter
            // 
            this.notificationReceiversGroupsTableAdapter.ClearBeforeFill = true;
            // 
            // buttonAddEmailToGroup
            // 
            this.buttonAddEmailToGroup.Location = new System.Drawing.Point(202, 210);
            this.buttonAddEmailToGroup.Name = "buttonAddEmailToGroup";
            this.buttonAddEmailToGroup.Size = new System.Drawing.Size(150, 23);
            this.buttonAddEmailToGroup.TabIndex = 6;
            this.buttonAddEmailToGroup.Text = "Dodaj email do grupy";
            this.buttonAddEmailToGroup.UseVisualStyleBackColor = true;
            this.buttonAddEmailToGroup.Click += new System.EventHandler(this.buttonAddEmailToGroup_Click);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // receiverGroupsIdDataGridViewTextBoxColumn
            // 
            this.receiverGroupsIdDataGridViewTextBoxColumn.DataPropertyName = "ReceiverGroups_Id";
            this.receiverGroupsIdDataGridViewTextBoxColumn.HeaderText = "ReceiverGroups_Id";
            this.receiverGroupsIdDataGridViewTextBoxColumn.Name = "receiverGroupsIdDataGridViewTextBoxColumn";
            // 
            // notificationReceiversIdDataGridViewTextBoxColumn
            // 
            this.notificationReceiversIdDataGridViewTextBoxColumn.DataPropertyName = "NotificationReceivers_Id";
            this.notificationReceiversIdDataGridViewTextBoxColumn.HeaderText = "NotificationReceivers_Id";
            this.notificationReceiversIdDataGridViewTextBoxColumn.Name = "notificationReceiversIdDataGridViewTextBoxColumn";
            // 
            // createdDataGridViewTextBoxColumn
            // 
            this.createdDataGridViewTextBoxColumn.DataPropertyName = "Created";
            this.createdDataGridViewTextBoxColumn.HeaderText = "Created";
            this.createdDataGridViewTextBoxColumn.Name = "createdDataGridViewTextBoxColumn";
            // 
            // GroupsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 493);
            this.Controls.Add(this.buttonAddEmailToGroup);
            this.Controls.Add(this.dataGridViewJoined);
            this.Controls.Add(this.dataGridViewReceivers);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.dataGridViewGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupy";
            this.Load += new System.EventHandler(this.GroupsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.receiverGroupsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.receiverGroupsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReceivers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notificationReceiversBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJoined)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notificationReceiversGroupsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightsDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currenciesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FlightsDataSet flightsDataSet;
        private System.Windows.Forms.BindingSource receiverGroupsBindingSource;
        private FlightsDataSetTableAdapters.ReceiverGroupsTableAdapter receiverGroupsTableAdapter;
        private System.Windows.Forms.DataGridView dataGridViewGroups;
        private System.Windows.Forms.BindingSource receiverGroupsBindingSource1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dataGridViewReceivers;
        private System.Windows.Forms.BindingSource notificationReceiversBindingSource;
        private FlightsDataSetTableAdapters.NotificationReceiversTableAdapter notificationReceiversTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dataGridViewJoined;
        private System.Windows.Forms.BindingSource flightsDataSetBindingSource;
        private System.Windows.Forms.BindingSource currenciesBindingSource;
        private FlightsDataSetTableAdapters.CurrenciesTableAdapter currenciesTableAdapter;
        private System.Windows.Forms.BindingSource notificationReceiversGroupsBindingSource;
        private FlightsDataSetTableAdapters.NotificationReceiversGroupsTableAdapter notificationReceiversGroupsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReceiver;
        private System.Windows.Forms.Button buttonAddEmailToGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn receiverGroupsIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notificationReceiversIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdDataGridViewTextBoxColumn;
    }
}
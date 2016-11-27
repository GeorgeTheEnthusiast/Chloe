using System;

namespace Chloe.Client
{
    partial class SearchCriteriaAddForm
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
            this.comboBoxReceiverGroup = new System.Windows.Forms.ComboBox();
            this.receiverGroupsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ChloeDataSet = new Chloe.Client.ChloeDataSet();
            this.label1 = new System.Windows.Forms.Label();
            this.notificationReceiversBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.notificationReceiversTableAdapter = new Chloe.Client.ChloeDataSetTableAdapters.NotificationReceiversTableAdapter();
            this.receiverGroupsTableAdapter = new Chloe.Client.ChloeDataSetTableAdapters.ReceiverGroupsTableAdapter();
            this.checkedListBoxCitiesFrom = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkedListBoxCitiesTo = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerDepartureDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.checkedListBoxFlightWebsites = new System.Windows.Forms.CheckedListBox();
            this.buttonAddSearchCriteria = new System.Windows.Forms.Button();
            this.citiesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.citiesTableAdapter = new Chloe.Client.ChloeDataSetTableAdapters.CitiesTableAdapter();
            this.flightWebsitesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flightWebsitesTableAdapter = new Chloe.Client.ChloeDataSetTableAdapters.FlightWebsitesTableAdapter();
            this.netBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.netTableAdapter = new Chloe.Client.ChloeDataSetTableAdapters.NetTableAdapter();
            this.searchCriteriasBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchCriteriasTableAdapter = new Chloe.Client.ChloeDataSetTableAdapters.SearchCriteriasTableAdapter();
            this.buttonSetAllToDefaults = new System.Windows.Forms.Button();
            this.checkBoxChloeChange = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.receiverGroupsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChloeDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notificationReceiversBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.citiesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightWebsitesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.netBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriasBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxReceiverGroup
            // 
            this.comboBoxReceiverGroup.DataSource = this.receiverGroupsBindingSource;
            this.comboBoxReceiverGroup.DisplayMember = "Name";
            this.comboBoxReceiverGroup.FormattingEnabled = true;
            this.comboBoxReceiverGroup.Location = new System.Drawing.Point(106, 13);
            this.comboBoxReceiverGroup.Name = "comboBoxReceiverGroup";
            this.comboBoxReceiverGroup.Size = new System.Drawing.Size(170, 21);
            this.comboBoxReceiverGroup.TabIndex = 0;
            this.comboBoxReceiverGroup.ValueMember = "Id";
            // 
            // receiverGroupsBindingSource
            // 
            this.receiverGroupsBindingSource.DataMember = "ReceiverGroups";
            this.receiverGroupsBindingSource.DataSource = this.ChloeDataSet;
            // 
            // ChloeDataSet
            // 
            this.ChloeDataSet.DataSetName = "ChloeDataSet";
            this.ChloeDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Grupa odbiorców";
            // 
            // notificationReceiversBindingSource
            // 
            this.notificationReceiversBindingSource.DataMember = "NotificationReceivers";
            this.notificationReceiversBindingSource.DataSource = this.ChloeDataSet;
            // 
            // notificationReceiversTableAdapter
            // 
            this.notificationReceiversTableAdapter.ClearBeforeFill = true;
            // 
            // receiverGroupsTableAdapter
            // 
            this.receiverGroupsTableAdapter.ClearBeforeFill = true;
            // 
            // checkedListBoxCitiesFrom
            // 
            this.checkedListBoxCitiesFrom.FormattingEnabled = true;
            this.checkedListBoxCitiesFrom.Location = new System.Drawing.Point(76, 36);
            this.checkedListBoxCitiesFrom.Name = "checkedListBoxCitiesFrom";
            this.checkedListBoxCitiesFrom.Size = new System.Drawing.Size(200, 109);
            this.checkedListBoxCitiesFrom.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Miasta Od";
            // 
            // checkedListBoxCitiesTo
            // 
            this.checkedListBoxCitiesTo.FormattingEnabled = true;
            this.checkedListBoxCitiesTo.Location = new System.Drawing.Point(76, 152);
            this.checkedListBoxCitiesTo.Name = "checkedListBoxCitiesTo";
            this.checkedListBoxCitiesTo.Size = new System.Drawing.Size(200, 109);
            this.checkedListBoxCitiesTo.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Miasta Do";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 272);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Data";
            // 
            // dateTimePickerDepartureDate
            // 
            this.dateTimePickerDepartureDate.Location = new System.Drawing.Point(76, 268);
            this.dateTimePickerDepartureDate.MinDate = new System.DateTime(2015, 11, 27, 10, 31, 28, 610);
            this.dateTimePickerDepartureDate.Name = "dateTimePickerDepartureDate";
            this.dateTimePickerDepartureDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerDepartureDate.TabIndex = 3;
            this.dateTimePickerDepartureDate.Value = new System.DateTime(2015, 11, 27, 10, 31, 28, 610);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 306);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Stronki";
            // 
            // checkedListBoxFlightWebsites
            // 
            this.checkedListBoxFlightWebsites.FormattingEnabled = true;
            this.checkedListBoxFlightWebsites.Location = new System.Drawing.Point(77, 306);
            this.checkedListBoxFlightWebsites.Name = "checkedListBoxFlightWebsites";
            this.checkedListBoxFlightWebsites.Size = new System.Drawing.Size(200, 79);
            this.checkedListBoxFlightWebsites.TabIndex = 4;
            // 
            // buttonAddSearchCriteria
            // 
            this.buttonAddSearchCriteria.Location = new System.Drawing.Point(151, 426);
            this.buttonAddSearchCriteria.Name = "buttonAddSearchCriteria";
            this.buttonAddSearchCriteria.Size = new System.Drawing.Size(75, 23);
            this.buttonAddSearchCriteria.TabIndex = 6;
            this.buttonAddSearchCriteria.Text = "Dodaj";
            this.buttonAddSearchCriteria.UseVisualStyleBackColor = true;
            this.buttonAddSearchCriteria.Click += new System.EventHandler(this.buttonAddSearchCriteria_Click);
            // 
            // citiesBindingSource
            // 
            this.citiesBindingSource.DataMember = "Cities";
            this.citiesBindingSource.DataSource = this.ChloeDataSet;
            // 
            // citiesTableAdapter
            // 
            this.citiesTableAdapter.ClearBeforeFill = true;
            // 
            // flightWebsitesBindingSource
            // 
            this.flightWebsitesBindingSource.DataMember = "FlightWebsites";
            this.flightWebsitesBindingSource.DataSource = this.ChloeDataSet;
            // 
            // flightWebsitesTableAdapter
            // 
            this.flightWebsitesTableAdapter.ClearBeforeFill = true;
            // 
            // netBindingSource
            // 
            this.netBindingSource.DataMember = "Net";
            this.netBindingSource.DataSource = this.ChloeDataSet;
            // 
            // netTableAdapter
            // 
            this.netTableAdapter.ClearBeforeFill = true;
            // 
            // searchCriteriasBindingSource
            // 
            this.searchCriteriasBindingSource.DataMember = "SearchCriterias";
            this.searchCriteriasBindingSource.DataSource = this.ChloeDataSet;
            // 
            // searchCriteriasTableAdapter
            // 
            this.searchCriteriasTableAdapter.ClearBeforeFill = true;
            // 
            // buttonSetAllToDefaults
            // 
            this.buttonSetAllToDefaults.Location = new System.Drawing.Point(45, 426);
            this.buttonSetAllToDefaults.Name = "buttonSetAllToDefaults";
            this.buttonSetAllToDefaults.Size = new System.Drawing.Size(75, 23);
            this.buttonSetAllToDefaults.TabIndex = 5;
            this.buttonSetAllToDefaults.Text = "Resetuj";
            this.buttonSetAllToDefaults.UseVisualStyleBackColor = true;
            this.buttonSetAllToDefaults.Click += new System.EventHandler(this.buttonSetAllToDefaults_Click);
            // 
            // checkBoxChloeChange
            // 
            this.checkBoxChloeChange.AutoSize = true;
            this.checkBoxChloeChange.Location = new System.Drawing.Point(15, 394);
            this.checkBoxChloeChange.Name = "checkBoxChloeChange";
            this.checkBoxChloeChange.Size = new System.Drawing.Size(140, 17);
            this.checkBoxChloeChange.TabIndex = 9;
            this.checkBoxChloeChange.Text = "Uwzględnij 1 przesiadkę";
            this.checkBoxChloeChange.UseVisualStyleBackColor = true;
            // 
            // SearchCriteriaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 461);
            this.Controls.Add(this.checkBoxChloeChange);
            this.Controls.Add(this.buttonSetAllToDefaults);
            this.Controls.Add(this.buttonAddSearchCriteria);
            this.Controls.Add(this.checkedListBoxFlightWebsites);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePickerDepartureDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkedListBoxCitiesTo);
            this.Controls.Add(this.checkedListBoxCitiesFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxReceiverGroup);
            this.Name = "SearchCriteriaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dodawanie nowego kryterium";
            this.Load += new System.EventHandler(this.SearchCriteriaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.receiverGroupsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChloeDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notificationReceiversBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.citiesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flightWebsitesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.netBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchCriteriasBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxReceiverGroup;
        private System.Windows.Forms.Label label1;
        private ChloeDataSet ChloeDataSet;
        private System.Windows.Forms.BindingSource notificationReceiversBindingSource;
        private ChloeDataSetTableAdapters.NotificationReceiversTableAdapter notificationReceiversTableAdapter;
        private System.Windows.Forms.BindingSource receiverGroupsBindingSource;
        private ChloeDataSetTableAdapters.ReceiverGroupsTableAdapter receiverGroupsTableAdapter;
        private System.Windows.Forms.CheckedListBox checkedListBoxCitiesFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox checkedListBoxCitiesTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerDepartureDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox checkedListBoxFlightWebsites;
        private System.Windows.Forms.Button buttonAddSearchCriteria;
        private System.Windows.Forms.BindingSource citiesBindingSource;
        private ChloeDataSetTableAdapters.CitiesTableAdapter citiesTableAdapter;
        private System.Windows.Forms.BindingSource flightWebsitesBindingSource;
        private ChloeDataSetTableAdapters.FlightWebsitesTableAdapter flightWebsitesTableAdapter;
        private System.Windows.Forms.BindingSource netBindingSource;
        private ChloeDataSetTableAdapters.NetTableAdapter netTableAdapter;
        private System.Windows.Forms.BindingSource searchCriteriasBindingSource;
        private ChloeDataSetTableAdapters.SearchCriteriasTableAdapter searchCriteriasTableAdapter;
        private System.Windows.Forms.Button buttonSetAllToDefaults;
        private System.Windows.Forms.CheckBox checkBoxChloeChange;
    }
}
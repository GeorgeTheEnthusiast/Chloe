using System;

namespace Flights.Client
{
    partial class AddNewSearchCriteriaForm
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
            this.comboBoxCityFrom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxCityTo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerDepartureDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxCarrier = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxReceiverGroup = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxCityFrom
            // 
            this.comboBoxCityFrom.FormattingEnabled = true;
            this.comboBoxCityFrom.Location = new System.Drawing.Point(82, 44);
            this.comboBoxCityFrom.Name = "comboBoxCityFrom";
            this.comboBoxCityFrom.Size = new System.Drawing.Size(190, 21);
            this.comboBoxCityFrom.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Do";
            // 
            // comboBoxCityTo
            // 
            this.comboBoxCityTo.FormattingEnabled = true;
            this.comboBoxCityTo.Location = new System.Drawing.Point(82, 73);
            this.comboBoxCityTo.Name = "comboBoxCityTo";
            this.comboBoxCityTo.Size = new System.Drawing.Size(190, 21);
            this.comboBoxCityTo.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Data wylotu";
            // 
            // dateTimePickerDepartureDate
            // 
            this.dateTimePickerDepartureDate.Location = new System.Drawing.Point(83, 104);
            this.dateTimePickerDepartureDate.MinDate = DateTime.Now;
            this.dateTimePickerDepartureDate.Name = "dateTimePickerDepartureDate";
            this.dateTimePickerDepartureDate.Size = new System.Drawing.Size(189, 20);
            this.dateTimePickerDepartureDate.TabIndex = 5;
            this.dateTimePickerDepartureDate.Value = new System.DateTime(2015, 11, 23, 10, 37, 2, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Przewoźnik";
            // 
            // comboBoxCarrier
            // 
            this.comboBoxCarrier.FormattingEnabled = true;
            this.comboBoxCarrier.Location = new System.Drawing.Point(82, 12);
            this.comboBoxCarrier.Name = "comboBoxCarrier";
            this.comboBoxCarrier.Size = new System.Drawing.Size(190, 21);
            this.comboBoxCarrier.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Nazwa";
            // 
            // comboBoxReceiverGroup
            // 
            this.comboBoxReceiverGroup.FormattingEnabled = true;
            this.comboBoxReceiverGroup.Location = new System.Drawing.Point(82, 131);
            this.comboBoxReceiverGroup.Name = "comboBoxReceiverGroup";
            this.comboBoxReceiverGroup.Size = new System.Drawing.Size(190, 21);
            this.comboBoxReceiverGroup.TabIndex = 10;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(59, 172);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 11;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(164, 172);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Anuluj";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // AddNewSearchCriteriaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 209);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.comboBoxReceiverGroup);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxCarrier);
            this.Controls.Add(this.dateTimePickerDepartureDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxCityTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxCityFrom);
            this.Name = "AddNewSearchCriteriaForm";
            this.Text = "Dodaj nowe kryterium";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCityFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxCityTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerDepartureDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxCarrier;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxReceiverGroup;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}
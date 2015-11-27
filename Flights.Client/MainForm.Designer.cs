namespace Flights.Client
{
    partial class MainForm
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
            this.buttonSearchCriterias = new System.Windows.Forms.Button();
            this.buttonFlights = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSearchCriterias
            // 
            this.buttonSearchCriterias.Location = new System.Drawing.Point(101, 21);
            this.buttonSearchCriterias.Name = "buttonSearchCriterias";
            this.buttonSearchCriterias.Size = new System.Drawing.Size(75, 23);
            this.buttonSearchCriterias.TabIndex = 1;
            this.buttonSearchCriterias.Text = "Kryteria";
            this.buttonSearchCriterias.UseVisualStyleBackColor = true;
            this.buttonSearchCriterias.Click += new System.EventHandler(this.buttonSearchCriterias_Click);
            // 
            // buttonFlights
            // 
            this.buttonFlights.Location = new System.Drawing.Point(101, 65);
            this.buttonFlights.Name = "buttonFlights";
            this.buttonFlights.Size = new System.Drawing.Size(75, 23);
            this.buttonFlights.TabIndex = 2;
            this.buttonFlights.Text = "Wyniki";
            this.buttonFlights.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 103);
            this.Controls.Add(this.buttonFlights);
            this.Controls.Add(this.buttonSearchCriterias);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flights.Client";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSearchCriterias;
        private System.Windows.Forms.Button buttonFlights;
    }
}
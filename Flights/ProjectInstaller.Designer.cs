namespace Flights
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FlightsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.FlightsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // FlightsServiceProcessInstaller
            // 
            this.FlightsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FlightsServiceProcessInstaller.Password = null;
            this.FlightsServiceProcessInstaller.Username = null;
            // 
            // FlightsServiceInstaller
            // 
            this.FlightsServiceInstaller.Description = "Usługa sprawdzająca najtańsze połączenia między miastami";
            this.FlightsServiceInstaller.DisplayName = "Flights";
            this.FlightsServiceInstaller.ServiceName = "Flights";
            this.FlightsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.ServiceName = "Service1";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.FlightsServiceProcessInstaller,
            this.FlightsServiceInstaller,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller FlightsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller FlightsServiceInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}
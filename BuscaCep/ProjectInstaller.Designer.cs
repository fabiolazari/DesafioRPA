namespace BuscaCep
{
	partial class ProjectInstaller
	{
		/// <summary>
		/// Variável de designer necessária.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Limpar os recursos que estão sendo usados.
		/// </summary>
		/// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código gerado pelo Designer de Componentes

		/// <summary>
		/// Método necessário para suporte ao Designer - não modifique 
		/// o conteúdo deste método com o editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.serviceProcessInstallerCep = new System.ServiceProcess.ServiceProcessInstaller();
			this.serviceInstallerCep = new System.ServiceProcess.ServiceInstaller();
			// 
			// serviceProcessInstallerCep
			// 
			this.serviceProcessInstallerCep.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
			this.serviceProcessInstallerCep.Password = null;
			this.serviceProcessInstallerCep.Username = null;
			// 
			// serviceInstallerCep
			// 
			this.serviceInstallerCep.DisplayName = "ServiceCep";
			this.serviceInstallerCep.ServiceName = "ServiceCep";
			this.serviceInstallerCep.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
			this.serviceInstallerCep.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstallerCep_AfterInstall);
			// 
			// ProjectInstaller
			// 
			this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerCep,
            this.serviceInstallerCep});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerCep;
		private System.ServiceProcess.ServiceInstaller serviceInstallerCep;
	}
}
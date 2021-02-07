using BuscaCep.Entities;
using BuscaCep.Metodos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace BuscaCep
{
	public class Dados
	{
		public static bool Executou = true;
	}

	public partial class ServiceCep : ServiceBase
	{
		private static Thread threadMain;
		private static CancellationTokenSource cst;

		public ServiceCep()
		{
			InitializeComponent();
			this.ServiceName = "Servico.BuscaCep";
		}

		protected override void OnStart(string[] args)
		{
#if DEBUG
			while (DoWork()) ;
#else
            try
            {
                threadMain = new Thread(new ThreadStart(() => { while (DoWork()) ; }));
                threadMain.Start();

				Log.geraLogInformacao(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + "Processo Iniciado!");
            }
            catch (Exception e)
            {
				Log.geraLogErro(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + "erro no processo: " + e.Message + " -- " + e.StackTrace);
            }
#endif
		}

		public static bool DoWork()
		{
			DateTime data = DateTime.Now;
			int minuto = (1000 * 60);
			int horaAtual = data.Hour;
			int horaExecucao = Convert.ToInt32(ConfigurationManager.AppSettings["horaExecucao"].ToString());
			string CepFolder = ConfigurationManager.AppSettings["CepFolder"].ToString();

			if (horaAtual == horaExecucao) 
			{
				if (Dados.Executou)
				{
					Log.geraLogInformacao("Iniciou o processo de busca do CEP ...");

					List<Task> executa = new List<Task>();
					executa.Add(Task.Factory.StartNew(() =>
					{
						cst = new CancellationTokenSource();
						CancellationToken cancelToken = cst.Token;
						List<Resultado> resultado = Executa.BuscaCepWS(Executa.BuscaCepPlanilha(CepFolder));

						Log.geraLogInformacao("executou a busca...");

						ExportaPlanilha exporta = new ExportaPlanilha();
						string caminhoCompleto = CepFolder + @"\Resultado.xlsx";
						exporta.Exporta(caminhoCompleto, "Resultado", resultado);

					}));

					Task.WaitAny(executa.ToArray());
					executa.RemoveAll(t => t.Status != TaskStatus.Running);
					try
					{
						Task.WaitAll(executa.ToArray());
					}
					catch (AggregateException ae)
					{
						Log.geraLogErro("Uma ou mais exceções ocorreram: ");
						foreach (var ex in ae.Flatten().InnerExceptions)
							Log.geraLogErro("Erro: " + ex.Message);
					}
				}
			}

			Log.geraLogInformacao("Finalizou o processo de busca do CEP ...");

			Thread.Sleep(minuto * 5);
			return true;
		}

		protected override void OnStop()
		{
#if DEBUG

#else
            threadMain.Abort();
			Log.geraLogInformacao(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + "Processo Parado!");
#endif
		}

		public void OnDebug()
		{
			OnStart(null);
		}

	}
}

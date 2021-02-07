using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BuscaCep
{
	static class Program
	{
		/// <summary>
		/// Ponto de entrada principal para o aplicativo.
		/// </summary>
		static void Main()
		{
#if DEBUG
			ServiceCep myService = new ServiceCep();
			myService.OnDebug();
			System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceCep()
            };
            ServiceBase.Run(ServicesToRun);
#endif
		}
	}
}

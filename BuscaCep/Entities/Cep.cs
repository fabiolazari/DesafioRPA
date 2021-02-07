using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaCep.Entities
{
	public class Cep
	{
		public string CepInicial { get; set; }
		public string CepFinal { get; set; }

		public Cep() { }

		public Cep(string cepInicial, string cepFinal)
		{
			CepInicial = cepInicial;
			CepFinal = cepFinal;
		}
	}
}

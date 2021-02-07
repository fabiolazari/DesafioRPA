using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaCep.Entities
{
	public class Resultado
	{
		public string Logradouro { get; set; }
		public string Bairro { get; set; }
		public string Localidade { get; set; }
		public string Cep { get; set; }
		public string Observacao { get; set; }

		public Resultado()
		{
		}

		public Resultado(string logradouro, string bairro, string localidade, string cep, string observacao)
		{
			Logradouro = logradouro;
			Bairro = bairro;
			Localidade = localidade;
			Cep = cep;
			Observacao = observacao;
		}
	}
}

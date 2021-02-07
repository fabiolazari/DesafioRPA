using BuscaCep.Entities;
using BuscaCep.Metodos;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
using HtmlAgilityPack;

namespace BuscaCep
{
	public class Executa
	{
		public Executa() { }

		public static List<Cep> BuscaCepPlanilha(string CepFolder)
		{
			ImportaPlanilha importa = new ImportaPlanilha();
			string caminhoCompleto = CepFolder + @"\Lista_de_CEPs.xlsx";
			DataTable dt = importa.Importa(caminhoCompleto, "Lista de CEPs");
			List<Cep> cep = new List<Cep>();

			foreach (DataRow dr in dt.Rows)
			{
				//if (dt.Rows.IndexOf(dr) >= 1) --> Caso precise pular alguma linha inicial, testar, iniciando em 0.
				cep.Add(new Cep()
				{
					CepInicial = Convert.ToString(dr[1]),
					CepFinal = Convert.ToString(dr[2])
				});
			}
			return cep;
		}

		public static List<Resultado> BuscaCepWS(List<Cep> cep)
		{
			List<Resultado> resultado = new List<Resultado>();

			foreach (Cep c in cep)
			{
				if ((!String.IsNullOrEmpty(c.CepInicial)) && (!String.IsNullOrEmpty(c.CepFinal)))
				{
					for (int i = int.Parse(c.CepInicial); i <= int.Parse(c.CepFinal); i++)
					{
						try
						{
							var client = new RestClient(ConfigurationManager.AppSettings["UriCorreios"]);
							var request = new RestRequest(Method.POST)
							{
								AlwaysMultipartFormData = true,
							};
							request.AddParameter("relaxation", i.ToString());
							request.AddParameter("tipoCEP", "ALL");
							request.AddParameter("semelhante", "N");
							IRestResponse response = client.Execute(request);

							if (response.StatusCode == HttpStatusCode.OK)
							{
								HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument()
								{
									OptionFixNestedTags = true
								};

								htmlDoc.LoadHtml(response.Content);
								var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//td");
								if (htmlNodes != null)
								{
									List<string> campos = new List<string>();
									foreach (var node in htmlNodes)
									{
										campos.Add(node.InnerHtml.Replace("&nbsp;", ""));
									}

									resultado.Add(new Resultado
									{
										Logradouro = (campos.Count >= 1) ? campos[0] : "",
										Bairro = (campos.Count >= 2) ? campos[1] : "",
										Localidade = (campos.Count >= 3) ? campos[2] : "",
										Cep = (campos.Count >= 4) ? campos[3] : "",
										Observacao = ""
									});
								}
								else
								{
									resultado.Add(new Resultado
									{
										Logradouro = "",
										Bairro = "",
										Localidade = "",
										Cep = i.ToString(),
										Observacao = "DADOS NAO ENCONTRADOS"
									});
								}
							}
							else
							{
								resultado.Add(new Resultado
								{
									Logradouro = "",
									Bairro = "",
									Localidade = "",
									Cep = i.ToString(),
									Observacao = "ERRO NA BUSCA DO CEP NO WEB SERRVICE DOS CORREIOS"
								});
							}

							/*
							var httpCliente = new HttpClient();
							httpCliente.DefaultRequestHeaders.Add("relaxation", i.ToString());
							httpCliente.DefaultRequestHeaders.Add("tipoCEP", "ALL");
							httpCliente.DefaultRequestHeaders.Add("semelhante", "N");

							using (var response = await httpCliente.GetAsync(ConfigurationManager.AppSettings["UriCorreios"]))
							{
								if (response.IsSuccessStatusCode)
								{
									var ProdutoJsonString = await response.Content.ReadAsStringAsync();

									resultado = JsonConvert.DeserializeObject<Resultado>(ProdutoJsonString);

									lResultado.Add(new Resultado
									{
										Logradouro = resultado.Logradouro,
										Complemento = resultado.Complemento,
										Bairro = resultado.Bairro,
										Localidade = resultado.Localidade,
										Uf = resultado.Uf,
										Cep = i.ToString(),
										Observacao = ""
									});
								}
								else
								{
									lResultado.Add(new Resultado
									{
										Logradouro = "",
										Complemento = "",
										Bairro = "",
										Localidade = "",
										Uf = "",
										Cep = i.ToString(),
										Observacao = "ERRO AO PROCURAR CEP, INEXISTENTE NO CADASTRO DOS CORREIOS"
									});
								}
							}*/
						}
						catch (Exception e)
						{
							Log.geraLogErro("Cep: " + i.ToString() + " - Mensagem: " + e.Message.ToString() + " - Track: " + e.StackTrace.ToString());
						}
					}
				}
			}
			return resultado;
		}
	}
}

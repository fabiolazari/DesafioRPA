	public static List<Resultado> BuscaCepWS(List<Cep> cep)
		{
			List<Resultado> resultado = new List<Resultado>();

			foreach (Cep c in cep)
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
									campos.Add(node.InnerHtml);
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
					}
					catch (Exception e)
					{
						Log.geraLogErro("Cep: "+ i.ToString() + " - Mensagem: " + e.Message.ToString() + " - Track: " + e.StackTrace.ToString());
					}
				}
			}
			return resultado;
		}
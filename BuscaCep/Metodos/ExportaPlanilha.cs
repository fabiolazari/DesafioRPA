using BuscaCep.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaCep.Metodos
{
	public class ExportaPlanilha
	{
		public void Exporta(string caminhoArquivo, string nomePlanilha, List<Resultado> resultado)
		{
			try
			{
				OleDbConnection _conexaoPlanilha = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + caminhoArquivo + ";Extended Properties=Excel 12.0;Format=xlsx");
				_conexaoPlanilha.Open();
				OleDbCommand _CommandPlanilha = new OleDbCommand();
				_CommandPlanilha.Connection = _conexaoPlanilha;
				_CommandPlanilha.CommandText = String.Format(@"CREATE TABLE {0} 
																	(Logradouro TEXT(50),
																		Bairro TEXT(50),
																		Localidade TEXT(30),
																		Cep TEXT(10),
																		Observacao TEXT(80))", nomePlanilha);

				_CommandPlanilha.ExecuteNonQuery();

				foreach (Resultado r in resultado)
				{
					_CommandPlanilha.CommandText = String.Format(@"INSERT INTO [{0}$]  (Logradouro, Bairro, Localidade, Cep, Observacao)
																   VALUES ('{1}', '{2}', '{3}', '{4}', '{5}') ",
														nomePlanilha, r.Logradouro, r.Bairro, r.Localidade, r.Cep, r.Observacao);
					_CommandPlanilha.ExecuteNonQuery();
				}
				_conexaoPlanilha.Close();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaCep.Metodos
{
	public class ImportaPlanilha
	{
		public DataTable Importa(string caminhoArquivo, string nomePlanilha)
		{
			DataTable dt = new DataTable();
			OleDbConnection _conexaoPlanilha = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + caminhoArquivo + ";Extended Properties=Excel 12.0;");
			OleDbDataAdapter _CommandPlanilha = new OleDbDataAdapter("SELECT * FROM [" + nomePlanilha + "$]", _conexaoPlanilha);
			_CommandPlanilha.TableMappings.Add("Table", "Table");

			try
			{
				_CommandPlanilha.Fill(dt);
				_conexaoPlanilha.Close();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return dt;
		}
	}
}

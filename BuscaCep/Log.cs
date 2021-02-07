using System;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace BuscaCep
{
    public static class Log
    {
        public static string MeuServico = "Servico.BuscaCep";
        public static bool wrLog(string Valor, string dest, string ano, string mes, string dia, string hora, string min)
        {
            string zhora = "";
            string logFolder = ConfigurationManager.AppSettings["CepFolder"].ToString();

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            if ((hora == "") || (min == ""))
            {
                zhora = "";
            }
            else
            {
                zhora = "_" + hora + "_" + min;
            }

            string path = logFolder + "log_" + dest + "_" + ano + "_" + mes + "_" + dia + zhora + ".txt";

            try
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(Valor);
                    }
                    return true;
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(Valor);
                    }
                    return true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return false;
            }
        }

        public static void geraLogInformacao(string mensagem)
        {

            if (!EventLog.SourceExists(MeuServico))
            {
                EventLog.CreateEventSource(MeuServico, "Application");
            }

            EventLog myLog = new EventLog();
            myLog.Source = MeuServico;
            EventLog.WriteEntry(MeuServico, mensagem, EventLogEntryType.Information);
            return;
        }


        public static void geraLogErro(string mensagem)
        {

            if (!EventLog.SourceExists(MeuServico))
            {
                EventLog.CreateEventSource(MeuServico, "Application");
                return;
            }

            EventLog myLog = new EventLog();
            myLog.Source = MeuServico;
            EventLog.WriteEntry(MeuServico, mensagem, EventLogEntryType.Error);
        }
    }
}

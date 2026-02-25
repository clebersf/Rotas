using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading;

namespace Vale.Tops.Integration.Infrastructure.TriggerSp
{
    class OptionExecSp
    {
        //Declare a variável global do tipo StreamWriter
        static StreamWriter arquivoLog;
        public ThreadStart ExecSpNPar()
        {
            int i = 1;
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectDB"].ConnectionString);
            SqlCommand comando = new SqlCommand(ConfigurationManager.AppSettings["NameSp"], cn);
            comando.CommandType = CommandType.StoredProcedure;
            if (ConfigurationManager.AppSettings != null)
            {
                while (i <= ConfigurationManager.AppSettings.Count)
                {
                    if (ConfigurationManager.AppSettings["ParVal" + i.ToString()] != "")
                    {
                        comando.Parameters.Add('@' + ConfigurationManager.AppSettings["Par" + i.ToString()], SqlDbType.VarChar).Value = ConfigurationManager.AppSettings["ParVal" + i.ToString()];
                    }
                    else
                    {
                        break;
                    }
                    i++;                    
                }
            }

            comando.Connection.Open();
            comando.ExecuteNonQuery();
            comando.Connection.Close();
            return new ThreadStart(() => { });
        }


    }
}

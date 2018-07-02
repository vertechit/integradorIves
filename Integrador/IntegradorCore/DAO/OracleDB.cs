using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Services;
using Oracle.ManagedDataAccess.Client;

namespace IntegradorCore.DAO
{
    public class OracleDB
    {
        private static OracleConnection GetConnection()
        {
            string oradb = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=host1)(PORT=port1))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=servicename1)));User ID=user1;Password=password1;";

            oradb = oradb.Replace("host1", StaticParametersDB.GetHost());
            oradb = oradb.Replace("port1", StaticParametersDB.GetPort());
            oradb = oradb.Replace("servicename1", StaticParametersDB.GetServiceName());
            oradb = oradb.Replace("user1", StaticParametersDB.GetUser());
            oradb = oradb.Replace("password1", StaticParametersDB.GetPassword());

            return new OracleConnection(oradb);
        }

        public static void GetData()
        {
            try
            {
                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();

                    using (var comm = new OracleCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "SELECT ID, XMLEVENTO FROM ZMDATVIVES_EVENTOS_ESOCIAL WHERE NROPROTOCOLO IS NULL";

                        var adapter = new OracleDataAdapter(comm);
                        var dataTable = new System.Data.DataTable();

                        adapter.Fill(dataTable);

                        foreach (System.Data.DataRow row in dataTable.Rows)
                        {
                            Armazenamento.AddProtocoloDB(
                                new Modelos.ProtocoloDB { idEvento = Convert.ToString(row["ID"]), xmlEvento = Convert.ToString(row["XMLEVENTO"]) }
                            );
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SetResponseEnvio()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void SetResponseConsulta()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void teste()
        {
            try
            {

                

                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();
                    Console.Write("Conexão efetuada com sucesso\n");
                    using (var comm = new OracleCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "SELECT * FROM ZMDATVIVES_EVENTOS_ESOCIAL";

                        /*Console.WriteLine("DataReader:");
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("ID {0}", reader["ID"]);
                            }
                        }*/

                        Console.WriteLine("DataAdapter:");
                        var adapter = new OracleDataAdapter(comm);
                        var dataTable = new System.Data.DataTable();
                        adapter.Fill(dataTable);
                        foreach (System.Data.DataRow row in dataTable.Rows)
                        {
                            Console.WriteLine("ID: {0}", row["XMLEVENTO"]);
                        }
                    }
                    conn.Close();
                }
            }
            // Retorna erro
            catch (Exception ex)
            {
                // Mostra menssagem de erro
                Console.Write(ex.ToString());

            }
        }
    }
}

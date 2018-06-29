using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace IntegradorCore.DAO
{
    public class OracleDB
    {
        public OracleConnection GetConnection()
        {
            string oradb = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=host)(PORT=port))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=servicename)));User ID=user;Password=password;";

            return new OracleConnection(oradb);
        }

        public void GetData()
        {
            try
            {
                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();

                    using (var comm = new OracleCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "SELECT ID, XMLEVENTO FROM ZMDATVIVES_EVENTOS_ESOCIAL WHERE NRPROTOCOLO IS NULL";

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

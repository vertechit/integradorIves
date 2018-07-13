using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using IntegradorCore.NHibernate;
using IntegradorCore.Services;
using NHibernate;
using IntegradorCore.NHibernate.DAO;
using IntegradorCore.Modelos;
using System.Windows.Forms;

namespace IntegradorCore.DAO
{
    public class SQLServerDB
    {
        private static SqlConnection GetConnection()
        {
            var strconnection = "Data Source=host,port;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword;";

            strconnection = strconnection.Replace("host", StaticParametersDB.GetHost());
            strconnection = strconnection.Replace("port", StaticParametersDB.GetPort());
            strconnection = strconnection.Replace("myDataBase", StaticParametersDB.GetServiceName());
            strconnection = strconnection.Replace("myUsername", StaticParametersDB.GetUser());
            strconnection = strconnection.Replace("myPassword", StaticParametersDB.GetPassword());

            return new SqlConnection(strconnection);
        }

        public static void GetData(ISession sessao)
        {
            var ProtocoloDAO = new ProtocoloDB_DAO(sessao);

            using (SqlConnection conn = GetConnection())
            {

                try
                {
                    conn.Open();

                    using (var comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "SELECT ID, XMLEVENTO FROM ZMDATVIVES_EVENTOS_ESOCIAL WHERE NROPROTOCOLO IS NULL";

                        var adapter = new SqlDataAdapter(comm);
                        var dataTable = new System.Data.DataTable();

                        adapter.Fill(dataTable);

                        foreach (System.Data.DataRow row in dataTable.Rows)
                        {
                            var prot = new Modelos.ProtocoloDB { idEvento = Convert.ToString(row["ID"]), xmlEvento = Convert.ToString(row["XMLEVENTO"]), driver = "sqlserver" };
                            ProtocoloDAO.Salvar(prot);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExDriverSQLServer(2, ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private static string CriaSQL(ProtocoloDB prot, int tipo)
        {
            var quote = '\'';

            if (tipo == 1)
            {
                var sql = "UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :Nrprot, XMLPROTOCOLO = :Xmlprot, MENSAGEMERRO = :Erro  WHERE ID = :Id";
                sql = sql.Replace(":Nrprot", string.Concat(quote + prot.nroProt + quote));
                sql = sql.Replace(":Xmlprot", string.Concat(quote + (prot.xmlProt = prot.xmlProt.Replace("> <", "><")) + quote));
                sql = sql.Replace(":Erro", string.Concat(quote + prot.erros + quote));
                sql = sql.Replace(":Id", string.Concat(quote + prot.idEvento + quote));

                return sql;
            }
            else
            {
                var sql = "UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :nrprot, XMLPROTOCOLO = :xmlprot, NRORECIBO = :nroRec, XMLRECIBO = :xmlRec WHERE ID = :Id";
                sql = sql.Replace(":nrprot", string.Concat(quote + prot.nroProt + quote));
                sql = sql.Replace(":xmlprot", string.Concat(quote + prot.xmlProt + quote));
                sql = sql.Replace(":nroRec", string.Concat(quote + prot.nroRec + quote));
                sql = sql.Replace(":xmlRec", string.Concat(quote + prot.xmlRec + quote));
                sql = sql.Replace(":Id", string.Concat(quote + prot.idEvento + quote));

                return sql;
            }
        }

        public static bool TesteConexao(string host, string port, string servicename, string user, string password)
        {
            var strconnection = "Data Source=host,port;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword;";

            strconnection = strconnection.Replace("host", host);
            strconnection = strconnection.Replace("port", port);
            strconnection = strconnection.Replace("myDataBase", servicename);
            strconnection = strconnection.Replace("myUsername", user);
            strconnection = strconnection.Replace("myPassword", password);

            var retorno = true;

            using (SqlConnection conn = new SqlConnection(strconnection))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    retorno = false;
                }
                finally
                {
                    conn.Close();
                }
            }

            return retorno;
        }

        public static bool UpdateDB(ProtocoloDB prot)
        {
            bool retorno = true;

            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    using (var comm = new SqlCommand())
                    {
                        if (prot.nroRec == null || prot.nroRec == "")
                        {
                            var sql = CriaSQL(prot, 1);
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = sql;
                            comm.ExecuteNonQuery();
                        }
                        else
                        {
                            var sql = CriaSQL(prot, 2);
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = sql;
                            comm.ExecuteNonQuery();
                        }

                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExDriverSQLServer(1, ex.Message);

                    retorno = false;
                }
                finally
                {
                    conn.Close();
                }
            }

            return retorno;
        }
    }
}

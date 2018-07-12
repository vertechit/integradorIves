﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IntegradorCore.NHibernate;
using IntegradorCore.Services;
using NHibernate;
using IntegradorCore.NHibernate.DAO;
using IntegradorCore.Modelos;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace IntegradorCore.DAO
{
    public class Banco
    {
        private static dynamic GetConnection()
        {
            if(StaticParametersDB.GetDriver() == "oracle")
            {
                string oradb = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=host1)(PORT=port1))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=servicename1)));User ID=user1;Password=password1;";

                oradb = oradb.Replace("host1", StaticParametersDB.GetHost());
                oradb = oradb.Replace("port1", StaticParametersDB.GetPort());
                oradb = oradb.Replace("servicename1", StaticParametersDB.GetServiceName());
                oradb = oradb.Replace("user1", StaticParametersDB.GetUser());
                oradb = oradb.Replace("password1", StaticParametersDB.GetPassword());

                return new OracleConnection(oradb);
            }
            else
            {
                var strconnection = "Data Source=host,port;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword;";

                strconnection = strconnection.Replace("host", StaticParametersDB.GetHost());
                strconnection = strconnection.Replace("port", StaticParametersDB.GetPort());
                strconnection = strconnection.Replace("myDataBase", StaticParametersDB.GetServiceName());
                strconnection = strconnection.Replace("myUsername", StaticParametersDB.GetUser());
                strconnection = strconnection.Replace("myPassword", StaticParametersDB.GetPassword());

                return new SqlConnection(strconnection);
            }
            
        }

        private static dynamic GetConnectionWithParam(string host, string port, string servicename, string user, string password, string driver)
        {
            if (driver.ToLower() == "oracle")
            {
                string oradb = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=host1)(PORT=port1))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=servicename1)));User ID=user1;Password=password1;";

                oradb = oradb.Replace("host1", host);
                oradb = oradb.Replace("port1", port);
                oradb = oradb.Replace("servicename1", servicename);
                oradb = oradb.Replace("user1", user);
                oradb = oradb.Replace("password1", password);

                return new OracleConnection(oradb);
            }
            else
            {
                var strconnection = "Data Source=host,port;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword;";

                strconnection = strconnection.Replace("host", host);
                strconnection = strconnection.Replace("port", port);
                strconnection = strconnection.Replace("myDataBase", servicename);
                strconnection = strconnection.Replace("myUsername", user);
                strconnection = strconnection.Replace("myPassword", password);

                return new SqlConnection(strconnection);
            }
        }

        private static dynamic GetCommand()
        {
            if (StaticParametersDB.GetDriver() == "oracle")
            {
                return new OracleCommand();
            }
            else
            {
                return new SqlCommand();
            }
        }

        private static dynamic GetAdapter(dynamic comm)
        {
            if (StaticParametersDB.GetDriver() == "oracle")
            {
                return new OracleDataAdapter(comm);
            }
            else
            {
                return new SqlDataAdapter(comm);
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

        public static bool TesteConexao(string host, string port, string servicename, string user, string password, string driver)
        {
            var retorno = true;

            using (var conn = GetConnectionWithParam(host, port, servicename, user, password, driver))
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

        public static void GetData(ISession sessao)
        {
            var ProtocoloDAO = new ProtocoloDB_DAO(sessao);

            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    using (var comm = GetCommand()) //funcion here
                    {
                        comm.Connection = conn;
                        comm.CommandText = "SELECT ID, XMLEVENTO FROM ZMDATVIVES_EVENTOS_ESOCIAL WHERE NROPROTOCOLO IS NULL";

                        var adapter = GetAdapter(comm);//new SqlDataAdapter(comm); //funcion here
                        var dataTable = new System.Data.DataTable();

                        adapter.Fill(dataTable);

                        foreach (System.Data.DataRow row in dataTable.Rows)
                        {
                            var prot = new Modelos.ProtocoloDB { idEvento = Convert.ToString(row["ID"]), xmlEvento = Convert.ToString(row["XMLEVENTO"]), driver = StaticParametersDB.GetDriver() };
                            ProtocoloDAO.Salvar(prot);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExBanco(1, ex.Message, StaticParametersDB.GetDriver());
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public static bool UpdateDB(ProtocoloDB prot)
        {
            bool retorno = true;

            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    using (var comm = GetCommand())
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
                    e.ExBanco(2, ex.Message, StaticParametersDB.GetDriver());

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

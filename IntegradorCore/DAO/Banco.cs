using System;
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
                if (StaticParametersDB.GetPort() != "0")
                {
                    var strconnection = "Data Source=host,port;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword;";

                    strconnection = strconnection.Replace("host", StaticParametersDB.GetHost());
                    strconnection = strconnection.Replace("port", StaticParametersDB.GetPort());
                    strconnection = strconnection.Replace("myDataBase", StaticParametersDB.GetServiceName());
                    strconnection = strconnection.Replace("myUsername", StaticParametersDB.GetUser());
                    strconnection = strconnection.Replace("myPassword", StaticParametersDB.GetPassword());

                    return new SqlConnection(strconnection);
                }
                else
                {
                    var strconnection = "Server=myInstanceName;Database=myDataBase;Trusted_Connection=False;User Id=myUsername;Password = myPassword; ";

                    strconnection = strconnection.Replace("myInstanceName", StaticParametersDB.GetHost());
                    //strconnection = strconnection.Replace("port", port);
                    strconnection = strconnection.Replace("myDataBase", StaticParametersDB.GetServiceName());
                    strconnection = strconnection.Replace("myUsername", StaticParametersDB.GetUser());
                    strconnection = strconnection.Replace("myPassword", StaticParametersDB.GetPassword());
                    return new SqlConnection(strconnection);
                }
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
                if(port != "0")
                {
                    var strconnection = "Data Source=host,port;Network Library=DBMSSOCN;Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword;";

                    strconnection = strconnection.Replace("host", host);
                    strconnection = strconnection.Replace("port", port);
                    strconnection = strconnection.Replace("myDataBase", servicename);
                    strconnection = strconnection.Replace("myUsername", user);
                    strconnection = strconnection.Replace("myPassword", password);

                    return new SqlConnection(strconnection);
                }
                else
                {
                    var strconnection = "Server=myInstanceName;Database=myDataBase;Trusted_Connection=True;User Id=myUsername;Password = myPassword; ";

                    strconnection = strconnection.Replace("myInstanceName", host);
                    //strconnection = strconnection.Replace("port", port);
                    strconnection = strconnection.Replace("myDataBase", servicename);
                    strconnection = strconnection.Replace("myUsername", user);
                    strconnection = strconnection.Replace("myPassword", password);

                    return new SqlConnection(strconnection);
                }
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
            string format = "dd-MM-yyyy hh:mm:ss";

            if (tipo == 1)
            {
                var data = RetornaArrayData(prot.dtconsulta);
                var sql = "UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :Nrprot, XMLPROTOCOLO = :Xmlprot, MENSAGEMERRO = :Erro, DATARETORNO = :Dtretorno, HORARETORNO = :Hrretorno, STATUS = :Status  WHERE ID = :Idevento AND IDSEQ = :Idseq";
                sql = sql.Replace(":Nrprot", string.Concat(quote + prot.nroProt + quote));
                sql = sql.Replace(":Xmlprot", string.Concat(quote + (prot.xmlProt = prot.xmlProt.Replace("> <", "><")) + quote));
                sql = sql.Replace(":Erro", string.Concat(quote + prot.erros + quote));
                if (StaticParametersDB.GetDriver() == "oracle")
                {
                    sql = sql.Replace(":Dtretorno", "trunc(SYSDATE)");
                }
                else
                {
                    sql = sql.Replace(":Dtretorno", string.Concat(quote + data.ToString(format) + quote));
                }
                sql = sql.Replace(":Hrretorno", string.Concat(quote + prot.hrconsulta + quote));
                sql = sql.Replace(":Status", string.Concat(quote + prot.status + quote));
                sql = sql.Replace(":Idevento", string.Concat(quote + prot.idEvento + quote));
                sql = sql.Replace(":Idseq", prot.idSeq);

                return sql;
            }
            else if (tipo == 2)
            {
                var data = RetornaArrayData(prot.dtconsulta);
                var sql = "UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :nrprot, XMLPROTOCOLO = :xmlprot, NRORECIBO = :nroRec, XMLRECIBO = :xmlRec, DATARETORNO = :Dtretorno, HORARETORNO = :Hrretorno, STATUS = :Status WHERE ID = :Idevento AND IDSEQ = :Idseq";
                sql = sql.Replace(":nrprot", string.Concat(quote + prot.nroProt + quote));
                sql = sql.Replace(":xmlprot", string.Concat(quote + prot.xmlProt + quote));
                sql = sql.Replace(":nroRec", string.Concat(quote + prot.nroRec + quote));
                sql = sql.Replace(":xmlRec", string.Concat(quote + prot.xmlRec + quote));
                if (StaticParametersDB.GetDriver() == "oracle")
                {
                    sql = sql.Replace(":Dtretorno", "trunc(SYSDATE)");
                }
                else
                {
                    sql = sql.Replace(":Dtretorno", string.Concat(quote + data.ToString(format) + quote));
                }
                sql = sql.Replace(":Hrretorno", string.Concat(quote + prot.hrconsulta + quote));
                sql = sql.Replace(":Status", string.Concat(quote + prot.status + quote));
                sql = sql.Replace(":Idevento", string.Concat(quote + prot.idEvento + quote));
                sql = sql.Replace(":Idseq", prot.idSeq);

                return sql;
            }
            else if(tipo == 3)//novo
            {
                var data = RetornaArrayData(prot.dtenvio);
                var sql = "UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :Nrprot, XMLPROTOCOLO = :Xmlprot, STATUS = :Status, DATAENVIO = :Dtenvio, HORAENVIO = :Hrenvio  WHERE ID = :Idevento AND IDSEQ = :Idseq";
                sql = sql.Replace(":Nrprot", string.Concat(quote + prot.nroProt + quote));
                sql = sql.Replace(":Xmlprot", string.Concat(quote + prot.xmlProt + quote));
                sql = sql.Replace(":Status", string.Concat(quote + prot.status + quote));
                if (StaticParametersDB.GetDriver() == "oracle")
                {
                    sql = sql.Replace(":Dtenvio", "trunc(SYSDATE)");
                }
                else
                {
                    sql = sql.Replace(":Dtenvio", string.Concat(quote + data.ToString(format) + quote));
                }
                sql = sql.Replace(":Hrenvio", string.Concat(quote + prot.hrenvio + quote));
                sql = sql.Replace(":Idevento", string.Concat(quote + prot.idEvento + quote));
                sql = sql.Replace(":Idseq", prot.idSeq);

                return sql;
            }
            else
            {
                var data = RetornaArrayData(prot.dtconsulta);
                var sql = "UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET DATARETORNO = :Dtretorno, HORARETORNO = :Hrretorno, STATUS = :Status  WHERE ID = :Idevento AND IDSEQ = :Idseq";
                if (StaticParametersDB.GetDriver() == "oracle")
                {
                    sql = sql.Replace(":Dtretorno", "trunc(SYSDATE)");
                }
                else
                {
                    sql = sql.Replace(":Dtretorno", string.Concat(quote + data.ToString(format) + quote));
                }
                sql = sql.Replace(":Hrretorno", string.Concat(quote + prot.hrconsulta + quote));
                sql = sql.Replace(":Status", string.Concat(quote + prot.status + quote));
                sql = sql.Replace(":Idevento", string.Concat(quote + prot.idEvento + quote));
                sql = sql.Replace(":Idseq", prot.idSeq);

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

                    using (var comm = GetCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "SELECT ID, XMLEVENTO, IDSEQ FROM ZMDATVIVES_EVENTOS_ESOCIAL WHERE NROPROTOCOLO IS NULL";

                        var adapter = GetAdapter(comm);
                        var dataTable = new System.Data.DataTable();

                        adapter.Fill(dataTable);
                        Processos proc = new Processos();
                        foreach (System.Data.DataRow row in dataTable.Rows)
                        {
                            var Base = proc.DefineBaseEnvioDB(Convert.ToString(row["XMLEVENTO"]));
                            var prot = new ProtocoloDB { id = string.Concat(Convert.ToString(row["ID"]), "-", Convert.ToString(row["IDSEQ"])),
                                                         idEvento = Convert.ToString(row["ID"]), idSeq = Convert.ToString(row["IDSEQ"]),
                                                         xmlEvento = Convert.ToString(row["XMLEVENTO"]),
                                                         driver = StaticParametersDB.GetDriver(),
                                                         baseEnv = Convert.ToString(Base) };
                            ProtocoloDAO.Salvar(prot);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExBanco(1, ex.Message, StaticParametersDB.GetDriver(), ex);
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
                            if (StaticParametersDB.GetDriver() == "sqlserver")
                            {
                                comm.CommandText = "SET DATEFORMAT dmy";
                                comm.ExecuteNonQuery();
                            }
                            comm.CommandText = sql;
                            comm.ExecuteNonQuery();
                        }
                        else
                        {
                            var sql = CriaSQL(prot, 2);
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            if (StaticParametersDB.GetDriver() == "sqlserver")
                            {
                                comm.CommandText = "SET DATEFORMAT dmy";
                                comm.ExecuteNonQuery();
                            }
                            comm.CommandText = sql;
                            comm.ExecuteNonQuery();
                        }

                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExBanco(2, ex.Message, StaticParametersDB.GetDriver(), ex);

                    retorno = false;
                }
                finally
                {
                    conn.Close();
                }
            }

            return retorno;
        }

        public static void CustomUpdateDB(ProtocoloDB prot, int tipo)
        {

            using (var conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    using (var comm = GetCommand())
                    {

                        var sql = CriaSQL(prot, tipo);
                        comm.Connection = conn;
                        comm.CommandType = CommandType.Text;

                        if(StaticParametersDB.GetDriver() == "sqlserver")
                        {
                            comm.CommandText = "SET DATEFORMAT dmy";
                            comm.ExecuteNonQuery();
                        }
                        
                        comm.CommandText = sql;
                        comm.ExecuteNonQuery();

                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExBanco(2, ex.Message, StaticParametersDB.GetDriver(), ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private static DateTime RetornaArrayData(string data)
        {
            int dia = Convert.ToInt32(data.Substring(0, 2));
            int mes = Convert.ToInt32(data.Substring(3, 2));
            int ano = Convert.ToInt32(data.Substring(6, 4));

            return new DateTime(ano, mes, dia);
        }
    }
}

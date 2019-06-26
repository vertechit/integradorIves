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
//using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Windows;

namespace IntegradorCore.DAO
{
    public class Banco
    {
        public static string query = "";

        private static dynamic GetConnection()
        {
            if(StaticParametersDB.GetDriver() == "oracle")
            {
                string oradb = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST={StaticParametersDB.GetHost()})(PORT={StaticParametersDB.GetPort()}))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={StaticParametersDB.GetServiceName()})));User ID={StaticParametersDB.GetUser()};Password={StaticParametersDB.GetPassword()};";

                //oradb = oradb.Replace("host1", StaticParametersDB.GetHost());
                //oradb = oradb.Replace("port1", StaticParametersDB.GetPort());
                //oradb = oradb.Replace("servicename1", StaticParametersDB.GetServiceName());
                //oradb = oradb.Replace("user1", StaticParametersDB.GetUser());
                //oradb = oradb.Replace("password1", StaticParametersDB.GetPassword());

                return new OracleConnection(oradb);
            }
            else
            {
                if (StaticParametersDB.GetPort() != "0")
                {
                    var strconnection = $"Data Source={StaticParametersDB.GetHost()},{StaticParametersDB.GetPort()};Network Library=DBMSSOCN;Initial Catalog = {StaticParametersDB.GetServiceName()}; User ID = {StaticParametersDB.GetUser()}; Password = {StaticParametersDB.GetPassword()};";

                    //strconnection = strconnection.Replace("host", StaticParametersDB.GetHost());
                    //strconnection = strconnection.Replace("port", StaticParametersDB.GetPort());
                    //strconnection = strconnection.Replace("myDataBase", StaticParametersDB.GetServiceName());
                    //strconnection = strconnection.Replace("myUsername", StaticParametersDB.GetUser());
                    //strconnection = strconnection.Replace("myPassword", StaticParametersDB.GetPassword());

                    return new SqlConnection(strconnection);
                }
                else
                {
                    var strconnection = $"Server={taticParametersDB.GetHost()};Database={StaticParametersDB.GetServiceName()};Trusted_Connection={StaticParametersDB.GetTrustedConn()};User Id={StaticParametersDB.GetUser()};Password = {StaticParametersDB.GetPassword()}; ";

                    //strconnection = strconnection.Replace("myInstanceName", StaticParametersDB.GetHost());
                    ////strconnection = strconnection.Replace("port", port);
                    //strconnection = strconnection.Replace("myDataBase", StaticParametersDB.GetServiceName());
                    //strconnection = strconnection.Replace("myUsername", StaticParametersDB.GetUser());
                    //strconnection = strconnection.Replace("myPassword", StaticParametersDB.GetPassword());
                    return new SqlConnection(strconnection);
                }
            }
            
        }

        private static dynamic GetConnectionWithParam(string host, string port, string servicename, string user, string password, string driver, string trusted_conn = "True")
        {
            if (driver.ToLower() == "oracle")
            {
                string oradb = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST={host})(PORT={port}))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={servicename})));User ID={user};Password={password};";

                //oradb = oradb.Replace("host1", host);
                //oradb = oradb.Replace("port1", port);
                //oradb = oradb.Replace("servicename1", servicename);
                //oradb = oradb.Replace("user1", user);
                //oradb = oradb.Replace("password1", password);

                return new OracleConnection(oradb);
            }
            else
            {
                if(port != "0" && port != "1")
                {
                    var strconnection = $"Data Source={host},{port};Network Library=DBMSSOCN;Initial Catalog = {servicename}; User ID = {user}; Password = {password};";

                    //strconnection = strconnection.Replace("host", host);
                    //strconnection = strconnection.Replace("port", port);
                    //strconnection = strconnection.Replace("myDataBase", servicename);
                    //strconnection = strconnection.Replace("myUsername", user);
                    //strconnection = strconnection.Replace("myPassword", password);

                    return new SqlConnection(strconnection);
                }
                else if(port == "0")
                {
                    var strconnection = $"Server={host};Database={servicename};Trusted_Connection={trusted_conn};User Id={user};Password = {password}; ";

                    //strconnection = strconnection.Replace("myInstanceName", host);
                    //strconnection = strconnection.Replace("port", port);
                    //strconnection = strconnection.Replace("myDataBase", servicename);
                    //strconnection = strconnection.Replace("myUsername", user);
                    //strconnection = strconnection.Replace("myPassword", password);

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
                var data = Convert.ToDateTime(prot.dtconsulta); //RetornaArrayData(prot.dtconsulta);
                var sql = "UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :Nrprot, XMLPROTOCOLO = :Xmlprot, MENSAGEMERRO = :Erro, DATARETORNO = :Dtretorno, HORARETORNO = :Hrretorno, STATUS = :Status  WHERE ID = :Idevento AND IDSEQ = :Idseq";
                sql = sql.Replace(":Nrprot", string.Concat(quote + prot.nroProt + quote));
                sql = sql.Replace(":Xmlprot", string.Concat(quote + (prot.xmlProt = prot.xmlProt.Replace("> <", "><")) + quote));
                if(String.IsNullOrEmpty(prot.erros)){
                    sql = sql.Replace(":Erro", string.Concat(quote + "null" + quote));
                }else{
                    sql = sql.Replace(":Erro", string.Concat(quote + prot.erros.Replace("'", "") + quote));
                }
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
                var data = Convert.ToDateTime(prot.dtconsulta);//RetornaArrayData(prot.dtconsulta);
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
                var data = Convert.ToDateTime(prot.dtenvio);//RetornaArrayData(prot.dtenvio);
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
                var data = Convert.ToDateTime(prot.dtconsulta);//RetornaArrayData(prot.dtconsulta);
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

        public static bool TesteConexao(string host, string port, string servicename, string user, string password, string driver, string trusted_conn = "True")
        {
            var retorno = true;

            using (var conn = GetConnectionWithParam(host, port, servicename, user, password, driver, trusted_conn))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    MessageBox.Show(e.Message, "", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Processos p = new Processos();
                    p.InsereLogInterno(StaticParametersDB.GetDriver(), e, "99", "Teste Conexão", "");
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
                            try
                            {
                                var Base = proc.DefineBaseEnvioDB(Convert.ToString(row["XMLEVENTO"]), (Convert.ToString(row["ID"]) + "-" + Convert.ToString(row["IDSEQ"])));
                                var prot = new ProtocoloDB
                                {
                                    id = string.Concat(Convert.ToString(row["ID"]), "-", Convert.ToString(row["IDSEQ"])),
                                    idEvento = Convert.ToString(row["ID"]),
                                    idSeq = Convert.ToString(row["IDSEQ"]),
                                    xmlEvento = Convert.ToString(row["XMLEVENTO"]),
                                    driver = StaticParametersDB.GetDriver(),
                                    baseEnv = Convert.ToString(Base)
                                };
                                ProtocoloDAO.Salvar(prot);
                            }
                            catch (Exception ex)
                            {
                                if (ex.HResult != -2147467261)
                                {
                                    ExceptionCore e = new ExceptionCore();
                                    e.ExBanco(30, "ID Evento: " + (Convert.ToString(row["ID"]) + "-" + Convert.ToString(row["IDSEQ"])) + " | Erro: " + ex.Message, StaticParametersDB.GetDriver(), ex, "");
                                }
                                else
                                {
                                    UpdateDB(
                                        proc.GeraProtocoloAux("1"
                                        , Convert.ToString(row["ID"])
                                        , Convert.ToString(row["IDSEQ"])
                                        , "<erro>Tag tipo de ambiente não presente no XML</erro>"
                                        , "0"
                                        , "Tag tipo de ambiente não presente no XML")
                                        );
                                }
                            }
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExBanco(1, ex.Message, StaticParametersDB.GetDriver(), ex, "");
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
                            query = sql;
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            if (StaticParametersDB.GetDriver() == "sqlserver")
                            {
                                comm.CommandText = "SET DATEFORMAT dmy";
                                comm.ExecuteNonQuery();
                            }
                            try //Tenta atualizar com tamanho original da mensagem de erro
                            {
                                using (var command = SqlCommandWithParameters(prot, 1))
                                {
                                    command.Connection = conn;
                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                if(ex.HResult == -2146232060 || ex.HResult == -2147467259) //Banco retorna erros de valor de caracteres excedido.
                                {
                                    using (var command = SqlCommandWithParameters(prot, 1, true)) //Valor true passado por parametro para forcar a atualização, alterando o valor da mensagem.
                                    {
                                        command.Connection = conn;
                                        command.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    throw ex; //Caso seja outro erro, grava log.
                                }
                            }
                            
                        }
                        else
                        {
                            var sql = CriaSQL(prot, 2);
                            query = sql;
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            if (StaticParametersDB.GetDriver() == "sqlserver")
                            {
                                comm.CommandText = "SET DATEFORMAT dmy";
                                comm.ExecuteNonQuery();
                            }
                            using (var command = SqlCommandWithParameters(prot, 2))
                            {
                                command.Connection = conn;
                                command.ExecuteNonQuery();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExBanco(2, ex.Message, StaticParametersDB.GetDriver(), ex, query);

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
                        query = sql;

                        comm.Connection = conn;
                        comm.CommandType = CommandType.Text;

                        if(StaticParametersDB.GetDriver() == "sqlserver")
                        {
                            comm.CommandText = "SET DATEFORMAT dmy";
                            comm.ExecuteNonQuery();
                        }

                        using (var command = SqlCommandWithParameters(prot, tipo))
                        {
                            command.Connection = conn;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionCore e = new ExceptionCore();
                    e.ExBanco(2, ex.Message, StaticParametersDB.GetDriver(), ex, query);
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

        private static dynamic SqlCommandWithParameters(ProtocoloDB prot, int tipo, bool forceUpdate = false)
        {
            string format = "dd-MM-yyyy hh:mm:ss";

            if (StaticParametersDB.GetDriver() == "oracle")
            {
                if (tipo == 1)
                {
                    OracleCommand oraCommand = new OracleCommand("UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :Nrprot, XMLPROTOCOLO = :Xmlprot, MENSAGEMERRO = :Erro, DATARETORNO = :Dtretorno, HORARETORNO = :Hrretorno, STATUS = :Status  WHERE ID = :Idevento AND IDSEQ = :Idseq");
                    oraCommand.Parameters.Add(new OracleParameter(":Nrprot", prot.nroProt));
                    oraCommand.Parameters.Add(new OracleParameter(":Xmlprot", prot.xmlProt));
                    if(String.IsNullOrEmpty(prot.erros))
                    {
                        oraCommand.Parameters.Add(new OracleParameter(":Erro", DBNull.Value));
                    }
                    else
                    {
                        if(forceUpdate == true && prot.erros.Length > 4000)
                        {
                            prot.erros = "Consulte o portal ives para detalhes do erro";
                        }
                        oraCommand.Parameters.Add(new OracleParameter(":Erro", prot.erros.Replace("'", "")));
                    }
                    oraCommand.Parameters.Add(new OracleParameter(":Dtretorno", prot.dtconsulta));
                    oraCommand.Parameters.Add(new OracleParameter(":Hrretorno", prot.hrconsulta));
                    oraCommand.Parameters.Add(new OracleParameter(":Status", prot.status));
                    oraCommand.Parameters.Add(new OracleParameter(":Idevento", prot.idEvento));
                    oraCommand.Parameters.Add(new OracleParameter(":Idseq", prot.idSeq));

                    return oraCommand;
                }
                else if(tipo == 2)
                {

                    OracleCommand oraCommand = new OracleCommand("UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :nrprot, XMLPROTOCOLO = :xmlprot, NRORECIBO = :nroRec, XMLRECIBO = :xmlRec, DATARETORNO = :Dtretorno, HORARETORNO = :Hrretorno, STATUS = :Status WHERE ID = :Idevento AND IDSEQ = :Idseq");
                    oraCommand.Parameters.Add(new OracleParameter(":nrprot", prot.nroProt));
                    oraCommand.Parameters.Add(new OracleParameter(":xmlprot", prot.xmlProt));
                    oraCommand.Parameters.Add(new OracleParameter(":nroRec", prot.nroRec));
                    oraCommand.Parameters.Add(new OracleParameter(":xmlRec", prot.xmlRec));
                    oraCommand.Parameters.Add(new OracleParameter(":Dtretorno", prot.dtconsulta));
                    oraCommand.Parameters.Add(new OracleParameter(":Hrretorno", prot.hrconsulta));
                    oraCommand.Parameters.Add(new OracleParameter(":Status", prot.status));
                    oraCommand.Parameters.Add(new OracleParameter(":Idevento", prot.idEvento));
                    oraCommand.Parameters.Add(new OracleParameter(":Idseq", prot.idSeq));

                    return oraCommand;
                }
                else if (tipo == 3)
                {
                    OracleCommand oraCommand = new OracleCommand("UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = :Nrprot, XMLPROTOCOLO = :Xmlprot, STATUS = :Status, DATAENVIO = :Dtenvio, HORAENVIO = :Hrenvio  WHERE ID = :Idevento AND IDSEQ = :Idseq");
                    oraCommand.Parameters.Add(new OracleParameter(":Nrprot", prot.nroProt));
                    oraCommand.Parameters.Add(new OracleParameter(":Xmlprot", prot.xmlProt));
                    oraCommand.Parameters.Add(new OracleParameter(":Status", prot.status));
                    oraCommand.Parameters.Add(new OracleParameter(":Dtenvio", prot.dtenvio));
                    oraCommand.Parameters.Add(new OracleParameter(":Hrenvio", prot.hrenvio));
                    oraCommand.Parameters.Add(new OracleParameter(":Idevento", prot.idEvento));
                    oraCommand.Parameters.Add(new OracleParameter(":Idseq", prot.idSeq));
                    return oraCommand;
                }
                else
                {
                    OracleCommand oraCommand = new OracleCommand("UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET DATARETORNO = :Dtretorno, HORARETORNO = :Hrretorno, STATUS = :Status  WHERE ID = :Idevento AND IDSEQ = :Idseq");
                    oraCommand.Parameters.Add(new OracleParameter(":Dtretorno", prot.dtconsulta));
                    oraCommand.Parameters.Add(new OracleParameter(":Hrretorno", prot.hrconsulta));
                    oraCommand.Parameters.Add(new OracleParameter(":Status", prot.status));
                    oraCommand.Parameters.Add(new OracleParameter(":Idevento", prot.idEvento));
                    oraCommand.Parameters.Add(new OracleParameter(":Idseq", prot.idSeq));
                    return oraCommand;
                }
            }
            else
            {
                if (tipo == 1)
                {
                    SqlCommand sqlCommand = new SqlCommand("UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = @Nrprot, XMLPROTOCOLO = @Xmlprot, MENSAGEMERRO = @Erro, DATARETORNO = @Dtretorno, HORARETORNO = @Hrretorno, STATUS = @Status  WHERE ID = @Idevento AND IDSEQ = @Idseq");
                    sqlCommand.Parameters.Add(new SqlParameter("@Nrprot", prot.nroProt));
                    sqlCommand.Parameters.Add(new SqlParameter("@Xmlprot", prot.xmlProt));
                    if(String.IsNullOrEmpty(prot.erros))
                    {
                        sqlCommand.Parameters.Add(new SqlParameter("@Erro", DBNull.Value));
                    }
                    else
                    {
                        if (forceUpdate == true && prot.erros.Length > 4000)
                        {
                            prot.erros = "Consulte o portal do iVes para obter detalhes do erro";
                        }
                        sqlCommand.Parameters.Add(new SqlParameter("@Erro", prot.erros.Replace("'", "")));
                    }
                    sqlCommand.Parameters.Add(new SqlParameter("@Dtretorno", Convert.ToDateTime(prot.dtconsulta).ToString(format)));
                    sqlCommand.Parameters.Add(new SqlParameter("@Hrretorno", prot.hrconsulta));
                    sqlCommand.Parameters.Add(new SqlParameter("@Status", prot.status));
                    sqlCommand.Parameters.Add(new SqlParameter("@Idevento", prot.idEvento));
                    sqlCommand.Parameters.Add(new SqlParameter("@Idseq", prot.idSeq));

                    return sqlCommand;
                }
                else if (tipo == 2)
                {

                    SqlCommand sqlCommand = new SqlCommand("UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = @nrprot, XMLPROTOCOLO = @xmlprot, NRORECIBO = @nroRec, XMLRECIBO = @xmlRec, DATARETORNO = @Dtretorno, HORARETORNO = @Hrretorno, STATUS = @Status WHERE ID = @Idevento AND IDSEQ = @Idseq");
                    sqlCommand.Parameters.Add(new SqlParameter("@nrprot", prot.nroProt));
                    sqlCommand.Parameters.Add(new SqlParameter("@xmlprot", prot.xmlProt));
                    sqlCommand.Parameters.Add(new SqlParameter("@nroRec", prot.nroRec));
                    sqlCommand.Parameters.Add(new SqlParameter("@xmlRec", prot.xmlRec));
                    sqlCommand.Parameters.Add(new SqlParameter("@Dtretorno", Convert.ToDateTime(prot.dtconsulta).ToString(format)));
                    sqlCommand.Parameters.Add(new SqlParameter("@Hrretorno", prot.hrconsulta));
                    sqlCommand.Parameters.Add(new SqlParameter("@Status", prot.status));
                    sqlCommand.Parameters.Add(new SqlParameter("@Idevento", prot.idEvento));
                    sqlCommand.Parameters.Add(new SqlParameter("@Idseq", prot.idSeq));

                    return sqlCommand;
                }
                else if (tipo == 3)
                {
                    SqlCommand sqlCommand = new SqlCommand("UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET NROPROTOCOLO = @Nrprot, XMLPROTOCOLO = @Xmlprot, STATUS = @Status, DATAENVIO = @Dtenvio, HORAENVIO = @Hrenvio  WHERE ID = @Idevento AND IDSEQ = @Idseq");
                    sqlCommand.Parameters.Add(new SqlParameter("@Nrprot", prot.nroProt));
                    sqlCommand.Parameters.Add(new SqlParameter("@Xmlprot", prot.xmlProt));
                    sqlCommand.Parameters.Add(new SqlParameter("@Status", prot.status));
                    sqlCommand.Parameters.Add(new SqlParameter("@Dtenvio", Convert.ToDateTime(prot.dtenvio).ToString(format)));
                    sqlCommand.Parameters.Add(new SqlParameter("@Hrenvio", prot.hrenvio));
                    sqlCommand.Parameters.Add(new SqlParameter("@Idevento", prot.idEvento));
                    sqlCommand.Parameters.Add(new SqlParameter("@Idseq", prot.idSeq));
                    return sqlCommand;
                }
                else
                {
                    SqlCommand sqlCommand = new SqlCommand("UPDATE ZMDATVIVES_EVENTOS_ESOCIAL SET DATARETORNO = @Dtretorno, HORARETORNO = @Hrretorno, STATUS = @Status  WHERE ID = @Idevento AND IDSEQ = @Idseq");
                    sqlCommand.Parameters.Add(new SqlParameter("@Dtretorno", Convert.ToDateTime(prot.dtconsulta).ToString(format)));
                    sqlCommand.Parameters.Add(new SqlParameter("@Hrretorno", prot.hrconsulta));
                    sqlCommand.Parameters.Add(new SqlParameter("@Status", prot.status));
                    sqlCommand.Parameters.Add(new SqlParameter("@Idevento", prot.idEvento));
                    sqlCommand.Parameters.Add(new SqlParameter("@Idseq", prot.idSeq));
                    return sqlCommand;
                }
            }

        }
    }
}

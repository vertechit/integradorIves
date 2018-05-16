using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using Vertech.Modelos;
using System.Security.Permissions;
using System.Security.AccessControl;
using Vertech.Services;

namespace Vertech.DAO
{
    public class Log
    {
        private static SQLiteConnection sqliteConnection;

        public Log()
        { }

        private static SQLiteConnection DbConnection()
        {
            try
            {
                sqliteConnection = new SQLiteConnection("Data Source=c:\\vch\\logs.sqlite;");
                sqliteConnection.Open();
                return sqliteConnection;
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(0, ex.Message);
            }
            return null;
        }

        public static void CriarBancoSQLite()
        {
            try
            {
                DbConnection();
            }
            catch
            {
                SQLiteConnection.CreateFile(@"c:\vch\logs.sqlite");
            }
        }

        public static void CriarTabelaSQlite()
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS logconsulta(" +
                                                                            "id integer primary key autoincrement," +
                                                                            "nome_arq varchar2(100) not null," +
                                                                            "protocolo varchar2(100) not null," +
                                                                            "msg varchar2(500) not null," +
                                                                            "acao varchar2(500)," +
                                                                            "data varchar2(10) not null," +
                                                                            "hora varchar2(8) not null" +
                                                                            ") ";

                    cmd.ExecuteNonQuery();
                }


                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS logenvia(" +
                                                                            "id integer primary key autoincrement," +
                                                                            "nome_arq varchar2(100)," +
                                                                            "msg varchar2(500)," +
                                                                            "acao varchar2(500)," +
                                                                            "data varchar2(10) not null," +
                                                                            "hora varchar2(8) not null" +
                                                                            ") ";

                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS logerro(" +
                                                                            "id integer primary key autoincrement," +
                                                                            "servico varchar2(30)," +
                                                                            "cdErro varchar2(10)," +
                                                                            "msg varchar2(500)," +
                                                                            "acao varchar2(500)," +
                                                                            "data varchar2(10) not null," +
                                                                            "hora varchar2(8) not null" +
                                                                            ") ";

                    cmd.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(1, ex.Message);
            }
        }

        public static void AddLogConsulta(LogConsulta log)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO logconsulta(nome_arq, protocolo, msg, acao, data, hora) values (@arq, @prot, @msg, @acao, @data, @hora)";
                    cmd.Parameters.AddWithValue("@arq", log.NomeArquivo);
                    cmd.Parameters.AddWithValue("@prot", log.Protocolo);
                    cmd.Parameters.AddWithValue("@msg", log.Msg);
                    cmd.Parameters.AddWithValue("@acao", log.Acao);
                    cmd.Parameters.AddWithValue("@data", log.Data);
                    cmd.Parameters.AddWithValue("@hora", log.Hora);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(2, ex.Message);
            }
        }

        public static void AddLogEnvia(LogEnvia log)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO logenvia(nome_arq, msg, acao, data, hora) values (@arq, @msg, @acao, @data, @hora)";
                    cmd.Parameters.AddWithValue("@arq", log.NomeArquivo);
                    cmd.Parameters.AddWithValue("@msg", log.Msg);
                    cmd.Parameters.AddWithValue("@acao", log.Acao);
                    cmd.Parameters.AddWithValue("@data", log.Data);
                    cmd.Parameters.AddWithValue("@hora", log.Hora);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(3, ex.Message);
            }
        }

        public static void AddLogErro(LogErros log)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO logerro(servico, cdErro, msg, acao, data, hora) values (@serv, @cderr, @msg, @acao, @data, @hora)";
                    cmd.Parameters.AddWithValue("@serv", log.Servico);
                    cmd.Parameters.AddWithValue("@cderr", log.CodErro);
                    cmd.Parameters.AddWithValue("@msg", log.Msg);
                    cmd.Parameters.AddWithValue("@acao", log.Acao);
                    cmd.Parameters.AddWithValue("@data", log.Data);
                    cmd.Parameters.AddWithValue("@hora", log.Hora);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(4, ex.Message);
            }
        }

        public static DataTable GetLogs(string param)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM " + param;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(5, ex.Message);
            }

            return null;
        }

        public static DataTable GetLogsWithParam(int Tipo, string Param, string Tabela)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            const string quote = "\"";
            string param = quote + Param + quote;

            try
            {
                if(Tabela == "logerro")
                {
                    if(Tipo == 1)
                    {
                        using (var cmd = DbConnection().CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM logerro where servico = "+ param;
                            cmd.Parameters.AddWithValue("@serv", Param);
                            da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                            da.Fill(dt);
                            return dt;
                        }
                    }
                    else
                    {
                        using (var cmd = DbConnection().CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM logerro where data = " + param;
                            cmd.Parameters.AddWithValue("@data", Param);
                            da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                            da.Fill(dt);
                            return dt;
                        }


                    }
                    
                }

                else
                {
                    if (Tipo == 1)
                    {
                        using (var cmd = DbConnection().CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM " + Tabela + " where nome_arq = " + param;
                            //cmd.Parameters.AddWithValue("@tabela", Tabela);
                            da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                            da.Fill(dt);
                            return dt;
                        }
                    }
                    else
                    {
                        using (var cmd = DbConnection().CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM " + Tabela + " where data = " + param;
                            //cmd.Parameters.AddWithValue("@tabela", Tabela);
                            da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                            da.Fill(dt);
                            return dt;
                        }


                    }
                }
                
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(6, ex.Message);
            }

            return null;
        }
    }
}

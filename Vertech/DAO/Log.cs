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
                                                                            "data varchar2(10) not null," +
                                                                            "hora varchar2(8) not null" +
                                                                            ") ";

                    cmd.ExecuteNonQuery();
                }


                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS logenvia(" +
                                                                            "id integer primary key autoincrement," +
                                                                            "nome_arq varchar2(100) not null," +
                                                                            "msg varchar2(500) not null," +
                                                                            "data varchar2(10) not null," +
                                                                            "hora varchar2(8) not null" +
                                                                            ") ";

                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS logerro(" +
                                                                            "id integer primary key autoincrement," +
                                                                            "servico varchar2(30) not null," +
                                                                            "cdErro varchar2(10) not null," +
                                                                            "msg varchar2(500) not null," +
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
                    cmd.CommandText = "INSERT INTO logconsulta(nome_arq, protocolo, msg, data, hora) values (@arq, @prot, @msg, @data, @hora)";
                    cmd.Parameters.AddWithValue("@arq", log.NomeArquivo);
                    cmd.Parameters.AddWithValue("@prot", log.Protocolo);
                    cmd.Parameters.AddWithValue("@msg", log.Msg);
                    cmd.Parameters.AddWithValue("@data", log.Data);
                    cmd.Parameters.AddWithValue("@hora", log.Hora);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(7, ex.Message);
            }
        }

        public static void AddLogEnvia(LogEnvia log)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO logenvia(nome_arq, msg, data, hora) values (@arq, @msg, @data, @hora)";
                    cmd.Parameters.AddWithValue("@arq", log.NomeArquivo);
                    cmd.Parameters.AddWithValue("@msg", log.Msg);
                    cmd.Parameters.AddWithValue("@data", log.Data);
                    cmd.Parameters.AddWithValue("@hora", log.Hora);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(7, ex.Message);
            }
        }

        public static void AddLogErro(LogErros log)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO logerro(servico, cdErro, msg, data, hora) values (@serv, @cderr, @msg, @data, @hora)";
                    cmd.Parameters.AddWithValue("@serv", log.Servico);
                    cmd.Parameters.AddWithValue("@cderr", log.CodErro);
                    cmd.Parameters.AddWithValue("@msg", log.Msg);
                    cmd.Parameters.AddWithValue("@data", log.Data);
                    cmd.Parameters.AddWithValue("@hora", log.Hora);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(7, ex.Message);
            }
        }

        public static DataTable GetLogsConsulta()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM logconsulta";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(2, ex.Message);
            }

            return null;
        }

        public static DataTable GetLogsEnvia()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM logenvia";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(2, ex.Message);
            }

            return null;
        }

        public static DataTable GetLogsErros()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM logerro";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(2, ex.Message);
            }

            return null;
        }
    }
}

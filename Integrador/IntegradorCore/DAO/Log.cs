﻿using System;
using System.Data;
using System.Data.SQLite;
using IntegradorCore.Modelos;
using IntegradorCore.Services;

namespace IntegradorCore.DAO
{
    public class Log
    {
        private static ExceptionCore exC = new ExceptionCore();
        private static SQLiteConnection sqliteConnection;

        public Log()
        { }

        private static SQLiteConnection DbConnection()
        {
            try
            {
                sqliteConnection = new SQLiteConnection("Data Source=c:\\vch\\logs.db;");
                sqliteConnection.Open();
                return sqliteConnection;
            }
            catch (Exception ex)
            {
                exC.ExSQLite(1, ex.Message);
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
                SQLiteConnection.CreateFile(@"c:\vch\logs.db");
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
                                                                            "identificador varchar2(100)," +
                                                                            "protocolo varchar2(100)," +
                                                                            "msg varchar2(500)," +
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
                                                                            "identificador varchar2(100)," +
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
                exC.ExSQLite(2, ex.Message);
            }
        }

        public static void AddLogConsulta(LogConsulta log)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO logconsulta(identificador, protocolo, msg, acao, data, hora) values (@arq, @prot, @msg, @acao, @data, @hora)";
                    cmd.Parameters.AddWithValue("@arq", log.Identificador);
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
                exC.ExSQLite(3, ex.Message);
            }
        }

        public static void AddLogEnvia(LogEnvia log)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO logenvia(identificador, msg, acao, data, hora) values (@arq, @msg, @acao, @data, @hora)";
                    cmd.Parameters.AddWithValue("@arq", log.Identificador);
                    cmd.Parameters.AddWithValue("@msg", log.Msg);
                    cmd.Parameters.AddWithValue("@acao", log.Acao);
                    cmd.Parameters.AddWithValue("@data", log.Data);
                    cmd.Parameters.AddWithValue("@hora", log.Hora);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(4, ex.Message);
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
                exC.ExSQLite(41, ex.Message);
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
                exC.ExSQLite(5, ex.Message);
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
                if (Tabela == "logerro")
                {
                    if (Tipo == 1)
                    {
                        using (var cmd = DbConnection().CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM logerro where servico = " + param;
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
                            cmd.CommandText = "SELECT * FROM " + Tabela + " where identificador = " + param;
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
                exC.ExSQLite(7, ex.Message);
            }

            return null;
        }


        public static void DeleteByID(string tabela, int id)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE from " + tabela + " where id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(8, ex.Message);
            }
        }

        public static void DeleteByData(string tabela, string data)
        {
            const string quote = "\"";
            data = quote + data + quote;
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE from " + tabela + " where data = " + data;
                    //cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(9, ex.Message);
            }
        }
    }
}

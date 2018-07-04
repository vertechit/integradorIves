﻿using System;
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
    public class Helper
    {
        private static SQLiteConnection sqliteConnection;

        public Helper()
        { }

        private static SQLiteConnection DbConnection()
        {
            try
            {
                sqliteConnection = new SQLiteConnection("Data Source=c:\\vch\\dados.sqlite;");
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
                SQLiteConnection.CreateFile(@"c:\vch\dados.sqlite");
            }
        }

        public static void CriarTabelaSQlite()
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS protocolo(" +
                                                                            "id integer primary key autoincrement," +
                                                                            "nome_arq varchar2(500) not null," +
                                                                            "nro_prot varchar2(1000) not null," +
                                                                            "base varchar2(50) not null" +
                                                                            ")";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS parametros(" +
                                                                            "id integer not null," +
                                                                            "CaminhoDir varchar2(500) not null," +
                                                                            "CaminhoFim varchar2(500) not null," +
                                                                            "CaminhoToke varchar2(500) not null," +
                                                                            "Ambiente varchar2(10) not null," +
                                                                            "Base varchar2(50) not null," +
                                                                            "constraint pk_parametros " +
                                                                            "primary key(id)" +
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


        public static DataTable GetProtocolos()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM protocolo";
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


        public static Protocolo GetProtocolo(string arqname)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            const string quote = "\"";
            arqname = quote + arqname + quote;
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM protocolo where nome_arq = " + arqname;
                    cmd.Parameters.AddWithValue("@arq", arqname);
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            return new Protocolo { Id = Convert.ToInt64(item.ItemArray[0]), NomeArquivo = Convert.ToString(item.ItemArray[1]), NroProtocolo = Convert.ToString(item.ItemArray[2]), Base = Convert.ToString(item.ItemArray[3]) };
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(3, ex.Message);
            }

            return null;
        }

        public static bool ExistsProtocolo(string arqname)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            const string quote = "\"";
            arqname = quote + arqname + quote;
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM protocolo where nome_arq = "+arqname;
                    cmd.Parameters.AddWithValue("@arq", arqname);
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);

                    if(dt.Rows.Count == 1)
                    {
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(4, ex.Message);
            }

            return false;
        }

        public static void DeleteProtocolo(string arqname)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE from protocolo where nome_arq = @arq";
                    cmd.Parameters.AddWithValue("@arq", arqname);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(5, ex.Message);
            }
        }

        public static Parametro GetParametros()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM parametros where id = 1";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    var param = new Parametro();
                    if (dt.Rows.Count > 0)
                    {
                        
                        foreach (DataRow item in dt.Rows)
                        {
                           param.Id = (Convert.ToInt64(item.ItemArray[0]));
                           param.CaminhoDir = (Convert.ToString(item.ItemArray[1]));
                           param.CaminhoFim = (Convert.ToString(item.ItemArray[2]));
                           param.CaminhoToke = (Convert.ToString(item.ItemArray[3]));
                           param.Ambiente = (Convert.ToString(item.ItemArray[4]));
                           param.Base = (Convert.ToString(item.ItemArray[5]));
                        }
                    }
                    else
                    {
                        return null;
                    }

                    return param;
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(6, ex.Message);
            }

            return null;
        }

        public static void AddProtocolo(Protocolo protocolo)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO protocolo(nome_arq, nro_prot, base ) values (@arq, @prot, @base)";
                    cmd.Parameters.AddWithValue("@arq", protocolo.NomeArquivo);
                    cmd.Parameters.AddWithValue("@prot", protocolo.NroProtocolo);
                    cmd.Parameters.AddWithValue("@base", protocolo.Base);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(7, ex.Message);
            }
        }

        public static void AddParametros(Parametro param)
        {
            try
            {
                var p = GetParametros();

                if (p == null)
                {
                    using (var cmd = DbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO parametros(id, CaminhoDir, CaminhoFim, CaminhoToke, Ambiente, Base) values (@id, @caminho_dir, @caminho_fim, @caminho_tok, @ambiente, @base)";
                        cmd.Parameters.AddWithValue("@id", param.Id);
                        cmd.Parameters.AddWithValue("@caminho_dir", param.CaminhoDir);
                        cmd.Parameters.AddWithValue("@caminho_fim", param.CaminhoFim);
                        cmd.Parameters.AddWithValue("@caminho_tok", param.CaminhoToke);
                        cmd.Parameters.AddWithValue("@ambiente", param.Ambiente);
                        cmd.Parameters.AddWithValue("@base", param.Base);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                    UpdateParametros(param);
                
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(8, ex.Message);
            }
        }

        public static void UpdateParametros(Parametro param)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    if (param.Id == 1)
                    {
                        cmd.CommandText = "UPDATE parametros SET CaminhoDir=@caminho_dir, CaminhoFim=@caminho_fim, CaminhoToke=@caminho_tok, Ambiente=@ambiente, Base=@base WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", param.Id);
                        cmd.Parameters.AddWithValue("@caminho_dir", param.CaminhoDir);
                        cmd.Parameters.AddWithValue("@caminho_fim", param.CaminhoFim);
                        cmd.Parameters.AddWithValue("@caminho_tok", param.CaminhoToke);
                        cmd.Parameters.AddWithValue("@ambiente", param.Ambiente);
                        cmd.Parameters.AddWithValue("@base", param.Base);
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception ex)
            {
                ClassException excep = new ClassException();
                excep.ExSQLite(9, ex.Message);
            }
        }
    }
}
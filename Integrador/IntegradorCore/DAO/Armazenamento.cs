using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using IntegradorCore.Modelos;
using System.Security.Permissions;
using System.Security.AccessControl;
using IntegradorCore.Services;

namespace IntegradorCore.DAO
{
    public class Armazenamento
    {
        private static ExceptionCore exC = new ExceptionCore();

        private static SQLiteConnection sqliteConnection;

        public Armazenamento()
        { }

        private static SQLiteConnection DbConnection()
        {
            try
            {
                sqliteConnection = new SQLiteConnection("Data Source=c:\\vch\\dados.db;");
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
                SQLiteConnection.CreateFile(@"c:\vch\dados.db");
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
                                                                            "base varchar2(50) not null," +
                                                                            "ambiente integer not null" +
                                                                            ")";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS parametros(" +
                                                                            "id integer primary key autoincrement," +
                                                                            "CaminhoDir varchar2(500) not null," +
                                                                            "CaminhoToke varchar2(500) not null," +
                                                                            "integraBanco BOOLEAN not null" +
                                                                            ") ";

                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS protocoloDB(" +
                                                                            "idEvento VARCHAR NOT NULL PRIMARY KEY UNIQUE," +
                                                                            "xmlEvento VARCHAR NOT NULL,"+
                                                                            "nroProt VARCHAR," +
                                                                            "xmlProt VARCHAR," +
                                                                            "nroRec VARCHAR," +
                                                                            "xmlRec VARCHAR," +
                                                                            "salvoDB BOOLEAN DEFAULT 0," +
                                                                            "baseEnv VARCHAR," +
                                                                            "erros VARCHAR," +
                                                                            "consultado BOOLEAN DEFAULT 0"+
                                                                            ") ";

                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS parametrosDB(" +
                                                                            "id integer primary key autoincrement," +
                                                                            "driver VARCHAR NOT NULL," +
                                                                            "host VARCHAR NOT NULL," +
                                                                            "port VARCHAR NOT NULL," +
                                                                            "serviceName VARCHAR NOT NULL," +
                                                                            "user VARCHAR NOT NULL," +
                                                                            "password VARCHAR NOT NULL" +
                                                                            ") ";

                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                exC.ExSQLite(2, ex.Message);
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
                exC.ExSQLite(3, ex.Message);
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
                            return new Protocolo { Id = Convert.ToInt64(item.ItemArray[0]), NomeArquivo = Convert.ToString(item.ItemArray[1]), NroProtocolo = Convert.ToString(item.ItemArray[2]), Base = Convert.ToString(item.ItemArray[3]), Ambiente = Convert.ToInt64(item.ItemArray[4]) };
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(4, ex.Message);
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
                    cmd.CommandText = "SELECT * FROM protocolo where nome_arq = " + arqname;
                    cmd.Parameters.AddWithValue("@arq", arqname);
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(5, ex.Message);
            }

            //return false;
            return true;
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
                exC.ExSQLite(6, ex.Message);
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
                            param.CaminhoToke = (Convert.ToString(item.ItemArray[2]));
                            param.IntegraBanco = (Convert.ToBoolean(item.ItemArray[3]));
                            //param.CaminhoFim = (Convert.ToString(item.ItemArray[2]));
                            //param.CaminhoToke = (Convert.ToString(item.ItemArray[3]));
                            //param.Ambiente = (Convert.ToString(item.ItemArray[4]));
                            //param.Base = (Convert.ToString(item.ItemArray[5]));
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
                exC.ExSQLite(7, ex.Message);
            }

            return null;
        }

        public static void AddProtocolo(Protocolo protocolo)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO protocolo(nome_arq, nro_prot, base, ambiente ) values (@arq, @prot, @base, @ambiente)";
                    cmd.Parameters.AddWithValue("@arq", protocolo.NomeArquivo);
                    cmd.Parameters.AddWithValue("@prot", protocolo.NroProtocolo);
                    cmd.Parameters.AddWithValue("@base", protocolo.Base);
                    cmd.Parameters.AddWithValue("@ambiente", protocolo.Ambiente);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(8, ex.Message);
            }
        }

        public static void AddParametros(Parametro param)
        {
            try
            {
                var p = GetParametros();

                if (p == null)
                {
                    /*using (var cmd = DbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO parametros(id, CaminhoDir, CaminhoFim, CaminhoToke, Ambiente, Base) values (@id, @caminho_dir, @caminho_fim, @caminho_tok, @ambiente, @base)";
                        cmd.Parameters.AddWithValue("@id", param.Id);
                        cmd.Parameters.AddWithValue("@caminho_dir", param.CaminhoDir);
                        cmd.Parameters.AddWithValue("@caminho_fim", param.CaminhoFim);
                        cmd.Parameters.AddWithValue("@caminho_tok", param.CaminhoToke);
                        cmd.Parameters.AddWithValue("@ambiente", param.Ambiente);
                        cmd.Parameters.AddWithValue("@base", param.Base);
                        cmd.ExecuteNonQuery();
                    }*/
                    using (var cmd = DbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO parametros(id, CaminhoDir, CaminhoToke, integraBanco) values (@id, @caminho_dir, @caminho_tok, @integraBanco)";
                        cmd.Parameters.AddWithValue("@id", param.Id);
                        cmd.Parameters.AddWithValue("@caminho_dir", param.CaminhoDir);
                        cmd.Parameters.AddWithValue("@caminho_tok", param.CaminhoToke);
                        cmd.Parameters.AddWithValue("@integraBanco", param.IntegraBanco);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                    UpdateParametros(param);

            }
            catch (Exception ex)
            {
                exC.ExSQLite(9, ex.Message);
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
                        //cmd.CommandText = "UPDATE parametros SET CaminhoDir=@caminho_dir, CaminhoFim=@caminho_fim, CaminhoToke=@caminho_tok, Ambiente=@ambiente, Base=@base WHERE Id=@Id";
                        cmd.CommandText = "UPDATE parametros SET CaminhoDir=@caminho_dir, CaminhoToke=@caminho_tok, integraBanco=@integra WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", param.Id);
                        cmd.Parameters.AddWithValue("@caminho_dir", param.CaminhoDir);
                        //cmd.Parameters.AddWithValue("@caminho_fim", param.CaminhoFim);
                        cmd.Parameters.AddWithValue("@caminho_tok", param.CaminhoToke);
                        cmd.Parameters.AddWithValue("@integra", param.IntegraBanco);
                        //cmd.Parameters.AddWithValue("@ambiente", param.Ambiente);
                        //cmd.Parameters.AddWithValue("@base", param.Base);
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception ex)
            {
                exC.ExSQLite(10, ex.Message);
            }
        }

        public static ProtocoloDB GetProtocoloDB(string idEvento)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            const string quote = "\"";
            idEvento = quote + idEvento + quote;
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM protocoloDB where idEvento = " + idEvento;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            
                            return new ProtocoloDB { idEvento = Convert.ToString(item.ItemArray[0])
                                , xmlEvento = Convert.ToString(item.ItemArray[1])
                                , nroProt = item.ItemArray[2].ToString()
                                , xmlProt = item.ItemArray[3].ToString()
                                , nroRec = item.ItemArray[4].ToString()
                                , xmlRec = item.ItemArray[5].ToString()
                                , salvoDB = Convert.ToBoolean(item.ItemArray[6])
                                , baseEnv = Convert.ToString(item.ItemArray[7])
                                , erros = Convert.ToString(item.ItemArray[8])
                            };
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(4, ex.Message);
            }

            return null;
        }

        public static void AddProtocoloDB(ProtocoloDB protocolo)
        {
            try
            {
                var ret = GetProtocoloDB(protocolo.idEvento);

                if(ret == null)
                {
                    using (var cmd = DbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO protocoloDB(idEvento, xmlEvento) values (@id, @xmlevent)";
                        cmd.Parameters.AddWithValue("@id", protocolo.idEvento);
                        cmd.Parameters.AddWithValue("@xmlevent", protocolo.xmlEvento);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    updateProtocoloDB(protocolo, ret);
                }
                
            }
            catch (Exception ex)
            {
                exC.ExSQLite(8, ex.Message);
            }
        }

        public static void updateProtocoloDB(ProtocoloDB protocolo, ProtocoloDB old)
        {
            if(protocolo.xmlProt == null)
            {
                protocolo.xmlProt = old.xmlProt;
            }
            if (protocolo.nroProt == null)
            {
                protocolo.nroProt = old.nroProt;
            }
            if (protocolo.xmlRec == null)
            {
                protocolo.xmlRec = old.xmlRec;
            }
            if (protocolo.nroRec == null)
            {
                protocolo.nroRec = old.nroRec;
            }
            if (protocolo.baseEnv == null)
            {
                protocolo.baseEnv = old.baseEnv;
            }
            if (protocolo.erros == null)
            {
                protocolo.erros = old.erros;
            }
            if(protocolo.consultado == null)
            {
                protocolo.consultado = old.consultado;
            }
            using (var cmd = new SQLiteCommand(DbConnection()))
            {
               
                cmd.CommandText = "UPDATE protocoloDB SET nroProt=@nrprot, xmlProt=@xmlprot, nroRec=@nrRec, xmlRec=@xmlrec, baseEnv=@base, erros=@erros, consultado=@consult WHERE idEvento=@Id";
                cmd.Parameters.AddWithValue("@Id", protocolo.idEvento);
                cmd.Parameters.AddWithValue("@nrprot", protocolo.nroProt);
                cmd.Parameters.AddWithValue("@xmlprot", protocolo.xmlProt);
                cmd.Parameters.AddWithValue("@nrRec", protocolo.nroRec);
                cmd.Parameters.AddWithValue("@xmlrec", protocolo.xmlRec);
                cmd.Parameters.AddWithValue("@base", protocolo.baseEnv);
                cmd.Parameters.AddWithValue("@erros", protocolo.erros);
                cmd.Parameters.AddWithValue("@consult", protocolo.consultado);
                cmd.ExecuteNonQuery();
            };
        }

        public static void updateSalvoDB(ProtocoloDB protocolo)
        {
            using (var cmd = new SQLiteCommand(DbConnection()))
            {

                cmd.CommandText = "UPDATE protocoloDB SET salvoDB=@salvo WHERE idEvento=@Id";
                cmd.Parameters.AddWithValue("@Id", protocolo.idEvento);
                cmd.Parameters.AddWithValue("@salvo", true);
                cmd.ExecuteNonQuery();
            };
        }

        public static void DeleteProtocoloDB(string idEvento)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DELETE from protocoloDB where idEvento = @Id";
                    cmd.Parameters.AddWithValue("@Id", idEvento);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(6, ex.Message);
            }
        }

        public static ParametroDB GetParametrosDB()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM parametrosDB where id = 1";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    var param = new ParametroDB();
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow item in dt.Rows)
                        {
                            param.Id = (Convert.ToInt64(item.ItemArray[0]));
                            param.Driver = (Convert.ToString(item.ItemArray[1]));
                            param.Host = (Convert.ToString(item.ItemArray[2]));
                            param.Port = (Convert.ToString(item.ItemArray[3]));
                            param.ServiceName = (Convert.ToString(item.ItemArray[4]));
                            param.User = (Convert.ToString(item.ItemArray[5]));
                            param.Password = (Convert.ToString(item.ItemArray[6]));
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
                exC.ExSQLite(7, ex.Message);
            }

            return null;
        }

        public static void AddParametrosDB(ParametroDB param)
        {
            try
            {
                var p = GetParametrosDB();

                if (p == null)
                {
                    using (var cmd = DbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO parametrosDB(id, driver, host, port, serviceName, user, password) values (@id, @driver, @host, @port, @service, @user, @password)";
                        cmd.Parameters.AddWithValue("@id", param.Id);
                        cmd.Parameters.AddWithValue("@driver", param.Driver);
                        cmd.Parameters.AddWithValue("@host", param.Host);
                        cmd.Parameters.AddWithValue("@port", param.Port);
                        cmd.Parameters.AddWithValue("@service", param.ServiceName);
                        cmd.Parameters.AddWithValue("@user", param.User);
                        cmd.Parameters.AddWithValue("@password", param.Password);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                    UpdateParametrosDB(param);

            }
            catch (Exception ex)
            {
                exC.ExSQLite(9, ex.Message);
            }
        }

        public static void UpdateParametrosDB(ParametroDB param)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    if (param.Id == 1)
                    {
                        cmd.CommandText = "UPDATE parametros SET driver=@driver, host=@host, port=@port, serviceName=@service, user=@user, password=@password WHERE id=@id";
                        cmd.Parameters.AddWithValue("@id", param.Id);
                        cmd.Parameters.AddWithValue("@driver", param.Driver);
                        cmd.Parameters.AddWithValue("@host", param.Host);
                        cmd.Parameters.AddWithValue("@port", param.Port);
                        cmd.Parameters.AddWithValue("@service", param.ServiceName);
                        cmd.Parameters.AddWithValue("@user", param.User);
                        cmd.Parameters.AddWithValue("@password", param.Password);
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception ex)
            {
                exC.ExSQLite(10, ex.Message);
            }
        }

        public static List<ProtocoloDB> GetProtocolosDBEnv()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            const string quote = "\"";
            //idEvento = quote + idEvento + quote;
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM protocoloDB where nroProt is null or nroProt = "+quote+quote;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var lista = new List<ProtocoloDB>();
                        foreach (DataRow item in dt.Rows)
                        {

                            lista.Add( new ProtocoloDB
                            {
                                idEvento = Convert.ToString(item.ItemArray[0])
                                ,
                                xmlEvento = Convert.ToString(item.ItemArray[1])
                                ,
                                nroProt = item.ItemArray[2].ToString()
                                ,
                                xmlProt = item.ItemArray[3].ToString()
                                ,
                                nroRec = item.ItemArray[4].ToString()
                                ,
                                xmlRec = item.ItemArray[5].ToString()
                                ,
                                salvoDB = Convert.ToBoolean(item.ItemArray[6])
                                ,
                                baseEnv = Convert.ToString(item.ItemArray[7])
                                ,
                                erros = Convert.ToString(item.ItemArray[8])
                                ,
                                consultado = Convert.ToBoolean(item.ItemArray[9])
                            });
                        }

                        return lista;
                    }

                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(4, ex.Message);
            }

            return null;
        }


        public static List<ProtocoloDB> GetProtocolosDBCon()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            const string quote = "\"";
            //idEvento = quote + idEvento + quote;
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM protocoloDB where nroProt is not null and nroProt != " + quote + quote +" and consultado is null";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var lista = new List<ProtocoloDB>();
                        foreach (DataRow item in dt.Rows)
                        {

                            lista.Add(new ProtocoloDB
                            {
                                idEvento = Convert.ToString(item.ItemArray[0])
                                ,
                                xmlEvento = Convert.ToString(item.ItemArray[1])
                                ,
                                nroProt = item.ItemArray[2].ToString()
                                ,
                                xmlProt = item.ItemArray[3].ToString()
                                ,
                                nroRec = item.ItemArray[4].ToString()
                                ,
                                xmlRec = item.ItemArray[5].ToString()
                                ,
                                salvoDB = Convert.ToBoolean(item.ItemArray[6])
                                ,
                                baseEnv = Convert.ToString(item.ItemArray[7])
                                ,
                                erros = Convert.ToString(item.ItemArray[8])
                                ,
                                consultado = Convert.ToBoolean(item.ItemArray[9])
                            });
                        }

                        return lista;
                    }

                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(4, ex.Message);
            }

            return null;
        }

        public static List<ProtocoloDB> GetProtocolosDBReadyUpdate()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            const string quote = "\"";
            //idEvento = quote + idEvento + quote;
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM protocoloDB where consultado = 1 and salvoDB = 0";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        var lista = new List<ProtocoloDB>();
                        foreach (DataRow item in dt.Rows)
                        {

                            lista.Add(new ProtocoloDB
                            {
                                idEvento = Convert.ToString(item.ItemArray[0])
                                ,
                                xmlEvento = Convert.ToString(item.ItemArray[1])
                                ,
                                nroProt = item.ItemArray[2].ToString()
                                ,
                                xmlProt = item.ItemArray[3].ToString()
                                ,
                                nroRec = item.ItemArray[4].ToString()
                                ,
                                xmlRec = item.ItemArray[5].ToString()
                                ,
                                salvoDB = Convert.ToBoolean(item.ItemArray[6])
                                ,
                                baseEnv = Convert.ToString(item.ItemArray[7])
                                ,
                                erros = Convert.ToString(item.ItemArray[8])
                                ,
                                consultado = Convert.ToBoolean(item.ItemArray[9])
                            });
                        }

                        return lista;
                    }

                }
            }
            catch (Exception ex)
            {
                exC.ExSQLite(4, ex.Message);
            }

            return null;
        }

    }
}

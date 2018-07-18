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

namespace IntegradorCore.Services
{
    public class Atualizador
    {
        private static SQLiteConnection sqliteConnection;

        public Atualizador()
        { }

        private static SQLiteConnection DbConnection()
        {
            try
            {
                sqliteConnection = new SQLiteConnection("Data Source=c:\\vch\\dados.db;Version=3;Password=secret;");
                sqliteConnection.Open();
                return sqliteConnection;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static int VerificaTabela()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();

            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "PRAGMA table_info(protocoloDB);";
                    cmd.ExecuteNonQuery();
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);

                    if (dt.Rows.Count >= 1)
                    {
                        int verifica = 0;
                        foreach (DataRow item in dt.Rows)
                        {
                            var campo = Convert.ToString(item.ItemArray[1]);
                            var tipo = Convert.ToString(item.ItemArray[2]);
                            if(campo == "dtenvio" && tipo != "TEXT")
                            {
                                verifica++;
                            }else if(campo == "dtconsulta" && tipo != "TEXT")
                            {
                                verifica++;
                            }
                        }
                        return verifica;
                    }
                }
            }
            catch (Exception e)
            {

            }
            return 0;
                
        }

        public static void Script()
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "PRAGMA foreign_keys = 0";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS temp_table AS SELECT * FROM protocoloDB";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DROP TABLE protocoloDB";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS protocoloDB (" +
                                        "idEvento TEXT NOT NULL UNIQUE," +
                                        "xmlEvento TEXT NOT NULL," +
                                        "driver     TEXT NOT NULL," +
                                        "nroProt TEXT," +
                                        "xmlProt    TEXT," +
                                        "nroRec TEXT," +
                                        "xmlRec     TEXT," +
                                        "base       TEXT," +
                                        "msgerros TEXT," +
                                        "consultado BOOL DEFAULT 0," +
                                        "salvoDB BOOL DEFAULT 0," +
                                        "dtenvio TEXT," +
                                        "dtconsulta TEXT," +
                                        "nrprotgov TEXT," +
                                        "PRIMARY KEY(idEvento))";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO protocoloDB (" +
                                        "idEvento," +
                                        "xmlEvento," +
                                        "driver," +
                                        "nroProt," +
                                        "xmlProt," +
                                        "nroRec," +
                                        "xmlRec," +
                                        "base," +
                                        "msgerros," +
                                        "consultado," +
                                        "salvoDB," +
                                        "dtenvio," +
                                        "dtconsulta," +
                                        "nrprotgov" +
                                     ")" +
                        "SELECT idEvento," +
                               "xmlEvento," +
                               "driver," +
                               "nroProt," +
                               "xmlProt," +
                               "nroRec," +
                               "xmlRec," +
                               "base," +
                               "msgerros," +
                               "consultado," +
                               "salvoDB," +
                               "dtenvio," +
                               "dtconsulta," +
                               "nrprotgov " +
                          "FROM temp_table";

                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "DROP TABLE temp_table";
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "PRAGMA foreign_keys = 1";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Services;
using IntegradorCore.Modelos;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using IntegradorCore.NHibernate;
using IntegradorCore.NHibernate.DAO;
using NHibernate;

namespace IntegradorCore.DAO
{
    public class OracleDB
    {
        private static OracleConnection GetConnection()
        {
            string oradb = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=host1)(PORT=port1))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=servicename1)));User ID=user1;Password=password1;";

            oradb = oradb.Replace("host1", StaticParametersDB.GetHost());
            oradb = oradb.Replace("port1", StaticParametersDB.GetPort());
            oradb = oradb.Replace("servicename1", StaticParametersDB.GetServiceName());
            oradb = oradb.Replace("user1", StaticParametersDB.GetUser());
            oradb = oradb.Replace("password1", StaticParametersDB.GetPassword());

            return new OracleConnection(oradb);
        }

        public static void GetData(ISession sessao)
        {
            //var sessao = AuxiliarNhibernate.AbrirSessao();
            var ProtocoloDAO = new ProtocoloDB_DAO(sessao);

            try
            {
                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();

                    using (var comm = new OracleCommand())
                    {
                        comm.Connection = conn;
                        comm.CommandText = "SELECT ID, XMLEVENTO FROM ZMDATVIVES_EVENTOS_ESOCIAL WHERE NROPROTOCOLO IS NULL";

                        var adapter = new OracleDataAdapter(comm);
                        var dataTable = new System.Data.DataTable();

                        adapter.Fill(dataTable);

                        foreach (System.Data.DataRow row in dataTable.Rows)
                        {
                            //Armazenamento.AddProtocoloDB(
                            var prot = new Modelos.ProtocoloDB { idEvento = Convert.ToString(row["ID"]), xmlEvento = Convert.ToString(row["XMLEVENTO"]) };
                            ProtocoloDAO.Salvar(prot);
                            //);
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
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
                sql = sql.Replace(":Nrprot", string.Concat(quote + prot.nroProt + quote));
                sql = sql.Replace(":Xmlprot", string.Concat(quote + prot.xmlProt + quote));
                sql = sql.Replace(":nroRec", string.Concat(quote + prot.nroRec + quote));
                sql = sql.Replace(":xmlRec", string.Concat(quote + prot.xmlRec + quote));
                sql = sql.Replace(":Id", string.Concat(quote + prot.idEvento + quote));

                return sql;
            }
        }

        public static bool UpdateDB(ProtocoloDB prot)
        {
            
            try
            {
                using (OracleConnection conn = GetConnection())
                {
                    conn.Open();
                    using (var comm = new OracleCommand())
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
                        conn.Close();
                        return true;
                    }
                }
            }
            catch(Exception Ex)
            {
                //conn.Close();
                return false;
            }
        }
    }
}

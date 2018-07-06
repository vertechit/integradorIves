using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using NHibernate;
using NHibernate.Criterion;

namespace IntegradorCore.NHibernate.DAO
{
    public class LogConsultaDAO
    {
        private ISession sessao;
        private ICriteria criterios;

        public LogConsultaDAO(ISession sessao)
        {
            this.sessao = sessao;
            this.criterios = sessao.CreateCriteria<LogConsulta>();

        }
        public IList<LogConsulta> BuscaTodos()
        {
            return criterios.List<LogConsulta>();
        }

        public IList<LogConsulta> BuscaPorData(string valor)
        {
            criterios.Add(Restrictions.Eq("Data", valor));
            return criterios.List<LogConsulta>();
        }

        public IList<LogConsulta> BuscaPorIdentificador(string valor)
        {
            criterios.Add(Restrictions.Eq("Identificador", valor));
            return criterios.List<LogConsulta>();
        }

        public LogConsulta BuscarPorID(Int64 id)
        {
            return sessao.Load<LogConsulta>(id);
        }
        public void Salvar(LogConsulta log)
        {

            if (log != null)
            {
                sessao.Save(log);
                sessao.Flush();
            }
        }

        public void Remover(LogConsulta log)
        {
            if (log != null)
            {
                sessao.Delete(log);
                sessao.Flush();
            }
        }

        public void DeleteByData(string data)
        {
            //ISession session = sessionFactory.OpenSession();
            ITransaction tx = sessao.BeginTransaction();

            String hqlDelete = "DELETE LogConsulta c where c.Data = :data";
            // or String hqlDelete = "delete Customer where name = :oldName";
            int deletedEntities = sessao.CreateQuery(hqlDelete)
                    .SetString("data", data)
                    .ExecuteUpdate();
            tx.Commit();
            sessao.Flush();
            //session.Close();

            //sessao.CreateQuery("DELETE LogConsulta c where c.Data = :data")
                //.SetParameterList("data", data)
                //.ExecuteUpdate();
            //sessao.Delete("delete LogConsulta c where c.Data = " + data);
            //sessao.Flush();
        }
    }
}

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
    public class LogInternoDAO
    {
        private ISession sessao;
        private ICriteria criterios;

        public LogInternoDAO(ISession sessao)
        {
            this.sessao = sessao;
            this.criterios = sessao.CreateCriteria<LogInterno>();
        }

        public void Salvar(LogInterno log)
        {

            if (log != null)
            {
                sessao.Save(log);
                sessao.Flush();
            }
        }

        public void Remover(LogInterno log)
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

            String hqlDelete = "DELETE LogInterno c where c.Data = :data";
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

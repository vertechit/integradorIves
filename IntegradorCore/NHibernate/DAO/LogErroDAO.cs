using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using IntegradorCore.Services;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace IntegradorCore.NHibernate.DAO
{
    public class LogErroDAO
    {
        private ISession sessao;
        private ICriteria criterios;

        public LogErroDAO(ISession sessao)
        {
            this.sessao = sessao;
            this.criterios = sessao.CreateCriteria<LogErros>();

        }
        public IList<LogErros> BuscaTodos()
        {
            return criterios.List<LogErros>();
        }

        public IList<LogErros> BuscaPorData(string valor)
        {
            criterios.Add(Restrictions.Eq("Data", valor));
            return criterios.List<LogErros>();
        }

        public IList<LogErros> BuscaPorServico(string valor)
        {
            criterios.Add(Restrictions.Eq("Servico", valor));
            return criterios.List<LogErros>();
        }

        public LogErros Buscar(string servico, string coderro, string mensagem, string data)
        {
            //ITransaction tx = sessao.BeginTransaction();

            String hqlSelect = "select c.* from LogErros c where c.servico = :servico and c.codErro = :coderro and c.mensagem = :mensagem and c.data = :data limit 1";

            LogErros entries = (LogErros) sessao.CreateQuery(hqlSelect)
                    .SetString("servico", servico)
                    .SetString("coderro", coderro)
                    .SetString("mensagem", mensagem)
                    .SetString("data", data)
                    .UniqueResult();
            return entries;
            //tx.Commit();
            //sessao.Flush();
        }

        public LogErros BuscarPorID(Int64 id)
        {
            return sessao.Load<LogErros>(id);
        }

        public void Salvar(LogErros log)
        {

            if (log != null)
            {
                sessao.Save(log);
                sessao.Flush();
            }
        }

        public void Remover(LogErros log)
        {
            if (log != null)
            {
                sessao.Delete(log);
                sessao.Flush();
            }
        }

        public void DeleteByData(string data)
        {
            ITransaction tx = sessao.BeginTransaction();

            String hqlDelete = "DELETE LogErros c where c.Data = :data";

            int deletedEntities = sessao.CreateQuery(hqlDelete)
                    .SetString("data", data)
                    .ExecuteUpdate();
            tx.Commit();
            sessao.Flush();
        }

    }
}

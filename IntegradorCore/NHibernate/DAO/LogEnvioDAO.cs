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
    public class LogEnvioDAO
    {
        private ISession sessao;
        private ICriteria criterios;

        public LogEnvioDAO(ISession sessao)
        {
            this.sessao = sessao;
            this.criterios = sessao.CreateCriteria<LogEnvia>();

        }
        public IList<LogEnvia> BuscaTodos()
        {
            return criterios.List<LogEnvia>();
        }

        public IList<LogEnvia> BuscaPorData(string valor)
        {
            criterios.Add(Restrictions.Eq("Data", valor));
            return criterios.List<LogEnvia>();
        }

        public IList<LogEnvia> BuscaPorIdentificador(string valor)
        {
            criterios.Add(Restrictions.Eq("Identificador", valor));
            return criterios.List<LogEnvia>();
        }

        public LogEnvia Buscar(string identificador, string mensagem, string acao, string data)
        {
            //ITransaction tx = sessao.BeginTransaction();

            String hqlSelect = "select c.* from logenvia c where c.identificador = :identificador and c.mensagem = :mensagem and c.acao = :acao and c.data = :data limit 1";

            LogEnvia entries = (LogEnvia)sessao.CreateQuery(hqlSelect)
                    .SetString("identificador", identificador)
                    .SetString("mensagem", mensagem)
                    .SetString("acao", acao)
                    .SetString("data", data)
                    .UniqueResult();
            return entries;
            //tx.Commit();
            //sessao.Flush();
        }

        public LogEnvia BuscarPorID(Int64 id)
        {
            return sessao.Load<LogEnvia>(id);
        }
        public void Salvar(LogEnvia log)
        {

            if (log != null)
            {
                sessao.Save(log);
                sessao.Flush();
            }
        }

        public void Remover(LogEnvia log)
        {
            if (log != null)
            {
                sessao.Delete(log);
                sessao.Flush();
            }
        }

        public void Atualizar(LogEnvia currentLog, LogEnvia newLog)
        {
            if (newLog != null)
            {
                currentLog.Hora = newLog.Hora;

                sessao.Update(currentLog);
                sessao.Flush();
            }
        }

        public void DeleteByData(string data)
        {
            ITransaction tx = sessao.BeginTransaction();

            String hqlDelete = "DELETE LogEnvia c where c.Data = :data";

            int deletedEntities = sessao.CreateQuery(hqlDelete)
                    .SetString("data", data)
                    .ExecuteUpdate();
            tx.Commit();
            sessao.Flush();
        }
    }
}

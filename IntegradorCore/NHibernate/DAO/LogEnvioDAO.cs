﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Util;

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
            ICriterion criterio1 = Restrictions.And(Restrictions.Eq("Identificador", identificador), Restrictions.Eq("Msg", mensagem));
            ICriterion criterio2 = Restrictions.And(Restrictions.Eq("Acao", acao), Restrictions.Eq("Data", data));
            criterios.Add(Restrictions.And(criterio1, criterio2));
            return criterios.SetMaxResults(1).UniqueResult<LogEnvia>();
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

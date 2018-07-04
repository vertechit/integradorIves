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
    public class ProtocoloDB_DAO
    {
        private ISession sessao;
        private ICriteria criterios;

        public ProtocoloDB_DAO(ISession sessao)
        {
            this.sessao = sessao;
            this.criterios = sessao.CreateCriteria<ProtocoloDB>();

        }

        public ProtocoloDB BuscarPorIDEvento(string id)
        {
            return sessao.Load<ProtocoloDB>(id);
        }

        public IList<ProtocoloDB> BuscaTodos()
        {
            return criterios.List<ProtocoloDB>();
        }

        public IList<ProtocoloDB> BuscaEnvio()
        {
            criterios.Add(Restrictions.Or(Restrictions.IsNull("nroProt"), Restrictions.Eq("nroProt", "")));
            return criterios.List<ProtocoloDB>();
        }

        public IList<ProtocoloDB> BuscaConsulta()
        {
            ICriterion criterio1 = Restrictions.And(
                Restrictions.Or(Restrictions.IsNull("nroRec"), Restrictions.Eq("nroRec", "")), 
                Restrictions.Or(Restrictions.IsNull("erros"), Restrictions.Eq("erros", "")));

            ICriterion criterio2 = Restrictions.IsNotNull("nroProt");
            ICriterion criterio3 = Restrictions.And(criterio1, criterio2);

            criterios.Add(criterio3);

            return criterios.List<ProtocoloDB>();
        }

        public IList<ProtocoloDB> BuscaParaAtualizarBanco()
        {
            ICriterion criterio1 = Restrictions.Eq("consultado", true);

            criterios.Add(criterio1);

            return criterios.List<ProtocoloDB>();
        }

        public void Salvar(ProtocoloDB protocolo)
        {
            try
            {
                var prot = BuscarPorIDEvento(protocolo.idEvento);

                if (prot.xmlEvento != null)
                {
                    Atualizar(AjustaParaAtualizar(prot, protocolo));
                }
            }
            catch (Exception ex)
            {
                if (protocolo != null)
                {
                    sessao.Save(protocolo);
                    sessao.Flush();
                }
            }

        }

        public void Atualizar(ProtocoloDB protocolo)
        {
            if (protocolo != null)
            {
                sessao.Update(protocolo);
                sessao.Flush();
            }
        }

        public void Remover(ProtocoloDB parametro)
        {
            if (parametro != null)
            {
                sessao.Delete(parametro);
                sessao.Flush();
            }
        }

        public ProtocoloDB AjustaParaAtualizar(ProtocoloDB protocoloCurrent, ProtocoloDB protocoloNew)
        {
            if (protocoloNew.nroRec != null && protocoloNew.nroRec != "")
            {
                protocoloCurrent.nroRec = protocoloNew.nroRec;
            }
            if (protocoloNew.xmlRec != null && protocoloNew.xmlRec != "")
            {
                protocoloCurrent.xmlRec = protocoloNew.xmlRec;
            }
            if (protocoloNew.nroProt != null && protocoloNew.nroProt != "")
            {
                protocoloCurrent.nroProt = protocoloNew.nroProt;
            }
            if (protocoloNew.xmlProt != null && protocoloNew.xmlProt != "")
            {
                protocoloCurrent.xmlProt = protocoloNew.xmlProt;
            }
            if (protocoloNew.salvoDB != false)
            {
                protocoloCurrent.salvoDB = protocoloNew.salvoDB;
            }
            if(protocoloNew.consultado != false)
            {
                protocoloCurrent.consultado = protocoloNew.consultado;
            }
            if (protocoloNew.baseEnv != null && protocoloNew.baseEnv != "")
            {
                protocoloCurrent.baseEnv = protocoloNew.baseEnv;
            }

            return protocoloCurrent;
        }
    }
}

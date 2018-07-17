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
    public class ProtocoloDAO
    {
        private ISession sessao;
        private ICriteria criterios;

        public ProtocoloDAO(ISession sessao)
        {
            this.sessao = sessao;
            this.criterios = sessao.CreateCriteria<Protocolo>();

        }

        public Protocolo BuscarPorNomeArquivo(string nomeArquivo, string Base, long? Ambiente)
        {
            ICriterion criterio1 = Restrictions.And(Restrictions.Eq("Base", Base), Restrictions.Eq("Ambiente", Ambiente));
            criterios.Add(Restrictions.And(criterio1, Restrictions.Eq("NomeArquivo", nomeArquivo)));
            return criterios.UniqueResult<Protocolo>();
        }

        public IList<Protocolo> BuscaTodos()
        {
            return criterios.List<Protocolo>();
        }

        public void Salvar(Protocolo protocolo)
        {
            try
            {
                var prot = BuscarPorNomeArquivo(protocolo.NomeArquivo, protocolo.Base, protocolo.Ambiente);

                if (prot.NroProtocolo != null)
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

        public void Atualizar(Protocolo protocolo)
        {
            if (protocolo != null)
            {
                sessao.Update(protocolo);
                sessao.Flush();
            }
        }

        public void Remover(Protocolo parametro)
        {
            if (parametro != null)
            {
                sessao.Delete(parametro);
                sessao.Flush();
            }
        }

        public Protocolo AjustaParaAtualizar(Protocolo protocoloCurrent, Protocolo protocoloNew)
        {
            if (protocoloNew.NroProtocolo != null && protocoloNew.NroProtocolo != "")
            {
                protocoloCurrent.NroProtocolo = protocoloNew.NroProtocolo;
            }
            if (protocoloNew.NomeArquivo != null && protocoloNew.NomeArquivo != "")
            {
                protocoloCurrent.NomeArquivo = protocoloNew.NomeArquivo;
            }
            if (protocoloNew.Ambiente != null && protocoloNew.Ambiente != 0)
            {
                protocoloCurrent.Ambiente = protocoloNew.Ambiente;
            }
            if (protocoloNew.Base != null && protocoloNew.Base != "")
            {
                protocoloCurrent.Base = protocoloNew.Base;
            }

            return protocoloCurrent;
        }
    }
}

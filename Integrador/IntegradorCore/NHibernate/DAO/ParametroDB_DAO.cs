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
    public class ParametroDB_DAO
    {
        private ISession sessao;

        public ParametroDB_DAO(ISession sessao)
        {
            this.sessao = sessao;

        }

        public ParametroDB BuscarPorID(Int64 id)
        {
            return sessao.Load<ParametroDB>(id);
        }

        public void Salvar(ParametroDB parametro)
        {
            try
            {
                var param = BuscarPorID(1);

                if(param.Driver != null)
                {
                    Atualizar(param, parametro);
                }
            }
            catch(Exception ex)
            {
                if (parametro != null)
                {
                    sessao.Save(parametro);
                    sessao.Flush();
                }
            }
        }

        public void Atualizar(ParametroDB parametroCurrent, ParametroDB paramentroNew)
        {
            if (paramentroNew != null)
            {
                parametroCurrent.Driver = paramentroNew.Driver;
                parametroCurrent.Host = paramentroNew.Host;
                parametroCurrent.Port = paramentroNew.Port;
                parametroCurrent.ServiceName = paramentroNew.ServiceName;
                parametroCurrent.User = paramentroNew.User;
                parametroCurrent.Password = paramentroNew.Password;

                sessao.Update(parametroCurrent);
                sessao.Flush();
            }
        }

        public void Remover(ParametroDB parametro)
        {
            if (parametro != null)
            {
                sessao.Delete(parametro);
                sessao.Flush();
            }
        }
    }
}

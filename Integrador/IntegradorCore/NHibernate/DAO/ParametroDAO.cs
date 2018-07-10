using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using NHibernate;
using NHibernate.Criterion;
using Iesi.Collections;

namespace IntegradorCore.NHibernate.DAO
{
    public class ParametroDAO
    {
        private ISession sessao;
        //private ICriteria criterios;

        public ParametroDAO(ISession sessao)
        {
            this.sessao = sessao;
            //this.criterios = sessao.CreateCriteria<Parametro>();

        }

        public Parametro BuscarPorID(Int64 id)
        {
            return sessao.Load<Parametro>(id);
        }

        public void Salvar(Parametro parametro)
        {
            try
            {
                var param = BuscarPorID(1);

                if(param.IntegraBanco != null)
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

        public void Atualizar(Parametro parametroCurrent, Parametro paramentroNew)
        {
            if (paramentroNew != null)
            {
                parametroCurrent.CaminhoDir = paramentroNew.CaminhoDir;
                parametroCurrent.CaminhoToke = paramentroNew.CaminhoToke;
                parametroCurrent.IntegraBanco = paramentroNew.IntegraBanco;

                sessao.Update(parametroCurrent);
                sessao.Flush();
            }
        }

        public void Remover(Parametro parametro)
        {
            if (parametro != null)
            {
                sessao.Delete(parametro);
                sessao.Flush();
            }
        }

    }
}

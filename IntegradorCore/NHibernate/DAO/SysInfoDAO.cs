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
    public class SysInfoDAO
    {
        private ISession sessao;

        public SysInfoDAO(ISession sessao)
        {
            this.sessao = sessao;
        }

        public SysInfo BuscarPorID(Int64 id)
        {
            return sessao.Load<SysInfo>(id);
        }

        public void Salvar(SysInfo newinfo)
        {
            try
            {
                var sysinfo = BuscarPorID(1);

                if (sysinfo.CurrentVersion != null)
                {
                    Atualizar(sysinfo, newinfo);
                }
            }
            catch (Exception ex)
            {
                if (newinfo != null)
                {
                    sessao.Save(newinfo);
                    sessao.Flush();
                }
            }

        }

        public void Atualizar(SysInfo infoCurrent, SysInfo infoNew)
        {
            if (infoNew != null)
            {
                infoCurrent.CurrentVersion = infoNew.CurrentVersion;
                infoCurrent.NeedUpdate = infoNew.NeedUpdate;

                sessao.Update(infoCurrent);
                sessao.Flush();
            }
        }

        public void Remover(SysInfo info)
        {
            if (info != null)
            {
                sessao.Delete(info);
                sessao.Flush();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using FluentNHibernate.Mapping;

namespace IntegradorCore.Mapeamento
{
    public class SysInfoMapa : ClassMap<SysInfo>
    {
        public SysInfoMapa()
        {
            Table("sysinfo");
            Id(x => x.Id).Column("id").Not.Nullable().GeneratedBy.Increment();
            Map(x => x.CurrentVersion).Column("versao");
            Map(x => x.NeedUpdate).Column("needUpdate").Default("0");
        }
        
    }
}

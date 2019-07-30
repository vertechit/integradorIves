using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using FluentNHibernate.Mapping;

namespace IntegradorCore.Mapeamento
{
    class ParametroDBMapa : ClassMap<ParametroDB>
    {
        public ParametroDBMapa()
        {
            Table("parametrosDB");
            Id(x => x.Id).Column("id").GeneratedBy.Increment().Unique();
            Map(x => x.Driver).Column("driver").Length(50).Not.Nullable();
            Map(x => x.Host).Column("host").Length(100).Not.Nullable();
            Map(x => x.Port).Column("port").Length(20).Not.Nullable();
            Map(x => x.ServiceName).Column("servicename").Length(50).Not.Nullable();
            Map(x => x.User).Column("user").Length(50).Not.Nullable();
            Map(x => x.Password).Column("password").Length(500).Not.Nullable();
            Map(x => x.Trusted_Conn).Column("trusted_conn").Length(10).Not.Nullable().Default("True");
            Map(x => x.Ativo).Column("ativo").Not.Nullable().Default("true");
        }
    }
}

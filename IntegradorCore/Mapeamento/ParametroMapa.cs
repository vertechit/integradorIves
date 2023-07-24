using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using FluentNHibernate.Mapping;

namespace IntegradorCore.Mapeamento
{
    class ParametroMapa : ClassMap<Parametro>
    {
        public ParametroMapa()
        {
            Table("parametros");
            Id(x => x.Id).Column("id");
            Map(x => x.CaminhoToke).Column("caminhoToken").Length(200).Not.Nullable();
            Map(x => x.CaminhoDir).Column("caminhoDir").Length(200);
            Map(x => x.IntegraBanco).Column("integraBanco").Not.Nullable();
            Map(x => x.GeraLog).Column("geralog").Not.Nullable();
            Map(x => x.UrlProd).Column("urlProd").Length(300);
            Map(x => x.UrlTeste).Column("urlTeste").Length(300);
            Map(x => x.UrlQa).Column("urlQa").Length(300);

        }
    }
}

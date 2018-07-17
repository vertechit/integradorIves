using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using FluentNHibernate.Mapping;

namespace IntegradorCore.Mapeamento
{
    class LogInternoMapa : ClassMap<LogInterno>
    {
        public LogInternoMapa()
        {
            Table("loginterno");
            Id(x => x.Id).Column("id").Not.Nullable().GeneratedBy.Increment();
            Map(x => x.Servico).Column("servico");
            Map(x => x.CodErro).Column("coderro");
            Map(x => x.Mensagem).Column("msg");
            Map(x => x.InnerException).Column("innerEx");
            Map(x => x.StackTrace).Column("stacktrace");
            Map(x => x.Source).Column("source");
            Map(x => x.Base).Column("base");
            Map(x => x.Ambiente).Column("ambiente");
            Map(x => x.Data).Column("data");
        }
    }
}

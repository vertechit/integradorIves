using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using FluentNHibernate.Mapping;

namespace IntegradorCore.Mapeamento
{
    class LogConsultaMapa : ClassMap<LogConsulta>
    {
        public LogConsultaMapa()
        {
            Table("logconsulta");
            Id(x => x.Id).Column("id").Not.Nullable().GeneratedBy.Increment();
            Map(x => x.Identificador).Column("identificador");
            Map(x => x.Protocolo).Column("protocolo");
            Map(x => x.Msg).Column("mensagem");
            Map(x => x.Acao).Column("acao");
            Map(x => x.Data).Column("data");
            Map(x => x.Hora).Column("hora");
        }
    }
}

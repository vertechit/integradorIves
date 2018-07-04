using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using FluentNHibernate.Mapping;

namespace IntegradorCore.Mapeamento
{
    class ProtocoloDBMapa : ClassMap<ProtocoloDB>
    {
        public ProtocoloDBMapa()
        {
            Table("protocoloDB");
            Id(x => x.idEvento).Unique().Column("idEvento").Not.Nullable();
            Map(x => x.xmlEvento).Column("xmlEvento").Not.Nullable();
            Map(x => x.nroProt).Column("nroProt");
            Map(x => x.xmlProt).Column("xmlProt");
            Map(x => x.nroRec).Column("nroRec");
            Map(x => x.xmlRec).Column("xmlRec");
            Map(x => x.baseEnv).Column("base");
            Map(x => x.erros).Column("msgerros");
            Map(x => x.consultado).Column("consultado").Default("0");
            Map(x => x.salvoDB).Column("salvoDB").Default("0");
        }
    }
}

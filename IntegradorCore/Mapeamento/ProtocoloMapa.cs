using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;
using FluentNHibernate.Mapping;

namespace IntegradorCore.Mapeamento
{
    class ProtocoloMapa : ClassMap<Protocolo>
    {
        public ProtocoloMapa() 
        {
            Table("protocolo");
            Id(x => x.Id).GeneratedBy.Increment().Column("id");
            Map(x => x.NomeArquivo).Column("nomeArquivo").Length(200).Not.Nullable();
            Map(x => x.NroProtocolo).Column("nroProt").Length(200).Not.Nullable();
            Map(x => x.Ambiente).Column("ambiente").Length(200).Not.Nullable();
            Map(x => x.Base).Column("base").Length(200).Not.Nullable();
        }
    }
}

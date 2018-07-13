using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class Protocolo
    {
        public virtual long? Id { get; set; }
        public virtual string NomeArquivo { get; set; }
        public virtual string NroProtocolo { get; set; }
        public virtual string Base { get; set; }
        public virtual long? Ambiente { get; set; }
    }
}

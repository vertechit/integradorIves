using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class LogConsulta
    {
        public virtual long? Id { get; set; }
        public virtual string Identificador { get; set; }
        public virtual string Protocolo { get; set; }
        public virtual string Msg { get; set; }
        public virtual string Acao { get; set; }
        public virtual string Data { get; set; }
        public virtual string Hora { get; set; }
    }
}

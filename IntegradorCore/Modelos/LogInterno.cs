using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class LogInterno
    {
        public virtual long? Id { get; set; }
        public virtual string Servico { get; set; }
        public virtual string CodErro { get; set; }
        public virtual string Mensagem { get; set; }
        public virtual string InnerException { get; set; }
        public virtual string StackTrace { get; set; }
        public virtual string Source { get; set; }
        public virtual bool Base { get; set; }
        public virtual long Ambiente { get; set; }
        public virtual string Xml { get; set; }
        public virtual string Identificacao { get; set; }
        public virtual DateTime Data { get; set; }
        
    }
}

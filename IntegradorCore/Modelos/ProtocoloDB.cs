using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class ProtocoloDB
    {
        public virtual string idEvento { get; set; }
        public virtual string xmlEvento { get; set; }
        public virtual string driver { get; set; }
        public virtual string nroProt { get; set; }
        public virtual string xmlProt { get; set; }
        public virtual string nroRec { get; set; }
        public virtual string xmlRec { get; set; }
        public virtual bool salvoDB { get; set; }
        public virtual string baseEnv { get; set; }
        public virtual string erros { get; set; }
        public virtual bool consultado { get; set; }
        public virtual string dtenvio { get; set; }
        public virtual string dtconsulta { get; set; }
        public virtual string nroProtGov { get; set; }
    }
}

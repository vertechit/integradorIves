using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class ParametroDB
    {
        public virtual long? Id { get; set; }
        public virtual string Driver { get; set; }
        public virtual string Host { get; set; }
        public virtual string Port { get; set; }
        public virtual string ServiceName { get; set; }
        public virtual string User { get; set; }
        public virtual string Password { get; set; }
        public virtual string Trusted_Conn {get; set;}

    }
}

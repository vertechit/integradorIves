using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class ParametroDB
    {
        public long? Id { get; set; }
        public string Driver { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string ServiceName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

    }
}

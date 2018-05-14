using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertech.Modelos
{
    public class LogErros
    {
        public long? Id { get; set; }
        public string Servico { get; set; }
        public string CodErro { get; set; }
        public string Msg { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
    }
}

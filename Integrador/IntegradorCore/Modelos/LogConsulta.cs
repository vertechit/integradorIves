using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class LogConsulta
    {
        public long? Id { get; set; }
        public string NomeArquivo { get; set; }
        public string Protocolo { get; set; }
        public string Msg { get; set; }
        public string Acao { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
    }
}

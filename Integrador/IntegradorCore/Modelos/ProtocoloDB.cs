using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class ProtocoloDB
    {
        public string idEvento { get; set; }
        public string xmlEvento { get; set; }
        public string nroProt { get; set; }
        public string xmlProt { get; set; }
        public string nroRec { get; set; }
        public string xmlRec { get; set; }
        public bool salvoDB { get; set; }
        public string baseEnv { get; set; }
        public string erros { get; set; }
        public bool consultado { get; set; }
    }
}

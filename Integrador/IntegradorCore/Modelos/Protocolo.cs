﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class Protocolo
    {
        public long? Id { get; set; }
        public string NomeArquivo { get; set; }
        public string NroProtocolo { get; set; }
        public string Base { get; set; }
        public long? Ambiente { get; set; }
    }
}

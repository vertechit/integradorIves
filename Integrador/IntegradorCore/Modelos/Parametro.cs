﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class Parametro
    {
        public long? Id { get; set; }
        public string CaminhoDir { get; set; }
        //public string CaminhoFim { get; set; }
        public string CaminhoToke { get; set; }
        public bool IntegraBanco { get; set; }
        //public string Ambiente { get; set; }
        //public string Base { get; set; }
    }
}

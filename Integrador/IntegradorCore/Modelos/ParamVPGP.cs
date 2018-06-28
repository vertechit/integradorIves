using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class ParamVPGP
    {
        public long Id { get; set; }
        public string CaminhoDir { get; set; }
        public long Ambiente { get; set; }
        public string Base { get; set; }

        public ParamVPGP(string caminho)
        {
            Id = 1;
            CaminhoDir = caminho;
            Ambiente = 1;
            Base = "False";
        }
    }
}

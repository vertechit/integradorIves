using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class ParamVPGQ
    {
        public long Id { get; set; }
        public string CaminhoDir { get; set; }
        public long Ambiente { get; set; }
        public string Base { get; set; }

        public ParamVPGQ(string caminho)
        {
            Id = 4;
            CaminhoDir = caminho;
            Ambiente = 2;
            Base = "False";
        }
    }
}
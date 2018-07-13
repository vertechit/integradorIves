using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class ParamVTGT
    {
        public long Id { get; set; }
        public string CaminhoDir { get; set; }
        public long Ambiente { get; set; }
        public string Base { get; set; }

        public ParamVTGT(string caminho)
        {
            Id = 3;
            CaminhoDir = caminho;
            Ambiente = 2;
            Base = "True";
        }
    }
}

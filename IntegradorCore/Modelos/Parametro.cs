using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class Parametro
    {
        public virtual long? Id { get; set; }
        public virtual string CaminhoDir { get; set; }
        public virtual string CaminhoToke { get; set; }
        public virtual bool IntegraBanco { get; set; }
        public virtual bool GeraLog { get; set; }
    }
}

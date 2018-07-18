using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Modelos
{
    public class SysInfo
    {
        public virtual long? Id { get; set; }
        public virtual string CurrentVersion { get; set; }
        public virtual bool NeedUpdate { get; set; }
    }
}

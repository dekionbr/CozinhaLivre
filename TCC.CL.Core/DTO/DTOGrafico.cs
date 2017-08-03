using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.DTO
{
    [Serializable]
    public class DTOGrafico
    {
        public virtual IList<DTOItem> datasets { get; set; }
        public virtual IList<string> labels { get; set; }
    }
}

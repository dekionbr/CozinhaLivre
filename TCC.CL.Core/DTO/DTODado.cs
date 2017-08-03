using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Core.DTO
{
    [Serializable]
    public class DTODado
    {
        public string Label { get; set; }
        public int Quantidade { get; set; }
        public string Corte { get; set; }
    }
}

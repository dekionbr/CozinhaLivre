using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.DTO
{
    [Serializable]
    public class DTOItem
    {
        public string label { get; set; }
        public IList<int> data { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public int borderWidth { get { return 0; } }
    }
}

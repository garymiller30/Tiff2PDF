using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFManipulate.Shema
{
    public class EmptyTemplate
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public int Count { get; set; }
        public int Multiplier { get; set; }
        public bool IsValidated()
        {
            if (Width == 0 || Height == 0 || Count == 0 || Multiplier == 0) return false;
            return true;
        }
    }
}

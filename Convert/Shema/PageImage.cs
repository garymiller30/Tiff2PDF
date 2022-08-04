using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFManipulate.Shema
{
    public class PageImage
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public double xDpi { get; set; }
        public double yDpi { get; set; }
        public string Colorspace { get; set; }

    }
}

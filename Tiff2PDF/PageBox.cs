using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiff2PDF
{
    public class PageBox
    {
        public double Left { get; set; }
        public double Bottom { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
    }
}

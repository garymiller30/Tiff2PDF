using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiff2PDF
{
    public class AutoTrimFormat
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Bleed { get; set; }
        public double Left { get; set; }
        public double Bottom { get; set; }
    }
}

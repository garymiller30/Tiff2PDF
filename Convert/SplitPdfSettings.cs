using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFManipulate
{
    public class SplitPdfSettings
    {
        public SplitPdfModeEnum Mode { get; set; } = SplitPdfModeEnum.FixedCountPages;
        public int FixedCountPages { get; set; } = 1;

        public int[] CustomCountPages { get; set; }
        public string [] Files { get; set; }
    }
}

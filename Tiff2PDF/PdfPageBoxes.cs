using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiff2PDF
{
    public class PdfPageBoxes
    {
        public PageBox MediaBox { get; set; } = new PageBox();
        public PageBox TrimBox { get; set; } = new PageBox();
    }
}

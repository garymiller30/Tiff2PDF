using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFManipulate.Shema
{
    public class PageInfo
    {
        public List<PageFont> UsedFonts = new List<PageFont>();
        public List<PageColorspace> UsedColorspaces = new List<PageColorspace>();
        public List<PageImage> UsedImages = new List<PageImage>();
    }
}

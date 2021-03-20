using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFManipulate.Converters;

namespace PDFManipulate
{
    public class ConverterSettings
    {
        public string[] Files { get; set; }
        public ConvertModeEnum Mode { get; set; } = ConvertModeEnum.SingleFile;
    }
}

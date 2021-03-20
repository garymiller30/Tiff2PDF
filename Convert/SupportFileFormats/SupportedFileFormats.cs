using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PDFManipulate.SupportFileFormats
{
    public static class SupportedFileFormats
    {

        static readonly List<AbstractFileFormat> FileFormats = new List<AbstractFileFormat>();

        static SupportedFileFormats()
        {
            RegisterFileFormat(new Jpeg());
            RegisterFileFormat(new Bmp());
            RegisterFileFormat(new Jpg());
            //RegisterFileFormat(new Jpeg());
            RegisterFileFormat(new Tif());
            RegisterFileFormat(new Tiff());
            RegisterFileFormat(new Pdf());
            RegisterFileFormat(new Psd());
            RegisterFileFormat(new Ps());
        }

        private static void RegisterFileFormat(AbstractFileFormat abstractFileFormat)
        {
            FileFormats.Add(abstractFileFormat);
        }

        public static AbstractFileFormat GetExtension(string extension)
        {
            return FileFormats.FirstOrDefault(x =>
                       x.Extension.Equals(extension, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using PDFlib_dotnet;

namespace PDFManipulate.SupportFileFormats
{
    public class Ps : AbstractFileFormat
    {
        public override void Convert(PDFlib p, string fileName)
        {
            var pdfConvert = SupportedFileFormats.GetExtension(".pdf");
            if (pdfConvert != null)
            {
                var tmpFile = Path.GetTempFileName() + ".pdf";

                var magickSettings = new MagickReadSettings();
                magickSettings.UseMonochrome = true;
                magickSettings.Density = new Density(300);
                magickSettings.Compression = CompressionMethod.LosslessJPEG;

                using (var reader = new MagickImage(fileName, magickSettings))
                {
                    reader.Write(tmpFile);
                }

                pdfConvert.Convert(p, tmpFile);

                File.Delete(tmpFile);
            }
        }

        public override void Convert(PDFlib p, string fileName, double width, double height)
        {
            throw new NotImplementedException();
        }

        public override void Convert(PDFlib p, string fileName, double bleed)
        {
            throw new NotImplementedException();
        }

        public override void Convert(PDFlib p, string fileName, double outside, double inside, double top, double bottom)
        {
            throw new NotImplementedException();
        }

        public Ps() : base(".ps")
        {

        }
    }
}

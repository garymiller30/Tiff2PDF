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
    public class Psd : AbstractFileFormat
    {
        public override void Convert(PDFlib p, string fileName)
        {

            var pdfConvert = SupportedFileFormats.GetExtension(".jpg");
            if (pdfConvert != null)
            {
                var tmpFile = Path.GetTempFileName() + ".jpg";

                var magickSettings = new MagickReadSettings();
                magickSettings.Compression = CompressionMethod.LosslessJPEG;

                using (var reader = new MagickImage(fileName,magickSettings))
                {
                    reader.Write(tmpFile);
                }
                pdfConvert.Convert(p,tmpFile);

                File.Delete(tmpFile);

            }
        }

        public override void Convert(PDFlib p, string fileName, double width, double height)
        {
            
        }

        public override void Convert(PDFlib p, string fileName, double bleed)
        {
            
        }

        public override void Convert(PDFlib p, string fileName, double outside, double inside, double top, double bottom)
        {
        }

        public Psd() : base(".psd")
        {
        }
    }
}

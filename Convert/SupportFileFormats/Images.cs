using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFlib_dotnet;

namespace PDFManipulate.SupportFileFormats
{
    public class Images : AbstractFileFormat
    {

        public override void Convert(PDFlib p, string fileName)
        {
            p.begin_page_ext(10, 10, "");

            _importImage(p,fileName, out var width,out var height);

            var format = Ext.GetAutoTrim(width, height);

            p.end_page_ext(
                $"trimbox {{{format.X} {format.Y} {format.Width} {format.Height}}}");
        }

        public override void Convert(PDFlib p, string fileName, double width, double height)
        {
            p.begin_page_ext(10, 10, "");

            _importImage(p,fileName, out var widthImage,out var heightImage);

            var format = Ext.GetTrimBox(widthImage,heightImage,width, height);

            p.end_page_ext(
                $"trimbox {{{format.X} {format.Y} {format.Width} {format.Height}}}");
        }

        public override void Convert(PDFlib p, string fileName, double bleed)
        {
            
        }

        public override void Convert(PDFlib p, string fileName, double outside, double inside, double top, double bottom)
        {
            
        }

        protected Images(string extension) : base(extension)
        {
        }

        private void _importImage(PDFlib p,string fileName,out double width,out double height)
        {
            var image = p.load_image("auto", fileName, "honoriccprofile=false ignoremask=true");
            width = p.info_image(image, "width", "");
            height = p.info_image(image, "height", "");

            p.fit_image(image, (float)0.0, (float)0.0, "adjustpage");
            p.close_image(image);
        }
    }
}

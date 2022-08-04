using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDFlib_dotnet;

namespace PDFManipulate.SupportFileFormats
{
    public class Pdf : AbstractFileFormat
    {
        public Pdf() : base(".pdf")
        {
        }

        public override void Convert(PDFlib p, string fileName)
        {

            var indoc = p.open_pdi_document(fileName, "");
            var endpage = (int)p.pcos_get_number(indoc, "length:pages");

            for (var pageno = 1; pageno <= endpage; pageno++)
            {
                int pagehdl = p.open_pdi_page(indoc, pageno, "cloneboxes");


                p.begin_page_ext(0, 0, "");
                p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                p.end_page_ext("");
                p.close_pdi_page(pagehdl);

                //if (_importPdf(p, indoc, pageno, fileName, out var width, out var height) != -1)
                //{
                //    var format = Ext.GetAutoTrim(width,height);

                //    p.end_page_ext(
                //        $"trimbox {{{format.X} {format.Y} {format.Width} {format.Height}}}");
                //}
            }
            p.close_pdi_document(indoc);
        }

        int _importPdf(PDFlib p,int indoc, int pageno, string fileName, out double width,out double height)
        {
            var page = p.open_pdi_page(indoc, pageno, "");

            if (page == -1)
            {
                width = 0;
                height = 0;
                return page;
            }

            //get page width
             width = p.pcos_get_number(indoc, "pages[" + page + "]/width");
            //get page heigth
            height = p.pcos_get_number(indoc, "pages[" + page + "]/height");

            /* Dummy page size; will be adjusted later */
            p.begin_page_ext(0, 0, "");

            /* Create a bookmark with the file name */
            //if (pageno == 1)
            //    p.create_bookmark(fileName, "");

            /* Place the imported page on the output page, and
             * adjust the page size
             */

            p.fit_pdi_page(page, 0, 0, "adjustpage");
            p.close_pdi_page(page);

            return 0;
        }


        public override void Convert(PDFlib p, string fileName, double width, double height)
        {
            var indoc = p.open_pdi_document(fileName, "");
            var endpage = (int)p.pcos_get_number(indoc, "length:pages");

            for (var pageno = 1; pageno <= endpage; pageno++)
            {
                if (_importPdf(p, indoc, pageno, fileName, out var mediaW, out var mediaH) != -1)
                {
                    var format = Ext.GetTrimBox(mediaW,mediaH,width,height);

                    p.end_page_ext(
                        $"trimbox {{{format.X} {format.Y} {format.Width} {format.Height}}}");
                }
            }
            p.close_pdi_document(indoc);
        }

        public override void Convert(PDFlib p, string fileName, double bleed)
        {
            var indoc = p.open_pdi_document(fileName, "");
            var endpage = (int)p.pcos_get_number(indoc, "length:pages");

            for (var pageno = 1; pageno <= endpage; pageno++)
            {
                if (_importPdf(p, indoc, pageno, fileName, out var mediaW, out var mediaH) != -1)
                {

                    var format = Ext.GetTrimBox(mediaW,mediaH,bleed);

                    p.end_page_ext(
                        $"trimbox {{{format.X} {format.Y} {format.Width} {format.Height}}}");
                }
            }
            p.close_pdi_document(indoc);
        }

        public override void Convert(PDFlib p, string fileName, double outside, double inside, double top, double bottom)
        {
            var indoc = p.open_pdi_document(fileName, "");
            var endpage = (int)p.pcos_get_number(indoc, "length:pages");

            for (var pageno = 1; pageno <= endpage; pageno++)
            {
                if (_importPdf(p, indoc, pageno, fileName, out var mediaW, out var mediaH) != -1)
                {

                    var format = Ext.GetTrimBox(pageno, mediaW,mediaH,outside,inside,top,bottom);

                    p.end_page_ext(
                        $"trimbox {{{format.X} {format.Y} {format.Width} {format.Height}}}");
                }
            }
            p.close_pdi_document(indoc);
        }
    }
}

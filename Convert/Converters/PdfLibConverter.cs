using PDFlib_dotnet;
using PDFManipulate.Shema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PDFManipulate.Converters
{
    public class PdfLibConverter : AbstractConvertComponent
    {
        private readonly Document _document;

        public override void AddFile(string fileName)
        {
            _document.Add(fileName);
        }

        public void Convert()
        {
            _document.Convert();
        }

        public override void Dispose()
        {
        }

        public override void SetTrimBox()
        {
            //try
            //{
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}
        }

        public PdfLibConverter(string pdfFileName) : base(pdfFileName)
        {
            _document = new Document(pdfFileName);
        }

        public static void SetTrimBoxByBleed(string file, double bleed)
        {
            var tmpFile = Path.GetTempFileName();

            var doc = new Document(tmpFile);
            doc.Add(file);
            doc.Convert(bleed);

            var res = DialogResult.Retry;
            while (res == DialogResult.Retry)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(tmpFile, file, true);
                    break;
                }
                catch (Exception e)
                {
                    if (MessageBox.Show(e.Message, @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Retry)
                    {
                        File.Delete(tmpFile);
                        break;
                    }
                }
            }
        }

        public static void SetTrimBoxBySpread(string file, double inside, double outside, double top, double bottom)
        {
            var tmpFile = Path.GetTempFileName();

            var doc = new Document(tmpFile);
            doc.Add(file);
            doc.Convert(inside: inside, outside: outside, top: top, bottom: bottom);

            var res = DialogResult.Retry;
            while (res == DialogResult.Retry)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(tmpFile, file, true);
                    break;
                }
                catch (Exception e)
                {
                    if (MessageBox.Show(e.Message, @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Retry)
                    {
                        File.Delete(tmpFile);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// отримати інформацію по сторінках у pdf
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static PageInfo[] GetPagesInfo(string file)
        {
            try
            {
                using (var p = new PDFlib())
                {
                    var d = p.begin_document("", "");
                    var doc = p.open_pdi_document(file, "");
                    var pagecount = (int)p.pcos_get_number(doc, "length:pages");

                    var pagesInfo = new PageInfo[pagecount];

                    for (int page = 0; page < pagecount; page++)
                    {
                        var pi = new PageInfo();
                        pagesInfo[page] = pi;

                        // get font infos

                        int fontcount = (int)p.pcos_get_number(doc, $"length:pages[{page}]/fonts");
                        if (fontcount > 0)
                        {
                            for (int i = 0; i < fontcount; i++)
                            {
                                var fi = new PageFont();
                                fi.Name = p.pcos_get_string(doc, $"pages[{page}]/fonts[{i}]/name");
                                fi.Embedded = p.pcos_get_number(doc, $"pages[{page}]/fonts[{i}]/embedded") != 0;
                                pi.UsedFonts.Add(fi);
                            }
                        }

                        //get images infos
                        pi.UsedImages.AddRange(GetPageImagesInfo(p, doc, page));

                        // get colorspace infos
                        pi.UsedColorspaces.AddRange(GetColorspace(p, doc, page));
                    }

                    p.close_pdi_document(doc);

                    return pagesInfo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// розділити файл на окремі сторінки
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pages"></param>
        public static void SplitPDF(string file, int pages)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                var outfile_basename = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");
                int outdoc_count = page_count / pages
                                   + (page_count % pages > 0 ? 1 : 0);

                for (int outdoc_counter = 0, page = 0;
                    outdoc_counter < outdoc_count; outdoc_counter += 1)
                {
                    String outfile = outfile_basename + "_" + (outdoc_counter + 1)
                                     + ".pdf";

                    /*
                     * Open new sub-document.
                     */
                    if (p.begin_document(outfile, "") == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    for (int i = 0; page < page_count && i < pages;
                        page += 1, i += 1)
                    {
                        /* Page size may be adjusted by fit_pdi_page() */
                        p.begin_page_ext(0, 0, "");

                        int pagehdl = p.open_pdi_page(indoc, page + 1, "cloneboxes");
                        if (pagehdl == -1)
                            throw new Exception("Error: " + p.get_errmsg());

                        /*
                         * Place the imported page on the output page, and adjust
                         * the page size
                         */
                        p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                        p.close_pdi_page(pagehdl);

                        p.end_page_ext("");
                    }

                    /* Close the current sub-document */
                    p.end_document("");
                }

                /* Close the input document */
                p.close_pdi_document(indoc);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        public static void CreateRectangle(string file)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                var dir = Path.GetDirectoryName(file);
                var filename = Path.GetFileNameWithoutExtension(file);
                var outfile = Path.Combine(dir, filename + "_rect.pdf");
                if (p.begin_document(outfile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int j = 1; j <= page_count; j++)
                {
                    int pagehdl = p.open_pdi_page(indoc, j, "");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    //get page width
                    var width = p.pcos_get_number(indoc, $"pages[{pagehdl}]/width");
                    //get page height
                    var height = p.pcos_get_number(indoc, $"pages[{pagehdl}]/height");

                    var trimbox = GetTrimBox(indoc, pagehdl, p);

                    p.begin_page_ext(width, height, "");

                    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");

                    p.set_gstate(gstate);
                    p.setcolor("fillstroke", "cmyk", 0.79, 0, 0.44, 0.21);

                    int spot = p.makespotcolor("ProofColor");

                    p.setlinewidth(1.0);

                    /* Red rectangle */
                    p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);
                    p.rect(trimbox.X, trimbox.Y, trimbox.Width, trimbox.Height);
                    p.stroke();

                    p.close_pdi_page(pagehdl);

                    p.end_page_ext($"trimbox {{{trimbox.X} {trimbox.Y} {trimbox.X + trimbox.Width} {trimbox.Height + trimbox.Y}}}");
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(indoc);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        public static void CreateElipse(string file)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                var dir = Path.GetDirectoryName(file);
                var filename = Path.GetFileNameWithoutExtension(file);
                var outfile = Path.Combine(dir, filename + "_ellipse.pdf");
                if (p.begin_document(outfile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int j = 1; j <= page_count; j++)
                {
                    int pagehdl = p.open_pdi_page(indoc, j, "");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    //get page width
                    var width = p.pcos_get_number(indoc, $"pages[{pagehdl}]/width");
                    //get page heigth
                    var height = p.pcos_get_number(indoc, $"pages[{pagehdl}]/height");

                    var trimbox = GetTrimBox(indoc, pagehdl, p);

                    p.begin_page_ext(width, height, "");

                    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");
                    p.set_gstate(gstate);

                    p.setcolor("fillstroke", "cmyk", 0.79, 0, 0.44, 0.21);
                    int spot = p.makespotcolor("ProofColor");

                    p.setlinewidth(1.0);

                    /* Red rectangle */
                    p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);
                    p.ellipse(width / 2, height / 2, trimbox.Width / 2, trimbox.Height / 2);
                    p.stroke();

                    p.close_pdi_page(pagehdl);

                    p.end_page_ext($"trimbox {{{trimbox.X} {trimbox.Y} {trimbox.X + trimbox.Width} {trimbox.Height + trimbox.Y}}}");
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(indoc);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        internal static void SplitOddAndEven(string file)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                var outfile_basename = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                string outFile = $"{outfile_basename}_odd.pdf";
                if (p.begin_document(outFile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                //odd
                for (int i = 0; i < page_count; i+=2)
                {
                    p.begin_page_ext(0, 0, "");
                    int pagehdl = p.open_pdi_page(indoc, i+1, "cloneboxes");
                    if (pagehdl == -1) throw new Exception("Error: " + p.get_errmsg());
                    
                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);
                    p.end_page_ext("");
                    
                }
                p.end_document("");
                outFile = $"{outfile_basename}_even.pdf";
                if (p.begin_document(outFile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                //even
                for (int i = 1; i <= page_count; i += 2)
                {
                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(indoc, i + 1, "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);

                    p.end_page_ext("");
                }
                p.end_document("");


                p.close_pdi_document(indoc);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        internal static void RotateMirrorFrontAndBack(string file)
        {
            PDFlib p = null;

            bool back = false;
            string[] angles = new string[2] { "west", "east" };


            try
            {
                p = new PDFlib();
                p.set_option("errorpolicy=return");

                var outputFile = Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}_90grad{Path.GetExtension(file)}");
                p.begin_document(outputFile, "");

                int indoc = p.open_pdi_document(file, "");
                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                var endpage = (int)p.pcos_get_number(indoc, "length:pages");


                for (var pageno = 1; pageno <= endpage; pageno++)
                {
                    var page = p.open_pdi_page(indoc, pageno, "");
                    var width = p.pcos_get_number(indoc, $"pages[{pageno-1}]/width");
                    var height = p.pcos_get_number(indoc, $"pages[{pageno-1}]/height");
                    var trimbox = GetTrimBox(indoc, pageno - 1, p);

                    if (back)
                    {
                        trimbox.RotateCounerClockWise90deg(new Box() { Width = width, Height = height });
                    }
                    else
                    {
                        trimbox.RotateClockWise90deg();
                    }

                    p.begin_page_ext(height, width, "");
                    p.fit_pdi_page(page, 0, 0, $"adjustpage orientate={angles[back ? 1 : 0]}");
                    p.end_page_ext($"trimbox {{{trimbox.X} {trimbox.Y} {trimbox.X + trimbox.Width} {trimbox.Height + trimbox.Y}}}");
                    p.close_pdi_page(page);

                    back = !back;
                }

                p.end_document("");

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                p?.Dispose();
            }
        }

        private static Box GetTrimBox(int doc, int page, PDFlib p)
        {
            var trims = new double[] { 0, 0, 0, 0 };
            var media = new double[] { 0, 0, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                media[i] = p.pcos_get_number(doc, $"pages[{page}]/MediaBox[{i}]");
                // "pages[" + ShablonPage + "]/" + "MediaBox" + "[" + i + "]");
            }

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = p.pcos_get_number(doc, $"pages[{page}]/TrimBox[{i}]");
                    // "pages[" + ShablonPage + "]/" + "TrimBox" + "[" + i + "]");
                }
            }
            catch
            {
                //Debug.WriteLine(e);
                for (int i = 0; i < 4; i++)
                {
                    trims[i] = media[i];
                    // "pages[" + ShablonPage + "]/" + "MediaBox" + "[" + i + "]");
                }
            }

            var box = new Box();

            //            pdfPage.MediaBox.Width = width;
            //pdfPage.MediaBox.Height = height;

            box.X = trims[0] - media[0];
            box.Y = trims[1] - media[1];
            //pdfPage.TrimBox.Right = trims[2] - media[2];
            //pdfPage.TrimBox.Top = trims[3] - media[3];

            box.Width = trims[2] - trims[0];
            box.Height = trims[3] - trims[1];

            return box;
        }

        public static void RepeatDocument(string file, int cnt)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                String outfile = file + ".tmp";
                if (p.begin_document(outfile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < page_count; j++)
                    {
                        p.begin_page_ext(0, 0, "");

                        int pagehdl = p.open_pdi_page(indoc, j + 1, "cloneboxes");
                        if (pagehdl == -1)
                            throw new Exception("Error: " + p.get_errmsg());

                        /*
                         * Place the imported page on the output page, and adjust
                         * the page size
                         */
                        p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                        p.close_pdi_page(pagehdl);

                        p.end_page_ext("");
                    }
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(indoc);

                File.Delete(file);
                File.Move(outfile, file);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        /// <summary>
        /// зберігти сторінки у зворотньому напрямку
        /// </summary>
        /// <param name="file"></param>
        public static void ReversePages(string file)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.set_option("errorpolicy=return");

                var outfile = Path.Combine(Path.GetDirectoryName(file),
                    Path.GetFileNameWithoutExtension(file) + "_reverse.pdf");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                if (p.begin_document(outfile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                /* Loop over all subsequent pages in reverse order */
                for (var pageno = page_count; pageno > 0; pageno--)
                {
                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(indoc, pageno, "cloneboxes");
                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);

                    p.end_page_ext("");
                }

                p.end_document("");
                p.close_pdi_document(indoc);
            }
            catch (PDFlibException e)
            {
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        private static IEnumerable<PageImage> GetPageImagesInfo(PDFlib p, int doc, int page)
        {
            var iiList = new List<PageImage>();

            int images_on_page = (int)p.pcos_get_number(doc, $"length:pages[{page}]/images");
            if (images_on_page > 0)
            {
                for (int image = 0; image < images_on_page; image += 1)
                {
                    var pi = new PageImage();

                    pi.Width = (int)p.pcos_get_number(doc, $"pages[{page}]/images[{image}]/Width");
                    pi.Height = (int)p.pcos_get_number(doc, $"pages[{page}]/images[{image}]/Height");
                    //pi.xDpi = (int) p.pcos_get_number(doc, $"images[{image}]/bpc");
                    pi.Colorspace = check_colorspace(p, doc, page, image);

                    iiList.Add(pi);
                }
            }

            return iiList;
        }

        private static string check_colorspace(PDFlib p, int doc, int page, int image)
        {
            string objtype = p.pcos_get_string(doc, "type:pages[" + page + "]/images[" + image + "]/ColorSpace");

            if (objtype.Equals("name"))
            {
                return p.pcos_get_string(doc, "pages[" + page + "]/images[" + image + "]/ColorSpace");
            }

            if (objtype.Equals("array"))
            {
                String cs = p.pcos_get_string(doc, "pages[" + page + "]/images[" + image + "]/ColorSpace[0]");

                if (cs.Equals("Indexed") || cs.Equals("Separation"))
                {
                    return p.pcos_get_string(doc, "pages[" + page + "]/images[" + image + "]/ColorSpace[1]");
                }
                //if (cs.Equals("ICCBased"))
                //{
                //    var iccprofileid = p.pcos_get_number(doc,"pages[" + page + "]/images[" + image + "]/alternateid");
                ////    String profilename = p.pcos_get_string(doc,"iccprofiles[" + iccprofileid + "]/profilename");
                ////    return p.pcos_get_string(doc, "pages[" + page + "]/images[" + image + "]/AlternateColorSpace[0]");
                //}
                return cs;
            }
            if (objtype.Equals("null"))
            {
                return "ImageMask";
            }

            return "(complex ColorSpace)";
        }

        private static IEnumerable<PageColorspace> GetColorspace(PDFlib p, int doc, int page)
        {
            var csList = new List<PageColorspace>();

            int colorspacecount = (int)p.pcos_get_number(doc, $"length:pages[{page}]/colorspaces");
            if (colorspacecount > 0)
            {
                for (int i = 0; i < colorspacecount; i++)
                {
                    var cs = new PageColorspace();
                    cs.Name = print_colorspace(p, doc, 0, $"pages[{page}]/colorspaces[{i}]");
                    csList.Add(cs);
                }
            }

            return csList;
        }

        private static string print_colorspace(PDFlib p, int doc, int level, string colorspace_path)
        {
            var name = p.pcos_get_string(doc, colorspace_path + "/name");
            int pcosinterface = (int)p.pcos_get_number(doc, "pcosinterface");

            if (name.Equals("Separation"))
            {
                String colorant = p.pcos_get_string(doc, colorspace_path + "/colorantname");
                return colorant;
            }

            if (name.Equals("ICCBased"))
            {
                //if (pcosinterface >= 10)
                //{
                int iccprofileid = (int)p.pcos_get_number(doc, colorspace_path + "/iccprofileid");
                String profilename = p.pcos_get_string(doc, "iccprofiles[" + iccprofileid + "]/profilename");
                return profilename;
                //}
            }

            return name;
        }

        public static void SetTrimBox(string file, double width, double height)
        {
            var tmpFile = Path.GetTempFileName();

            var doc = new Document(tmpFile);
            doc.Add(file);
            doc.Convert(width, height);

            var res = DialogResult.Retry;
            while (res == DialogResult.Retry)
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(tmpFile, file, true);
                    break;
                }
                catch (Exception e)
                {
                    if (MessageBox.Show(e.Message, @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Retry)
                    {
                        File.Delete(tmpFile);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// дублювати сторінки потрібну кількість разів
        /// </summary>
        /// <param name="file"></param>
        /// <param name="pages"></param>
        public static void RepeatPages(string file, int pages)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                //var tmpfile = Path.Combine(file,".tmp") ;

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");
                int outdoc_count = page_count * pages;

                String outfile = file + ".tmp";
                if (p.begin_document(outfile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int i = 0; i < page_count; i++)
                {
                    int pagehdl = p.open_pdi_page(indoc, i + 1, "cloneboxes");
                    for (int j = 0; j < pages; j++)
                    {
                        p.begin_page_ext(0, 0, "");

                        if (pagehdl == -1)
                            throw new Exception("Error: " + p.get_errmsg());

                        /*
                         * Place the imported page on the output page, and adjust
                         * the page size
                         */
                        p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");

                        p.end_page_ext("");
                    }
                    p.close_pdi_page(pagehdl);
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(indoc);

                File.Delete(file);
                File.Move(outfile, file);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        /// <summary>
        /// об'єднати файл з лицями з файлом звороту, таким чином, щоб вийшло лице-зворот, лице-зворот...
        /// </summary>
        /// <param name="front"></param>
        /// <param name="back"></param>
        public static void MergeFrontsAndBack(string front, string back)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                //var tmpfile = Path.Combine(file,".tmp") ;

                p.set_option("errorpolicy=return");

                int frontfile = p.open_pdi_document(front, "");

                if (frontfile == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int backfile = p.open_pdi_document(back, "");
                if (backfile == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(frontfile, "length:pages");

                String outfile = Path.Combine(Path.GetDirectoryName(front),
                    Path.GetFileNameWithoutExtension(front) + "_merged.pdf");

                if (p.begin_document(outfile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int i = 0; i < page_count; i++)
                {
                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(frontfile, i + 1, "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);
                    p.end_page_ext("");

                    p.begin_page_ext(0, 0, "");

                    int pageback = p.open_pdi_page(backfile, 1, "cloneboxes");

                    p.fit_pdi_page(pageback, 0, 0, "cloneboxes");
                    p.close_pdi_page(pageback);

                    p.end_page_ext("");
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(frontfile);
                p.close_pdi_document(backfile);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        public static void MergeOddAndEven(string odd,string even)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.set_option("errorpolicy=return");

                int frontfile = p.open_pdi_document(odd, "");

                if (frontfile == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int backfile = p.open_pdi_document(even, "");
                if (backfile == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(frontfile, "length:pages");

                string outfile = Path.Combine(Path.GetDirectoryName(odd),
                    Path.GetFileNameWithoutExtension(odd) + "_merged.pdf");

                if (p.begin_document(outfile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int i = 0; i < page_count; i++)
                {
                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(frontfile, i + 1, "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);
                    p.end_page_ext("");

                    p.begin_page_ext(0, 0, "");

                    int pageback = p.open_pdi_page(backfile, i + 1, "cloneboxes");

                    p.fit_pdi_page(pageback, 0, 0, "cloneboxes");
                    p.close_pdi_page(pageback);

                    p.end_page_ext("");
                }

                p.end_document("");

                /* Close the input document */
                p.close_pdi_document(frontfile);
                p.close_pdi_document(backfile);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        public static void SplitPDF(string file, IEnumerable<int> customCountPages)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                var outfile_basename = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                if (page_count < customCountPages.Sum()) throw new Exception("Error: кількість вказаних сторінок більша за кількість сторінок в документі");

                int lastPage = customCountPages.Sum();
                int nextPage = 0;

                var pagesCount = customCountPages.ToList();

                if (page_count > lastPage)
                {
                    pagesCount.Add(page_count - lastPage);
                }


                for (int i = 0; i < pagesCount.Count; i++)
                {
                    string outFile = $"{outfile_basename}_{i + 1}.pdf";

                    if (p.begin_document(outFile, "") == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    for (int j = 0; j < pagesCount[i]; j++)
                    {
                        p.begin_page_ext(0, 0, "");

                        int pagehdl = p.open_pdi_page(indoc, nextPage + 1, "cloneboxes");
                        if (pagehdl == -1)
                            throw new Exception("Error: " + p.get_errmsg());

                        nextPage++;

                        /*
                         * Place the imported page on the output page, and adjust
                         * the page size
                         */
                        p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                        p.close_pdi_page(pagehdl);

                        p.end_page_ext("");
                    }
                    p.end_document("");
                }

                p.close_pdi_document(indoc);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        public static void ExtractPages(string file, IEnumerable<int> pages)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                var outfile_basename = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));

                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");

                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                //if (page_count < customCountPages.Sum()) throw new Exception("Error: кількість вказаних сторінок більша за кількість сторінок в документі");

                int maxPage = pages.Max();
                if (maxPage > page_count) throw new Exception("Error: Номер сторінки більший за кількість сторінок в документі");

                var pagesCount = pages.ToList();

                for (int i = 0; i < pagesCount.Count; i++)
                {
                    string outFile = $"{outfile_basename}_{pagesCount[i]}.pdf";

                    if (p.begin_document(outFile, "") == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.begin_page_ext(0, 0, "");

                    int pagehdl = p.open_pdi_page(indoc, pagesCount[i], "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    /*
                     * Place the imported page on the output page, and adjust
                     * the page size
                     */
                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);

                    p.end_page_ext("");
                    p.end_document("");
                }

                p.close_pdi_document(indoc);
            }
            catch (Exception e)
            {
            }
            finally
            {
                p?.Dispose();
            }
        }

        public static void SplitCoverAndBlock(string file)
        {
            PDFlib p = null;
            try
            {
                p = new PDFlib();
                var outfile_basename = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                p.set_option("errorpolicy=return");

                int indoc = p.open_pdi_document(file, "");
                if (indoc == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                int page_count = (int)p.pcos_get_number(indoc, "length:pages");

                if (page_count < 5) throw new Exception("Error: count pages less than 5");

                int[] cover = new int[4] { 1, 2, page_count - 1, page_count };

                string outFile = $"{outfile_basename}_cover.pdf";
                if (p.begin_document(outFile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());



                foreach (int i in cover)
                {
                    p.begin_page_ext(0, 0, "");
                    int pagehdl = p.open_pdi_page(indoc, i, "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);

                    p.end_page_ext("");
                }
                p.end_document("");

                outFile = $"{outfile_basename}_block.pdf";
                if (p.begin_document(outFile, "") == -1)
                    throw new Exception("Error: " + p.get_errmsg());

                for (int i = 3; i <= page_count - 2; i++)
                {
                    p.begin_page_ext(0, 0, "");
                    int pagehdl = p.open_pdi_page(indoc, i, "cloneboxes");
                    if (pagehdl == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    p.fit_pdi_page(pagehdl, 0, 0, "cloneboxes");
                    p.close_pdi_page(pagehdl);

                    p.end_page_ext("");
                }
                p.end_document("");

                p.close_pdi_document(indoc);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                p?.Dispose();
            }
        }

        public static void CreateEmptyPdfTemplateWithCount(string pathTo, EmptyTemplate template)
        {
            PDFlib p = null;

            try
            {
                p = new PDFlib();

                p.set_option("errorpolicy=return");

                var filename = $"{template.Width}x{template.Height}";

              

                for (int i = 1; i <= template.Multiplier; i++)
                {
                    int fileCount = i;

                    string outfile;
                    do
                    {
                        outfile = Path.Combine(pathTo, $"{filename}_{fileCount}#{template.Count}.pdf");
                        fileCount++;
                    } while (File.Exists(outfile));

                    if (p.begin_document(outfile, "") == -1)
                        throw new Exception("Error: " + p.get_errmsg());

                    Box trimbox = new Box();
                    trimbox.CreateCustomBox(template.Width, template.Height, 3);

                    var (width, height) = trimbox.GetMediaBox();

                    p.begin_page_ext(width, height, "");

                    int gstate = p.create_gstate("overprintmode=1 overprintfill=true overprintstroke=true");

                    p.set_gstate(gstate);
                    p.setcolor("fillstroke", "cmyk", 0.79, 0, 0.44, 0.21);

                    int spot = p.makespotcolor("ProofColor");

                    p.setlinewidth(1.0);

                    /* Red rectangle */
                    p.setcolor("stroke", "spot", spot, 1.0, 0.0, 0.0);
                    p.rect(trimbox.X, trimbox.Y, trimbox.Width, trimbox.Height);
                    p.stroke();

                    p.end_page_ext($"trimbox {{{trimbox.X} {trimbox.Y} {trimbox.X + trimbox.Width} {trimbox.Height + trimbox.Y}}}");
                    p.end_document("");

                  
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                p?.Dispose();
            }
        }


    }
}
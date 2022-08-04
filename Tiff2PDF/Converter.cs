using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
//using ImageMagick;
//using PDFlib_dotnet;

namespace Tiff2PDF
{
//    public sealed class Converter
//    {
//        public delegate void FinishHandle();
//
//
//        public delegate void ProgressHandle(int p);
//
//        private const double Mn = 2.83465;
//        private readonly List<AutoTrimFormat> _autoTrimFormats = new List<AutoTrimFormat>();
//
//        private readonly Dictionary<string, ImageConverter> _extensions = new Dictionary<string, ImageConverter>();
//
//        private readonly List<string> _tiffFileNames = new List<string>();
//
//        public bool MultiplyMode;
//        public bool OpenAfterConvert;
//
//        public Converter()
//        {
//            _extensions.Add(".bmp", _images);
//            _extensions.Add(".ccitt", _images);
//            _extensions.Add(".g3", _images);
//            _extensions.Add(".g4", _images);
//            _extensions.Add(".fax", _images);
//            _extensions.Add(".gif", _images);
//            _extensions.Add(".jbig2", _images);
//            _extensions.Add(".jb2", _images);
//            _extensions.Add(".jpg", _images);
//            _extensions.Add(".jpeg", _images);
//            _extensions.Add(".jpx", _images);
//            _extensions.Add(".jp2", _images);
//            _extensions.Add(".jpf", _images);
//            _extensions.Add(".j2k", _images);
//            _extensions.Add(".png", _images);
//            _extensions.Add(".raw", _images);
//            _extensions.Add(".tif", _images);
//            _extensions.Add(".tiff", _images);
//            _extensions.Add(".psd", _psdFile);
//
//            _extensions.Add(".pdf", _pdf);
//
//            LoadAutoTrimsFormats();
//        }
//
//        public int CountFiles { get; private set; }
//
//        public event ProgressHandle EventProgress = delegate {};
//
//        public event FinishHandle EventFinish = delegate {};
//
//
//        public void Start()
//        {
//            if (MultiplyMode)
//            {
//                Task.Factory.StartNew(Multiple);
//            }
//            else
//                Task.Factory.StartNew(Single);
//
//
//            //_tiffFileNames.Clear();
//        }
//
//        private void Single()
//        {
//            var p = new PDFlib();
//
//            for (var index = 0; index < _tiffFileNames.Count; index++)
//            {
//                var file = _tiffFileNames[index];
//                var fn = file + ".pdf";
//
//                var extension = Path.GetExtension(file);
//
//                if (extension != null)
//                {
//                    var ext = extension.ToLower();
//
//                    if (_extensions.ContainsKey(ext))
//                    {
//                        p.begin_document(fn, "");
//                        //p.set_option("preserveoldpantonenames=true");
//                        try
//                        {
//                            var act = _extensions[ext];
//                            act(p, file);
//                            p.end_document("");
//                            //SetAutoTrimBox(fn);
//                            if (OpenAfterConvert) OpenFile(fn);
//                        }
//                        catch (Exception e)
//                        {
//                            MessageBox.Show(e.Message);
//                        }
//                        finally
//                        {
//                            EventProgress(index + 1);
//                        }
//                    }
//                }
//            }
//
//            _tiffFileNames.Clear();
//            EventFinish();
//
//            p.Dispose();
//        }
//
//        private void Multiple()
//        {
//            var p = new PDFlib();
//
//            var fn = _tiffFileNames[0] + ".pdf";
//
//
//            p.begin_document(fn, "");
//
//            for (var index = 0; index < _tiffFileNames.Count; index++)
//            {
//                var file = _tiffFileNames[index];
//                var extension = Path.GetExtension(file);
//                if (extension != null)
//                {
//                    var ext = extension.ToLower();
//                    if (_extensions.ContainsKey(ext))
//                    {
//                        var act = _extensions[ext];
//                        act(p, file);
//                    }
//                }
//
//                EventProgress(index+1);
//            }
//
//            p.end_document("");
//
//            _tiffFileNames.Clear();
//            //SetAutoTrimBox(fn);
//            if (OpenAfterConvert) OpenFile(fn);
//
//            EventFinish();
//            p.Dispose();
//        }
//
//        private void _pdf(PDFlib p, string file)
//        {
//            int pageno;
//
//            var indoc = p.open_pdi_document(file, "");
//            var endpage = (int) p.pcos_get_number(indoc, "length:pages");
//
//            // отримати Mediabox і Trimbox
//            var pdfPagesBoxes = GetFileAllTrims(file);
//
//
//            for (pageno = 1; pageno <= endpage; pageno++)
//            {
//                var page = p.open_pdi_page(indoc, pageno, "");
//
//                if (page == -1)
//                {
//                    continue;
//                }
//                var format = GetFormat(pdfPagesBoxes[pageno - 1].TrimBox.Width, pdfPagesBoxes[pageno - 1].TrimBox.Height);
//
//                /* Dummy page size; will be adjusted later */
//                p.begin_page_ext(0, 0, "");
//
//                /* Create a bookmark with the file name */
//                if (pageno == 1)
//                    p.create_bookmark(file, "");
//
//                /* Place the imported page on the output page, and
//                 * adjust the page size
//                 */
//                
//                
//
//                p.fit_pdi_page(page, 0, 0, "adjustpage");
//                p.close_pdi_page(page);
//
//                p.end_page_ext(
//                $"trimbox {{{format.Left} {format.Bottom} {format.Left + format.Width} {format.Height + format.Bottom}}}");
//            }
//            p.close_pdi_document(indoc);
//        }
//
//        /// <summary>
//        /// отримати всі тріми сторінок
//        /// </summary>
//        /// <param name="file"></param>
//        /// <returns></returns>
//        public static PdfPageBoxes[] GetFileAllTrims(string file)
//        {
//
//            var pages = GetCntPages(file);
//
//            var pdfPageBoxeses = new PdfPageBoxes[pages];
//
//            var trims = new double[] { 0, 0, 0, 0 };
//            var media = new double[] { 0, 0, 0, 0 };
//
//            using (var p = new PDFlib())
//            {
//
//                p.begin_document("", "");
//                var doc = p.open_pdi_document(file, ""); // откроем файл
//
//
//                for (int ip = 1; ip <= pages; ip++)
//                {
//                    var page = p.open_pdi_page(doc, ip, "");
//
//                    if (page != -1)
//                    {
//                        //get page width
//                        var width = p.pcos_get_number(doc, "pages[" + page + "]/width");
//                        //get page heigth
//                        var height = p.pcos_get_number(doc, "pages[" + page + "]/height");
//
//                        for (int i = 0; i < 4; i++)
//                        {
//                            media[i] = p.pcos_get_number(doc, $"pages[{page}]/MediaBox[{i}]");
//                            // "pages[" + ShablonPage + "]/" + "MediaBox" + "[" + i + "]");
//                        }
//
//                        try
//                        {
//                            for (int i = 0; i < 4; i++)
//                            {
//                                trims[i] = p.pcos_get_number(doc, $"pages[{page}]/TrimBox[{i}]");
//                                // "pages[" + ShablonPage + "]/" + "TrimBox" + "[" + i + "]");
//                            }
//
//                        }
//                        catch
//                        {
//                            //Debug.WriteLine(e);
//                            for (int i = 0; i < 4; i++)
//                            {
//                                trims[i] = media[i];
//                                // "pages[" + ShablonPage + "]/" + "MediaBox" + "[" + i + "]");
//                            }
//                        }
//                        p.close_pdi_page(page);
//
//                        var pdfPage = new PdfPageBoxes();
//
//                        pdfPage.MediaBox.Width = width;
//                        pdfPage.MediaBox.Height = height;
//
//                        pdfPage.TrimBox.Left = trims[0] - media[0];
//                        pdfPage.TrimBox.Bottom = trims[1] - media[1];
//                        pdfPage.TrimBox.Right = trims[2] - media[2];
//                        pdfPage.TrimBox.Top = trims[3] - media[3];
//
//                        pdfPage.TrimBox.Width = trims[2] - trims[0];
//                        pdfPage.TrimBox.Height = trims[3] - trims[1];
//
//                        pdfPageBoxeses[ip - 1] = pdfPage;
//                    }
//                }
//                p.close_pdi_document(doc);
//
//            }
//            return pdfPageBoxeses;
//        }
//
//        private static int GetCntPages(string file)
//        {
//            int pages = 0;
//
//            try
//            {
//                using (var p = new PDFlib()) // инициализировали библиотеку
//                {
//                    p.begin_document("", "");         //создадим новый файл, куда будем кидать все
//                    int doc = p.open_pdi_document(file, "");
//                    pages = (int)p.pcos_get_number(doc, "length:pages");
//                    p.close_pdi_document(doc);
//                }
//            }
//            catch (Exception)
//            {
//
//
//            }
//
//
//            return pages;
//
//        }
//
//
//        /// <summary>
//        /// конвертація зображень
//        /// </summary>
//        /// <param name="p"></param>
//        /// <param name="file"></param>
//        private void _images(PDFlib p, string file)
//        {
//            p.begin_page_ext(10, 10, "");
//
//            var image = p.load_image("auto", file, "honoriccprofile=false ignoremask=true");
//            var width = p.info_image(image, "width", "");
//            var height = p.info_image(image, "height", "");
//
//            p.fit_image(image, (float) 0.0, (float) 0.0, "adjustpage");
//            p.close_image(image);
//            
//            var format = GetFormat(width, height);
//
//            p.end_page_ext(
//                $"trimbox {{{format.Left} {format.Bottom} {format.Left + format.Width} {format.Height + format.Bottom}}}");
//            //p.end_page_ext("");
//        }
//
//        private void _tifFile(PDFlib p, string file)
//        {
//            var target = file + "_temp.pdf";
//            using (var reader = new MagickImage(file))
//            {
//                reader.Write(target);
//            }
//
//            _pdf(p, target);
//
//            File.Delete(target);
//        }
//
//
//        private void _psdFile(PDFlib p, string file)
//        {
//            var target = file + "_temp.pdf";
//
//
//            using (var reader = new MagickImage(file))
//            {
//                reader.Write(target);
//            }
//
//            _pdf(p,target);
//
//            File.Delete(target);
//            
//        }
//
//        private void OpenFile(string fn)
//        {
//            try
//            {
//                Process.Start(fn);
//            }
//            catch (Exception)
//            {
//                // ignored
//            }
//        }
//
//
//        public void Add(string[] files)
//        {
//            _tiffFileNames.AddRange(files);
//            CountFiles = files.Length;
//        }
//
//
//        private AutoTrimFormat GetFormat(double pageWidth, double pageHeight)
//        {
//            const double delta = 1;
//
//            foreach (var format in _autoTrimFormats)
//            {
//                if (pageWidth >= format.Width + format.Bleed * 2 - delta * Mn &&
//                    pageWidth <= format.Width + format.Bleed * 2 + delta * Mn &&
//                    pageHeight >= format.Height + format.Bleed * 2 - delta * Mn &&
//                    pageHeight <= format.Height + format.Bleed * 2 + delta * Mn)
//                {
//
//                    return new AutoTrimFormat{Bleed = format.Bleed,Bottom = (pageWidth - format.Width)/2, Left = (pageHeight - format.Height)/2, Width = format.Width, Height = format.Height};
//                }
//            }
//
//            return new AutoTrimFormat{Height = pageHeight, Width = pageWidth};
//        }
//
//
//        private void LoadAutoTrimsFormats()
//        {
//            _autoTrimFormats.Clear();
//
//
//            if (File.Exists("AutoTrimBoxes.txt"))
//            {
//                var str = File.ReadAllLines("AutoTrimBoxes.txt");
//
//                foreach (var s in str)
//                {
//                    try
//                    {
//                        var v = s.Split(';');
//                        _autoTrimFormats.Add(new AutoTrimFormat
//                        {
//                            Width = double.Parse(v[0])*Mn,
//                            Height = double.Parse(v[1])*Mn,
//                            Bleed = double.Parse(v[2])*Mn
//                        });
//                    }
//                    catch
//                    {
//                        // ignored
//                    }
//                }
//            }
//        }
//
//        private delegate void ImageConverter(PDFlib p, string file);
//    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDFlib_dotnet;

namespace PDFManipulate.Shema
{
    public class Document
    {
        public string FileName { get; set; }

        List<Page> Pages { get; set; } = new List<Page>();

        public Document(string fileName)
        {
            FileName = fileName;
        }
        public void Add(Page page)
        {
            Pages.Add(page);
        }

        public void Convert()
        {
            if (Pages.Any())
            {
                using (var p = new PDFlib())
                {
                    p.begin_document(FileName, "");

                    foreach (var page in Pages)
                    {
                        var supportedFileFormat =
                            SupportFileFormats.SupportedFileFormats.GetExtension(Path.GetExtension(page.FileName));

                        supportedFileFormat.Convert(p,page.FileName);
                    }

                    p.end_document("");
                }

            }

        }

        public void Convert(double trimW, double trimH)
        {
            if (Pages.Any())
            {
                try
                {
                    using (var p = new PDFlib())
                    {
                        p.begin_document(FileName, "");

                        foreach (var page in Pages)
                        {
                            var supportedFileFormat =
                                SupportFileFormats.SupportedFileFormats.GetExtension(Path.GetExtension(page.FileName));

                            supportedFileFormat.Convert(p, page.FileName,trimW,trimH);
                        }

                        p.end_document("");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error");
                }
                

            }
        }

        public void Convert(double bleed)
        {
            if (Pages.Any())
            {
                try
                {
                    using (var p = new PDFlib())
                    {
                        p.begin_document(FileName, "");

                        foreach (var page in Pages)
                        {
                            var supportedFileFormat =
                                SupportFileFormats.SupportedFileFormats.GetExtension(Path.GetExtension(page.FileName));

                            supportedFileFormat.Convert(p, page.FileName,bleed);
                        }

                        p.end_document("");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error");
                }
                

            }
        }

        public void Convert(double inside, double outside, double top, double bottom)
        {
            if (Pages.Any())
            {
                try
                {
                    using (var p = new PDFlib())
                    {
                        p.begin_document(FileName, "");

                        foreach (var page in Pages)
                        {
                            var supportedFileFormat =
                                SupportFileFormats.SupportedFileFormats.GetExtension(Path.GetExtension(page.FileName));

                            supportedFileFormat.Convert(p, page.FileName,outside:outside,inside:inside,top:top,bottom:bottom);
                        }

                        p.end_document("");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error");
                }
                

            }
        }

        public void Add(string fileName)
        {
            if (CheckExtension(fileName))
            {
                var page = new Page()
                {
                    FileName = fileName
                };

                Pages.Add(page);

            }


        }

        private bool CheckExtension(string fileName)
        {
            var ext = Path.GetExtension(fileName);

            return SupportFileFormats.SupportedFileFormats.GetExtension(ext) != null;


        }

   
    }
}

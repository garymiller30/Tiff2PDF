using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PDFManipulate.Converters
{
    public class MultipleFiles : AbstractConvert
    {
        public override void Start(string[] files)
        {
            if (files.Length == 0) return;

            OnBegin(this, files.Length);

            var fn = files.First() + ".pdf";

            using (var converter = new PdfLibConverter(fn))
            {
                int cnt = 0;

                foreach (var file in files)
                {

                    if (Directory.Exists(file))
                    {
                        // якщо це папка, то все що в папці конвертуємо окремо

                        _recursiveDirectory(Directory.GetFileSystemEntries(file));

                    }
                    else
                    {
                        converter.AddFile(file);
                    }
                    

                    //CreatePdfPage(converter, file);
                    OnProcess(this, ++cnt);
                }

                //converter.SetTrimBox();
                converter.Convert();
            }

            OnFinish(this,null);

        }


        private void _recursiveDirectory(string[] files)
        {
            if (files.Length == 0) return;
            var fn = files.First() + ".pdf";

            using (var converter = new PdfLibConverter(fn))
            {
                int cnt = 0;

                foreach (var file in files)
                {

                    if (Directory.Exists(file))
                    {
                        // якщо це папка, то все що в папці конвертуємо окремо
                        _recursiveDirectory(Directory.GetFileSystemEntries(file));
                    }
                    else
                    {
                        converter.AddFile(file);
                    }
                }
                converter.Convert();
            }
        }

        public override event EventHandler<int> OnBegin = delegate { };
        public override event EventHandler<int> OnProcess = delegate { };
        public override event EventHandler OnFinish = delegate { };
    }
}

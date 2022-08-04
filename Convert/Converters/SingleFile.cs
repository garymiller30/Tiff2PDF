using System;
using System.Collections.Generic;
using System.Linq;

namespace PDFManipulate.Converters
{
    public class SingleFile : AbstractConvert
    {


        public override void Start(string[] files)
        {
            if (files.Length==0) return;

            OnBegin(this, files.Length);

            int cnt = 0;

            foreach (var file in files)
            {
                var fn = file + ".pdf";

                using (var converter = new PdfLibConverter(fn))
                {
                    converter.AddFile(file);
                    converter.Convert();
                    //CreatePdfPage(converter, file);

                    //converter.SetTrimBox();
                }

                OnProcess(this, ++cnt);
            }
            OnFinish(this, null);
        }

        public override event EventHandler<int> OnBegin = delegate { };
        public override event EventHandler<int> OnProcess = delegate { };
        public override event EventHandler OnFinish = delegate { };
    }
}

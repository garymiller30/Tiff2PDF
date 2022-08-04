using System;

namespace PDFManipulate.Converters
{
    public abstract class AbstractConvertComponent : IDisposable
    {
        protected string PdfFileName{get; }



        protected AbstractConvertComponent(string pdfFileName)
        {
            PdfFileName = pdfFileName;
        }

        public abstract void AddFile(string fileName);
        public abstract void Dispose();

        public abstract void SetTrimBox();
    }
}

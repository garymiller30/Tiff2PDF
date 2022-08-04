using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFlib_dotnet;

namespace PDFManipulate.SupportFileFormats
{
    public abstract class AbstractFileFormat
    {
        
        public string Extension { get; set; }

        protected AbstractFileFormat(string extension)
        {
            Extension = extension;
        }

        public abstract void Convert(PDFlib p, string fileName);
        /// <summary>
        /// кожна сторінка виставляється за форматом
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public abstract void Convert(PDFlib p, string fileName, double width, double height);
        /// <summary>
        /// кожна сторінка виставляється по блідам
        /// </summary>
        /// <param name="p"></param>
        /// <param name="fileName"></param>
        /// <param name="bleed"></param>
        public abstract void Convert(PDFlib p, string fileName, double bleed);

        public abstract void Convert(PDFlib p, string fileName, double outside, double inside, double top,
            double bottom);
    }
}

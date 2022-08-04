using System;
using System.Collections.Generic;

namespace PDFManipulate.Converters
{
    public abstract class AbstractConvert
    {

        public abstract void Start(string[] files);

        public abstract event EventHandler<int> OnBegin;
        public abstract event EventHandler<int> OnProcess;
        public abstract event EventHandler OnFinish;
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using PDFManipulate.Boxes;
using PDFManipulate.Converters;
using PDFManipulate.Forms;

namespace PDFManipulate
{
    public class Converter
    {
        const string AutoTrimBoxFile = "AutoTrimBoxes.xml";
        public ConvertModeEnum Mode { set; get; } = ConvertModeEnum.NotAssigned;

        private ConverterSettings _settings;
        public SplitPdfSettings SplitPdfSettings { get; set; }

        public static readonly List<AutoTrimBox> AutoTrimBoxes = new List<AutoTrimBox>();

        public event EventHandler<int> OnBegin = delegate { };
        public event EventHandler<int> OnProcess = delegate { };
        public event EventHandler OnFinish = delegate { };

        internal static AutoTrimBox AddAutoTrimBox()
        {
            var box = new AutoTrimBox{Height = 210,Width = 297,Bleeds = 3};
            AutoTrimBoxes.Add(box);
            return box;
        }

        public Converter()
        {
            LoadAutoTrimBoxes();
        }

        public Converter(ConverterSettings settings)
        {
            _settings = settings;
        }


        private void LoadAutoTrimBoxes()
        {
            AutoTrimBoxes.Clear();
            if (File.Exists(AutoTrimBoxFile))
            {
                var list = FromXml<List<AutoTrimBox>>(AutoTrimBoxFile);
                AutoTrimBoxes.AddRange(list);
            }
        }

        internal static void RemoveAutoTrimBox(object o)
        {
            if (o is AutoTrimBox box)
            {
                AutoTrimBoxes.Remove(box);
            }
            
        }

        public void SaveAutoTrimBoxes()
        {
            ToXml(AutoTrimBoxFile,AutoTrimBoxes);
        }

     

        public void Start(string[] files)
        {
            Start(files,Mode);
        }

        /// <summary>
        /// Start with using CovertSettings
        /// </summary>
        public void Start()
        {
            Start(_settings.Files,_settings.Mode);
        }

        public void Start(string[] files, ConvertModeEnum mode)
        {
            if (mode == ConvertModeEnum.NotAssigned) throw new Exception("Convert mode is NotAssigned");

            var convertor = GetConvertor(mode);

            convertor.OnFinish += OnFinish;
            convertor.OnBegin += OnBegin;
            convertor.OnProcess += OnProcess;
            
            convertor.Start(files);

            convertor.OnFinish -= OnFinish;
            convertor.OnBegin -= OnBegin;
            convertor.OnProcess -= OnProcess;

        }

        private AbstractConvert GetConvertor(ConvertModeEnum mode)
        {
            var name = Enum.GetName(typeof(ConvertModeEnum),mode);

            var inst = Activator.CreateInstance(Assembly.GetExecutingAssembly().GetName().Name,"PDFManipulate.Converters."+ name);
            var conv = (AbstractConvert) inst.Unwrap();
            return conv;
        }

        private static T FromXml<T>(string xml) where T : new()
        {
            var t = new T();
            
            try
            {
                var str = File.ReadAllText(xml);

                using (var stringReader = new StringReader(str))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    t = (T) serializer.Deserialize(stringReader);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
 
            }

            return t;
        }

        private void ToXml<T>(string file,T obj)
        {
            try
            {
                using (var stringWriter = new StringWriter(new StringBuilder()))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(stringWriter, obj);

                    File.WriteAllText(file,stringWriter.ToString());
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ShowEditAutoTrimDialog()
        {
            using (var form = new FormEditAutoTrimDialog())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SaveAutoTrimBoxes();
                }
            }
        }

        public void SplitPdf()
        {
            switch (SplitPdfSettings.Mode)
            {
                case SplitPdfModeEnum.FixedCountPages:

                    foreach (var file in SplitPdfSettings.Files)
                    {
                        PdfLibConverter.SplitPDF(file, SplitPdfSettings.FixedCountPages);
                    }

                    break;
                case SplitPdfModeEnum.CustomCountPages:

                    foreach (var file in SplitPdfSettings.Files)
                    {
                        PdfLibConverter.SplitPDF(file, SplitPdfSettings.CustomCountPages);
                    }


                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ExtractPages()
        {
            switch (SplitPdfSettings.Mode)
            {
                case SplitPdfModeEnum.FixedCountPages:
                    foreach (var file in SplitPdfSettings.Files)
                    {
                        PdfLibConverter.ExtractPages(file,new []{SplitPdfSettings.FixedCountPages});
                    }
                    
                    break;
                case SplitPdfModeEnum.CustomCountPages:
                    foreach (var file in SplitPdfSettings.Files)
                    {
                        PdfLibConverter.ExtractPages(file, SplitPdfSettings.CustomCountPages);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

using PDFManipulate;
using PDFManipulate.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tiff2PDF.Properties;

namespace Tiff2PDF
{
    public partial class Form1 : Form
    {
        private bool _exitAfterConvert;

        private Converter _converter;
        

        public Form1()
        {
            InitializeComponent();
            Text = $"{Application.ProductName} v.{Application.ProductVersion}";
            InitConverter();

            var t = new ToolTip();
            t.SetToolTip(pictureBox_Multiply, @"Перетягни сюди файли, щоб зробити один PDF файл");

            t = new ToolTip();
            t.SetToolTip(pictureBox_Single, @"Перетягни сюди файли для конвертації у окремі PDF файли");

            pictureBox_Multiply.AllowDrop = true;
            pictureBox_Single.AllowDrop = true;

            //var list = new List<string>();
            //list.Add(@"F:\Jobs\USK\2021\#2021-12-21_USK_FOCUS_PERFEKT_SB_01\CVR_PERFEKT_SB_01_UKR.pdf");
            //PDFManipulate.Fasades.Pdf.RotateMirrorFrontAndBack(list);

            //PDFManipulate.Fasades.Pdf.CreateRectangle(new []
            //{
            //    @"F:\Jobs\OLGA_VOVK\2019\#2019-8-12_OLGA_VOVK_NAKLEYKA_ORACAL_(PROZRACHNAYA_100-140)_120_40MM,\Наклейки НРМ  ВнБл ТХ-PDF_25,03_2_PRINT.pdf"
            //});

            //PDFManipulate.Fasades.Pdf.MergeFrontsAndBack(new []
            //{
            //    @"F:\Jobs\USK\2019\#2019-7-17_USK_YAROSLAV_KARTON\1.tif.pdf",
            //    @"F:\Jobs\USK\2019\#2019-7-17_USK_YAROSLAV_KARTON\4.tif.pdf"
            //});

            //AsposeConverter.Run(@"F:\Jobs\USK\2019\#2019-6-26_USK_MAKETS_GIDRANT\Газета.pdf");
            //var pi = PDFManipulate.Fasades.Pdf.GetPagesInfo(@"f:\Dev\work\ActiveWorks\bin\Debug\Utils\PDF Checker\PDF_Checker.pdf");
            //PDFManipulate.Fasades.Pdf.SetTrimBox(@"D:\Users\miller\Nextcloud\work\MAYKO\#2019-3-18_MAYKO_LIVERPUL_EVRO_1\liverpul_evro_1 – копія.tif.pdf",100, 100);
            //if (pi != null)
            //{
            //}
        }

        private bool ProcessCommandLine()
        {
            var args = Environment.GetCommandLineArgs().Skip(1).ToArray();


            string path = string.Empty;
            string mode = string.Empty;

            if (args.Length==3)
            {
                if (args[0] == "-mode")
                {
                    mode = args[1];
                    path = args[2];

                    if (Directory.Exists(path))
                    {
                        if (mode == "single")
                        {
                            DropSingle(Directory.GetFileSystemEntries(path));
                        }
                        else if (mode == "multiply")
                        {
                            DropMultiply(Directory.GetFileSystemEntries(path));
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (File.Exists(path))
                            DropSingle(new[] {path});
                        else
                        {
                            return false;
                        }

                    }

                }
                else
                {
                    return false;
                }
                _exitAfterConvert = true;
                return true;
            }
            return false;
        }

        private void InitConverter()
        {
            _converter = new Converter();
            _converter.OnBegin += ConverterOnOnBegin;
            _converter.OnProcess += _converter_OnProcess;
            _converter.OnFinish += _converter_OnFinish;
        }

        private void _converter_OnFinish(object sender, EventArgs e)
        {
            ResetProgress();
        }

        private void ResetProgress()
        {
            Invoke((MethodInvoker)delegate
           {
               progressBar1.Value = 0;
               Converter_EventFinish();
           });
        }

        private void _converter_OnProcess(object sender, int e)
        {
            ChangeProgress(e);
        }

        private void ChangeProgress(int e)
        {
            Invoke((MethodInvoker)delegate { progressBar1.Value = e; });
        }

        private void ConverterOnOnBegin(object sender, int e)
        {
            InitProgress(e);
        }

        private void InitProgress(int e)
        {
            Invoke((MethodInvoker)delegate
          {
              progressBar1.Maximum = e;
          });
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void Label_Multiply_DragDrop(object sender, DragEventArgs e)
        {
            DropMultiply((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        private async void DropMultiply(string[] files)
        {

            pictureBox_Multiply.Image = Resources.LoadingGIF;

            await Task.Run(() => _converter.Start(files, ConvertModeEnum.MultipleFiles));


        }

        private void Label_Multiply_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Label_Single_DragDrop(object sender, DragEventArgs e)
        {
            DropSingle((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        private async void DropSingle(string[] files)
        {
            // DragEventArgs e;
            pictureBox_Single.Image = Resources.LoadingGIF;

            await Task.Run(() => _converter.Start(files, ConvertModeEnum.SingleFile));


        }

        void Converter_EventFinish()
        {

            Invoke((MethodInvoker)delegate
           {
               pictureBox_Multiply.Enabled = true;
               pictureBox_Single.Enabled = true;

               pictureBox_Multiply.Image = Resources.multiple_100x100;
               pictureBox_Single.Image = Resources.Single_100x100;
               progressBar1.Value = 0;

               if (_exitAfterConvert) Close();
           });


        }

        private void Label_Multiply_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = Color.BurlyWood;
        }

        private void Label_Multiply_MouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackColor = SystemColors.Control;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_exitAfterConvert) // непотрібно зберігати налаштування, коли працює в автоматичному режимі
            {
                Settings.Default.Save();
            }
        }

        private void Button_Settings_Click(object sender, EventArgs e)
        {
            _converter.ShowEditAutoTrimDialog();
        }

        private void Button_List_Click(object sender, EventArgs e)
        {
            Hide();

            using (var f = new PDFManipulate.Forms.FormList())
            {
                f.ShowDialog();

                Show();

                if (f.ConvertFiles != null)
                {
                    DropMultiply(f.ConvertFiles);
                }
            }
        }

        private void PictureBox_Multiply_Click(object sender, EventArgs e)
        {

            var files = SelectFiles();
            if (files != null)
            {
                DropMultiply(files);
            }
        }

        private string[] SelectFiles()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileNames;
            }

            return new string[] { };
        }

        private void PictureBox_Single_Click(object sender, EventArgs e)
        {
            var files = SelectFiles();
            if (files != null)
            {
                DropSingle(files);
            }
        }

        private void  Form1_Shown(object sender, EventArgs e)
        {

            ProcessCommandLine();


        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFManipulate.Forms
{
    public partial class FormSelectCountPages : Form
    {
        public SplitPdfSettings SplitSettings { get; set; } = new SplitPdfSettings();
        //public int CountPages { get; set; } = 1;
        public FormSelectCountPages()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            SplitSettings.Mode = radioButtonFixed.Checked
                ? SplitPdfModeEnum.FixedCountPages
                : SplitPdfModeEnum.CustomCountPages;


            switch (SplitSettings.Mode)
            {
                case SplitPdfModeEnum.FixedCountPages:
                    SplitSettings.FixedCountPages = (int)numericUpDown1.Value;
                    Close();
                    break;
                case SplitPdfModeEnum.CustomCountPages:
                    bool res = ConvertToIntArray(textBoxCustom.Text, out int[] customs);

                    if (res)
                    {
                        SplitSettings.CustomCountPages = customs;
                        Close();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            

            
        }

        private bool ConvertToIntArray(string text, out int[] ints)
        {

            var listInt = new List<int>();

            var splitted = text.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Any())
            {

                bool err = false;

                foreach (var s in splitted)
                {
                    var r = int.TryParse(s, out int res);
                    if (r)
                        listInt.Add(res);
                    else
                    {
                        err = true;
                        break;
                    }
                }

                if (err == false)
                {
                    ints = listInt.ToArray();
                    return true;
                }
            }

            ints = listInt.ToArray();
            return false;
        }

        private void FormSelectCountPages_Load(object sender, EventArgs e)
        {
            Activate();
        }
    }
}

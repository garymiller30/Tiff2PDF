using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDFManipulate.Converters;

namespace PDFManipulate.Forms
{
    public partial class FormSelectMode : Form
    {

        public ConvertModeEnum ConvertMode { get; set; } = ConvertModeEnum.SingleFile;
        public FormSelectMode()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void ButtonAllInOne_Click(object sender, EventArgs e)
        {
            ConvertMode = ConvertModeEnum.MultipleFiles;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonEachSeparatelly_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            
            Close();
        }

        private void FormSelectMode_Load(object sender, EventArgs e)
        {
            Activate();
        }
    }
}

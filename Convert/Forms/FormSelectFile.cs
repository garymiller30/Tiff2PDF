using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Path = System.IO.Path;

namespace PDFManipulate.Forms
{
    public partial class FormSelectFile : Form
    {
        public string Back { get; set; }
        public FormSelectFile(IEnumerable<string> files)
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;

            var displayFiles = files.Select(x => new DisplayFile(x));

            comboBox1.Items.AddRange(displayFiles.ToArray());
            comboBox1.SelectedIndex = 0;

        }


        class DisplayFile
        {
            public string Name { get; set; }
            public string FullName { get; set; }

            public DisplayFile(string file)
            {
                FullName = file;
                Name = Path.GetFileName(file);
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Back = ((DisplayFile) comboBox1.SelectedItem).FullName;
        }

        private void FormSelectFile_Load(object sender, EventArgs e)
        {
            Activate();
        }
    }
}

using System;
using System.Windows.Forms;

namespace PDFManipulate.Forms
{
    public partial class FormEditAutoTrimDialog : Form
    {
        public FormEditAutoTrimDialog()
        {
            InitializeComponent();

            objectListView1.AddObjects(Converter.AutoTrimBoxes);
            DialogResult = DialogResult.Cancel;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objectListView1.AddObject(Converter.AddAutoTrimBox());
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objectListView1.SelectedObjects != null )
            {
                foreach (var o in objectListView1.SelectedObjects)
                {
                    Converter.RemoveAutoTrimBox(o);
                }
                objectListView1.RemoveObjects(objectListView1.SelectedObjects);

            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

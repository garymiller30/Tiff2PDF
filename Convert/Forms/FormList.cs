using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace PDFManipulate.Forms
{
    public partial class FormList : Form
    {
        public string[] ConvertFiles;

        public FormList()
        {
            InitializeComponent();

            objectListView1.DragSource = new SimpleDragSource();
            objectListView1.DropSink = new RearrangingDropSink(false);

            DialogResult = DialogResult.Cancel;

        }

        public FormList(string[] files): this()
        {
            AddFiles(files);
        }

        private void AddFiles(string[] files)
        {
            var f = files.Select(x => new { Name = Path.GetFileName(x), FullPath = x });
            objectListView1.AddObjects(f.ToArray());
        }

        private void objectListView1_CanDrop(object sender, OlvDropEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void objectListView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;

          
        }

        private void toolStripButton_Play_Click(object sender, EventArgs e)
        {
            if (objectListView1.Objects != null)
            {
                ConvertFiles = (from object o in objectListView1.Objects 
                    select (string) o.GetType().GetProperty("FullPath")?.GetValue(o)).ToArray();
                //ConvertFiles = objectListView1.Objects.Cast<string>().ToArray();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void objectListView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                var files = ((string[]) e.Data.GetData(DataFormats.FileDrop)).Select(x=>new {Name = Path.GetFileName(x),FullPath = x });

                objectListView1.AddObjects(files.ToArray());
            }
        }

        private void toolStripButton_AddFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                objectListView1.AddObjects(openFileDialog1.FileNames);
            }
        }

        private void видалитиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteFiles();
        }
        /// <summary>
        /// видалити файли зі списку
        /// </summary>
        private void DeleteFiles()
        {
            if (objectListView1.SelectedObjects != null)
                objectListView1.RemoveObjects(objectListView1.SelectedObjects);
        }

        private void toolStripButton_Del_Click(object sender, EventArgs e)
        {
            DeleteFiles();
        }
    }
}

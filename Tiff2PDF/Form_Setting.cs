using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Tiff2PDF
{
    public partial class Form_Setting : Form
    {
        readonly List<AutoTrimFormat> _autoTrimFormats = new List<AutoTrimFormat>();

        public Form_Setting()
        {
            InitializeComponent();

            LoadTrimBoxes();


            dataGridView1.Columns.Add("Width", "Width");
            dataGridView1.Columns.Add("Height", "Height");
            dataGridView1.Columns.Add("Bleed", "Bleed");

            foreach (var atf in _autoTrimFormats)
            {
                dataGridView1.Rows.Add(atf.Width,atf.Height,atf.Bleed);
            }

            // dataGridView1.DataSource = _autoTrimFormats;
        }

        private void LoadTrimBoxes()
        {
            _autoTrimFormats.Clear();

            if (File.Exists("AutoTrimBoxes.txt"))
            {
                var str = File.ReadAllLines("AutoTrimBoxes.txt");

                foreach (var s in str)
                {
                    try
                    {
                        var v = s.Split(';');

                        var atf = new AutoTrimFormat
                        {
                            Width = double.Parse(v[0]),
                            Height = double.Parse(v[1]),
                            Bleed = double.Parse(v[2])
                        };
                        _autoTrimFormats.Add(atf);

                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        private void Form_Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveAutoTrims();
        }

        private void SaveAutoTrims()
        {
            List<String> formats = new List<string>();


            for (int index = 0; index < dataGridView1.Rows.Count-1; index++)
            {
                DataGridViewRow row = dataGridView1.Rows[index];
                formats.Add(String.Format("{0};{1};{2}", row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value));
            }

            File.WriteAllLines("AutoTrimBoxes.txt", formats);

        }
    }
}

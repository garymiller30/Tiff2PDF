namespace Tiff2PDF
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox_Multiply = new System.Windows.Forms.PictureBox();
            this.pictureBox_Single = new System.Windows.Forms.PictureBox();
            this.button_Settings = new System.Windows.Forms.Button();
            this.button_List = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Multiply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Single)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.progressBar1, 3);
            this.progressBar1.Location = new System.Drawing.Point(3, 161);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(215, 11);
            this.progressBar1.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_Settings, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.button_List, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(221, 175);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 3);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_Multiply);
            this.flowLayoutPanel1.Controls.Add(this.pictureBox_Single);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(215, 123);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // pictureBox_Multiply
            // 
            this.pictureBox_Multiply.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Multiply.Image = global::Tiff2PDF.Properties.Resources.multiple_100x100;
            this.pictureBox_Multiply.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_Multiply.Name = "pictureBox_Multiply";
            this.pictureBox_Multiply.Size = new System.Drawing.Size(100, 100);
            this.pictureBox_Multiply.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_Multiply.TabIndex = 4;
            this.pictureBox_Multiply.TabStop = false;
            this.pictureBox_Multiply.Click += new System.EventHandler(this.PictureBox_Multiply_Click);
            this.pictureBox_Multiply.DragDrop += new System.Windows.Forms.DragEventHandler(this.Label_Multiply_DragDrop);
            this.pictureBox_Multiply.DragEnter += new System.Windows.Forms.DragEventHandler(this.Label_Multiply_DragEnter);
            this.pictureBox_Multiply.MouseEnter += new System.EventHandler(this.Label_Multiply_MouseEnter);
            this.pictureBox_Multiply.MouseLeave += new System.EventHandler(this.Label_Multiply_MouseLeave);
            // 
            // pictureBox_Single
            // 
            this.pictureBox_Single.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Single.Image = global::Tiff2PDF.Properties.Resources.Single_100x100;
            this.pictureBox_Single.Location = new System.Drawing.Point(109, 3);
            this.pictureBox_Single.Name = "pictureBox_Single";
            this.pictureBox_Single.Size = new System.Drawing.Size(100, 100);
            this.pictureBox_Single.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_Single.TabIndex = 5;
            this.pictureBox_Single.TabStop = false;
            this.pictureBox_Single.Click += new System.EventHandler(this.PictureBox_Single_Click);
            this.pictureBox_Single.DragDrop += new System.Windows.Forms.DragEventHandler(this.Label_Single_DragDrop);
            this.pictureBox_Single.DragEnter += new System.Windows.Forms.DragEventHandler(this.Label_Multiply_DragEnter);
            this.pictureBox_Single.MouseEnter += new System.EventHandler(this.Label_Multiply_MouseEnter);
            this.pictureBox_Single.MouseLeave += new System.EventHandler(this.Label_Multiply_MouseLeave);
            // 
            // button_Settings
            // 
            this.button_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Settings.FlatAppearance.BorderSize = 0;
            this.button_Settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Settings.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Settings.Image = global::Tiff2PDF.Properties.Resources._1348902242_Settings;
            this.button_Settings.Location = new System.Drawing.Point(157, 132);
            this.button_Settings.Name = "button_Settings";
            this.button_Settings.Size = new System.Drawing.Size(27, 23);
            this.button_Settings.TabIndex = 7;
            this.button_Settings.UseVisualStyleBackColor = true;
            this.button_Settings.Click += new System.EventHandler(this.Button_Settings_Click);
            // 
            // button_List
            // 
            this.button_List.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_List.FlatAppearance.BorderSize = 0;
            this.button_List.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_List.Image = global::Tiff2PDF.Properties.Resources.application_view_list;
            this.button_List.Location = new System.Drawing.Point(190, 132);
            this.button_List.Name = "button_List";
            this.button_List.Size = new System.Drawing.Size(28, 23);
            this.button_List.TabIndex = 8;
            this.button_List.UseVisualStyleBackColor = true;
            this.button_List.Click += new System.EventHandler(this.Button_List_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.ShowReadOnly = true;
            this.openFileDialog1.SupportMultiDottedExtensions = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 175);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(247, 310);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(132, 214);
            this.Name = "Form1";
            this.Text = "TIFF2PDF";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Multiply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Single)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox_Multiply;
        private System.Windows.Forms.PictureBox pictureBox_Single;
        private System.Windows.Forms.Button button_Settings;
        private System.Windows.Forms.Button button_List;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}


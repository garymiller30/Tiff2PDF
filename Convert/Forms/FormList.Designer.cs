namespace PDFManipulate.Forms
{
    partial class FormList
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
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Play = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_AddFiles = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Del = new System.Windows.Forms.ToolStripButton();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnFileName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFolder = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.видалитиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Play,
            this.toolStripSeparator1,
            this.toolStripButton_AddFiles,
            this.toolStripButton_Del});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(505, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_Play
            // 
            this.toolStripButton_Play.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Play.Image = Properties.Resources.control_play_blue;
            this.toolStripButton_Play.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Play.Name = "toolStripButton_Play";
            this.toolStripButton_Play.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Play.Text = "Почати конвертацію";
            this.toolStripButton_Play.Click += new System.EventHandler(this.toolStripButton_Play_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_AddFiles
            // 
            this.toolStripButton_AddFiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_AddFiles.Image = Properties.Resources.folder_add;
            this.toolStripButton_AddFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_AddFiles.Name = "toolStripButton_AddFiles";
            this.toolStripButton_AddFiles.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_AddFiles.Text = "додати файли";
            this.toolStripButton_AddFiles.Click += new System.EventHandler(this.toolStripButton_AddFiles_Click);
            // 
            // toolStripButton_Del
            // 
            this.toolStripButton_Del.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Del.Image = Properties.Resources.Delete;
            this.toolStripButton_Del.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Del.Name = "toolStripButton_Del";
            this.toolStripButton_Del.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Del.Text = "видалити файли";
            this.toolStripButton_Del.ToolTipText = "видалити файли зі списку";
            this.toolStripButton_Del.Click += new System.EventHandler(this.toolStripButton_Del_Click);
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.olvColumnFileName);
            this.objectListView1.AllColumns.Add(this.olvColumnFolder);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnFileName,
            this.olvColumnFolder});
            this.objectListView1.ContextMenuStrip = this.contextMenuStrip1;
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListView1.IsSimpleDragSource = true;
            this.objectListView1.IsSimpleDropSink = true;
            this.objectListView1.Location = new System.Drawing.Point(0, 25);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.ShowGroups = false;
            this.objectListView1.Size = new System.Drawing.Size(505, 291);
            this.objectListView1.TabIndex = 1;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.CanDrop += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.objectListView1_CanDrop);
            this.objectListView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.objectListView1_DragDrop);
            this.objectListView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.objectListView1_DragEnter);
            // 
            // olvColumnFileName
            // 
            this.olvColumnFileName.AspectName = "Name";
            this.olvColumnFileName.Text = "Файл";
            this.olvColumnFileName.Width = 300;
            // 
            // olvColumnFolder
            // 
            this.olvColumnFolder.AspectName = "FullPath";
            this.olvColumnFolder.Text = "Повний шлях";
            this.olvColumnFolder.Width = 80;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.видалитиToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(127, 26);
            // 
            // видалитиToolStripMenuItem
            // 
            this.видалитиToolStripMenuItem.Image = Properties.Resources.Delete;
            this.видалитиToolStripMenuItem.Name = "видалитиToolStripMenuItem";
            this.видалитиToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.видалитиToolStripMenuItem.Text = "Видалити";
            this.видалитиToolStripMenuItem.Click += new System.EventHandler(this.видалитиToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.ShowReadOnly = true;
            this.openFileDialog1.SupportMultiDottedExtensions = true;
            // 
            // FormList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 316);
            this.Controls.Add(this.objectListView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "файли для конвертації";
            this.TopMost = true;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Play;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn olvColumnFileName;
        private System.Windows.Forms.ToolStripButton toolStripButton_AddFiles;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem видалитиToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Del;
        private BrightIdeasSoftware.OLVColumn olvColumnFolder;
    }
}
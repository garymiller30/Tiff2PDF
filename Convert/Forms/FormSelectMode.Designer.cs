namespace PDFManipulate.Forms
{
    partial class FormSelectMode
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
            this.buttonAllInOne = new System.Windows.Forms.Button();
            this.buttonEachSeparatelly = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAllInOne
            // 
            this.buttonAllInOne.Location = new System.Drawing.Point(27, 12);
            this.buttonAllInOne.Name = "buttonAllInOne";
            this.buttonAllInOne.Size = new System.Drawing.Size(151, 37);
            this.buttonAllInOne.TabIndex = 0;
            this.buttonAllInOne.Text = "все у один файл";
            this.buttonAllInOne.UseVisualStyleBackColor = true;
            this.buttonAllInOne.Click += new System.EventHandler(this.ButtonAllInOne_Click);
            // 
            // buttonEachSeparatelly
            // 
            this.buttonEachSeparatelly.Location = new System.Drawing.Point(27, 55);
            this.buttonEachSeparatelly.Name = "buttonEachSeparatelly";
            this.buttonEachSeparatelly.Size = new System.Drawing.Size(151, 37);
            this.buttonEachSeparatelly.TabIndex = 1;
            this.buttonEachSeparatelly.Text = "кожний файл окремо";
            this.buttonEachSeparatelly.UseVisualStyleBackColor = true;
            this.buttonEachSeparatelly.Click += new System.EventHandler(this.ButtonEachSeparatelly_Click);
            // 
            // FormSelectMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 102);
            this.Controls.Add(this.buttonEachSeparatelly);
            this.Controls.Add(this.buttonAllInOne);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectMode";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "як конвертувати";
            this.Load += new System.EventHandler(this.FormSelectMode_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAllInOne;
        private System.Windows.Forms.Button buttonEachSeparatelly;
    }
}
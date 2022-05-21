namespace CSX.Skia.OpenGLTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.skglControl = new SkiaSharp.Views.Desktop.SKGLControl();
            this.SuspendLayout();
            // 
            // skglControl
            // 
            this.skglControl.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.skglControl.BackColor = System.Drawing.Color.Black;
            this.skglControl.Location = new System.Drawing.Point(-2, -1);
            this.skglControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.skglControl.Name = "skglControl";
            this.skglControl.Size = new System.Drawing.Size(802, 451);
            this.skglControl.TabIndex = 0;
            this.skglControl.VSync = false;
            this.skglControl.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs>(this.skglControl1_PaintSurface);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.skglControl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private SkiaSharp.Views.Desktop.SKGLControl skglControl;
    }
}
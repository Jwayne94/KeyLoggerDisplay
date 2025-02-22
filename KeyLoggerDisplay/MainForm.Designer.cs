namespace KeyLoggerDisplay
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.keyLogListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // keyLogListBox
            // 
            this.keyLogListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.keyLogListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyLogListBox.FormattingEnabled = true;
            this.keyLogListBox.Location = new System.Drawing.Point(0, 0);
            this.keyLogListBox.Name = "keyLogListBox";
            this.keyLogListBox.Size = new System.Drawing.Size(300, 150);
            this.keyLogListBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.keyLogListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MainForm";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox keyLogListBox;
    }
}


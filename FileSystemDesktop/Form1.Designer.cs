namespace FileSystemDesktop
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
            this.tbDirectory = new System.Windows.Forms.TextBox();
            this.lblEnterDir = new System.Windows.Forms.Label();
            this.analyzeButton = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbDirectory
            // 
            this.tbDirectory.Location = new System.Drawing.Point(15, 36);
            this.tbDirectory.Name = "tbDirectory";
            this.tbDirectory.Size = new System.Drawing.Size(265, 20);
            this.tbDirectory.TabIndex = 0;
            // 
            // lblEnterDir
            // 
            this.lblEnterDir.AutoSize = true;
            this.lblEnterDir.Location = new System.Drawing.Point(12, 20);
            this.lblEnterDir.Name = "lblEnterDir";
            this.lblEnterDir.Size = new System.Drawing.Size(162, 13);
            this.lblEnterDir.TabIndex = 1;
            this.lblEnterDir.Text = "Enter a directory path to analyze:";
            // 
            // analyzeButton
            // 
            this.analyzeButton.Location = new System.Drawing.Point(298, 32);
            this.analyzeButton.Name = "analyzeButton";
            this.analyzeButton.Size = new System.Drawing.Size(75, 23);
            this.analyzeButton.TabIndex = 2;
            this.analyzeButton.Text = "Analyze";
            this.analyzeButton.UseVisualStyleBackColor = true;
            this.analyzeButton.Click += new System.EventHandler(this.analyzeButton_Click);
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(13, 87);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(267, 292);
            this.tbResult.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 411);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.analyzeButton);
            this.Controls.Add(this.lblEnterDir);
            this.Controls.Add(this.tbDirectory);
            this.Name = "Form1";
            this.Text = "File System Analyzer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDirectory;
        private System.Windows.Forms.Label lblEnterDir;
        private System.Windows.Forms.Button analyzeButton;
        private System.Windows.Forms.TextBox tbResult;
    }
}


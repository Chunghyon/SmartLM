namespace SBXPCDLLSampleCSharp
{
    partial class MainForm
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
			this.btnStartMulti = new System.Windows.Forms.Button();
			this.rtbLog = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// btnStartMulti
			// 
			this.btnStartMulti.Location = new System.Drawing.Point(418, 12);
			this.btnStartMulti.Name = "btnStartMulti";
			this.btnStartMulti.Size = new System.Drawing.Size(125, 60);
			this.btnStartMulti.TabIndex = 0;
			this.btnStartMulti.Text = "Start Muti Thread";
			this.btnStartMulti.UseVisualStyleBackColor = true;
			this.btnStartMulti.Click += new System.EventHandler(this.btnStartMulti_Click);
			// 
			// rtbLog
			// 
			this.rtbLog.Location = new System.Drawing.Point(12, 78);
			this.rtbLog.Name = "rtbLog";
			this.rtbLog.Size = new System.Drawing.Size(530, 277);
			this.rtbLog.TabIndex = 1;
			this.rtbLog.Text = "";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 367);
			this.Controls.Add(this.rtbLog);
			this.Controls.Add(this.btnStartMulti);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartMulti;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}


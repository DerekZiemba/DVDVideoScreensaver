namespace DVDScreenSaver {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.LogoBox = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.LogoBox)).BeginInit();
			this.SuspendLayout();
			// 
			// LogoBox
			// 
			this.LogoBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LogoBox.Location = new System.Drawing.Point(0, 0);
			this.LogoBox.MaximumSize = new System.Drawing.Size(640, 396);
			this.LogoBox.MinimumSize = new System.Drawing.Size(80, 49);
			this.LogoBox.Name = "LogoBox";
			this.LogoBox.Size = new System.Drawing.Size(150, 65);
			this.LogoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.LogoBox.TabIndex = 0;
			this.LogoBox.TabStop = false;
			// 
			// timer1
			// 
			this.timer1.Interval = 16;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.MenuText;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.LogoBox);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.LogoBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox LogoBox;
		private System.Windows.Forms.Timer timer1;
	}
}


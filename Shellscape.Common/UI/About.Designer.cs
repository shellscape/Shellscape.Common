namespace Shellscape.UI {
	partial class About {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
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
			this._Button = new System.Windows.Forms.Button();
			this._TextVersion = new System.Windows.Forms.TextBox();
			this._ButtonDonate = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _Button
			// 
			this._Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._Button.AutoSize = true;
			this._Button.Location = new System.Drawing.Point(515, 266);
			this._Button.Name = "_Button";
			this._Button.Padding = new System.Windows.Forms.Padding(2);
			this._Button.Size = new System.Drawing.Size(87, 40);
			this._Button.TabIndex = 0;
			this._Button.UseVisualStyleBackColor = true;
			// 
			// _TextVersion
			// 
			this._TextVersion.BackColor = System.Drawing.SystemColors.Window;
			this._TextVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._TextVersion.Location = new System.Drawing.Point(14, 83);
			this._TextVersion.Name = "_TextVersion";
			this._TextVersion.ReadOnly = true;
			this._TextVersion.Size = new System.Drawing.Size(257, 16);
			this._TextVersion.TabIndex = 1;
			this._TextVersion.Text = "Version";
			// 
			// _ButtonDonate
			// 
			this._ButtonDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._ButtonDonate.AutoSize = true;
			this._ButtonDonate.Image = global::Shellscape.Resources.Images.DonatePaypal;
			this._ButtonDonate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._ButtonDonate.Location = new System.Drawing.Point(12, 266);
			this._ButtonDonate.Name = "_ButtonDonate";
			this._ButtonDonate.Padding = new System.Windows.Forms.Padding(2, 2, 6, 2);
			this._ButtonDonate.Size = new System.Drawing.Size(182, 40);
			this._ButtonDonate.TabIndex = 2;
			this._ButtonDonate.Text = "Please consider donating";
			this._ButtonDonate.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this._ButtonDonate.UseVisualStyleBackColor = true;
			// 
			// About
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(614, 318);
			this.Controls.Add(this._ButtonDonate);
			this.Controls.Add(this._TextVersion);
			this.Controls.Add(this._Button);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.Text = "Shellscape.UI.About";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.Button _Button;
		private System.Windows.Forms.TextBox _TextVersion;
		protected System.Windows.Forms.Button _ButtonDonate;

	}
}
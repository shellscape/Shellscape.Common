namespace Shellscape.UI.Controls.Preferences {
	partial class PreferencesForm {
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
			this._PanelParent = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this._PanelGeneral = new Shellscape.UI.Controls.Preferences.PreferencesPanel();
			this._ButtonGroup = new Shellscape.UI.Controls.Preferences.PreferencesButtonGroup();
			this._ButtonGeneral = new Shellscape.UI.Controls.Preferences.PreferencesButton();
			this._PanelParent.SuspendLayout();
			this._ButtonGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// _PanelParent
			// 
			this._PanelParent.Controls.Add(this._PanelGeneral);
			this._PanelParent.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelParent.Location = new System.Drawing.Point(200, 0);
			this._PanelParent.Name = "_PanelParent";
			this._PanelParent.Padding = new System.Windows.Forms.Padding(12, 12, 12, 6);
			this._PanelParent.Size = new System.Drawing.Size(524, 412);
			this._PanelParent.TabIndex = 9;
			// 
			// _PanelGeneral
			// 
			this._PanelGeneral.BackColor = System.Drawing.Color.Transparent;
			this._PanelGeneral.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._PanelGeneral.ControlBackColor = System.Drawing.Color.White;
			this._PanelGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
			this._PanelGeneral.DrawHeader = true;
			this._PanelGeneral.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._PanelGeneral.HeaderColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._PanelGeneral.HeaderColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._PanelGeneral.HeaderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._PanelGeneral.HeaderHeight = 39;
			this._PanelGeneral.HeaderImage = null;
			this._PanelGeneral.HeaderPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._PanelGeneral.HeaderText = "General Preferences";
			this._PanelGeneral.Location = new System.Drawing.Point(12, 12);
			this._PanelGeneral.Margin = new System.Windows.Forms.Padding(6);
			this._PanelGeneral.Name = "_PanelGeneral";
			this._PanelGeneral.Padding = new System.Windows.Forms.Padding(10, 45, 10, 10);
			this._PanelGeneral.SeperatorColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._PanelGeneral.Size = new System.Drawing.Size(500, 394);
			this._PanelGeneral.TabIndex = 1;
			// 
			// _ButtonGroup
			// 
			this._ButtonGroup.BackColor = System.Drawing.Color.Transparent;
			this._ButtonGroup.Controls.Add(this._ButtonGeneral);
			this._ButtonGroup.Dock = System.Windows.Forms.DockStyle.Left;
			this._ButtonGroup.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._ButtonGroup.Location = new System.Drawing.Point(0, 0);
			this._ButtonGroup.Name = "_ButtonGroup";
			this._ButtonGroup.Padding = new System.Windows.Forms.Padding(6, 10, 6, 10);
			this._ButtonGroup.Size = new System.Drawing.Size(200, 412);
			this._ButtonGroup.TabIndex = 8;
			// 
			// _ButtonGeneral
			// 
			this._ButtonGeneral.AssociatedPanel = this._PanelGeneral;
			this._ButtonGeneral.BackColor = System.Drawing.Color.Transparent;
			this._ButtonGeneral.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(119)))));
			this._ButtonGeneral.ButtonBackColor = System.Drawing.Color.Transparent;
			this._ButtonGeneral.ButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
			this._ButtonGeneral.ButtonHeight = 40;
			this._ButtonGeneral.ButtonImage = null;
			this._ButtonGeneral.ButtonPadding = new System.Windows.Forms.Padding(10, 5, 10, 5);
			this._ButtonGeneral.ButtonText = "General";
			this._ButtonGeneral.DownColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this._ButtonGeneral.DownColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this._ButtonGeneral.Expandable = true;
			this._ButtonGeneral.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._ButtonGeneral.HoverColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._ButtonGeneral.HoverColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(251)))));
			this._ButtonGeneral.Location = new System.Drawing.Point(6, 10);
			this._ButtonGeneral.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
			this._ButtonGeneral.Name = "_ButtonGeneral";
			this._ButtonGeneral.NormalColorFrom = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this._ButtonGeneral.NormalColorTo = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
			this._ButtonGeneral.ShadowColor = System.Drawing.SystemColors.ActiveBorder;
			this._ButtonGeneral.Size = new System.Drawing.Size(188, 40);
			this._ButtonGeneral.TabIndex = 2;
			// 
			// PreferencesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(724, 412);
			this.Controls.Add(this._PanelParent);
			this.Controls.Add(this._ButtonGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PreferencesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Preferences";
			this._PanelParent.ResumeLayout(false);
			this._ButtonGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		public PreferencesPanel _PanelGeneral;
		public DoubleBufferedPanel _PanelParent;
		public PreferencesButton _ButtonGeneral;
		public PreferencesButtonGroup _ButtonGroup;

	}
}
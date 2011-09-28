namespace Shellscape.UI.ControlPanel {
	partial class ControlPanelForm {
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
			this._Tasks = new Shellscape.UI.ControlPanel.ControlPanelNavigation();
			this._Panels = new Shellscape.UI.Controls.DoubleBufferedPanel();
			this.SuspendLayout();
			// 
			// _Tasks
			// 
			this._Tasks.BackColor = System.Drawing.Color.Transparent;
			this._Tasks.Dock = System.Windows.Forms.DockStyle.Left;
			this._Tasks.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._Tasks.Location = new System.Drawing.Point(0, 0);
			this._Tasks.Name = "_Tasks";
			this._Tasks.OtherTasksText = "Other Tasks";
			this._Tasks.Padding = new System.Windows.Forms.Padding(26, 12, 14, 16);
			this._Tasks.Size = new System.Drawing.Size(200, 475);
			this._Tasks.TabIndex = 0;
			// 
			// _Panels
			// 
			this._Panels.Dock = System.Windows.Forms.DockStyle.Fill;
			this._Panels.Location = new System.Drawing.Point(200, 0);
			this._Panels.Name = "_Panels";
			this._Panels.Padding = new System.Windows.Forms.Padding(6, 24, 6, 24);
			this._Panels.Size = new System.Drawing.Size(645, 475);
			this._Panels.TabIndex = 1;
			// 
			// ControlPanelForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(845, 475);
			this.Controls.Add(this._Panels);
			this.Controls.Add(this._Tasks);
			this.Name = "ControlPanelForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Control Panel";
			this.ResumeLayout(false);

		}

		#endregion

		private ControlPanelNavigation _Tasks;
		protected Controls.DoubleBufferedPanel _Panels;



	}
}
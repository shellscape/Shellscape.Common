using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Controls.Preferences {
	public partial class PreferencesForm : Form {

		protected PreferencesButton _activeButton = null;
		protected List<PreferencesPanel> _panels = new List<PreferencesPanel>();
		protected Boolean _activateFirstItem = true;

		protected EventHandler PanelShown { get; set; }		

		public PreferencesForm() {
			SetStyle(ControlStyles.ContainerControl, false);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			UpdateStyles();

			Padding = new System.Windows.Forms.Padding(0);

			InitializeComponent();
		}

		protected virtual void OnPanelShown(object sender, EventArgs e) {
			if (PanelShown != null) {
				PanelShown(sender, e);
			}
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			HidePanels();

			if (_ButtonGroup != null && _ButtonGroup.Controls.Count > 0) {
				foreach (PreferencesButton button in _ButtonGroup.Controls) {
					button.Activated += button_Activated;
					button.Click += button_Click;
					button.ItemActivated += button_ItemActivated;
				}

				(_ButtonGroup.Controls[0] as PreferencesButton).Activate();
			}
		}

		private void _PanelParent_ControlAdded(object sender, ControlEventArgs e) {

			if (e.Control is PreferencesPanel) {
				_panels.Add(e.Control as PreferencesPanel);
			}
		}

		protected void button_Activated(object sender, EventArgs e) {

			if (_activeButton == sender) {
				return;
			}

			if (_activeButton != null) {
				_activeButton.TriggerExpandCollapse();
			}

			PreferencesButton button = sender as PreferencesButton;

			HidePanels();

			button.TriggerExpandCollapse();

			_activeButton = button;

			if (button.AssociatedPanel != null) {
				button.AssociatedPanel.Show();
				OnPanelShown(button.AssociatedPanel, EventArgs.Empty);
			}
			else if (button.ButtonItems.Count > 0 && _activateFirstItem) {
				button.ButtonItems[0].Activate();
			}
		}

		protected void button_Click(object sender, EventArgs e) {
			PreferencesButton button = sender as PreferencesButton;

			if (!button.AssociatedPanel.Visible) {
				HidePanels();

				foreach (PreferencesButtonItem item in button.ButtonItems) {
					item.ActiveButton = false;
				}

				button.AssociatedPanel.BringToFront();
				button.AssociatedPanel.Show();
				OnPanelShown(button.AssociatedPanel, EventArgs.Empty);
			}
		}

		protected void button_ItemActivated(object sender, PreferencesButtonItemEventArgs e) {

			HidePanels();

			if (e.Item.AssociatedPanel != null) {
				e.Item.AssociatedPanel.BringToFront();
				e.Item.AssociatedPanel.Show();

				_activeButton = e.Item.Parent as PreferencesButton;
			}

		}

		protected void HidePanels() {
			foreach (var panel in _panels) {
				panel.Hide();
			}
		}

		/// <summary>
		/// If true, a the first child of a button's items will be activated when the button is activated.
		/// </summary>
		protected Boolean ActivateFirstItem {
			get { return _activateFirstItem; }
			set { _activateFirstItem = value; }
		}
	}
}

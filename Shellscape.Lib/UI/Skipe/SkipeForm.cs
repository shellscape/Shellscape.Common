using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Skipe {
	public class SkipeForm : Form {

		protected SkipeButton _activeButton = null;
		protected List<SkipePanel> _panels;
		protected Boolean _activateFirstItem = true;

		protected virtual SkipeButtonGroup ButtonGroup { get { return null;  } }

		protected EventHandler PanelShown { get; set; }

		public SkipeForm() {
			SetStyle(ControlStyles.ContainerControl, false);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			UpdateStyles();

			Padding = new System.Windows.Forms.Padding(8);
		}

		protected virtual void OnPanelShown(object sender, EventArgs e) {
			if (PanelShown != null) {
				PanelShown(sender, e);
			}
		}

		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			if (_panels == null) {
				_panels = new List<SkipePanel>();
			}

			HidePanels();

			if (ButtonGroup != null && ButtonGroup.Controls.Count > 0) {
				foreach (SkipeButton button in ButtonGroup.Controls) {
					button.Activated += button_Activated;
					button.Click += button_Click;
					button.ItemActivated += button_ItemActivated;
				}

				(ButtonGroup.Controls[0] as SkipeButton).Activate();
			}
		}

		protected override void OnControlAdded(ControlEventArgs e) {
			base.OnControlAdded(e);

			if (_panels == null) {
				_panels = new List<SkipePanel>();
			}

			if (e.Control is SkipePanel) {
				_panels.Add(e.Control as SkipePanel);
			}
		}

		protected void button_Activated(object sender, EventArgs e) {

			if (_activeButton == sender) {
				return;
			}

			if (_activeButton != null) {
				_activeButton.TriggerExpandCollapse();
			}

			SkipeButton button = sender as SkipeButton;

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
			SkipeButton button = sender as SkipeButton;

			if (!button.AssociatedPanel.Visible) {
				HidePanels();

				foreach (SkipeButtonItem item in button.ButtonItems) {
					item.ActiveButton = false;
				}
				
				button.AssociatedPanel.Show();
				OnPanelShown(button.AssociatedPanel, EventArgs.Empty);
			}
		}

		protected void button_ItemActivated(object sender, SkipeButtonItemEventArgs e) {

			HidePanels();

			if (e.Item.AssociatedPanel != null) {
				e.Item.AssociatedPanel.Show();

				_activeButton = e.Item.Parent as SkipeButton;
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

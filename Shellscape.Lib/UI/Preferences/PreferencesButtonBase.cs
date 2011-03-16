using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Controls.Preferences {

	public class PreferencesButtonBase : PreferencesBaseControl {

		protected PreferencesPanel _associatedPanel;
		protected Image _buttonImage;
		protected string _buttonText;
		protected Padding _buttonPadding = new Padding(10, 5, 10, 5);

		[Category("Appearance")]
		public Image ButtonImage {
			get { return _buttonImage; }
			set { _buttonImage = value; Invalidate(); }
		}

		[Category("Appearance")]
		public string ButtonText {
			get { return _buttonText; }
			set {
				_buttonText = value;

				if (_associatedPanel != null) {
					_associatedPanel.HeaderTextPrefix = value;
					_associatedPanel.Invalidate();
				}

				Invalidate();
			}
		}

		[Category("Layout")]
		public Padding ButtonPadding {
			get { return _buttonPadding; }
			set { _buttonPadding = value; Invalidate(); }
		}

		public PreferencesPanel AssociatedPanel {
			get { return _associatedPanel; }
			set {
				_associatedPanel = value;

				if (_associatedPanel != null) {
					_associatedPanel.HeaderTextPrefix = this.ButtonText;
					_associatedPanel.Invalidate();
				}
			}
		}

	}
}

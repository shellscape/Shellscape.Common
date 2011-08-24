using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape.UI.Controls.Preferences {

	public class PreferencesButtonItemCollection : CollectionBase {

		public event EventHandler<PreferencesButtonItemEventArgs> ItemAdded;
		public event EventHandler ItemRemoved;
		public event EventHandler<PreferencesButtonItemEventArgs> BeforeItemRemoved;
		public event EventHandler<PreferencesButtonItemEventArgs> ItemActivated;

		public IEnumerable<PreferencesButtonItem> All() {
			foreach (PreferencesButtonItem control in this.List) {
				yield return control;
			}
		}

		private void OnItemAdded(PreferencesButtonItem obj) {
			if (ItemAdded != null) {
				ItemAdded(this, new PreferencesButtonItemEventArgs(obj, IndexOf(obj)));
			}
		}

		private void OnBeforeItemRemoved(PreferencesButtonItem obj) {
			if (BeforeItemRemoved != null) {
				BeforeItemRemoved(this, new PreferencesButtonItemEventArgs(obj, IndexOf(obj)));
			}
		}

		private void OnItemRemoved() {
			if (ItemRemoved != null) {
				ItemRemoved(this, EventArgs.Empty);
			}
		}

		public PreferencesButtonItem this[int index] {
			get {
				return (PreferencesButtonItem)this.List[index];
			}
			set {
				this.List[index] = value;
			}
		}

		public void Add(PreferencesButtonItem buttonItem) {
			this.List.Add(buttonItem);
			this.OnItemAdded(buttonItem);

			buttonItem.Activated += delegate(object sender, EventArgs e) {
				foreach (PreferencesButtonItem val in this) {
					val.ActiveButton = false;
				}

				PreferencesButtonItem item = (PreferencesButtonItem)sender;

				item.ActiveButton = true;

				if (ItemActivated != null) {
					ItemActivated(this, new PreferencesButtonItemEventArgs(item, IndexOf(item)));
				}

			};
		}

		public void Remove(PreferencesButtonItem buttonItem) {
			this.RemoveAt(this.List.IndexOf(buttonItem));
		}

		public void Remove(int index) {
			this.RemoveAt(index);
		}

		public new void RemoveAt(int index) {
			this.OnBeforeItemRemoved((PreferencesButtonItem)this.List[index]);
			base.RemoveAt(index);
			this.OnItemRemoved();
		}

		public int IndexOf(PreferencesButtonItem buttonItem) {
			return this.List.IndexOf(buttonItem);
		}

	}

}

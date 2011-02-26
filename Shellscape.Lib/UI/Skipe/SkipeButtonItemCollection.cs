using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape.UI.Skipe {

	public class SkipeButtonItemCollection : CollectionBase {

		public event EventHandler<SkipeButtonItemEventArgs> ItemAdded;
		public event EventHandler ItemRemoved;
		public event EventHandler<SkipeButtonItemEventArgs> BeforeItemRemoved;
		public event EventHandler<SkipeButtonItemEventArgs> ItemActivated;

		public IEnumerable<SkipeButtonItem> All() {
			foreach (SkipeButtonItem control in this.List) {
				yield return control;
			}
		}

		private void OnItemAdded(SkipeButtonItem obj) {
			if (ItemAdded != null) {
				ItemAdded(this, new SkipeButtonItemEventArgs(obj, IndexOf(obj)));
			}
		}

		private void OnBeforeItemRemoved(SkipeButtonItem obj) {
			if (BeforeItemRemoved != null) {
				BeforeItemRemoved(this, new SkipeButtonItemEventArgs(obj, IndexOf(obj)));
			}
		}

		private void OnItemRemoved() {
			if (ItemRemoved != null) {
				ItemRemoved(this, EventArgs.Empty);
			}
		}

		public SkipeButtonItem this[int index] {
			get {
				return (SkipeButtonItem)this.List[index];
			}
			set {
				this.List[index] = value;
			}
		}

		public void Add(SkipeButtonItem buttonItem) {
			this.List.Add(buttonItem);
			this.OnItemAdded(buttonItem);

			buttonItem.Activated += delegate(object sender, EventArgs e) {
				foreach (SkipeButtonItem val in this) {
					val.ActiveButton = false;
				}

				SkipeButtonItem item = (SkipeButtonItem)sender;

				item.ActiveButton = true;

				if (ItemActivated != null) {
					ItemActivated(this, new SkipeButtonItemEventArgs(item, IndexOf(item)));
				}

			};
		}

		public void Remove(SkipeButtonItem buttonItem) {
			this.RemoveAt(this.List.IndexOf(buttonItem));
		}

		public void Remove(int index) {
			this.RemoveAt(index);
		}

		public new void RemoveAt(int index) {
			this.OnBeforeItemRemoved((SkipeButtonItem)this.List[index]);
			base.RemoveAt(index);
			this.OnItemRemoved();
		}

		public int IndexOf(SkipeButtonItem buttonItem) {
			return this.List.IndexOf(buttonItem);
		}

	}

}

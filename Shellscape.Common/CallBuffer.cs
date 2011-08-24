using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Shellscape {

	/// <summary>
	/// Used for buffering text input. eg. we want to be notified when the user stops inputting text, rather than being notified on every single change.
	/// </summary>
	public sealed class CallBuffer {

		private TimeSpan _timeSpan;
		private Timer _timer;
		private Action _call = null;

		public CallBuffer(Action call, int milliseconds) {
			_timeSpan = TimeSpan.FromMilliseconds(milliseconds);
			_call = call;
		}

		public void Buffer() {

			this.Active = true;
			
			if (_timer == null) {
				_timer = new Timer( new TimerCallback(Finished));
				_timer.Change(_timeSpan, TimeSpan.Zero);
			}
			else {
				_timer.Change(_timeSpan, TimeSpan.FromMilliseconds(-1));
			}
		}

		private void Finished(object state){
			_timer.Dispose();
			_timer = null;

			this.Active = false;

			_call.Invoke();
		}

		public Boolean Active { get; private set; }
	}
}

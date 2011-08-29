using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Shellscape {
	
	/// <summary>
	/// Exception thrown by AsyncOperation if attempt to start an already running process is attempted.
	/// </summary>
	public class AlreadyRunningException : System.ApplicationException {
		public AlreadyRunningException()
			: base("Asynchronous operation already running") {
		}
	}

	/// <summary>
	/// Abstract class to assist with building an background threaded process which interacts with the UI.   
	/// </summary>
	public abstract class AsyncOperation {

		private ISynchronizeInvoke isiTarget;
		private bool completeFlag;
		private bool cancelledFlag;
		private bool cancelAcknowledgedFlag;
		private bool failedFlag;
		private bool isRunning;

		public event EventHandler Completed;
		public event EventHandler Cancelled;
		public event System.Threading.ThreadExceptionEventHandler Failed;

		public AsyncOperation() {
			isRunning = false;
		}

		/// <summary>
		/// Creates a new instance of the class and specifies the target to fire events to
		/// </summary>
		/// <param name="target">The target object to fire events to</param>
		public AsyncOperation(ISynchronizeInvoke target) {
			isiTarget = target;
			isRunning = false;
		}

		/// <summary>
		/// Start running
		/// </summary>
		public void Start() {
			lock (this) {
				if (isRunning) {
					throw new AlreadyRunningException();
				}
				isRunning = true;
			}
			new MethodInvoker(InternalStart).BeginInvoke(null, null);
		}

		/// <summary>
		/// Cancel running
		/// </summary>
		public void Cancel() {
			lock (this) {
				cancelledFlag = true;
			}
		}

		/// <summary>
		/// Cancel and wait until the async operation acknowledges completion
		/// </summary>
		/// <returns>False if operation completed, True if Cancelled</returns>
		public bool CancelAndWait() {
			lock (this) {

				cancelledFlag = true;

				while (!IsDone) {
					Monitor.Wait(this, 200); // SPM: reduced time-out
				}
			}
			return !HasCompleted;
		}

		/// <summary>
		/// Wait until the async operation has completed
		/// </summary>
		/// <returns>True if operation is completed, False otherwise</returns>
		public bool WaitUntilDone() {
			lock (this) {
				while (!IsDone) {
					Monitor.Wait(this, 200); // SPM: reduced time-out
				}
			}
			return HasCompleted;
		}

		/// <summary>
		/// Gets whether the operation is complete or not
		/// </summary>
		public bool IsDone {
			get {
				lock (this) {
					// SPM check if the thing is actually running
					return (isRunning ?
						(completeFlag || cancelAcknowledgedFlag || failedFlag) :
						true);
				}
			}
		}

		/// <summary>
		/// Gets the ISynchronize invoke Target - the UI object
		/// to which you want to send events to
		/// </summary>
		protected ISynchronizeInvoke Target {
			get {
				return isiTarget;
			}
		}

		/// <summary>
		/// Override to perform the asynchronous work 
		/// </summary>
		protected abstract void DoWork();

		/// <summary>
		/// Gets whether a request to cancel has been made or not
		/// </summary>
		protected bool CancelRequested {
			get {
				lock (this) {
					return cancelledFlag;
				}
			}
		}

		/// <summary>
		/// Gets whether the operation has completed or not
		/// </summary>
		protected bool HasCompleted {
			get {
				lock (this) {
					return completeFlag;
				}
			}
		}

		/// <summary>
		/// Provides a method for the asynchronous object to Acknowledge receipt of a cancellation submitted through CancelRequest
		/// </summary>
		protected void AcknowledgeCancel() {
			lock (this) {
				cancelAcknowledgedFlag = true;
				isRunning = false;

				FireAsync(Cancelled, this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Starts the asynchronous operation
		/// </summary>
		private void InternalStart() {
			// Reset our state - we might be run more than once.
			cancelledFlag = false;
			completeFlag = false;
			cancelAcknowledgedFlag = false;
			failedFlag = false;
			// isRunning is set during Start to avoid a race condition
			try {
				DoWork();
			}
			catch (Exception e) {
				// Raise the Failed event.  We're in a catch handler, so we had better try not to throw another exception.
				try {
					FailOperation(e);
				}
				catch { }

				// The documentation recommends not catching SystemExceptions, so having notified the caller we rethrow if it was one of them.
				if (e is SystemException) {
					throw;
				}
			}

			lock (this) {
				if (!cancelAcknowledgedFlag && !failedFlag) {
					CompleteOperation();
				}
			}
		}

		/// <summary>
		/// Sets the operation as completed and fires the completed event
		/// </summary>
		private void CompleteOperation() {
			lock (this) {
				completeFlag = true;
				isRunning = false;
				Monitor.Pulse(this);
				
				FireAsync(Completed, this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Fires the operation failed event
		/// </summary>
		/// <param name="e">Exception which caused failure</param>
		private void FailOperation(Exception e) {
			lock (this) {
				failedFlag = true;
				isRunning = false;
				Monitor.Pulse(this);
				FireAsync(Failed, this, new ThreadExceptionEventArgs(e));
			}
		}

		/// <summary>
		/// Provides a thread-safe manner to fire a delegate on the Target object.
		/// </summary>
		/// <param name="dlg">Delegate to fire</param>
		/// <param name="pList">Parameter List to the Delegate</param>
		protected void FireAsync(Delegate dlg, params object[] pList) {
			if (dlg != null && Target != null) {
				Target.BeginInvoke(dlg, pList);
			}
		}
	}
}

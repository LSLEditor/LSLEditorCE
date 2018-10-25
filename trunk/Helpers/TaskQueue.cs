// <copyright file="gpl-2.0.txt">
// ORIGINAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden.
// The code was donated on 2010-04-28 by Alphons van der Heijden to Brandon 'Dimentox Travanti' Husbands &
// Malcolm J. Kudra, who in turn License under the GPLv2 in agreement with Alphons van der Heijden's wishes.
//
// The community would like to thank Alphons for all of his hard work, blood sweat and tears. Without his work
// the community would be stuck with crappy editors.
//
// The source code in this file ("Source Code") is provided by The LSLEditor Group to you under the terms of the GNU
// General Public License, version 2.0 ("GPL"), unless you have obtained a separate licensing agreement ("Other
// License"), formally executed by you and The LSLEditor Group.
// Terms of the GPL can be found in the gplv2.txt document.
//
// GPLv2 Header
// ************
// LSLEditor, a External editor for the LSL Language.
// Copyright (C) 2010 The LSLEditor Group.
//
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any
// later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free
// Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// ********************************************************************************************************************
// The above copyright notice and this permission notice shall be included in copies or substantial portions of the
// Software.
// ********************************************************************************************************************
// </copyright>
//
// <summary>
//
//
// </summary>

using System;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Threading;

// http://www.codeproject.com/csharp/messageloop.asp

namespace LSLEditor.Helpers
{
	/// <summary>
	/// Represents an object that performs a certain action asynchronously, by using an internal buffer queue
	/// and one internal thread.
	/// </summary>
	public class TaskQueue : IDisposable
	{
		#region Member Variables

		/// <summary>Reference to the thread used to empty the queue</summary>
		private Thread WorkerThread;
		/// <summary>Internal queue that serves as buffer for required actions</summary>
		private Queue Tasks;
		/// <summary>Used to signal the thread when a new object is added to the queue</summary>
		private AutoResetEvent SignalNewTask;
		/// <summary>Flag that notifies that the object should be disposed</summary>
		private bool stop;
		#endregion Member Variables

		#region Constructor

		/// <summary>Creates a new buffered object</summary>
		public TaskQueue()
		{
			WorkerThread = null;

			// Make sure the queue is synchronized. This is required because items are added to the queue
			// from a different thread than the thread that empties the queue
			Tasks = Queue.Synchronized(new Queue());

			SignalNewTask = new AutoResetEvent(false);

			stop = false;
		}
		#endregion Ctor

		#region Public Methods

		public void Start()
		{
			Stop();

			stop = false;
			Tasks.Clear();

			WorkerThread = new Thread(new ThreadStart(Worker));
			WorkerThread.IsBackground = true;
			WorkerThread.Start();
		}

		public void Stop()
		{
			if (WorkerThread != null) {
				WorkerThread.Abort();
				if (!WorkerThread.Join(2000)) {
					// problems
					System.Windows.Forms.MessageBox.Show("TaskQueue thread not Aborted", "Oops...");
				}
				WorkerThread = null;
			}
		}

		public void Invoke(object ActiveObject, string MethodName, params object[] args)
		{
			if (ActiveObject == null)
				return;
			try {
				// Add the object to the internal buffer
				Tasks.Enqueue(new Task(ActiveObject, MethodName, args));

				// Signal the internal thread that there is some new object in the buffer
				SignalNewTask.Set();
			} catch (Exception e) {
				Trace.WriteLine(string.Format("An exception occurred in TaskQueue.Invoke: {0}", e.Message));
				// Since the exception was not actually handled and only logged - propagate it
				throw;
			}
		}
		#endregion Public Methods

		#region Private Methods

		/// <summary>Method executed by the internal thread to empty the queue</summary>
		private void Worker()
		{
			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
			Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US", false);

			while (!stop) {
				try {
					// Note: this code is safe (i.e. performing the .Count and .Dequeue not inside a lock)
					// because there is only one thread emptying the queue.
					// Even if .Count returns 0, and before Dequeue is called a new object is added to the Queue
					// then still the system will behave nicely: the next if statement will return false and
					// since this is run in an endless loop, in the next iteration we will have .Count > 0.
					if (Tasks.Count > 0) {
						(Tasks.Dequeue() as Task).Execute();
					}

					// Wait until new objects are received or Dispose was called
					if (Tasks.Count == 0) {
						SignalNewTask.WaitOne();
					}
				} catch (ThreadAbortException) {
					Trace.WriteLine("TaskQueue.Worker: ThreadAbortException, no problem");
				} catch (Exception e) {
					Trace.WriteLine(string.Format("TaskQueue.Worker: {0}", e.Message));
					// Since the exception was not actually handled and only logged - propagate it
					throw;
				}
			}
		}

		#endregion Private Methods

		#region IDisposable Members and Dispose Pattern

		private bool disposed = false;

		~TaskQueue()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed) {
				if (disposing) {
					try {
						stop = true;
						SignalNewTask.Set();
					} catch (Exception e) {
						Trace.WriteLine(string.Format("An exception occurred in MessageLoop.AddToBuffer: {0}", e.Message));
						// Since the exception was not actually handled and only logged - propagate it
						throw;
					}
				}
				this.disposed = true;
			}
		}
		#endregion IDisposable Members and Dispose Pattern

		#region Task
		/// <summary>The tasks being saved in the queue</summary>
		private class Task
		{
			private object ActiveObject;
			private object[] args;
			public string MethodName;
			public Task(object ActiveObject, string MethodName, params object[] args)
			{
				this.ActiveObject = ActiveObject;
				this.MethodName = MethodName;
				this.args = args;
			}

			public void Execute()
			{
				try {
					MethodInfo mi = ActiveObject.GetType().GetMethod(MethodName,
						BindingFlags.Public |
						BindingFlags.Instance |
						//BindingFlags.DeclaredOnly |
						BindingFlags.NonPublic
						);
					mi.Invoke(ActiveObject, args);
				} catch (ThreadAbortException) {
					Trace.WriteLine("TaskQueue.Task.Execute: ThreadAbortException, no problem");
				} catch (Exception exception) {
					Exception innerException = exception.InnerException;
					if (innerException == null)
						innerException = exception;
					string strMessage = OopsFormatter.ApplyFormatting(innerException.Message);
					string strStackTrace = OopsFormatter.ApplyFormatting(innerException.StackTrace);

					System.Windows.Forms.MessageBox.Show(strMessage + "\r\n" + strStackTrace, "Oops...");
				}
			}
		}
		#endregion Task
	}
}
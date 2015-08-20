//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Runtime.ConstrainedExecution;

namespace Implementation
{
    /// <summary>
    /// Base class for managing native resources.
    /// </summary>
    public abstract class DisposableBase : CriticalFinalizerObject, IDisposable
    {
        private volatile bool _mIsDisposed;

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (!_mIsDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _mIsDisposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected abstract void Dispose(bool disposing);
        //      if (disposing)
        //      {
        //         // get rid of managed resources 
        //      }
        //      // get rid of unmanaged resources 

        /// <summary>
        /// 
        /// </summary>
        ~DisposableBase()
        {
            if (!_mIsDisposed)
            {
                Dispose(false);
                _mIsDisposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void VerifyObjectNotDisposed()
        {
            if (_mIsDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
    }
}

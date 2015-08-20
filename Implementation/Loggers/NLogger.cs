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

using Declarations;

namespace Implementation
{
   internal class NLogger : ILogger
   {
      NLog.Logger _mLogImpl;

      public NLogger()
      {
         _mLogImpl = NLog.LogManager.GetCurrentClassLogger();
      }

      #region ILogger Members

      public void Debug(string debug)
      {
         _mLogImpl.Debug(debug);
      }

      public void Info(string info)
      {
         _mLogImpl.Info(info);
      }

      public void Warning(string warn)
      {
         _mLogImpl.Warn(warn);
      }

      public void Error(string error)
      {
         _mLogImpl.Error(error);
      }

      #endregion
   }
}

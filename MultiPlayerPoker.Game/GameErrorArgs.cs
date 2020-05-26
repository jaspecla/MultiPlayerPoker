using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MultiPlayerPoker.Game
{
  internal class GameErrorEventArgs : EventArgs
  {
    internal string Message { get; private set; }
    internal Exception Exception { get; private set; }
    internal Player Player { get; private set; }
    internal GameErrorEventArgs(string message, Player player = null, Exception exception = null)
    {
      Message = message;
      Player = player;
      Exception = exception;
    }
  }
}

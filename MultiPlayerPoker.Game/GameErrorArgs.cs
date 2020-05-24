using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class GameErrorEventArgs : EventArgs
  {
    public string Message { get; private set; }
    public Exception Exception { get; private set; }
    public GameErrorEventArgs(string message, Exception exception = null)
    {
      Message = message;
      Exception = exception;
    }
  }
}

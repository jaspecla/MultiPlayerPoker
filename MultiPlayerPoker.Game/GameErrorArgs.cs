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
    public Player Player { get; private set; }
    public GameErrorEventArgs(string message, Player player = null, Exception exception = null)
    {
      Message = message;
      Player = player;
      Exception = exception;
    }
  }
}

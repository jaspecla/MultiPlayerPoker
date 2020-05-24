using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class BetException : Exception
  {
    public BetException() { }
    public BetException(string message) : base(message) { }
    public BetException(string message, Exception innerException) : base(message, innerException) { }

  }
}

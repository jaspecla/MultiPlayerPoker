using System;
using System.Collections.Generic;
using System.Text;

namespace MultiPlayerPoker.Game
{
  public class AddPlayerException : Exception
  {
    public AddPlayerException() { }
    public AddPlayerException(string message) : base(message) { }
    public AddPlayerException(string message, Exception innerException) : base(message, innerException) { }
  }
}

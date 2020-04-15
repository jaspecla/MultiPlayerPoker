using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayerPoker.Functions.Data
{
  public interface IGameDb
  {
    Task CreateGame(NewGame newGame);
  }
}

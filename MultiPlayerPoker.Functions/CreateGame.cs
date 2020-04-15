using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MultiPlayerPoker.Functions.Data;
using System.Web.Http;

namespace MultiPlayerPoker.Functions
{
  public static class CreateGame
  {
    [FunctionName("CreateGame")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        
        ILogger log)
    {
      log.LogInformation("CreateGame function called.");

      string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      var newGame = JsonConvert.DeserializeObject<NewGame>(requestBody);

      if (newGame == null)
      {
        return new BadRequestErrorMessageResult("Invalid new game.");
      }

      if (string.IsNullOrWhiteSpace(newGame.DisplayName))
      {
        return new BadRequestErrorMessageResult("Game must have a display name.");
      }

      if (newGame.BigBlind < 1)
      {
        return new BadRequestErrorMessageResult("Big Blind must be at least 1 unit.");
      }

      newGame.Id = Guid.NewGuid().ToString();

      return new OkObjectResult(newGame);
    }
  }
}

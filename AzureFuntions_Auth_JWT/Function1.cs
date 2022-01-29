using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AzureFuntions_Auth_JWT
{
    public static class Function1
    {
        [FunctionName(nameof(UserAuthenication))]
        public static async Task<IActionResult> UserAuthenication(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")] UserCredentials userCredentials,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            // TODO: Perform custom authentication here; we're just using a simple hard coded check for this example
            bool authenticated = userCredentials?.User.Equals("Jay", StringComparison.InvariantCultureIgnoreCase) ?? false;

            if (!authenticated)
            {
                return await Task.FromResult(new UnauthorizedResult()).ConfigureAwait(false);
            }
            else
            {
                return await Task.FromResult(new OkObjectResult("User is Authenticated")).ConfigureAwait(false);
            }

        }
    }

    public class UserCredentials
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}

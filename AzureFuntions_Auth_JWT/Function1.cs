using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
                GenerateJWTToken generateJWTToken = new();
                string token = generateJWTToken.IssuingJWT(userCredentials.User);
                return await Task.FromResult(new OkObjectResult(token)).ConfigureAwait(false);
            }

        }
        [FunctionName(nameof(GetData))]
        public static async Task<IActionResult> GetData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "data")] HttpRequest req,
            ILogger log)
        {
            // Check if we have authentication info.
            ValidateJWT auth = new ValidateJWT(req);

            if (!auth.IsValid)
            {
                return new UnauthorizedResult(); // No authentication info.
            }

            string postData = await req.ReadAsStringAsync();

            return new OkObjectResult($"{postData}");

        }
    }

    public class UserCredentials
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}


using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace AzureRegistration
{
    public static class GetAllRegisteredUsers
    {
        [FunctionName("GetAllRegisteredUsers")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Start function GetAllRegisteredUsers().");

            try
            {
                UserDataManager manager = new UserDataManager();
                var result = manager.GetAllUsers();
                return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(result));
            }
            catch (System.Exception ex)
            {
                return new BadRequestObjectResult("Invalid query, please ensure you have provided all the data. Exception: " + ex.ToString());
            }
        }
    }
}

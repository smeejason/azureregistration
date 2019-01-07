
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace AzureRegistration
{
    public static class UserRegistration
    {
        [FunctionName("UserRegistration")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string email = req.Query["email"];
            string lastName = req.Query["lastname"];
            string plusOne = req.Query["plusone"];
            bool plusOneBool = false;
            string plusOneName = req.Query["plusonename"];
            string foodPreferences = req.Query["foodpreferences"];
            string cantMakeIt = req.Query["cantmakeit"];
            bool cantMakeItBool = false;
            string canISleepOver = req.Query["canisleepover"];
            bool canISleepOverBool = false;
            string comments = req.Query["comments"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            email = email ?? data?.email;
            lastName = lastName ?? data?.lastname;
            plusOne = plusOne ?? data?.plusone;
            bool.TryParse(plusOne, out plusOneBool);
            plusOneName = plusOneName ?? data?.plusonename;
            foodPreferences = foodPreferences ?? data?.foodpreferences;
            cantMakeIt = cantMakeIt ?? data?.cantmakeit;
            bool.TryParse(cantMakeIt, out cantMakeItBool);
            canISleepOver = canISleepOver ?? data?.canisleepover;
            bool.TryParse(canISleepOver, out canISleepOverBool);
            comments = comments ?? data?.comments;

            try
            {
                UserDataManager manager = new UserDataManager();
                var result = manager.AddNewUser(email, name, lastName, plusOneBool, plusOneName, foodPreferences, cantMakeItBool, canISleepOverBool, comments);
                return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(result));
            }
            catch (System.Exception ex)
            {
                return new BadRequestObjectResult("Invalid query, please ensure you have provided all the data. Exception: " + ex.ToString());
            }
        }
    }
}

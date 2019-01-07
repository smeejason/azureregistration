
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace AzureRegistration
{
    public static class UploadImage
    {
        [FunctionName("UploadImage")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string fileName = data.fileName;
            var fileData = data.fileContent;
            string val = fileData.ToObject<string>();
            val = val.Substring(val.IndexOf("base64,") + 7);
            var base64EncodedBytes = System.Convert.FromBase64String(val);
            var result = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            // log.Info($"file data val : "+result);
            StringReader stringReader = new StringReader(result);

            return result != null
                ? (ActionResult)new OkObjectResult(result)
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}

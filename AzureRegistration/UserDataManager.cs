using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using AzureRegistration.Model;
using System.Threading.Tasks;

namespace AzureRegistration
{
    public class UserDataManager
    {
        private string StorageConnectionString;

        #region Methods

        public UserEntity AddNewUser(string email, string name, string lastName, bool plusOne, string plusOneName, string foodPreferences, bool cantMakeIt, bool canISleepOver, string comments)
        {
            var table = GetTable(StorageConnectionString);
            UserEntity customer1 = new UserEntity(email, name, lastName, plusOne , plusOneName, foodPreferences, cantMakeIt, canISleepOver, comments);
            TableOperation insertOperation = TableOperation.Insert(customer1);
            table.Result.ExecuteAsync(insertOperation);
            return GetUserByEmail(email);
        }

        public UserEntity GetUserByEmail(string email)
        {
            var table = GetTable(StorageConnectionString);
            TableOperation retOp = TableOperation.Retrieve<UserEntity>("None", email);
            TableResult tr = table.Result.ExecuteAsync(retOp).Result;
            return tr.Result as UserEntity;
        }

        public List<UserEntity> GetAllUsers()
        {
            var results = this.GetAllUsersAsync();
            return results.Result;
        }

        private async Task<List<UserEntity>> GetAllUsersAsync()
        {
            var table = GetTable(StorageConnectionString);
            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "None"));

            TableContinuationToken token = null;
            List<UserEntity> returnValue = new List<UserEntity>();
            do
            {
                TableQuerySegment<UserEntity> resultSegment = await table.Result.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                foreach (UserEntity entity in resultSegment.Results)
                {
                    if (entity.CantMakeIt == false)
                    {
                        returnValue.Add(entity);
                    }
                }
            } while (token != null);

            return returnValue;
        }

        private async Task<CloudTable> GetTable(string StorageConnectionString)
        {
            StorageConnectionString = GetEnvironmentVariable("AzureWebJobsStorage");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Users");
            
            await table.CreateIfNotExistsAsync();
            return table;
        }

        /// <summary>
        /// A method that gets the app settings for the applicaiton. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }

        #endregion
    }
}

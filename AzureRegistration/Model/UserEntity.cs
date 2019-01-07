namespace AzureRegistration.Model
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;

    public class UserEntity : TableEntity
    {
        public UserEntity(string email, string firstName, string lastName, bool plusOne, string plusOneName, string foodPreferences, bool cantMakeIt, bool canISleepOver, string comments)
        {
            ///This is being left as a single partition here as we are aiming on 200 items per partition.
            this.PartitionKey = "None";
            this.Created = DateTime.Now;
            this.RowKey = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PlusOne = plusOne;
            this.PlusOneName = plusOneName;
            this.FoodPreferences = foodPreferences;
            this.CantMakeIt = cantMakeIt;
            this.CanISleepOver = canISleepOver;
            this.Comments = comments;
        }

        public UserEntity() { }

        public DateTime Created { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool PlusOne { get; set; }

        public string PlusOneName { get; set; }

        public bool CantMakeIt { get; set; }

        public bool CanISleepOver { get; set; }

        public string FoodPreferences { get; set; }

        public string Comments { get; set; }

    }
}

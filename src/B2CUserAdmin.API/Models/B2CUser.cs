using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace B2CUserAdmin.API.Models
{
    public class B2CUser
    {
        public Guid ObjectId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }

        public B2CUser(Guid objectId,
                       string email,
                       string displayName,
                       string firstName,
                       string lastName,
                       string companyName)
        {
            ObjectId = objectId;
            Email = email;
            DisplayName = displayName;
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
        }
    }
}

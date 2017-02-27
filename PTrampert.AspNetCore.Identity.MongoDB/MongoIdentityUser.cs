using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class MongoIdentityUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
    }
}

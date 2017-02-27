using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class MongoIdentityUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}

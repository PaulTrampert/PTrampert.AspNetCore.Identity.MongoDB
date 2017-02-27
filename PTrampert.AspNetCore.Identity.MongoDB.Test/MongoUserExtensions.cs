using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    public static class MongoUserExtensions
    {
        public static bool PropertiesEqual(this MongoIdentityUser self, MongoIdentityUser other)
        {
            var properties = typeof(MongoIdentityUser).GetProperties();
            var result = true;
            foreach (var prop in properties)
            {
                result = result && prop.GetValue(self).Equals(prop.GetValue(other));
            }
            return result;
        }
    }
}

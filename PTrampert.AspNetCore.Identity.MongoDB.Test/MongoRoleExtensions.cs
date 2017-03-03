using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PTrampert.AspNetCore.Identity.MongoDB.Test
{
    public static class MongoRoleExtensions
    {
        public static bool PropertiesEqual(this IdentityRole self, IdentityRole other)
        {
            var properties = typeof(IdentityRole).GetProperties();
            var result = true;
            foreach (var prop in properties)
            {
                result = result && prop.GetValue(self).Equals(prop.GetValue(other));
            }
            return result;
        }
    }
}

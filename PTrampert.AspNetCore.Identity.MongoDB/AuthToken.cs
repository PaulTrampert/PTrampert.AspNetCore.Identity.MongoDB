using System;
using System.Collections.Generic;
using System.Text;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class AuthToken : IEquatable<AuthToken>
    {
        public string LoginProvider { get; private set; }
        public string Name { get; private set; }
        public string Value { get; set; }

        private AuthToken()
        {
        }

        public AuthToken(string loginProvider, string name, string value)
        {
            LoginProvider = loginProvider;
            Name = name;
            Value = value;
        }

        public bool Equals(AuthToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(LoginProvider, other.LoginProvider) && string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AuthToken) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (LoginProvider != null ? LoginProvider.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}

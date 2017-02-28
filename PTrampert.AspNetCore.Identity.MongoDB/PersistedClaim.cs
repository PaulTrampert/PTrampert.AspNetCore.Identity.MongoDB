using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PTrampert.AspNetCore.Identity.MongoDB
{
    public class PersistedClaim : IEquatable<PersistedClaim>, IEquatable<Claim>
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public PersistedClaim(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() * 17 + Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is PersistedClaim)
            {
                return Equals((PersistedClaim)obj);
            }
            else if (obj is Claim)
            {
                return Equals((Claim)obj);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public bool Equals(PersistedClaim other)
        {
            return Type == other.Type && Value == other.Value;
        }

        public bool Equals(Claim other)
        {
            return Type == other.Type && Value == other.Value;
        }
    }
}

using System;
using Chaos.Core.Extensions;
using Newtonsoft.Json;

namespace DKPBot.Services.AliasModel
{
    [JsonObject]
    public class Alias : IEquatable<Alias>
    {
        [JsonProperty]
        public string Original { get; }
        [JsonProperty]
        public string Result { get; }

        [JsonConstructor]
        public Alias(string original, string result)
        {
            Original = original;
            Result = result;
        }

        public bool Equals(Alias other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Original.EqualsI(other.Original) && Result.EqualsI(other.Result);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((Alias) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Original != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Original) : 0) * 397) ^
                       (Result != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Result) : 0);
            }
        }
    }
}
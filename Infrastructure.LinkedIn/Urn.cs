using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Infrastructure.LinkedIn
{
    public class Urn
    {
        
        public string Namespace { get; }
        public string EntityType { get; }
        public string Id { get; }
        public Urn(string urn)
        {
            var urnValidator = new Regex("^urn:[a-z0-9][a-z0-9-]{0,31}:[a-z0-9()+,\\-.:=@;$_!*'%/?#]+$", RegexOptions.IgnoreCase);
            if ( ! urnValidator.Match(urn).Success)
            {
                throw new ArgumentException("Invalid urn: "+urn);
            }
            
            var parts = urn.Split(":");
            Namespace = parts[1];
            EntityType = parts[2];
            Id = parts[3];
        }
        public Urn(string @namespace, string entityType, string id)
        {
            Namespace = @namespace;
            EntityType = entityType;
            Id = id;
        }


        public override string ToString()
        {
            return $"urn:{Namespace}:{EntityType}:{Id}";
        }
    }
}
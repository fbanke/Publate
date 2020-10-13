using System;
using Xunit;

namespace Infrastructure.LinkedIn.UnitTest
{
    public class UrnTest
    {
        [Fact]
        public void Should_GiveUrn_When_UsingToString()
        {
            var urn = new Urn("namespace", "entityType", "id");
            Assert.Equal("urn:namespace:entityType:id", urn.ToString());
        }

        [Fact]
        public void Should_ParseUrn_When_InstantiatedWithValidUrn()
        {
            var urn = new Urn("urn:namespace:entityType:id");
            
            Assert.Equal("namespace", urn.Namespace);
            Assert.Equal("entityType", urn.EntityType);
            Assert.Equal("id", urn.Id);
        }
        
        [Fact]
        public void Should_ThrowException_When_InstantiatedWithInvalidUrn()
        {
            Assert.Throws<ArgumentException>( () => new Urn("invalidUrn"));
        }
    }
}
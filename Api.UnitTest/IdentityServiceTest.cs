using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Xunit;

namespace Api.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Should_Given_ValidNameIdentifierClaim_GiveTheNameIdentifier()
        {
            var fixture = new Fixture();
            var userId = fixture.Create<string>();
            FakeHttpContext.NameIdentifierValue = userId;
            
            var accessor = new FakeHttpAccessor();
            var sut = new IdentityService(accessor);
            
            Assert.Equal(userId, sut.GetUserId());
        }
        
        [Fact]
        public void Should_Given_MissingNameIdentifierClaim_GiveNotFoundException()
        {
            var accessor = new FakeHttpAccessor {HttpContext = {User = null}};
            var sut = new IdentityService(accessor);
            
            Assert.Throws<InvalidOperationException>(() => sut.GetUserId());
        }
    }

    public class FakeHttpAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; } = new FakeHttpContext();
    }

    public class FakeHttpContext : HttpContext
    {
        public static string NameIdentifierValue = "";
        public override void Abort()
        {
            
        }

        public override ConnectionInfo Connection { get; }
        public override IFeatureCollection Features { get; }
        public override IDictionary<object, object> Items { get; set; }
        public override HttpRequest Request { get; }
        public override CancellationToken RequestAborted { get; set; }
        public override IServiceProvider RequestServices { get; set; }
        public override HttpResponse Response { get; }
        public override ISession Session { get; set; }
        public override string TraceIdentifier { get; set; }

        public override ClaimsPrincipal User { get; set; } = new ClaimsPrincipal(new List<ClaimsIdentity>
        {
            new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, NameIdentifierValue)
            })
        });
        public override WebSocketManager WebSockets { get; }
    }
}
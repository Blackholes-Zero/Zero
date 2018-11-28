using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WechatBusiness.Api.Tests
{
    public class ValuesTests
    {
        public HttpClient Client { get; }
        public ValuesTests()
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
           .UseStartup<Startup>());
            Client = server.CreateClient();
        }

        //[Fact]
        [Theory]
        [InlineData(1)]
        public async Task GetId(int id)
        {
            // Arrange
            //var id = 1;

            // Act
            var response = await Client.GetAsync($"/api/Account/{id}");

            var responseText = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }

}

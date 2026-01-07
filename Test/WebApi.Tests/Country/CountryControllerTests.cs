using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests.Country
{
    public class CountryControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CountryControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetCountries_ShouldReturn200()
        {
            var response = await _client.GetAsync("/api/countries");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}

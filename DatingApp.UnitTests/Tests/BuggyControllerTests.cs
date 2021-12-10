using DatingApp.Api.DTOs;
using DatingApp.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DatingApp.UnitTests.Tests
{
    public class BuggyControllerTests
    {
        private string apiRoute = "api/buggy";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private string registeredObject;
        private HttpContent httpContent;

        public BuggyControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("OK", "karen", "Pa$$w0rd")]
        public async Task GetSecret_ShouldReturnOK(string statusCode, string username, string password)
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };
            
            registeredObject = GetRegisterObject(loginDto);
            httpContent = GetHttpContent(registeredObject);
            
            var result = await _client.PostAsync("api/account/login", httpContent);
            var userJson = await result.Content.ReadAsStringAsync();
            var user = userJson.Split(',');
            var token = user[1].Split("\"")[3];
            
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestUri = $"{apiRoute}/auth";

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NotFound")]
        public async Task GetNotFound_ShouldReturnNotFound(string statusCode)
        {
            // Arrange
            requestUri = $"{apiRoute}/not-fund";

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("InternalServerError")]
        public async Task GetServerError_ShouldReturnNotFound(string statusCode)
        {
            // Arrange
            requestUri = $"{apiRoute}/server-error";

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("BadRequest")]
        public async Task GetBadRequest_ShouldReturnBadRequest(string statusCode)
        {
            // Arrange
            requestUri = $"{apiRoute}/bad-request";

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods

        private static string GetRegisterObject(LoginDto loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
            return entityObject.ToString();
        }

        private static string GetRegisterObject(string roles)
        {
            var entityObject = new JObject()
            {
                { "roles", roles }
            };
            return entityObject.ToString();
        }

        private StringContent GetHttpContent(string objectToEncode)
        {
            return new StringContent(objectToEncode, Encoding.UTF8, "application/json");
        }

        #endregion
    }
}

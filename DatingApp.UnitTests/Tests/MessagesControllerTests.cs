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
    public class MessagesControllerTests
    {
        private string apiRoute = "api/messages";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private string registeredObject;
        private HttpContent httpContent;
       
        public MessagesControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }
        
        [Theory]
        [InlineData("BadRequest", "lois", "Pa$$w0rd", "lois", "Hola")]
        public async Task CreateMessage_BadRequest(string statusCode, string username, string password, string recipientUsername, string content)
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

            var messageDto = new MessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };
            
            registeredObject = GetRegisterObject(messageDto);
            httpContent = GetHttpContent(registeredObject);
            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        
        [Theory]
        [InlineData("NotFound", "lois", "Pa$$w0rd", "pedritosola", "Hola")]
        public async Task CreateMessage_NotFound(string statusCode, string username, string password, string recipientUsername, string content)
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

            var messageDto = new MessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };
            
            registeredObject = GetRegisterObject(messageDto);
            httpContent = GetHttpContent(registeredObject);
            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        
        [Theory]
        [InlineData("OK", "lois", "Pa$$w0rd", "lisa", "Hola")]
        public async Task CreateMessage_OK(string statusCode, string username, string password, string recipientUsername, string content)
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

            var messageDto = new MessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };
            
            registeredObject = GetRegisterObject(messageDto);
            httpContent = GetHttpContent(registeredObject);
            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.PostAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lois", "Pa$$w0rd", "lisa", "Hola")]
        public async Task GetMessagesForUser_OK(string statusCode, string username, string password, string recipientUsername, string content)
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
            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lois", "Pa$$w0rd", "Outbox")]
        public async Task GetMessagesForUserFromQuery_OK(string statusCode, string username, string password, string container)
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
            requestUri = $"{apiRoute}" + "?container=" + container;

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lois", "Pa$$w0rd", "lisa")]
        public async Task GetMessagesThread_OK(string statusCode, string username, string password, string user2)
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

            requestUri = $"{apiRoute}/thread/" + user2;

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lois", "Pa$$w0rd", "lisa", "Hola")]
        public async Task DeleteMessage_OK(string statusCode, string username, string password, string recipientUsername, string content)
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

            var messageDto = new MessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };

            registeredObject = GetRegisterObject(messageDto);
            httpContent = GetHttpContent(registeredObject);
            requestUri = $"{apiRoute}";
            result = await _client.PostAsync(requestUri, httpContent);

            var messageJson = await result.Content.ReadAsStringAsync();
            var message = messageJson.Split(',');
            var id = message[0].Split("\"")[2].Split(":")[1];
            requestUri = $"{apiRoute}/" + id;

            // Act
            httpResponse = await _client.DeleteAsync(requestUri);

            loginDto = new LoginDto
            {
                Username = recipientUsername,
                Password = password
            };

            registeredObject = GetRegisterObject(loginDto);
            httpContent = GetHttpContent(registeredObject);
            result = await _client.PostAsync("api/account/login", httpContent);
            userJson = await result.Content.ReadAsStringAsync();
            user = userJson.Split(',');
            token = user[1].Split("\"")[3];
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            httpResponse = await _client.DeleteAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("Unauthorized", "lois", "Pa$$w0rd", "lisa", "Hola", "todd")]
        public async Task DeleteMessage_Unauthorized(string statusCode, string username, string password, string recipientUsername, string content, string unauth)
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

            var messageDto = new MessageDto
            {
                RecipientUsername = recipientUsername,
                Content = content
            };

            registeredObject = GetRegisterObject(messageDto);
            httpContent = GetHttpContent(registeredObject);
            requestUri = $"{apiRoute}";
            result = await _client.PostAsync(requestUri, httpContent);

            var messageJson = await result.Content.ReadAsStringAsync();
            var message = messageJson.Split(',');
            var id = message[0].Split("\"")[2].Split(":")[1];

            requestUri = $"{apiRoute}/" + id;

            loginDto = new LoginDto
            {
                Username = unauth,
                Password = password
            };

            registeredObject = GetRegisterObject(loginDto);
            httpContent = GetHttpContent(registeredObject);
            result = await _client.PostAsync("api/account/login", httpContent);
            userJson = await result.Content.ReadAsStringAsync();
            user = userJson.Split(',');
            token = user[1].Split("\"")[3];

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            httpResponse = await _client.DeleteAsync(requestUri);

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

        private static string GetRegisterObject(MessageDto message)
        {
            var entityObject = new JObject()
            {
                { nameof(message.RecipientUsername), message.RecipientUsername },
                { nameof(message.Content), message.Content }
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

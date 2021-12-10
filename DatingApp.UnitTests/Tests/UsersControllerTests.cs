using DatingApp.Api.DTOs;
using DatingApp.UnitTests.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Xunit;

namespace DatingApp.UnitTests.Tests
{
    public class UsersControllerTests
    {
        private string apiRoute = "api/users";
        private readonly HttpClient _client;
        private HttpResponseMessage httpResponse;
        private string requestUri;
        private string registeredObject;
        private HttpContent httpContent;

        private string requestUrl;
        private string loginObjetct;
        private string memberObjetct;

        public UsersControllerTests()
        {
            _client = TestHelper.Instance.Client;
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd")]
        public async Task GetUsers_ShouldOK(string statusCode, string username, string password)
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
        [InlineData("OK", "admin", "Pa$$w0rd")]
        public async Task GetUserByUsername_ShouldOK(string statusCode, string username, string password)
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
            requestUri = $"{apiRoute}/" + username;

            // Act
            httpResponse = await _client.GetAsync(requestUri);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("NoContent", "lisa", "Pa$$w0rd", "IntroductionU", "LookingForU", "InterestsU", "CityU", "CountryU")]
        public async Task UpdateUser_ShouldNoContent(string statusCode, string username, string password, string introduction, string lookingFor, string interests, string city, string country)
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

            var memberUpdateDto = new MemberUpdateDto
            {
                Introduction = introduction,
                LookingFor = lookingFor,
                Interests = interests,
                City = city,
                Country = country
            };
            
            registeredObject = GetRegisterObject(memberUpdateDto);
            httpContent = GetHttpContent(registeredObject);
            requestUri = $"{apiRoute}";

            // Act
            httpResponse = await _client.PutAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("Created", "lisa", "Pa$$w0rd", "img.jpg")]
        public async Task AddPhoto_ShouldNoContent(string statusCode, string username, string password, string file)
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

            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent(file);
            form.Add(content, file);
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(file);
            var stream = await sampleFile.OpenStreamForReadAsync();
            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = sampleFile.Name
            };
            
            form.Add(content);
            requestUri = $"{apiRoute}" + "/add-photo";

            // Act
            httpResponse = await _client.PostAsync(requestUri, form);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        /*
        [Theory]
        [InlineData("NoContent", "lisa", "Pa$$w0rd", "1")]
        public async Task SetMainPhoto_OK(string statusCode, string username, string password, string id)
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

            requestUri = $"{apiRoute}" + "/photos/" + id;

            // Act
            httpResponse = await _client.PutAsync(requestUri, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }
        */

        [Theory]
        [InlineData("NoContent", "lisa", "Pa$$w0rd", "/Users/jesus_reynoso/projects/webservices/datingapp/datingapp.unittests/img.jpg")]
        public async Task SetMainPhoto_ShouldOK(string statusCode, string username, string password, string namePhoto)
        {
            // Arrange
            requestUrl = "api/account/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            loginObjetct = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            var reponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            MultipartFormDataContent formDataContent = new MultipartFormDataContent();
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync(namePhoto);
            var stream = await storageFile.OpenStreamForReadAsync();

            httpContent = new StreamContent(stream);

            formDataContent.Add(httpContent, "File", namePhoto);

            requestUrl = $"{apiRoute}/add-photo";

            httpResponse = await _client.PostAsync(requestUrl, formDataContent);
            reponse = await httpResponse.Content.ReadAsStringAsync();
            var photoDto = JsonSerializer.Deserialize<PhotoDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            requestUrl = $"{apiRoute}/photos/" + photoDto.Id;

            // Act
            httpResponse = await _client.PutAsync(requestUrl, httpContent);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        [Theory]
        [InlineData("OK", "lisa", "Pa$$w0rd", "/Users/jesus_reynoso/projects/webservices/datingapp/datingapp.unittests/imgd.jpg")]
        public async Task DeletePhoto_ShouldOK(string statusCode, string username, string password, string namePhoto)
        {
            // Arrange
            requestUrl = "api/account/login";
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            loginObjetct = GetLoginObject(loginDto);
            httpContent = GetHttpContent(loginObjetct);

            httpResponse = await _client.PostAsync(requestUrl, httpContent);
            var reponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<UserDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userDto.Token);

            MultipartFormDataContent formDataContent = new MultipartFormDataContent();
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync(namePhoto);
            var stream = await storageFile.OpenStreamForReadAsync();

            httpContent = new StreamContent(stream);

            formDataContent.Add(httpContent, "File", namePhoto);

            requestUrl = $"{apiRoute}/add-photo";

            httpResponse = await _client.PostAsync(requestUrl, formDataContent);
            reponse = await httpResponse.Content.ReadAsStringAsync();
            var photoDto = JsonSerializer.Deserialize<PhotoDto>(reponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            requestUrl = $"{apiRoute}/photos/" + photoDto.Id;

            // Act
            httpResponse = await _client.DeleteAsync(requestUrl);

            // Assert
            Assert.Equal(Enum.Parse<HttpStatusCode>(statusCode, true), httpResponse.StatusCode);
            Assert.Equal(statusCode, httpResponse.StatusCode.ToString());
        }

        #region Privated methods

        private static string GetLoginObject(LoginDto loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };

            return entityObject.ToString();
        }

        private static string GetMemberObject(MemberDto memberDto)
        {
            var entityObject = new JObject()
            {
                { nameof(memberDto.Introduction), memberDto.Introduction },
                { nameof(memberDto.LookingFor), memberDto.LookingFor },
                { nameof(memberDto.Interests), memberDto.Interests },
                { nameof(memberDto.City), memberDto.City },
                { nameof(memberDto.Country), memberDto.Country }
            };

            return entityObject.ToString();
        }

        private static string GetRegisterObject(LoginDto loginDto)
        {
            var entityObject = new JObject()
            {
                { nameof(loginDto.Username), loginDto.Username },
                { nameof(loginDto.Password), loginDto.Password }
            };
            return entityObject.ToString();
        }
        
        private static string GetRegisterObject(MemberUpdateDto memberUpdateDto)
        {
            var entityObject = new JObject()
            {
                { nameof(memberUpdateDto.Introduction), memberUpdateDto.Introduction },
                { nameof(memberUpdateDto.LookingFor), memberUpdateDto.LookingFor },
                { nameof(memberUpdateDto.Interests), memberUpdateDto.Interests },
                { nameof(memberUpdateDto.City), memberUpdateDto.City },
                { nameof(memberUpdateDto.Country), memberUpdateDto.Country }
            };
            return entityObject.ToString();
        }
        
        private static string GetRegisterObject(string file)
        {
            var entityObject = new JObject()
            {
                { "File", file}
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

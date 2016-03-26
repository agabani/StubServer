﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using StubServer.Http;

namespace StubServer.Tests.Acceptance.Http
{
    internal class SampleTests
    {
        [Test]
        public void SimpleTest()
        {
            // Arrange
            IHttpStubServer httpStubServer = new HttpStubServer(new Uri("http://localhost:5000"));

            httpStubServer
                .Setup(message => true)
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000")};

            // Act
            var httpResponseMessage = httpClient.GetAsync("/").GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Clean Up
            httpStubServer.Dispose();
        }

        [Test]
        public void DemoTest()
        {
            // Arrange
            IHttpStubServer httpStubServer = new HttpStubServer(new Uri("http://localhost:5000"));

            httpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/JohnSmith"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000")};

            // Act
            var httpResponseMessage = httpClient.GetAsync("/JohnSmith")
                .GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Clean Up
            httpStubServer.Dispose();
        }

        [Test]
        public void HighFilterTest()
        {
            // Arrange
            IHttpStubServer httpStubServer = new HttpStubServer(new Uri("http://localhost:5000"));

            httpStubServer
                .Setup(message => message.RequestUri.PathAndQuery.Equals("/account/signup") &&
                                  message.Method.Equals(HttpMethod.Post) &&
                                  message.Content.Headers.ContentType.MediaType
                                      .Equals("application/x-www-form-urlencoded") &&
                                  message.Content.ReadAsStringAsync()
                                      .GetAwaiter().GetResult()
                                      .Equals(
                                          "email=JohnSmith%40example.com&password=P%40ssword1&confirmPassword=P%40ssword1"))
                .Returns(() => new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Headers = {Location = new Uri("http://location:5000/account/1")}
                });

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };

            // Act
            var httpResponseMessage = httpClient.PostAsync("account/signup", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", "JohnSmith@example.com"),
                new KeyValuePair<string, string>("password", "P@ssword1"),
                new KeyValuePair<string, string>("confirmPassword", "P@ssword1")
            })).GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(httpResponseMessage.Headers.Location, Is.EqualTo(new Uri("http://location:5000/account/1")));

            // Clean Up
            httpStubServer.Dispose();
        }

        [Test]
        public void AsyncTests()
        {
            // Arrange
            IHttpStubServer httpStubServer = new HttpStubServer(new Uri("http://localhost:5000"));

            httpStubServer
                .Setup(message => true)
                .Returns(async () =>
                {
                    await Task.Delay(50);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000")};

            // Act
            var httpResponseMessage = httpClient.GetAsync("/").GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Clean Up
            httpStubServer.Dispose();
        }

        [Test]
        public void AsyncCancellationTokenTests()
        {
            // Arrange
            IHttpStubServer httpStubServer = new HttpStubServer(new Uri("http://localhost:5000"));

            httpStubServer
                .Setup(message => true)
                .Returns(async cancellationToken =>
                {
                    await Task.Delay(50, cancellationToken);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            var httpClient = new HttpClient {BaseAddress = new Uri("http://localhost:5000")};

            // Act
            var httpResponseMessage = httpClient.GetAsync("/").GetAwaiter().GetResult();

            // Assert
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Clean Up
            httpStubServer.Dispose();
        }
    }
}
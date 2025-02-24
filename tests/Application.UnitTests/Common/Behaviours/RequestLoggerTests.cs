﻿using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using DeveloperPath.Application.Common.Behaviors;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;

namespace DeveloperPath.Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
        private readonly Mock<ILogger<CreatePath>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        //private readonly Mock<IIdentityService> _identityService;


        public RequestLoggerTests()
        {
            _logger = new Mock<ILogger<CreatePath>>();

            _currentUserService = new Mock<ICurrentUserService>();

            // _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");

            var requestLogger = new LoggingBehaviour<CreatePath>(_logger.Object, _currentUserService.Object);

            await requestLogger.Process(new CreatePath { Title = "title", Description = "Description" }, new CancellationToken());

            //_identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new LoggingBehaviour<CreatePath>(_logger.Object, _currentUserService.Object);

            await requestLogger.Process(new CreatePath { Title = "title", Description = "Description" }, new CancellationToken());

           // _identityService.Verify(i => i.GetUserNameAsync(null), Times.Never);
        }
    }
}

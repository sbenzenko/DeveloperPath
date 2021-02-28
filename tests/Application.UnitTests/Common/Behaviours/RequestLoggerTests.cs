﻿using DeveloperPath.Application.Common.Behaviours;
using DeveloperPath.Application.Common.Interfaces;
using DeveloperPath.Application.Paths.Commands.CreatePath;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperPath.Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
        private readonly Mock<ILogger<CreatePathCommand>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;
        private readonly Mock<IIdentityService> _identityService;


        public RequestLoggerTests()
        {
            _logger = new Mock<ILogger<CreatePathCommand>>();

            _currentUserService = new Mock<ICurrentUserService>();

            _identityService = new Mock<IIdentityService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");

            var requestLogger = new LoggingBehaviour<CreatePathCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreatePathCommand { Title = "title", Description = "Description" }, new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new LoggingBehaviour<CreatePathCommand>(_logger.Object, _currentUserService.Object, _identityService.Object);

            await requestLogger.Process(new CreatePathCommand { Title = "title", Description = "Description" }, new CancellationToken());

            _identityService.Verify(i => i.GetUserNameAsync(null), Times.Never);
        }
    }
}

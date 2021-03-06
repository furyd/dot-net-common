using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReservoirDevs.Common.Extensions.Logging;
using Xunit;

namespace ReservoirDevs.Common.Extensions.Tests.Logging.ILoggerExtensions
{
    // ReSharper disable once InconsistentNaming
    public class ILoggerExtensionsTests
    {
        private readonly Mock<ILogger<ILoggerExtensionsTests>> _logger;

        public ILoggerExtensionsTests()
        {
            _logger = new Mock<ILogger<ILoggerExtensionsTests>>();
        }

        [Fact]
        public void BeginScope_ShouldReturnIDisposableObject()
        {
            _logger.Setup(logger => logger.BeginScope(It.IsAny<IDictionary<string, string>>())).Returns(new Mock<IDisposable>().Object);

            var sut = _logger.Object.CreateScope(nameof(BeginScope_ShouldReturnIDisposableObject));

            sut.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public void BeginScope_ShouldCallBeginScopeWithCorrectData()
        {
            _logger.Setup(logger => logger.BeginScope(It.IsAny<IDictionary<string, string>>())).Returns(new Mock<IDisposable>().Object);

            var data = new Dictionary<string, string>
            {
                {"class", typeof(ILoggerExtensionsTests).FullName}, {"method", nameof(BeginScope_ShouldCallBeginScopeWithCorrectData)}
            };

            var sut = _logger.Object.CreateScope(nameof(BeginScope_ShouldCallBeginScopeWithCorrectData));

            _logger.Verify(logger => logger.BeginScope(It.Is<IDictionary<string, string>>(dictionary => dictionary.SequenceEqual(data))), Times.Once);
        }

        [Fact]
        public void BeginScope_ShouldCallBeginScopeWithCorrectData_WhenPassesExtraInformation()
        {
            _logger.Setup(logger => logger.BeginScope(It.IsAny<IDictionary<string, string>>())).Returns(new Mock<IDisposable>().Object);

            var extraData = new Dictionary<string, string>
            {
                { "a", "1" },
                { "b", "2" }
            };

            var data = new Dictionary<string, string>
            {
                {"class", typeof(ILoggerExtensionsTests).FullName}, {"method", nameof(BeginScope_ShouldCallBeginScopeWithCorrectData_WhenPassesExtraInformation)}
            };

            var expected = new Dictionary<string, string>();

            extraData.ToList().ForEach(item => expected.Add(item.Key, item.Value));
            data.ToList().ForEach(item => expected.Add(item.Key, item.Value));

            var sut = _logger.Object.CreateScope(nameof(BeginScope_ShouldCallBeginScopeWithCorrectData_WhenPassesExtraInformation), extraData);

            _logger.Verify(logger => logger.BeginScope(It.Is<IDictionary<string, string>>(dictionary => dictionary.SequenceEqual(expected))), Times.Once);
        }
    }
}

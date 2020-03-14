using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Moq;
using Xunit;

namespace ReservoirDevs.FeatureManagement.Tests.ClassSwitcher
{
    public class ConstructorTests
    {
        private readonly Mock<IFeatureManagerSnapshot> _featureManager;
        private readonly Mock<IOptions<ClassSwitcherOptions<IDisposable>>> _options;
        private readonly Mock<ILogger<ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>>> _logger;

        public ConstructorTests()
        {
            _featureManager = new Mock<IFeatureManagerSnapshot>();
            _options = new Mock<IOptions<ClassSwitcherOptions<IDisposable>>>();
            _logger = new Mock<ILogger<ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>>>();
        }

        private static IEnumerable<string> GetClassSwitchParameterNames => typeof(ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>).GetConstructors().First().GetParameters().Select(parameterInfo => parameterInfo.Name);

        private static IEnumerable<IDisposable> InvalidClasses => new List<IDisposable>
        {
            new HttpClientHandler(),
            new HttpClientHandler()
        };

        private static IEnumerable<IDisposable> ValidClasses => new List<IDisposable>
        {
            new HttpRequestMessage(),
            new HttpClientHandler()
        };

        [Fact]
        public void Constructor_ThrowsException_WhenClassesAreNull()
        {
            Action action = () => new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(null, null, null, null);

            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be(GetClassSwitchParameterNames.First());
        }

        //TODO - add in class to use inline data to range this
        [Fact]
        public void Constructor_ThrowsException_WhenClassesHaveLessThanTwoItems()
        {
            Action action = () => new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(new List<IDisposable>(), null, null, null);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>().And.ParamName.Should().Be(GetClassSwitchParameterNames.First());
        }

        [Fact]
        public void Constructor_ThrowsException_WhenFeatureManagerIsNull()
        {
            Action action = () => new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(InvalidClasses, null, null, null);

            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be(GetClassSwitchParameterNames.Skip(1).First());
        }

        [Fact]
        public void Constructor_ThrowsException_WhenOptionsAreNull()
        {
            Action action = () => new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(InvalidClasses, _featureManager.Object, null, null);

            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be(GetClassSwitchParameterNames.Skip(2).First());
        }

        [Fact]
        public void Constructor_ThrowsException_WhenLoggerIsNull()
        {
            _options.Setup(option => option.Value).Returns(new ClassSwitcherOptions<IDisposable>());

            Action action = () => new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(InvalidClasses, _featureManager.Object, _options.Object, null);

            action.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be(GetClassSwitchParameterNames.Skip(3).First());
        }

        [Fact]
        public void Constructor_ThrowsNoException_WhenPassedValidParameters()
        {
            _options.Setup(option => option.Value).Returns(new ClassSwitcherOptions<IDisposable>());

            Action action = () => new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(ValidClasses, _featureManager.Object, _options.Object, _logger.Object);

            action.Should().NotThrow<Exception>();
        }
    }
}

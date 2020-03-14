using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Moq;
using Xunit;

namespace ReservoirDevs.FeatureManagement.Tests.ClassSwitcher
{
    public class GetActiveClassTests
    {
        private readonly Mock<IFeatureManagerSnapshot> _featureManager;
        private readonly Mock<IOptions<ClassSwitcherOptions<IDisposable>>> _options;
        private readonly Mock<ILogger<ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>>> _logger;

        public GetActiveClassTests()
        {
            _featureManager = new Mock<IFeatureManagerSnapshot>();
            _options = new Mock<IOptions<ClassSwitcherOptions<IDisposable>>>();
            _logger = new Mock<ILogger<ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>>>();

            _options.Setup(option => option.Value).Returns(new ClassSwitcherOptions<IDisposable>{ Flag = "A" });
        }

        private static IEnumerable<IDisposable> ValidClasses => new List<IDisposable>
        {
            new HttpRequestMessage(),
            new HttpClientHandler()
        };

        [Fact]
        public async Task GetActiveClass_ReturnsEnabledClass_WhenFeatureFlagIsActive()
        {
            _featureManager.Setup(manager => manager.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);

            var switcher = new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(ValidClasses, _featureManager.Object, _options.Object, _logger.Object);

            var result = await switcher.GetActiveClass();

            result.Should().BeAssignableTo<HttpRequestMessage>();
        }

        [Fact]
        public async Task GetActiveClass_ReturnsDisabledClass_WhenFeatureFlagIsNotActive()
        {
            _featureManager.Setup(manager => manager.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(false);

            var switcher = new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(ValidClasses, _featureManager.Object, _options.Object, _logger.Object);

            var result = await switcher.GetActiveClass();

            result.Should().BeAssignableTo<HttpClientHandler>();
        }
    }
}
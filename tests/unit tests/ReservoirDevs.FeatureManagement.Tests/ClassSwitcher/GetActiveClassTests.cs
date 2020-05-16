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
        private readonly Mock<IEnumerable<IOptions<ClassSwitcherOptions>>> _options;

        public GetActiveClassTests()
        {
            _featureManager = new Mock<IFeatureManagerSnapshot>();
            _options = new Mock<IEnumerable<IOptions<ClassSwitcherOptions>>>();

            var option = new Mock<IOptions<ClassSwitcherOptions>>();

            option.Setup(option => option.Value).Returns(new ClassSwitcherOptions{ Flag = "A", Interface = typeof(IDisposable)});
            _options.Setup(x => x.GetEnumerator()).Returns(new List<IOptions<ClassSwitcherOptions>>{ option.Object }.GetEnumerator());
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

            var switcher = new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(ValidClasses, _featureManager.Object, _options.Object);

            var result = await switcher.GetActiveClass();

            result.Should().BeAssignableTo<HttpRequestMessage>();
        }

        [Fact]
        public async Task GetActiveClass_ReturnsDisabledClass_WhenFeatureFlagIsNotActive()
        {
            _featureManager.Setup(manager => manager.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(false);

            var switcher = new ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>(ValidClasses, _featureManager.Object, _options.Object);

            var result = await switcher.GetActiveClass();

            result.Should().BeAssignableTo<HttpClientHandler>();
        }
    }
}
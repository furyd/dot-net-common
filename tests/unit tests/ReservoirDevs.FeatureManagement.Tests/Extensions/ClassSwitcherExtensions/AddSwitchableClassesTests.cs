using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using ReservoirDevs.FeatureManagement.Extensions;
using Xunit;

namespace ReservoirDevs.FeatureManagement.Tests.Extensions.ClassSwitcherExtensions
{
    public class AddSwitchableClassesTests
    {
        [Fact]
        public void AddSwitchableClasses_ShouldInjectExpectedServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSwitchableClasses<IDisposable, HttpRequestMessage, HttpClientHandler>("A");
            serviceCollection.AddFeatureManagement();
            serviceCollection.AddSingleton<IConfiguration>(new ConfigurationRoot(new List<IConfigurationProvider>()));

            var services = serviceCollection.BuildServiceProvider();

            var resolved = services.GetServices<IDisposable>()?.ToList();

            resolved.Should().NotBeNull();
            resolved?.Count().Should().Be(2);

            services.GetService<IOptions<ClassSwitcherOptions>>().Should().NotBeNull();
            services.GetService<ClassSwitcher<IDisposable, HttpRequestMessage, HttpClientHandler>>().Should().NotBeNull();
        }
    }
}
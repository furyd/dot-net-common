using System;
using FluentAssertions;
using Xunit;

namespace ReservoirDevs.Common.Models.Tests.Result
{
    public class ResultTests
    {
        [Fact]
        public void IsSuccessful_IsFalse_WhenExceptionIsNotNull()
        {
            var sut = new Models.Result(new Exception());

            sut.Exception.Should().NotBeNull();
            sut.IsSuccessful.Should().BeFalse();
        }

        [Fact]
        public void IsSuccessful_IsTrue_WhenExceptionIsNull()
        {
            var sut = new Models.Result();

            sut.Exception.Should().BeNull();
            sut.IsSuccessful.Should().BeTrue();
        }
    }
}

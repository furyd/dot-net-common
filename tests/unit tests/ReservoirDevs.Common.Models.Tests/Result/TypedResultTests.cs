using System;
using FluentAssertions;
using Xunit;

namespace ReservoirDevs.Common.Models.Tests.Result
{
    public class TypedResultTests
    {
        [Fact]
        public void IsSuccessful_IsFalse_WhenExceptionIsNotNull()
        {
            var sut = new Result<string>(new Exception());

            sut.Exception.Should().NotBeNull();
            sut.IsSuccessful.Should().BeFalse();
        }

        [Fact]
        public void IsSuccessful_IsTrue_WhenExceptionIsNull()
        {
            const string item = "a";
            var sut = new Result<string>(item);

            sut.Exception.Should().BeNull();
            sut.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void HasValue_IsFalse_WhenNoValuePassedInConstructor()
        {
            var sut = new Result<string>(new Exception());

            sut.HasValue.Should().BeFalse();
        }

        [Fact]
        public void HasValue_IsTrue_WhenValuePassedInConstructor()
        {
            const string item = "a";
            var sut = new Result<string>(item);

            sut.HasValue.Should().BeTrue();
        }

        [Fact]
        public void Item_IsPopulatedCorrectly_WhenValuePassedInConstructor()
        {
            const string item = "a";
            var sut = new Result<string>(item);

            sut.Item.Should().NotBeNull();
            sut.Item.GetType().Should().Be(item.GetType());
        }

        [Fact]
        public void HasValue_IsTrue_WhenValueAnExceptionPassedInConstructor()
        {
            const string item = "a";
            var sut = new Result<string>(item, new Exception());

            sut.HasValue.Should().BeTrue();
        }

        [Fact]
        public void IsSuccessful_IsTrue_WhenValueAnExceptionPassedInConstructor()
        {
            const string item = "a";
            var sut = new Result<string>(item, new Exception());

            sut.IsSuccessful.Should().BeFalse();
        }
    }
}
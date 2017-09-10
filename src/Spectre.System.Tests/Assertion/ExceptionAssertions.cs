using System;
using Shouldly;

// ReSharper disable once CheckNamespace
namespace Spectre.System.Tests
{
    public static class ExceptionAssertions
    {
        public static void ShouldBeArgumentNullException(
            this Exception exception,
            string name)
        {
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ArgumentNullException>()
                .And().ParamName.ShouldBe(name);
        }
    }
}
using System;
using Shouldly;

// ReSharper disable once CheckNamespace
namespace Spectre.System.Tests
{
    public static class ExceptionAssertions
    {
        public static void ShouldBeArgumentException(this Exception exception, string name, string message)
        {
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ArgumentException>()
                .And(ex => ex.ParamName.ShouldBe(name))
                .And(ex => ex.Message?.SplitLines()[0].ShouldBe(message));
        }
        
        public static void ShouldBeArgumentNullException(this Exception exception, string name)
        {
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ArgumentNullException>()
                .And().ParamName.ShouldBe(name);
        }
    }
}
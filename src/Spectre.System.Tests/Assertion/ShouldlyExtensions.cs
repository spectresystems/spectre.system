using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Spectre.System.IO;

// ReSharper disable once CheckNamespace
namespace Spectre.System.Tests
{
    public static class ShouldlyExtensions
    {
        public static T And<T>(this T obj)
        {
            return obj;
        }
        
        public static T And<T>(this T obj, Action<T> expression)
        {
            expression(obj);
            return obj;
        }
    }
}
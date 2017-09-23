// Licensed to Spectre Systems AB under one or more agreements.
// Spectre Systems AB licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

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
﻿using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SkypeBot.Extensions
{
    public static class HeaderDictionaryExtensions
    {
        public static string ToDetailString(this IHeaderDictionary header) 
        {
            var value = header.Keys
                .Select(key => $"{key}: {header[key]}")
                .ToArray();

            return value.Length > 0 ? string.Join("\r\n", value) : string.Empty;
        }
    }
}

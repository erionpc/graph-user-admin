using System;
using System.Text.Json;

namespace B2CUserAdmin.UI.Extensions
{
    internal static class GenericExtensions
    {
        public static T Clone<T>(this T source) where T : notnull
        {
            var serializedObject = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<T>(serializedObject) ?? throw new NullReferenceException("Error trying to clone object, deserialized object was null.");
        }
    }
}
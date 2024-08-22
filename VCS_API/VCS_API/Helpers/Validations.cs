using System.Reflection;

namespace VCS_API.Helpers
{
    public static class Validations
    {
        /// <summary>
        /// Validates property values for null, empty or whitespace. It skips enumerable objects. Recommended way of passing exempted properties is by using the nameof() feature of C#
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exemptedProperties"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ValidateProperties(object? obj, HashSet<string>? exemptedProperties = null)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if(obj is string)
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(obj.ToString());
            }

            // Get all properties of the object
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Skip exempted properties
                if (exemptedProperties != null && exemptedProperties.Contains(property.Name))
                    continue;

                // Get the value of the property
                var value = property.GetValue(obj);

                // Convert the value to a string and check if it's null, empty, or whitespace
                if (value is IEnumerable<object> or IEnumerable<string>)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(value?.ToString()))
                {
                    throw new ArgumentException($"The property '{property.Name}' cannot be null, empty, or whitespace.");
                }
            }
        }

        public static void ThrowIfNullOrWhiteSpace(params string?[]? strings)
        {
            if (strings is null) return;

            foreach (var item in strings)
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(item);
            }
        }
    }
}

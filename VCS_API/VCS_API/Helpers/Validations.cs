namespace VCS_API.Helpers
{
    public static class Validations
    {
        public static void ThrowIfInvalidName(string? name, int maxLength, int minLength = 1)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > maxLength || name.Length < minLength)
            {
                throw new InvalidDataException($"Name should not be null, empty or whitespace. Also the length should be between or including {minLength} and {maxLength}");
            }

            HashSet<char> prohibitedCharacters = ['#', '@', '$', '%', '^', '&', '*', '(', ')', '!', '~', '\"', '/', '\\', '|', '`', '=', '+', '?', ',', '>', '<', '[', ']', '{', '}', ';', ':', '\'', ' ', '\n', '\t'];
            foreach (char item in name)
            {
                if (prohibitedCharacters.Contains(item))
                {
                    throw new InvalidDataException("No special characters other than a period ('.') is allowed in the name.");
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

        public static void ThrowIfNull(params object?[]? strings)
        {
            if (strings is null) return;

            foreach (var item in strings)
            {
                ArgumentNullException.ThrowIfNull(item);
            }
        }
    }
}

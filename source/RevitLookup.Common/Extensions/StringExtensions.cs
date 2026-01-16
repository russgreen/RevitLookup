using JetBrains.Annotations;

namespace RevitLookup.Common.Extensions;

/// <summary>
/// Provides extension methods for string collections and arrays.
/// </summary>
[PublicAPI]
public static class StringExtensions
{
    /// <param name="source">The collection or array of strings to join. Null strings are treated as empty strings.</param>
    extension(IEnumerable<string?> source)
    {
        /// <summary>
        /// Joins the elements of the provided string collection or array into a single string, separated by the specified separator.
        /// </summary>
        /// <param name="separator">The string or character to use as a separator between the joined elements.</param>
        /// <returns>A single concatenated string consisting of the elements in the source collection or array, separated by the specified separator.</returns>
        public string Join(string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Joins the elements of the provided string collection or array into a single string, separated by the specified separator character.
        /// </summary>
        /// <param name="separator">The character to use as a separator between the joined elements.</param>
        /// <returns>A single concatenated string consisting of the elements in the source collection or array, separated by the specified separator character.</returns>
        public string Join(char separator)
        {
            return string.Join(separator, source);
        }
    }

    /// <param name="source">The array of strings to join. Null strings are treated as empty strings.</param>
    extension(string[] source)
    {
        /// <summary>
        /// Joins the elements of the provided string array into a single string, separated by the specified separator.
        /// </summary>
        /// <param name="separator">The string to use as a separator between the joined elements.</param>
        /// <returns>A single concatenated string consisting of the elements in the array, separated by the specified separator.</returns>
        public string Join(string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Joins the elements of the provided string collection or array into a single string, separated by the specified character separator.
        /// </summary>
        /// <param name="separator">The character to use as a separator between the joined elements.</param>
        /// <returns>A single concatenated string consisting of the elements in the source collection or array, separated by the specified character separator.</returns>
        public string Join(char separator)
        {
            return string.Join(separator, source);
        }
    }
}
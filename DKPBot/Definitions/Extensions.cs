using System;
using System.Collections.Generic;
using System.Linq;

namespace DKPBot.Definitions
{
    internal static class Extensions
    {
        internal static bool ContainsI(this IEnumerable<string> enumerable, string str) =>
            enumerable.Contains(str, StringComparer.OrdinalIgnoreCase);

        internal static bool ContainsI(this string str1, string str2) => str1.IndexOf(str2, StringComparison.OrdinalIgnoreCase) != -1;

        internal static bool EqualsI(this string str1, string str2) => str1.Equals(str2, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///     Checks if a string is a valid URI.
        /// </summary>
        /// <param name="str">A string object.</param>
        internal static bool IsValidURI(this string str) =>
            !string.IsNullOrWhiteSpace(str) && Uri.TryCreate(str, UriKind.Absolute, out var result) && result.Scheme.Contains("http");

        /// <summary>
        ///     Converts a timespan into a more easily readable string.
        /// </summary>
        /// <param name="timeSpan">A timespan object.</param>
        internal static string ToStringF(this TimeSpan timeSpan)
        {
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            var seconds = timeSpan.Seconds;

            return $"{(hours == 0 ? string.Empty : $"{hours}:")}{minutes}:{seconds:D2}";
        }

        /// <summary>
        ///     Calculates the pixel width of a string.
        /// </summary>
        /// <param name="str">A string object.</param>
        internal static float MeasureString(this string str) =>
            CONSTANTS.GRAPHICS.MeasureString(str, CONSTANTS.WHITNEY_FONT)
                .Width;

        /// <summary>
        ///     Normalizes the widths of all strings in the collection by adding spaces to shorter strings so that they match the
        ///     pixel width of longer strings.
        /// </summary>
        /// <param name="strings">An IEnumerable of strings.</param>
        /// <param name="alignment">The type of alignment to perform while normalizing widths.</param>
        internal static IEnumerable<string> NormalizeWidth(this IEnumerable<string> strings, TextAlignment alignment)
        {
            var enumerable = strings as string[] ?? strings.ToArray();
            var widths = enumerable.Select(str => str.MeasureString())
                .ToArray();
            var maxWidth = widths.Max();

            for (var i = 0; i < enumerable.Length; i++)
            {
                var str = enumerable[i];
                var width = widths[i];

                if (width < maxWidth)
                {
                    var spacesToAdd = (int) Math.Round((maxWidth - width) / CONSTANTS.SPACE_LENGTH, MidpointRounding.AwayFromZero);

                    switch (alignment)
                    {
                        case TextAlignment.LeftAlign:
                            yield return string.Create(spacesToAdd + str.Length, str, (chars, state) =>
                            {
                                state.AsSpan()
                                    .CopyTo(chars);

                                var position = str.Length;

                                for (var x = 0; x < chars.Length - str.Length; x++)
                                    chars[position++] = ' ';
                            });
                            break;
                        case TextAlignment.Center:
                            yield return string.Create(spacesToAdd + str.Length, str, (chars, state) =>
                            {
                                var position = 0;
                                var spacesPerSide = (int) Math.Round((chars.Length - (float) str.Length) / 2, MidpointRounding.ToEven);

                                for (; position < spacesPerSide; position++)
                                    chars[position] = ' ';

                                state.AsSpan()
                                    .CopyTo(chars.Slice(position));

                                position += state.Length;

                                for (; position < chars.Length; position++)
                                    chars[position] = ' ';
                            });
                            break;
                        case TextAlignment.RightAlign:
                            yield return string.Create(spacesToAdd + str.Length, str, (chars, state) =>
                            {
                                var position = 0;

                                for (; position < chars.Length - str.Length; position++)
                                    chars[position] = ' ';

                                state.AsSpan()
                                    .CopyTo(chars.Slice(position));
                            });
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
                    }
                } else
                    yield return str;
            }
        }

        internal static bool TrySwap<TItem>(this IList<TItem> list, int index1, int index2)
        {
            if (index1 > list.Count || index2 > list.Count || index1 < 0 || index2 < 0)
                return false;

            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return true;
        }
    }
}
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using NanoidDotNet;

namespace CourseService.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string ToSlug(this string value)
        {
            string slug = value.ToLowerInvariant();

            slug = slug.RemoveAccents();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"[\s-]+", "-");
            slug = slug.Trim('-');

            return slug;
        }

        public static string GenerateNumericIdString(int length)
        {
            return Nanoid.Generate(Nanoid.Alphabets.Digits, length);
        }

        public static string GenerateIdString(int length)
        {
            return Nanoid.Generate(Nanoid.Alphabets.LettersAndDigits, length);
        }

        private static string RemoveAccents(this string value)
        {
            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

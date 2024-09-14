using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CourseService.Domain.Utils
{
    public static class StringUtils
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
            int rangeStart = (int)Math.Pow(10, length - 1);
            int rangeEnd = (int)Math.Pow(10, length) - 1;

            byte[] randomBytes = new byte[4]; // 4 bytes = 32-bit int
            RandomNumberGenerator.Fill(randomBytes);
            int randomValue = BitConverter.ToInt32(randomBytes, 0) & int.MaxValue;

            var result = rangeStart + (randomValue % (rangeEnd - rangeStart + 1));

            return result.ToString();
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

using CourseService.Domain.Exceptions;
using System.Globalization;

namespace CourseService.Domain.Objects
{
    public class Locale
    {
        public string Name { get; private set; }
        public string EnglishTitle { get; private set; }

        public Locale(string locale)
        {
            try
            {
                var culture = new CultureInfo(locale);

                Name = culture.Name;
                EnglishTitle = culture.EnglishName;
            }
            catch (CultureNotFoundException)
            {
                throw new DomainValidationException($"Locale {locale} is not valid");
            }
        }
    }
}

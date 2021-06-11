using System;

namespace ReportingReflection
{
    class Program
    {
        static void Main()
        {
            new CSVGenerator<Book>(BookData.Books, "Books").Generate();
            new CSVGenerator<Weather>(WeatherData.Weather, "Weather").Generate();
        }
    }
}

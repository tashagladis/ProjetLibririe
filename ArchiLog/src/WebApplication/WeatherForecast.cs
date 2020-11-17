using APILibrary.Core.Attributs;
using System;

namespace WebApplication
{
    public class WeatherForecast
    {
        [NotJson]
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [NotJson]
        public string Summary { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFixtureDemo.Models
{
   public class Weather
    {
        public Weather(DateTime time, float temperature)
        {
            Time = time;
            Temperature = temperature;
        }

        public DateTime Time { get; set; }
        public float Temperature { get; set; }
    }
}

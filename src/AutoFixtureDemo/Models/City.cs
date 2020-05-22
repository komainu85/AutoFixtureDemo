using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFixtureDemo.Models
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
        public int TouristRating { get; set; }
        public DateTime DateEstablished { get; set; }
        public int EstimatedPopulation { get; set; }
    }
}

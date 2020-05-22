using System;
using System.Collections.Generic;
using System.Text;

namespace AutoFixtureDemo.Models
{
    public class Country
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<string> States { get; set; } = new List<string>();
    }
}

using System;
using System.Collections.Generic;

namespace Digify
{
	public class Dam
	{
        public string Name {get; set;}
        public Location Location {get; set;}
        public Reservoir Reservoir {get; set;}
        public double DischargeCap {get; set;}
        public List<DamStation> Stations;



        public Dam(string name, Location location, Reservoir reservoir, double dischargeCap, 
            List<DamStation> stations)
        {
            this.Name = name;
            this.Location = location;
            this.Reservoir = reservoir;
            this.DischargeCap = dischargeCap;
            this.Stations = stations;
        }
	}
}
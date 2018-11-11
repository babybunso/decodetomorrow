using System;
using System.Collections.Generic;

namespace Digify
{
	public class DamStation
	{
		public int Id {get; set;}
		public string Name {get; set;}
		public Location Location {get; set;}
		public List<Forecast> Forecasts {get; set;}

		public DamStation(int id, string name, Location location, List<Forecast> forecasts)
		{
			this.Id = id;
			this.Name = name;
			this.Location = location;
			this.Forecasts = forecasts;
		}
	}
}
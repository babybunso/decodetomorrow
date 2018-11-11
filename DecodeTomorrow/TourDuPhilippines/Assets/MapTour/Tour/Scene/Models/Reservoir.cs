using System;
using System.Collections.Generic;

namespace Digify
{
	public class Reservoir
	{
        public double StorageCapacity {get; set;}
        public double MinimumSupplyLevel {get; set;}
        public double MaximumFloodLevel {get; set;}


        public Reservoir(double storageCapacity, double minimumSupplyLevel, double maximumFloodLevel)
        {
            this.StorageCapacity = storageCapacity;
            this.MinimumSupplyLevel = minimumSupplyLevel;
            this.MaximumFloodLevel = maximumFloodLevel;
        }
	}
}
using System;
using System.Collections.Generic;

namespace Digify
{
	public class Reservoir
	{
        public double StorageCapacity {get; set;}
        public double MinimumSupplyLevel {get; set;}
        public double MaximumSupplyLevel {get; set;}


        public Reservoir(double storageCapacity, double minimumSupplyLevel, double maximumSupplyLevel)
        {
            this.StorageCapacity = storageCapacity;
            this.MinimumSupplyLevel = minimumSupplyLevel;
            this.MaximumSupplyLevel = maximumSupplyLevel;
        }
	}
}
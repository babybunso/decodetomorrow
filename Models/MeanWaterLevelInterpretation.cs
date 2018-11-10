using System;
using System.Collections.Generic;

namespace Digify
{
	public class MeanWaterLevelInterpretation
	{
        private double minWaterLevel;
        private double maxWaterLevel;
        //private double capacity;
        private double waterLevel;

        public MeanWaterLevelInterpretation(Reservoir reservoir, double waterLevel) 
        {
            this.minWaterLevel = reservoir.MinimumSupplyLevel;
            this.maxWaterLevel = reservoir.MaximumSupplyLevel;
            this.waterLevel = waterLevel;
            //this.capacity = reservoir.StorageCapacity;
        }

        public string Weather()
        {
            return waterLevel + " Meters";
        }


        public string Interpret()
        {
            string interpretation = "";

            if (waterLevel >= (maxWaterLevel *1.25))
                interpretation = "Critical - Above Maximum Water Level";
            else if (waterLevel > maxWaterLevel * 0.9)
                interpretation = "Warning";
            else if (waterLevel >= minWaterLevel)
                interpretation = "Normal";
            else
                interpretation = "Critical - Below Minimum Water Level";

            return interpretation;        
        }
    }
}

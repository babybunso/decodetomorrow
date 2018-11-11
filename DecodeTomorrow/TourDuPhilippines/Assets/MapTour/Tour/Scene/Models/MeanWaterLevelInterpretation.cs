using System;
using System.Collections.Generic;

namespace Digify
{
	public class MeanWaterLevelInterpretation
	{
        private double minWaterLevel;
        private double maxFloodLevel;
        private double capacity;
        private double waterLevel;

        public MeanWaterLevelInterpretation(Dam dam, double waterLevel) 
        {
            this.minWaterLevel = dam.Reservoir.MinimumSupplyLevel;
            this.maxFloodLevel = dam.Reservoir.MaximumFloodLevel;
            this.capacity = dam.Reservoir.StorageCapacity;
            this.waterLevel = waterLevel;
        }

        public string Weather()
        {
            return waterLevel + " cubic meters";
        }


        public string Interpret()
        {
            string interpretation = "";

            if (waterLevel >= (capacity *1.25))
                interpretation = "Critical - Above Maximum Water Level";
            else if (waterLevel >= capacity * 0.95)
                interpretation = "Warning";
            else
               interpretation = "Normal";

            return interpretation;        
        }
    }
}

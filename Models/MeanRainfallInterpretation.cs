using System;
using System.Collections.Generic;

namespace Digify
{
	public class MeanRainfallInterpretation
	{
        private double rain;

        public MeanRainfallInterpretation(Rainfall rainfall) 
        {
            this.rain = rainfall.Amount;
        }

        public string Weather()
        {
            return rain + " Millimeters";
        }


        public string Interpret()
        {
            string interpretation = "";

            if (rain >= 20)
                interpretation = "Extreme Rain";
            else if (rain >= 10)
                interpretation = "Heavy Rain";
            else if (rain >= 5)
                interpretation = "Moderate Rain";
            else if (rain >= 1)
                interpretation = "Light Rain";
            else if (rain > 0.1)
                interpretation = "Drizzle";
            else
                interpretation = "Sunny";

            return interpretation;        
        }
    }
}

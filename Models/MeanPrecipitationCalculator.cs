using System;
using System.Collections.Generic;

namespace Digify
{
	public class MeanPrecipitationCalculator
	{
        public Dictionary<int, Dam> Dams;

        public MeanPrecipitationCalculator(Dictionary<int, Dam> dams)
        {            
            this.Dams = dams;
        }

        public Dictionary<int, Rainfall> GetRainFall(int damId)
        {
            Dam dam = getDam(damId);
            return ComputeRainFall(dam);
        }

        public Dictionary<int, Rainfall> GetWaterLevel(int damId)
        {
            Dam dam = getDam(damId);
            // return ComputeWaterLevel(dam);
            return ComputeRainFall(dam); // from mmm to m
        }

        private Dam getDam(int id)
        {
            return this.Dams[id];
        }

        private Dictionary<int, Rainfall> ComputeRainFall(Dam dam)
        {
            Dictionary<int, Rainfall> result = new Dictionary<int, Rainfall>();

            Dictionary<int, List<Rainfall>> stations = new Dictionary<int, List<Rainfall>>();
            foreach (DamStation station in dam.Stations)
            {
                int id = station.Id;
                List<Rainfall> rainFalls = new List<Rainfall>();

                foreach (Forecast forecast in station.Forecasts)
                {
                    Rainfall rainfall = new Rainfall(forecast.Timestamp, forecast.Rain);
                    rainFalls.Add(rainfall);
                }

                stations.Add(id, rainFalls);
            }

            foreach (int key in stations.Keys)
            {
                List<Rainfall> rainfalls = stations[key];
                double rains = 0;

                DateTime ts = new DateTime();
                foreach (Rainfall rainfall in rainfalls)
                {   
                    rains += rainfall.Amount;
                    ts = rainfall.DateTime;
                }

                double meanPrecipitation = rains / rainfalls.Count; 
                Rainfall newRainfall = new Rainfall(ts, meanPrecipitation);    
                result.Add(key, newRainfall);       
            }

            return result;
        }

        /* 
        private Dictionary<int, double> ComputeWaterLevel(Dam dam)
        {
            Dictionary<int, double> result = new Dictionary<int, double>();
            foreach (int key in rainfalls.Keys)
            {
                Rainfall rainfall = rainfalls[key];
                double volume = RainfallToVolume(rainfall.Amount);
                double waterLevel = volume - dam.DischargeCap;
                result.Add(key, waterLevel);       
            }
            return result;
        }

        private double RainfallToVolume(double rainAmount)
        {
            double volume = rainAmount;
            return volume;
        }
        */
	}
}
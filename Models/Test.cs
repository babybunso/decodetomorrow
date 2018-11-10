using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Digify
{
	public class Test
	{
        public static void Main()
        {
            Dictionary<int, Dam> dams = new Dictionary<int, Dam>();

            Location damLocation = new Location(16.9040145, 121.3094922, 12/*11.75 */);
            double dischargeCap = 30600; // mmm
            double storageCapacity = 1080000000; // 1.8B mmm
            double minSupply = 160; // masl
            double maxSupply = 193; // masl
            Reservoir reservoir = new Reservoir(storageCapacity, minSupply, maxSupply);

            // STATIONS, FORECASTS
            List<Forecast> forecasts = new List<Forecast>();
            Forecast forecast = new Forecast(DateTime.Now, 28.65,36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);    
            forecasts.Add(forecast);
            forecast = new Forecast(DateTime.Now.AddHours(1), 28.65,36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);    
            forecasts.Add(forecast);

            Location stationLocation = new Location(11.3581, 120.7242, 26);
            List<DamStation> stations = new List<DamStation>();
            DamStation station = new DamStation(980001, "Pamalican (Amanpulo) 1", stationLocation, forecasts); 
            stations.Add(station);
            station = new DamStation(980002, "Pamalican (Amanpulo) 2", stationLocation, forecasts); 
            stations.Add(station);

            Dam dam  = new Dam("Magat", damLocation, reservoir, dischargeCap, stations);
            dams.Add(1, dam);


            damLocation = new Location(16.4587389, 120.7430438, 17);
            dischargeCap = 30000; // mmm
            storageCapacity = 121000000; // 121M mmm
            minSupply = 740; // masl
            maxSupply = 752; // masl
            reservoir = new Reservoir(storageCapacity, minSupply, maxSupply);
            
            // STATIONS, FORECASTS
            forecasts = new List<Forecast>();
            forecast = new Forecast(DateTime.Now.AddHours(2), 28.65,36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);    
            forecasts.Add(forecast);
            forecast = new Forecast(DateTime.Now.AddHours(3), 28.65,36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);    
            forecasts.Add(forecast);

            stationLocation = new Location(11.3581, 120.7242, 26);
            stations = new List<DamStation>();
            station = new DamStation(980003, "Pamalican (Amanpulo) 1", stationLocation, forecasts); 
            stations.Add(station);
            station = new DamStation(980004, "Pamalican (Amanpulo) 2", stationLocation, forecasts); 
            stations.Add(station);

            dam  = new Dam("Ambuklao", damLocation, reservoir, dischargeCap, stations);
            dams.Add(2, dam);
            
            
            damLocation = new Location(16.3948684, 120.7279512, 17);
            dischargeCap = 10521; // mmm
            storageCapacity = 21000000; // 121M mmm
            minSupply = 757; // masl
            maxSupply = 566; // masl
            reservoir = new Reservoir(storageCapacity, minSupply, maxSupply);

            // STATIONS, FORECASTS
            forecasts = new List<Forecast>();
            forecast = new Forecast(DateTime.Now.AddHours(4), 28.65,36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);    
            forecasts.Add(forecast);
            forecast = new Forecast(DateTime.Now.AddHours(5), 28.65,36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);    
            forecasts.Add(forecast);

            stationLocation = new Location(11.3581, 120.7242, 26);
            stations = new List<DamStation>();
            station = new DamStation(980003, "Pamalican (Amanpulo) 1", stationLocation, forecasts); 
            stations.Add(station);
            station = new DamStation(980004, "Pamalican (Amanpulo) 2", stationLocation, forecasts); 
            stations.Add(station);

            dam  = new Dam("Binga", damLocation, reservoir, dischargeCap, stations);
            dams.Add(3, dam);


            MeanPrecipitationCalculator meanPrecipitation = new MeanPrecipitationCalculator(dams);
     
            foreach (int damId in dams.Keys)
            {
                Console.WriteLine("\nDam Id: " + damId + " Name: " + dams[damId].Name);

                Dictionary<int, Rainfall> meanRainfalls = meanPrecipitation.GetRainFall(damId);
                foreach (int key in meanRainfalls.Keys)
                {
                    Rainfall rainfall = meanRainfalls[key];
                    Console.WriteLine("Station Id: " + key 
                        + ", Timestamp: " + rainfall.DateTime + ", Mean Rainfall: " + rainfall.Amount
                        + ", Interpretation: " + new MeanRainfallInterpretation(rainfall).Interpret()
                    );
                }
            
                Dictionary<int, Rainfall> meanWaterLevel = meanPrecipitation.GetWaterLevel(damId);
                foreach (int key in meanWaterLevel.Keys)
                {
                    Rainfall rainfall = meanWaterLevel[key];
                    double observedDamWaterAmount = 600;
                    double waterLevel = rainfall.Amount + observedDamWaterAmount;

                    Console.WriteLine("Station Id: " + key 
                        + ", Timestamp: " + rainfall.DateTime + ", Mean Water Level: " + waterLevel 
                        + ", Interpretation: " + new MeanWaterLevelInterpretation(
                            dams[damId].Reservoir, waterLevel).Interpret()
                    );
                }
            }

        }
    }
}
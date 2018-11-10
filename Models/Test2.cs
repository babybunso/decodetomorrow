using System;
using System.Collections.Generic;

namespace Digify
{
	public class Test2
	{
		public static void Main2()
		{
            DataStoreFactory factory = new DataStoreFactory();

            Location location = new Location(11.3581, 120.7242, 26);
            Forecast forecast = new Forecast(new DateTime(), 28.65,36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);    

            StationDataStore stations = (StationDataStore) factory.Create(StoreType.STATION);
            Station station = new Station(980001, "Pamalican (Amanpulo)", location, forecast);            
            stations.Add(station); 
            Console.WriteLine(stations.Size().ToString());

            ForecastDataStore forecasts = (ForecastDataStore) factory.Create(StoreType.FORECAST);
            forecasts.Location = location;
            forecasts.Add(forecast);
            Console.WriteLine(forecasts.Size().ToString());

            string weatherInterpretation = WeatherInterpretationFactory.Interpret(forecast, WeatherType.HEAT_INDEX);
            Console.WriteLine(forecast.HeatIndex + " = " + weatherInterpretation);

            weatherInterpretation = WeatherInterpretationFactory.Interpret(forecast, WeatherType.RAIN);
            Console.WriteLine(forecast.Rain + " = " + weatherInterpretation);

            weatherInterpretation = WeatherInterpretationFactory.Interpret(forecast, WeatherType.RAIN_PROBABILITY);
            Console.WriteLine(forecast.RainProbability + " = " + weatherInterpretation);

            weatherInterpretation = WeatherInterpretationFactory.Interpret(forecast, WeatherType.TEMPERATURE);
            Console.WriteLine(forecast.Temperature + " = " + weatherInterpretation);

            weatherInterpretation = WeatherInterpretationFactory.Interpret(forecast, WeatherType.TOTAL_CLOUD_COVER);
            Console.WriteLine(forecast.TotalCloudCover + " = " + weatherInterpretation);

            weatherInterpretation = WeatherInterpretationFactory.Interpret(forecast, WeatherType.WIND_DIRECTION);
            Console.WriteLine(forecast.WindDirection + " = " + weatherInterpretation);

            weatherInterpretation = WeatherInterpretationFactory.Interpret(forecast, WeatherType.WIND_GUST);
            Console.WriteLine(forecast.WindGust + " = " + weatherInterpretation);

            weatherInterpretation = WeatherInterpretationFactory.Interpret(forecast, WeatherType.WIND_SPEED);
            Console.WriteLine(forecast.WindSpeed + " = " + weatherInterpretation);
		}
    }
}
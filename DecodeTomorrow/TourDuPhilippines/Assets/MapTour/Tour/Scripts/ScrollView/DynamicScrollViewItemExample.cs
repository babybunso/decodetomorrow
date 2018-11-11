
namespace DigiFrame {

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Digify;
    using System.Collections;
    using System.Collections.Generic;

    public class DynamicScrollViewItemExample : UIBehaviour, IDynamicScrollViewItem 
    {
        //Color blue1 = new Color(255, 255, 255);
        private readonly Color[] colors = new Color[] {
            new Color(142, 225, 252),
            new Color(214, 246, 251)
	    };

	    public Text  title;
	    public Image background;
        public Button weatherDate;
        [HideInInspector]


        void LoadWeatherDetailsByDate(int weatherIndex)
        {
            List<Forecast> forecasts = WeatherAtDam.instance.GetForecast("location_name");
            if (null != forecasts && forecasts.Count > 0)
            {
                Debug.Log("&&&&& DATE TIME " + forecasts[weatherIndex].Timestamp);
                Debug.Log("&&&&&&& Rain Probability: " + forecasts[weatherIndex].RainProbability.ToString());
                Debug.Log("&&&&&&& Rain GAILE: " + forecasts[weatherIndex].Rain.ToString());
            }

        }

        public void onUpdateItem( int index ) {
            if (WeatherAtDam.instance.canAccessWeather)
            {
                List<Forecast> forecasts = WeatherAtDam.instance.GetForecast("location_name");
                if (null != forecasts && forecasts.Count > 0)
                {
                    this.title.text = forecasts[index].Timestamp.ToString();
                    Button weatherDateClick = this.weatherDate.GetComponent<Button>();

                    weatherDateClick.onClick.AddListener(() => WeatherAtDam.instance.UpdateWeatherTextInfo(index, "location_name"));
                }
            }
        }
    }
}
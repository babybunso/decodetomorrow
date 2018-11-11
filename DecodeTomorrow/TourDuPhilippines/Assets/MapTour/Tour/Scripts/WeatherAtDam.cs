using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Digi.RainMaker;
using Digify;

public class WeatherAtDam : MonoBehaviour 
{

    static public WeatherAtDam instance;
    public string baseUrl = "https://api-dev.weathersolutions.ph/api/v1/forecast/";
    public string oauth_token = "Token cc84d57f4084333434f1068cde634ea6b7d20fa4";
    private ForecastDataStore forecastDataStore;
    private Dictionary<string, List<Forecast>> forecastData;
    public Text choosenDate, temperatureText, radiationText,
    cloudCoverText, rainText, recommendationText, placeText;


    [HideInInspector]
    public float damLatitude, damLongitude;
    [HideInInspector]
    public bool canAccessWeather = false;
    private bool isWeatherAvailable = false;
    public RainScript RainScript;

    public Button magatBtn, ambuklaoBtn, bingaBtn;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Button magatBtnChoosen = magatBtn.GetComponent<Button>();
        magatBtnChoosen.onClick.AddListener(() => ShowMapAtDam(16.9040145F, 121.3094922F, "Magat Dam"));

        Button ambukBtnChoosen = ambuklaoBtn.GetComponent<Button>();
        ambukBtnChoosen.onClick.AddListener(() => ShowMapAtDam(16.4587389F, 120.7430438F, "Ambuklao Dam"));

        Button bingaBtnChoosen = bingaBtn.GetComponent<Button>();
        bingaBtnChoosen.onClick.AddListener(() => ShowMapAtDam(16.3948684F, 120.7279512F, "Binga Dam"));


        damLatitude = 16.4587389F;
        damLongitude = 120.7430438F;
        string url = baseUrl + damLatitude + "," + damLongitude + "/?";

        string key = "location_name";
        //PlayerPrefs.SetString("currentLocation", key);
        StartCoroutine(LoadForecast(url, key));
        Debug.Log("My Location is at Batanes");

        placeText.text = "Ambuklao Dam";
        forecastData = new Dictionary<string, List<Forecast>>();

    }
    void ShowMapAtDam(float damLat, float damLong, string location)
    {
        damLatitude = damLat;
        damLongitude = damLong;
        string url = baseUrl + damLatitude + "," + damLongitude + "/?";

        string key = "location_name";
        StartCoroutine(LoadForecast(url, location));
        Debug.Log("My Location is at: "+ location);
        UpdateWeatherTextInfo(0, key);
        //PlayerPrefs.SetString("currentLocation", location);
        placeText.text = location;

    }


    public void UpdateWeatherTextInfo(int index, string location)
    {
        List<Forecast> forecasts = GetForecast("location_name");
        if (null != forecasts && forecasts.Count > 0)
        {
            choosenDate.text = forecasts[index].Timestamp.ToString();
            temperatureText.text = forecasts[index].Temperature.ToString();
            radiationText.text = forecasts[index].SolarRadiation.ToString();
            cloudCoverText.text = forecasts[index].TotalCloudCover.ToString();
            RainScript.RainIntensity = (float)forecasts[index].Rain / 20;
            string weatherInterpretationRain = WeatherInterpretationFactory.Interpret(forecasts[index], WeatherType.RAIN);
            rainText.text = weatherInterpretationRain;
            //placeText.text = location;
        }
    }


    public IEnumerator LoadForecast(string url, string locationKey)
    {
        Debug.Log("WEATHER API URL: " + url);
        Hashtable headers = new Hashtable();
        headers.Add("Authorization", oauth_token);
        WWW www = new WWW(url, null, headers);
        yield return www;


        ParseJob job = new ParseJob();
        job.InData = www.text;
        job.Start();
        Debug.Log("#####WEATHER Response" + www.text);
        yield return StartCoroutine(job.WaitFor());

        IDictionary response = (IDictionary)((IDictionary)job.OutData);
        IList results = (IList)response["results"];

        forecastDataStore = (ForecastDataStore)new DataStoreFactory().Create(StoreType.FORECAST);

        foreach (IDictionary result in results)
        {//This example only takes GPS location and the name of the object. There's lot more, take a look at the Foursquare API documentation

            IDictionary currentData = ((IDictionary)result);
            string timeStamp = currentData["timestamp"].ToString();
            DateTime enteredDate = DateTime.Parse(timeStamp);
            double temperature = double.Parse(currentData["temperature"].ToString());
            double windSpeed = double.Parse(currentData["wind_speed"].ToString());
            double solarRadiation = double.Parse(currentData["solar_radiation"].ToString());
            double meanSeaLevelPressure = double.Parse(currentData["mean_sea_level_pressure"].ToString());
            double rain = double.Parse(currentData["rain"].ToString());
            double dewpoint = double.Parse(currentData["dewpoint"].ToString());
            double windGust = double.Parse(currentData["wind_gust"].ToString());
            double windDirection = double.Parse(currentData["wind_direction"].ToString());
            double heatIndex = double.Parse(currentData["heat_index"].ToString());
            double totalCloudCover = double.Parse(currentData["total_cloud_cover"].ToString());
            double rainProbability = double.Parse(currentData["rain_probability"].ToString());
            if (timeStamp != null)
            {
                Debug.Log("GAILE Current Data:" + enteredDate + " " + temperature);
                Forecast forecast = new Forecast(enteredDate, temperature, windSpeed,
                                                 solarRadiation, meanSeaLevelPressure,
                                                 rain, dewpoint, windGust, windDirection, heatIndex,
                                                 totalCloudCover, rainProbability);

                forecastDataStore.Add(forecast);
            }
        }

        forecastData.Add(locationKey, forecastDataStore.All());
        isWeatherAvailable = true;
        canAccessWeather = true;
        PlayerPrefs.SetInt("IsDoneText", 1);
    }

    public List<Forecast> GetForecast(string locationKey)
    {
        return forecastData[locationKey];
    }

    private void Update()
    {
        if (isWeatherAvailable)
        {
            Location location = new Location(damLatitude, damLongitude, 0);
            List<Forecast> forecasts = GetForecast("location_name");
            if (null != forecasts && forecasts.Count > 0)
            {
                foreach (Forecast forecast in forecasts)
                {

                    //Debug.Log("@@@@@Rain Probability: " + forecast.RainProbability.ToString());
                    //Debug.Log("@@@@@Rain GAILE: " + forecast.Rain.ToString());
                    RainScript.RainIntensity = (float)forecast.Rain / 20;
                }
                isWeatherAvailable = false;
            }
        }
    }
}

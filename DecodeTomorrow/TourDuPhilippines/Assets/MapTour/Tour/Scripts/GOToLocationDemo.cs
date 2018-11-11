using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using DigiMap;
using System.Collections.Generic;
using Digify;

namespace DigiShared {

	public class GOToLocationDemo : MonoBehaviour {

		public InputField inputField;
		public Button button;
		public GOMap goMap;
		public GameObject addressMenu;

		GameObject addressTemplate;

        public GameObject stationPrefabAmbuk;
        public GameObject stationPrefabMagat;
        public GameObject stationPrefabBinga;

        private List<string> weatherStationMagat;
        private List<string> weatherStationAmbuklao;
        private List<string> weatherStationBinga;

		public void Start () {
		
			addressTemplate = addressMenu.transform.Find ("Address template").gameObject;

			inputField.onEndEdit.AddListener(delegate(string text) {
				GoToAddress();
			});
		}


		public void GoToAddress() {

			if (inputField.text.Any (char.IsLetter)) { //Text contains letters
				SearchAddress();
			} else if (inputField.text.Contains(",")){

				string s = inputField.text;
				Coordinates coords = new Coordinates (inputField.text);

                LocationManager locationManager = (LocationManager)goMap.locationManager;
				locationManager.SetLocation (coords);
				Debug.Log ("NewCoords: " + coords.latitude +" "+coords.longitude);
			}
		}

        public void GoTOMagatdam()
        {
            string s = "16.8288461,121.451844";
            Coordinates coords = new Coordinates(s);
            LocationManager locationManager = (LocationManager)goMap.locationManager;
            locationManager.SetLocation(coords);
            Debug.Log("NewCoords: " + coords.latitude + " " + coords.longitude);
            LoadWeatherStationMagat();
        }
        void LoadWeatherStationMagat()
        {
            weatherStationMagat = new List<string>();
            weatherStationMagat.Add("16.156318, 120.903032");
            weatherStationMagat.Add("16.344304,120.92571");
            weatherStationMagat.Add("16.519903,121.076134");
            weatherStationMagat.Add("16.49, 121.16");
            weatherStationMagat.Add("16.4334, 121.1133");
            weatherStationMagat.Add("16.6489, 121.1774");
            weatherStationMagat.Add("16.720732, 121.06945");
            weatherStationMagat.Add("16.7782, 121.0872");
            weatherStationMagat.Add("16.6749, 120.9368");
            weatherStationMagat.Add("16.9789, 121.3263");
            weatherStationMagat.Add("16.8389, 121.0056");
            weatherStationMagat.Add("16.9752, 121.2199");
            weatherStationMagat.Add("16.9514, 121.4972");
            weatherStationMagat.Add("16.7579, 121.2342 ");


            foreach (string station in weatherStationMagat)
            {
                Coordinates coordinates = new Coordinates(station);
                GameObject go = GameObject.Instantiate(stationPrefabMagat);
                go.transform.localPosition = coordinates.convertCoordinateToVector(0);
                go.transform.parent = transform;
            }

        }
        public void GoToAmbuklaoDam()
        {
            string s = "16.4587389,120.7430438";
            Coordinates coords = new Coordinates(s);
            LocationManager locationManager = (LocationManager)goMap.locationManager;
            locationManager.SetLocation(coords);
            Debug.Log("NewCoords: " + coords.latitude + " " + coords.longitude);
            LoadWeatherStationAmbuklao();
        }

        void DamInterpretation()
        {
            Dictionary<int, Dam> dams = new Dictionary<int, Dam>();

            Location damLocation = new Location(16.4587389, 120.7430438, 12/*11.75 */);
            double dischargeCap = 30600; // mmm
            double storageCapacity = 1080000000; // 1.8B mmm
            double minSupply = 160; // masl
            double maxSupply = 193; // masl
            double observedDamWaterAmount = 108000000;

            Reservoir reservoir = new Reservoir(storageCapacity, minSupply, maxSupply);

            // STATIONS, FORECASTS
            List<Forecast> forecasts = new List<Forecast>();
            Forecast forecast = new Forecast(DateTime.Now, 28.65, 36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);
            forecasts.Add(forecast);
           
            Location stationLocation = new Location(11.3581, 120.7242, 26);
            List<DamStation> stations = new List<DamStation>();
            DamStation station = new DamStation(980001, "Pamalican (Amanpulo) 1", stationLocation, forecasts);
            stations.Add(station);
          
            Dam dam = new Dam("Magat", damLocation, reservoir, dischargeCap, stations, observedDamWaterAmount);
            dams.Add(1, dam);
            MeanPrecipitationCalculator meanPrecipitation = new MeanPrecipitationCalculator(dams);


            Dictionary<int, Rainfall> meanWaterLevel = meanPrecipitation.GetWaterLevel(1);
            foreach (int key in meanWaterLevel.Keys)
            {
                Rainfall rainfall = meanWaterLevel[key];
                double waterLevel = rainfall.Amount + dam.ObservedVolume;

                Console.WriteLine("Station Id: " + key
                    + ", Timestamp: " + rainfall.DateTime + ", Mean Water Level: " + waterLevel
                    + ", Interpretation: " + new MeanWaterLevelInterpretation(
                        dams[1], waterLevel).Interpret()
                );
            }

            /*
            damLocation = new Location(16.4587389, 120.7430438, 17);
            dischargeCap = 30000; // mmm
            storageCapacity = 121000000; // 121M mmm
            minSupply = 740; // masl
            maxSupply = 752; // masl
            reservoir = new Reservoir(storageCapacity, minSupply, maxSupply);
            observedDamWaterAmount = 12100000;

            // STATIONS, FORECASTS
            forecasts = new List<Forecast>();
            forecast = new Forecast(DateTime.Now.AddHours(2), 28.65, 36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);
            forecasts.Add(forecast);
            forecast = new Forecast(DateTime.Now.AddHours(3), 28.65, 36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);
            forecasts.Add(forecast);

            stationLocation = new Location(11.3581, 120.7242, 26);
            stations = new List<DamStation>();
            station = new DamStation(980003, "Pamalican (Amanpulo) 1", stationLocation, forecasts);
            stations.Add(station);
            station = new DamStation(980004, "Pamalican (Amanpulo) 2", stationLocation, forecasts);
            stations.Add(station);

            dam = new Dam("Ambuklao", damLocation, reservoir, dischargeCap, stations, observedDamWaterAmount);
            dams.Add(2, dam);


            damLocation = new Location(16.3948684, 120.7279512, 17);
            dischargeCap = 10521; // mmm
            storageCapacity = 21000000; // 121M mmm
            minSupply = 757; // masl
            maxSupply = 566; // masl
            reservoir = new Reservoir(storageCapacity, minSupply, maxSupply);
            observedDamWaterAmount = 22000000;

            // STATIONS, FORECASTS
            forecasts = new List<Forecast>();
            forecast = new Forecast(DateTime.Now.AddHours(4), 28.65, 36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);
            forecasts.Add(forecast);
            forecast = new Forecast(DateTime.Now.AddHours(5), 28.65, 36, 0, 1010.85, 0.2, 25.2, 44, 54, 34, 29, 34);
            forecasts.Add(forecast);

            stationLocation = new Location(11.3581, 120.7242, 26);
            stations = new List<DamStation>();
            station = new DamStation(980003, "Pamalican (Amanpulo) 1", stationLocation, forecasts);
            stations.Add(station);
            station = new DamStation(980004, "Pamalican (Amanpulo) 2", stationLocation, forecasts);
            stations.Add(station);

            dam = new Dam("Binga", damLocation, reservoir, dischargeCap, stations, observedDamWaterAmount);
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
                    double waterLevel = rainfall.Amount + dam.ObservedVolume;

                    Console.WriteLine("Station Id: " + key
                        + ", Timestamp: " + rainfall.DateTime + ", Mean Water Level: " + waterLevel
                        + ", Interpretation: " + new MeanWaterLevelInterpretation(
                            dams[damId], waterLevel).Interpret()
                    );
                }
            }*/
        }

        void LoadWeatherStationAmbuklao()
        {
            weatherStationAmbuklao = new List<string>();
            weatherStationAmbuklao.Add("16.4599,120.7454");
            weatherStationAmbuklao.Add("16.8573, 120.7935");
            weatherStationAmbuklao.Add("16.3946, 120.7298");

            foreach (string station in weatherStationAmbuklao)
            {
                Coordinates coordinates = new Coordinates(station);
                GameObject go = GameObject.Instantiate(stationPrefabAmbuk);
                go.transform.localPosition = coordinates.convertCoordinateToVector(0);
                go.transform.parent = transform;
            }
        }

        public void GoToBingaDam()
        {
            string s = "16.3948684,120.7279512";
            Coordinates coords = new Coordinates(s);
            LocationManager locationManager = (LocationManager)goMap.locationManager;
            locationManager.SetLocation(coords);
            Debug.Log("NewCoords: " + coords.latitude + " " + coords.longitude);
            LoadWeatherStationBinga();
        }

        void LoadWeatherStationBinga()
        {
            weatherStationBinga = new List<string>();
            weatherStationBinga.Add("16.3946, 120.7298");

            foreach (string station in weatherStationAmbuklao)
            {
                Coordinates coordinates = new Coordinates(station);
                GameObject go = GameObject.Instantiate(stationPrefabBinga);
                go.transform.localPosition = coordinates.convertCoordinateToVector(0);
                go.transform.parent = transform;
            }
        }


        public void SearchAddress () {
		
			addressMenu.SetActive (false);
			string completeUrl;


				string baseUrl = "https://api.mapbox.com/geocoding/v5/mapbox.places/";
				string apiKey = goMap.mapbox_accessToken;
				string text = inputField.text;
				completeUrl = baseUrl + WWW.EscapeURL (text) +".json" + "?access_token=" + apiKey;

                if (goMap.locationManager.currentLocation != null) {
					completeUrl += "&proximity=" + goMap.locationManager.currentLocation.latitude + "%2C" + goMap.locationManager.currentLocation.longitude;
                } else if (goMap.locationManager.worldOrigin != null){
                    completeUrl += "&proximity=" + goMap.locationManager.worldOrigin.latitude + "%2C" + goMap.locationManager.worldOrigin.longitude;
				}
			
			Debug.Log (completeUrl);

			IEnumerator request = GOUrlRequest.jsonRequest (this, completeUrl, false, null, (Dictionary<string,object> response, string error) => {

				if (string.IsNullOrEmpty(error)){
					IList features = (IList)response["features"];
					LoadChoices(features);
				}

			});

			StartCoroutine (request);
		}

		public void LoadChoices(IList features) {

			while (addressMenu.transform.childCount > 1) {
				foreach (Transform child in addressMenu.transform) {
					if (!child.gameObject.Equals (addressTemplate)) {
						DestroyImmediate (child.gameObject);
					}
				}
			}


			for (int i = 0; i<Math.Min(features.Count,5); i++) {

				IDictionary feature = (IDictionary) features [i];

				IDictionary geometry = (IDictionary)feature["geometry"];
				IList coordinates = (IList)geometry["coordinates"];
				GOLocation location = new GOLocation ();
				IDictionary properties = (IDictionary)feature ["properties"];
				Coordinates coords = new Coordinates (Convert.ToDouble (coordinates [1]), Convert.ToDouble (coordinates [0]), 0);

				if (goMap.mapType == GOMap.GOMapType.Mapzen_Legacy) {
					location.addressString = (string)properties ["label"];
				} else {
					location.addressString = (string)feature ["place_name"];
				}
				location.coordinates = coords;
				location.properties = properties;

				GameObject cell = Instantiate (addressTemplate);
				cell.transform.SetParent(addressMenu.transform);
				cell.transform.GetComponentInChildren<Text> ().text = location.addressString;
				cell.name = location.addressString;
				cell.SetActive (true);

				Button btn = cell.GetComponent<Button> ();
				btn.onClick.AddListener(() => { LoadLocation(location); }); 
			
			}
			addressMenu.SetActive (true);
		}

		public void LoadLocation (GOLocation location) {

			inputField.text = location.addressString;
			addressMenu.SetActive (false);
            LocationManager locationManager = (LocationManager)goMap.locationManager;
            locationManager.SetLocation(location.coordinates);
		}

	}

	[System.Serializable]
	public class GOLocation {

		public Coordinates coordinates;
		public IDictionary properties;
		public string addressString;

	}
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace surnewsbytesAPI.Helper
{
    public class CoinMarketCapHelper
    {
        private static string API_KEY = "6b610d4c-b987-400a-914f-0c75a71e533e";


        public static async Task<string>  coinMarketAPI(string methodName)
        {
          
            var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/" + methodName + "?limit=100");





            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
            client.Headers.Add("Accepts", "application/json");
            var response =  client.DownloadString(URL.ToString());
            var responseIds = await Task.Run(()=> fetchData(response));
            responseFull responsefull = new responseFull();
            responsefull.all = response;
            responsefull.ids = responseIds;
            string mergeresponse = JsonConvert.SerializeObject(responsefull);
            return mergeresponse;
        }

        public static string fetchData(string response){
            try
            {
                var numberList = JsonConvert.DeserializeObject<Root>(response);
                string ids = "";
                foreach(var item in numberList.data)
                {
                    ids += item.id.ToString()+',';
                }
                ids = ids.Substring(0, ids.Length-1);
                var aux = "logo,status";
                var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/info");
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["id"] = ids;
                queryString["aux"] = aux;
                URL.Query = queryString.ToString();
                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.Headers.Add("Accepts", "application/json");
                var responseIds = client.DownloadString(URL.ToString());
                return responseIds;
            }
            catch (Exception e)
            {
                return "";
            }
        }


        public static async Task<string> coinMarketCurrencies()
        {
            try
            {
                var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/fiat/map" + "?limit=100");
                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.Headers.Add("Accepts", "application/json");
                var response = client.DownloadString(URL.ToString());
                var crypto = await Task.Run(() => cryptoCurrencyList());
                currenciesResponse currency = new currenciesResponse();
                currency.country = response;
                currency.crypto = crypto;
                string mergeresponse = JsonConvert.SerializeObject(currency);

                return mergeresponse;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static async Task<string> cryptoCurrencyList()
        {
            try
            {
                var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/cryptocurrency/map" + "?limit=100");
                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.Headers.Add("Accepts", "application/json");
                var response = client.DownloadString(URL.ToString());
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<string> cryptoConverter(convertModel model)
        {
            try
            {
                var URL = new UriBuilder("https://pro-api.coinmarketcap.com/v1/tools/price-conversion");

                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["id"] = model.id.ToString();
                queryString["amount"] = model.amount.ToString();
                queryString["convert"] = model.convert_id;
                URL.Query = queryString.ToString(); 
                var client = new WebClient();
                client.Headers.Add("X-CMC_PRO_API_KEY", API_KEY);
                client.Headers.Add("Accepts", "application/json");
                var response = client.DownloadString(URL.ToString());
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Status
    {
        public DateTime timestamp { get; set; }
        public int error_code { get; set; }
        public object error_message { get; set; }
        public int elapsed { get; set; }
        public int credit_count { get; set; }
        public string notice { get; set; }
        public int total_count { get; set; }
    }

    public class Platform
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public string token_address { get; set; }
    }

    public class USD
    {
        public double price { get; set; }
        public double volume_24h { get; set; }
        public double percent_change_1h { get; set; }
        public double percent_change_24h { get; set; }
        public double percent_change_7d { get; set; }
        public double market_cap { get; set; }
        public DateTime last_updated { get; set; }
    }

    public class Quote
    {
        public USD USD { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string slug { get; set; }
        public int num_market_pairs { get; set; }
        public DateTime date_added { get; set; }
        public List<string> tags { get; set; }
        public long? max_supply { get; set; }
        public double circulating_supply { get; set; }
        public double total_supply { get; set; }
        public Platform platform { get; set; }
        public int cmc_rank { get; set; }
        public DateTime last_updated { get; set; }
        public Quote quote { get; set; }
    }

    public class Root
    {
        public Status status { get; set; }
        public List<Datum> data { get; set; }
    }

    public class responseFull {
        public string all { get; set; }
        public string ids { get; set; }
    }

    public class currenciesResponse
    {
        public string country { get; set; }
        public string crypto { get; set; }
    }
    public class convertModel
    {
        public double amount { get; set; }
        public int id { get; set; }

        public string convert_id { get; set; }
    }
}
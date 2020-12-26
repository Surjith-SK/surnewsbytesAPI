using surnewsbytesAPI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace surnewsbytesAPI.Controllers
{
    public class newsBytesController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetList()
        {
            try
            {
                var allResult = await Task.Run(() => CoinMarketCapHelper.coinMarketAPI("listings/latest"));
                return Ok(allResult);
            }
            catch (Exception)
            {

                return Ok(0);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCurrencies()
        {
            try
            {
                var allCurrencies = await Task.Run(() => CoinMarketCapHelper.coinMarketCurrencies());
                return Ok(allCurrencies);
            }
            catch (Exception)
            {

                return Ok(0);
            }
        }

        [HttpPost]

        public async Task<IHttpActionResult> convertCurrency(convertModel data)
        {
            try
            {
                var convertedCurrency = await Task.Run(() => CoinMarketCapHelper.cryptoConverter(data));
                return Ok(convertedCurrency);
            }
            catch (Exception)
            {

                return Ok(0);
            }
        }

        
    }
}

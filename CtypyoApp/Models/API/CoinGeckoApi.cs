﻿using CoinGecko.Parameters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CtypyoApp.Models.API
{

    public class CoinGeckoApi

    {
        HttpClient _httpClient = new HttpClient();

        public CoinGeckoApi()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        public async Task<List<string>> GetSupportedCurrenciesAsync()
        {
            string url = $"https://api.coingecko.com/api/v3/simple/supported_vs_currencies";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string jsonResponse = "";

                if (response.IsSuccessStatusCode)
                    jsonResponse = await response.Content.ReadAsStringAsync();
                else
                    jsonResponse = APIResponceImitation.GetSupportedCurrenciesAsync();

                List<string> supportedCurrencySymbols = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

                return supportedCurrencySymbols;
            }
            catch (Exception)
            {

            }

            return new List<string>();
        }

        public async Task<List<CryptoCurrency>> GetTopNCurrenciesAsync(int topN, int pageNum)
        {
            string validatedTopN = topN > 250 ? "250" : (topN < 0 ? "1" : topN.ToString());
            string validatedpageNum = pageNum < 0 ? "1" : pageNum.ToString();

            string url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page={validatedTopN}&page={validatedpageNum}&sparkline=false&locale=en";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string jsonResponse = "";

                if (response.IsSuccessStatusCode)
                    jsonResponse = await response.Content.ReadAsStringAsync();
                else
                    jsonResponse = APIResponceImitation.GetTopNCurrenciesAsync();

                var topNCurrencies = JsonConvert.DeserializeObject<List<CryptoCurrency>>(jsonResponse);

                return topNCurrencies;
            }
            catch (Exception)
            {
                // Обработка ошибок здесь
            }

            return new List<CryptoCurrency>();
        }

        

        public async Task<decimal> GetCurrencyPriceByIdAsync(string id, string targetCurrencyId)
        {
            string url = $"https://api.coingecko.com/api/v3/simple/price?ids={id}&vs_currencies={targetCurrencyId}&include_market_cap=false&include_24hr_vol=false&include_24hr_change=false&include_last_updated_at=false";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string jsonResponse = "";

                if (response.IsSuccessStatusCode)
                    jsonResponse = await response.Content.ReadAsStringAsync();
                else
                    jsonResponse = APIResponceImitation.GetCurrencyPriceByIdAsync();

                CurrencyDetails currencyDetails = await GetCurrencyDetailsByIdAsync(id, targetCurrencyId);

                decimal price = currencyDetails.Price;

                return price;
            }
            catch (Exception)
            {

            }

            return -1;
        }
        public async Task<CurrencyDetails> GetCurrencyDetailsByIdAsync(string id, string targetCurrencyId)
        {
            string url = $"https://api.coingecko.com/api/v3/coins/{id}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                string jsonResponse = "";

                if (response.IsSuccessStatusCode)
                    jsonResponse = await response.Content.ReadAsStringAsync();
                else
                    jsonResponse = APIResponceImitation.GetCurrencyDetailsByIdAsync();

                JObject jsonObject = JObject.Parse(jsonResponse);

                CurrencyDetails CryptoCurrency = new CurrencyDetails();
                CryptoCurrency.Id = (string)jsonObject["id"];
                CryptoCurrency.Name = (string)jsonObject["symbol"];
                CryptoCurrency.Symbol = (string)jsonObject["symbol"];
                CryptoCurrency.Image = (string)jsonObject["image"]["large"];
                CryptoCurrency.Price = (decimal)jsonObject["market_data"]["current_price"][targetCurrencyId];
                CryptoCurrency.Volume = (decimal)jsonObject["market_data"]["total_volume"][targetCurrencyId];
                return CryptoCurrency;
            }
            catch (Exception)
            {

            }

            return new CurrencyDetails();
        }
    }
}

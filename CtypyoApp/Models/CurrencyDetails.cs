﻿using Newtonsoft.Json;

public class CurrencyDetails : CryptoCurrency
{
    [JsonProperty("fully_diluted_valuation")]
    public decimal FullyDilutedValuation { get; set; }

    [JsonProperty("circulating_supply")]
    public decimal CirculatingSupply { get; set; }

    [JsonProperty("market_cap")]
    public decimal MarketCap { get; set; }

    [JsonProperty("total_volume")]
    public decimal TotalVolume { get; set; }

    [JsonProperty("total_supply")]
    public decimal TotalSupply { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; } // Путь к изображению
}

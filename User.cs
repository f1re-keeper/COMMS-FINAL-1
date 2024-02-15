using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject
{
    internal class User
    {
        [JsonProperty(Order = 1)] public string firstName {  get; set; }
        [JsonProperty(Order = 2)] public string lastName { get; set; }

        [JsonProperty(Order = 3)] public CardDetails cardDetails;

        [JsonProperty(Order = 4)] public string pinCode { get; set; }

        [JsonProperty(Order = 5)] public List<Transaction> transactionHistory;

    }
}

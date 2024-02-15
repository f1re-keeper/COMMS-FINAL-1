using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    [DataContract]
    internal class CardDetails
    {
        [DataMember]
        internal string cardNumber;

        [DataMember]
        internal string expirationDate;

        [DataMember]
        internal string CVC;

        public CardDetails(string cardNumber, string expirationDate, string cvc)
        {
            this.cardNumber = cardNumber;
            this.expirationDate = expirationDate;
            this.CVC = cvc;
        }

    }
}

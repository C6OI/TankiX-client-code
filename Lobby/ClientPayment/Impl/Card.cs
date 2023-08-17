using System;
using UnityEngine;

namespace Lobby.ClientPayment.Impl {
    public class Card {
        public string cvc;

        public string expiryMonth;

        public string expiryYear;

        public string generationtime;

        public string holderName;
        public string number;

        public override string ToString() {
            generationtime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            return JsonUtility.ToJson(this);
        }
    }
}
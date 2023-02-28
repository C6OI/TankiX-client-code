using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl.Payguru {
    public class PayguruBankItem : MonoBehaviour {
        [SerializeField] TextMeshProUGUI bankData;

        public string BankData {
            get => bankData.text;
            set => bankData.text = value;
        }

        public void Init(PayguruAbbreviatedBankInfo bank) {
            BankData = bank.Name + "\nBranch and account No: " + bank.BranchNumber + "-" + bank.AccountNumber + "\nIBAN: " + bank.AccountIban + "\n";
        }
    }
}
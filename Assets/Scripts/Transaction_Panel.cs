using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transaction_Panel : MonoBehaviour
{
    public int id;
    public TextMeshProUGUI label_txt;
    public TextMeshProUGUI price_txt;
    public TextMeshProUGUI description_text;
    public TextMeshProUGUI date_text;
    public TextMeshProUGUI typePurchase_text;
    public TextMeshProUGUI isRevenue_text;
    public Button removeDailyPayment_btn;

    public Payment lastPayment;

    public void OpenFullPanel(Payment lastPayment)
    {
        Transaction_Prefab payment = Main_Manager.instance.transaction_fullPanel.GetComponent<Transaction_Prefab>();
    }

    public void RemovePayment()
    {
        Save_Manager.RemovePayment(id);
    }

    public void RemoveFromDailyPayments()
    {
        Save_Manager.RemoveDailyPayment(id);
    }
}

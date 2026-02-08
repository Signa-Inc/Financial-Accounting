using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transaction : MonoBehaviour
{
    [Header("Transaction Prefab")]
    public int id;
    public Image view_img;
    public TextMeshProUGUI label_txt;
    public TextMeshProUGUI price_txt;

    [Header("Transaction Full Panel")]
    public TextMeshProUGUI description_text;
    public TextMeshProUGUI date_text;
    public TextMeshProUGUI typePurchase_text;
    public TextMeshProUGUI isRevenue_text;
    public Button removeDailyPayment_btn;

    public Payment lastPayment;

    public void OpenFullPanel()
    {
        Main_Manager.instance.transaction_fullPanel.SetActive(true);

        Transaction payment = Main_Manager.instance.transaction_fullPanel.GetComponent<Transaction>();

        payment.id = id;

        List<Payment> payments = Save_Manager.payments;

        payment.label_txt.text = payments[id].label;
        payment.description_text.text = payments[id].description;
        payment.isRevenue_text.text = payments[id].isRevenue? "Is Revenue" : "Is Expense";
        payment.date_text.text = payments[id].date;
        payment.typePurchase_text.text = payments[id].typePurchase.ToString();
        payment.price_txt.text = payments[id].price;

        // Делаем кнопку удаления платежа из ежедневныз покупок интерактивной если этот платеж есть в ежедневных платежах
        payment.removeDailyPayment_btn.interactable = payments[id].isDailyPayment? true : false;

        payment.lastPayment = payments[id];
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

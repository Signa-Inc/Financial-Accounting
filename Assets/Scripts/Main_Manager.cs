using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_Manager : MonoBehaviour
{
    public static Main_Manager instance;

    [Header("Balance")]
    [SerializeField] TextMeshProUGUI balance_txt;

    [Header("Transactions")]
    public RectTransform parent_transform;
    public GameObject transaction_prefab;
    public GameObject transaction_fullPanel;
    public GameObject date_prefab;

    [Header("Error")]
    public GameObject error_panel;
    public TextMeshProUGUI error_txt;

    [HideInInspector, Header("Init Payments")]
    private List<Payment> payments;
    private Transaction trans;
    private List<string> dates; // В 0 индексе самая старая дата
    private Dictionary<string, List<int>> payment_dic;

    void Awake() => instance = this;

    void Start()
    {
        UpdateBalance();
    }

    public void UpdateBalance() => balance_txt.text = $"Balance: {Save_Manager.GetBalance()}";

    public void UpdatePayments()
    {
        // Удаляем старые платежи и очищаем переменные
        for (int i = parent_transform.childCount - 1; i >= 0; i--)
            Destroy(parent_transform.GetChild(i).gameObject);

        payments = new List<Payment>();
        trans = new Transaction();
        dates = new List<string>();
        payment_dic = new Dictionary<string, List<int>>();

        SetNewPaymentSizeInContent(true); // Очищаем content чтобы он не был слишком длинным

        // Узнаем сколько всего у нас есть дат
        payments = Save_Manager.payments; // Потом будем присваивать транзакции который были совершены за последние 30 дней 

        foreach(Payment payment in payments)
        {
            if(!dates.Contains(payment.date)){
                dates.Add(payment.date);
                SetNewPaymentSizeInContent();
            }
        }

        // Сортируем список по дате (от новой к старой)
        dates.Sort();

        // Заполняем словарь
        foreach (string date in dates)
        {
            List<int> ids = new List<int>();

            for (int i = 0; i < payments.Count; i++)
                if(payments[i].date == date)
                    ids.Add(payments[i].id);

            payment_dic.Add(date, ids);

            for (int i = 0; i < payment_dic[date].Count; i++)
                print($"Key = {date}, value = {payment_dic[date][i]}");
        }

        // Создаем префабы дат и транзакций
        for(int j = 0; j < payment_dic.Count; j++)
        {
            Date_Prefab df = Instantiate(date_prefab, parent_transform).GetComponent<Date_Prefab>(); // Спавним блок с датой
            df.label_txt.text = dates[j];

            float minus = 0;
            float plus = 0;
            float equal = 0;

            foreach (int item in payment_dic[dates[j]])
            {
                try
                {
                    trans = Instantiate(transaction_prefab, parent_transform).GetComponent<Transaction>();

                    trans.id = payments[item].id;
                    trans.label_txt.text = payments[item].label;
                    trans.price_txt.text = payments[item].price;

                    SetNewPaymentSizeInContent();

                    float price = float.Parse(payments[item].price);
                    bool isRevenue = payments[item].isRevenue;

                    if (isRevenue)
                        plus += price;
                    else
                        minus += price;
                }
                catch (Exception ex)
                {
                    Debug.Log("Id = " + payments[item-1].id);
                    Debug.Log("Error: " + ex);
                }
            }
            equal = plus - minus;

            df.minus_txt.text = $"{minus}";
            df.plus_txt.text = $"{plus}";
            df.equal_txt.text = $"{equal}";

            if (equal > 0)
                df.equal_txt.color = Color.green;
            else if (equal < 0)
                df.equal_txt.color = Color.red;
            else
                df.equal_txt.color = Color.white;
        }
    }

    public void Error(string error)
    {
        error_panel.SetActive(true);
        error_txt.text = error;
    }

    public void SetNewPaymentSizeInContent(bool isClear = false) => parent_transform.sizeDelta = new Vector2(parent_transform.sizeDelta.x, isClear? (parent_transform.sizeDelta.y * 0) + 20 : parent_transform.sizeDelta.y + 148.3f);
    // isClear равен true, когда мы хотим очистить наш ScrollBar (Content). 148,3f = высота блока с транзакциями, а 20 = начальному размеру ScrollBar (Content)
}

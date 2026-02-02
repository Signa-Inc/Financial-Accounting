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
    private List<string> dates = new(); // В 0 индексе самая старая дата
    private Dictionary<string, List<int>> payment_dic = new();

    void Awake() => instance = this;

    void Start()
    {
        UpdateBalance();
        UpdatePayments();
    }

    void Update()
    {

    }

    public void UpdateBalance() => balance_txt.text = $"Balance: {Save_Manager.GetBalance()}";

    public void UpdatePayments()
    {
        // Удаляем старые платежи
        for (int i = parent_transform.childCount - 1; i >= 0; i--)
            Destroy(parent_transform.GetChild(i).gameObject);

        SetNewPaymentSizeInContent(true, parent_transform.childCount + dates.Count * 148.3f); // Cчитаем сколько всего объектов и умножаем на их высоту (148.3)

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

            //for (int i = 0; i < payment_dic[date].Count; i++)
            //    print($"Key = {date}, value = {payment_dic[date][i]}");
        }

        // Создаем префабы дат и транзакций
        for(int j = 0; j < payment_dic.Count; j++)
        {
            Instantiate(date_prefab, parent_transform).GetComponent<Date_Prefab>().label.text = dates[j]; // Устанавливаем дату

            foreach (int item in payment_dic[dates[j]])
            {
                trans = Instantiate(transaction_prefab, parent_transform).GetComponent<Transaction>();

                trans.id = payments[item].id;
                trans.label_txt.text = payments[item].label;
                trans.price_txt.text = payments[item].price;

                SetNewPaymentSizeInContent();
            }
        }
    }

    public void Error(string error)
    {
        error_panel.SetActive(true);
        error_txt.text = error;
    }

    public void SetNewPaymentSizeInContent(bool isClear = false, float numToRemove = 0f) => parent_transform.sizeDelta = new Vector2(parent_transform.sizeDelta.x, isClear? parent_transform.sizeDelta.y - numToRemove : parent_transform.sizeDelta.y + 148.3f);
    // isClear равен true, когда мы хотим очистить наш ScrollBar, а numToRemove показывает насколько его нужно очистить
}

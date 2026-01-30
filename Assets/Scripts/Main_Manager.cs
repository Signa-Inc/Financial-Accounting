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

    Transaction trans;
    List<Payment> payments;

    void Awake() => instance = this;

    void Start()
    {
        UpdateBalance();
        UpdatePayments();
    }

    void Update()
    {

    }

    public void UpdateBalance()
    {
        balance_txt.text = $"Balance: {Save_Manager.GetBalance()}";
    }

    public void UpdatePayments()
    {
        // ”дал€ем старые платежи
        for (int i = parent_transform.childCount - 1; i >= 0; i--)
            Destroy(parent_transform.GetChild(i).gameObject);

        SetNewPaymentSizeInContent(true, parent_transform.childCount * 148.3f); // Cчитаем сколько всего объектов и умножаем на их высоту (148.3)

        // ¬водим новые платежи
        payments = Save_Manager.payments;

        for (int i = 0; i < payments.Count; i++)
        {
            trans = Instantiate(transaction_prefab, parent_transform).GetComponent<Transaction>();

            trans.id = payments[i].id;
            trans.label_txt.text = payments[i].label;
            trans.price_txt.text = payments[i].price;

            SetNewPaymentSizeInContent();
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

using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Add_Payment : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] TMP_InputField label_if;
    [SerializeField] TMP_Dropdown type_dd;
    [SerializeField] TMP_InputField date_if;
    [SerializeField] TMP_InputField description_if;
    [SerializeField] Toggle isRevenue_tgl;
    [SerializeField] TMP_InputField price_if;

    Payment lastPayment;

    void Start()
    {
        // Настраиваем чтобы при старте игры сразу повялавлялась текущая дата
        date_if.text = DateTime.Now.ToString("dd.MM.yyyy");

        // Заполняем выпадающий список типами покупок
        List<string> allTypesOfPayment = new List<string>();

        foreach (var type in Enum.GetValues(typeof(TypePurchases)))
            allTypesOfPayment.Add(type.ToString());

        type_dd.AddOptions(allTypesOfPayment);
    }

    public void PaymentComplete(bool isDailyPayment = false) // Передаём true в том случае если платёж должен быть ежедневным 
    {
        if(isDailyPayment){
            foreach (Payment p in Save_Manager.payments)
                if (label_if.text == p.label && p.isDailyPayment){
                    Main_Manager.instance.Error("Ежедневный платёж с таким именем уже существует, пожалуйста переименуйте его");
                    return;
                }
        }

        //Переводим дату в формат ДД.ММ.ГГГГ
        DateTime parsedDate = DateTime.Parse(date_if.text);

        if(parsedDate > DateTime.Today){
            Main_Manager.instance.Error("Вы указали дату которая ещё не наступала");
            return;
        }

        Payment newPayment = new Payment
        {
            label = label_if.text,
            description = description_if.text,
            typePurchase = (TypePurchases)Enum.Parse(typeof(TypePurchases), type_dd.options[type_dd.value].text),
            date = parsedDate.ToString("dd.MM.yyyy"),
            isRevenue = isRevenue_tgl.isOn,
            price = price_if.text,
            isDailyPayment = isDailyPayment,
        };

        lastPayment = newPayment;

        Transaction trans = Instantiate(Main_Manager.instance.transaction_prefab, Main_Manager.instance.parent_transform).GetComponent<Transaction>();

        trans.id = Save_Manager.InitUnicId();
        trans.label_txt.text = newPayment.label;
        trans.price_txt.text = newPayment.price;

        Save_Manager.SetPayment(newPayment);
        Main_Manager.instance.UpdatePayments();

        gameObject.SetActive(false);
    }
}

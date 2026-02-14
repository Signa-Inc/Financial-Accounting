using System;

[Serializable]
public class Payment
{
    public int id;
    public string label;
    public string description;
    public bool isRevenue;
    public TypePurchases typePurchase;
    public string date;
    public string price;
    public bool isDailyPayment;
    public bool isTemplate;

    public Payment(int id, string label, string description, bool isRevenue, TypePurchases typePurchase, string date, string price, bool isDailyPayment, bool isTemplate)
    {
        this.id = id;
        this.label = label;
        this.description = description;
        this.isRevenue = isRevenue;
        this.typePurchase = typePurchase;
        this.date = date;
        this.price = price;
        this.isDailyPayment = isDailyPayment;
        this.isTemplate = isTemplate;
    }

    // Конструктор по умолчанию (нужен для десериализации)
    public Payment() { }
}
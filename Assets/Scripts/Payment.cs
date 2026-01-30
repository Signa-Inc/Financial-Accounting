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

    public Payment(int id, string label, string description, bool isRevenue, TypePurchases typePurchase, string date, string price)
    {
        this.id = id;
        this.label = label;
        this.description = description;
        this.isRevenue = isRevenue;
        this.typePurchase = typePurchase;
        this.date = date;
        this.price = price;
    }

    // Конструктор по умолчанию (нужен для десериализации)
    public Payment() { }
}
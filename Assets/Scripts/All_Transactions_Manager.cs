using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class All_Transactions_Manager : MonoBehaviour
{
    //[SerializeField] RectTransform rt;

    [SerializeField] Image diagram_prefab;
    [SerializeField] Transform parent;
    [SerializeField] List<Color> sprites;

    List<Color> _sprites;

    public void Open_Anim()
    {
        
    }

    public void Open()
    {
        _sprites = sprites;

        float curFillAmount = 1; // Текущая заполненость круга

        Dictionary<TypePurchases, float> dict = new(); // Словарь со всеми покупками
        float totalSum = 0; // Общая сумма всех покупок

        foreach(var item in Save_Manager.payments) // Присваиваем словарю покупки
        {
            if (dict.ContainsKey(item.typePurchase))
                dict[item.typePurchase] += float.Parse(item.price);
            else
                dict.Add(item.typePurchase, float.Parse(item.price));

            totalSum += float.Parse(item.price); // Добавляем каждую сумму покупки в сумму всех покупок
        }

        var sortedDict = dict.OrderByDescending(x => x.Value).ToList(); // Сортировка от большего типа трат к меньшему

        for (int i = sortedDict.Count - 1; i >= 0; i--)
        {
            Image newPiece = Instantiate(diagram_prefab, parent);

            int randomNum = Random.Range(0, _sprites.Count);
            newPiece.color = _sprites[randomNum];
            _sprites.RemoveAt(randomNum);

            if (i == 0)
            {
                newPiece.transform.SetAsFirstSibling(); // Делаем так чтобы самый большой кусок круга, который равен 1, был сзади всех
                newPiece.fillAmount = 1;
                return;
            }

            newPiece.fillAmount = curFillAmount - (sortedDict[i].Value / totalSum);

            curFillAmount = newPiece.fillAmount;
        }
    }
}
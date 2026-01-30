using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save_Manager : MonoBehaviour
{
    public static List<Payment> payments;

    void Awake() => InitPayments();

    public static void SetBalance(float balance) => PlayerPrefs.SetFloat("Balance", PlayerPrefs.GetFloat("Balance", 0f) + balance);

    public static float GetBalance() => PlayerPrefs.GetFloat("Balance", 0f);

    public static void SetPayment(Payment payment)
    {
        payment.id = InitUnicId();

        string path = Application.persistentDataPath + "/payments.json";
        string jsonLine = JsonConvert.SerializeObject(payment);
        File.AppendAllText(path, jsonLine + "\n");

        payments.Add(payment);

        // Set Balance and update UI
        SetBalance(payment.isRevenue ? float.Parse(payment.price) : -float.Parse(payment.price));
        Main_Manager.instance.UpdateBalance();
    }

    public static List<Payment> GetPayment()
    {
        string path = Application.persistentDataPath + "/payments.json";
        List<Payment> payments = new List<Payment>();

        if (!File.Exists(path)) return payments;

        var lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                try
                {
                    Payment p = JsonConvert.DeserializeObject<Payment>(line);
                    payments.Add(p);
                }
                catch (JsonException ex)
                {
                    Debug.LogError("Ошибка десериализации строки: " + ex.Message);
                }
            }
        }

        return payments;
    }

    public static void RemovePayment(int paymentId)
    {
        // Находим покупку для удаления
        Payment paymentToRemove = payments.Find(p => p.id == paymentId);

        if (paymentToRemove == null)
        {
            Debug.LogWarning($"Payment with ID {paymentId} not found");
            return;
        }

        try
        {
            // Корректируем баланс (обратная операция)
            float amount = float.Parse(paymentToRemove.price);
            SetBalance(paymentToRemove.isRevenue ? -amount : amount);

            // Удаляем покупку из списка
            payments.Remove(payments[paymentId]);

            // Перезаписываем весь файл с обновленными данными
            string path = Application.persistentDataPath + "/payments.json";

            // Создаем временный файл для записи
            string tempPath = path + ".tmp";

            using (StreamWriter writer = new StreamWriter(tempPath))
            {
                foreach (var payment in payments)
                {
                    string jsonLine = JsonConvert.SerializeObject(payment);
                    writer.WriteLine(jsonLine);
                }
            }

            // Заменяем оригинальный файл временным
            if (File.Exists(path))
                File.Delete(path);
            File.Move(tempPath, path);

            // Инициализируем payments снова, во избежание ошибок отображения списка на главном экране
            InitPayments();

            // Обновляем UI
            Main_Manager.instance.UpdateBalance();
            Main_Manager.instance.UpdatePayments();

            Debug.Log($"Payment with ID {paymentId} successfully removed");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error removing payment: {ex.Message}");
            Debug.LogError($"Problematic price value: '{paymentToRemove.price}'");

            // Восстанавливаем список платежей в случае ошибки
            InitPayments();
        }
    }

    public static int InitUnicId() { return payments.Count == 0 ? 0 : payments[payments.Count - 1].id + 1; }

    public static List<Payment> InitPayments() => payments = GetPayment();
}

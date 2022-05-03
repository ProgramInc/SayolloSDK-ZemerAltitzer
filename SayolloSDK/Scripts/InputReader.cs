using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(PurchaseView))]
[RequireComponent(typeof(PurchaseViewRequestHandler))]
public class InputReader : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField creditCardInputField;
    [SerializeField] TMP_InputField ExpirationDateInputField;

    List<TMP_InputField> inputFields = new List<TMP_InputField>();

    public delegate void ItemPurchaseAction(string userInput);
    public static event ItemPurchaseAction OnPurchaseSubmitted;

    private void Awake()
    {
        //Adding the input fields to a list so that they can be iterated over later
        inputFields.Add(emailInputField);
        inputFields.Add(creditCardInputField);
        inputFields.Add(ExpirationDateInputField);
    }

    public void SubmitPurchase()
    {
        //Making sure that the input fields are not empty or null
        for (int i = 0; i < inputFields.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(inputFields[i].text) || string.IsNullOrEmpty(inputFields[i].text))
            {
                return;
            }
        }

        CustomerDetails customerDetails = new CustomerDetails(emailInputField.text, creditCardInputField.text, ExpirationDateInputField.text); ;
        OnPurchaseSubmitted?.Invoke(customerDetails.ToJson());
    }
}
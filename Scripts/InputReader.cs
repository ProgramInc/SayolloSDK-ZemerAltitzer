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

        //Format the user input as json and invoke the OnPurchaseSubmitted event 
        //which will send the json string to the PurchaseRequestHandler
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("\'email_address\'", "\'" + emailInputField.text + "\'");
        dictionary.Add("\'credit_card_number\'", "\'" + creditCardInputField.text + "\'");
        dictionary.Add("\'expiration_date\'", "\'" + ExpirationDateInputField.text + "\'");
        string dictionaryString = "{";

        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
            dictionaryString += keyValuePair.Key + " : " + keyValuePair.Value + ", ";
        }
        dictionaryString = dictionaryString.TrimEnd(',', ' ') + "}";

        OnPurchaseSubmitted?.Invoke(dictionaryString);
    }
}

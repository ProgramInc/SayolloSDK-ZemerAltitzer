using UnityEngine;


[System.Serializable]
public struct CustomerDetails
{
    public string email;
    public string credit_card;
    public string expiration_date;

    public CustomerDetails(string email, string credit_card, string expiration_date)
    {
        this.email = email;
        this.credit_card = credit_card;
        this.expiration_date = expiration_date;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}


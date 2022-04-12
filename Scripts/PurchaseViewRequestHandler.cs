using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(PurchaseView))]
public class PurchaseViewRequestHandler : MonoBehaviour
{
    [SerializeField] string url;

    PurchaseViewItem item;
    string imageLink;

    public delegate void ServerConfirmationAction();
    public static event ServerConfirmationAction OnServerConfirmedOrder;

    public delegate void ItemReceivedAction(Vector2 imageSize, Texture texture, PurchaseViewItem item);
    public static event ItemReceivedAction OnImageReceived;


    private void OnEnable()
    {
        InputReader.OnPurchaseSubmitted += submitPurchaseRequestWrapper;
    }

    private void OnDisable()
    {
        InputReader.OnPurchaseSubmitted -= submitPurchaseRequestWrapper;
    }

    private void Start()
    {
        StartCoroutine(nameof(PurchaseViewPostRequest), url);
        StartCoroutine(nameof(GetItemImage));
    }

    // Sending a POST request to query the item from the server.
    IEnumerator PurchaseViewPostRequest(string url)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url, "");
        UploadHandler uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{}"));
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.uploadHandler = uploadHandler;

        yield return webRequest.SendWebRequest();

        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            string result = webRequest.downloadHandler.text;
            string substring = result.Substring(1, result.Length - 2);
            string[] splitString = substring.Split(',');
            Dictionary<string, string> jsonItem = new Dictionary<string, string>();

            foreach (string itemValue in splitString)
            {
                string[] keyValuePair = itemValue.Split(new[] { ':' }, 2);
                string formattedKey = keyValuePair[0].Trim();
                string formattedValue = keyValuePair[1].Trim();
                jsonItem.Add(formattedKey, formattedValue);
            }

            item = new PurchaseViewItem(jsonItem["\'title\'"], jsonItem["\'item_name\'"], jsonItem["\'item_image\'"], jsonItem["\'price\'"], jsonItem["\'currency\'"], jsonItem["\'currency_sign\'"]);
        }
    }

    IEnumerator GetItemImage()
    {
        //Making sure that the image link is ready before attempting the download request.
        while (item.itemImageLink == null)
        {
            yield return null;
        }
        string formattedImagelink = item.itemImageLink.Substring(1, item.itemImageLink.Length - 2);

        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(formattedImagelink);
        yield return webRequest.SendWebRequest();

        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            Vector2 imageSize = new Vector2(((DownloadHandlerTexture)webRequest.downloadHandler).texture.width / 1.5f, ((DownloadHandlerTexture)webRequest.downloadHandler).texture.height / 1.5f);
            Texture imageTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;

            OnImageReceived?.Invoke(imageSize, imageTexture, item);
        }
    }

    // A simple wrapper to allow registering the coroutine to the OnPurchaseSubmitted event.
    void submitPurchaseRequestWrapper(string postData)
    {
        StartCoroutine(nameof(submitPurchasePostRequest), postData);
    }


    //Sending a POST request to the server with the user details
    IEnumerator submitPurchasePostRequest(string postData)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post("https://6u3td6zfza.execute-api.us-east-2.amazonaws.com/prod/v1/gcom/action", postData);
        UploadHandler uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{}"));

        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.uploadHandler = uploadHandler;

        yield return webRequest.SendWebRequest();

        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            if (webRequest.downloadHandler.text.Contains("'status': 'Success'"))
            {
                OnServerConfirmedOrder?.Invoke();
            }
        }
    }
}

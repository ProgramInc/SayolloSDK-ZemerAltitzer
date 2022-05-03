using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(PurchaseView))]
public class PurchaseViewRequestHandler : MonoBehaviour
{
    [SerializeField] string url;

    PurchaseViewItem item;

    public delegate void ServerConfirmationAction(string statusMessage);
    public static event ServerConfirmationAction OnPurchaseResponseFromServer;

    public delegate void ItemReceivedAction(Vector2 imageSize, Texture texture, PurchaseViewItem item);
    public static event ItemReceivedAction OnImageReceived;


    void OnEnable()
    {
        InputReader.OnPurchaseSubmitted += SubmitPurchaseRequestWrapper;
    }

    void OnDisable()
    {
        InputReader.OnPurchaseSubmitted -= SubmitPurchaseRequestWrapper;
    }

    IEnumerator Start()
    {
        yield return StartCoroutine(nameof(PurchaseViewPostRequest), url);
        StartCoroutine(nameof(GetItemImage));
    }

    // Sending a POST request to query the item from the server.
    IEnumerator PurchaseViewPostRequest(string url)
    {
        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes("{}"));
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            string replaceString = webRequest.downloadHandler.text.Replace("\'", "\"");
            item = JsonUtility.FromJson<PurchaseViewItem>(replaceString);
        }
    }

    IEnumerator GetItemImage()
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(item.item_image);
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
    void SubmitPurchaseRequestWrapper(string postData)
    {
        StartCoroutine(nameof(SubmitPurchasePostRequest), postData);
    }


    //Sending a POST request to the server with the user details
    IEnumerator SubmitPurchasePostRequest(string postData)
    {
        UnityWebRequest webRequest = new UnityWebRequest("https://6u3td6zfza.execute-api.us-east-2.amazonaws.com/prod/v1/gcom/action", "POST");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData));
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.LogError(webRequest.error);
            OnPurchaseResponseFromServer?.Invoke(webRequest.error);
        }
        else
        {
            string replaceString = webRequest.downloadHandler.text.Replace("\'", "\"");
            ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(replaceString);
            OnPurchaseResponseFromServer?.Invoke(serverResponse.status + "!\n" + serverResponse.user_message);
        }
    }
}

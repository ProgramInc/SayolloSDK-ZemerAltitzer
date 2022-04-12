using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml;

public class VideoRequestHandler : MonoBehaviour
{
    [Header("Enter Ad URL:")]
    [SerializeField] string adUri;
    public string EscapedPathToFile { get; private set; }
    public string VideoLink { get; private set; }


    public void RequestAd()
    {
        StartCoroutine(GetRequest(adUri));
        StartCoroutine(DownloadVideoIfDoesntExistOnDevice());
    }

    IEnumerator GetRequest(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            Debug.LogError("Please enter an Ad URL and try again..");
            yield break;
        }

        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError("Something Went Wrong When Trying To Find The Ad.\nPlease Check Your network connection and Ad URL And Try Again..");
            yield break;
        }
        else
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(webRequest.downloadHandler.text);
            }
            catch (XmlException)
            {
                throw new XmlException("Something went wrong while trying to load the XML");
            }

            //Parse the XML to get the link to the Video
            XmlNode root = xmlDocument.FirstChild;
            if (root.HasChildNodes)
            {
                XmlNode secondChild = root.ChildNodes[0].FirstChild;
                string rawLink = secondChild.InnerText;
                VideoLink = rawLink.Substring(8);
            }
        }
    }

    IEnumerator DownloadVideoIfDoesntExistOnDevice()
    {
        while (VideoLink == null)
        {
            yield return null;
        }

        string escapedFileName = UnityWebRequest.EscapeURL(VideoLink);
        EscapedPathToFile = string.Format("{0}/{1}", Application.persistentDataPath, escapedFileName);

        if (System.IO.File.Exists(EscapedPathToFile))
        {
            yield break;
        }
        else
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(VideoLink);
            yield return webRequest.SendWebRequest();
            {
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.LogError("An Error Occured, Please Check Your Network Connection and Ad URL And Try Again..");
                    yield break;
                }

                System.IO.File.WriteAllBytes(EscapedPathToFile, webRequest.downloadHandler.data);
            }
        }
    }
}

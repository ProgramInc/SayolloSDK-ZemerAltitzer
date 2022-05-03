using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using TMPro;


[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(VideoRequestHandler))]
public class VideoHandler : MonoBehaviour
{
    [Tooltip("Use this value to rescale the Ad to your needs")]
    [Range(0.01f, 10f)]
    [SerializeField] float rescaleFactor = 1f;

    VideoPlayer videoPlayer;
    VideoRequestHandler videoRequestHandler;
    GameObject adLoadingPanel;
    TextMeshProUGUI adLoadingText;
    bool wasPlaying;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoRequestHandler = GetComponent<VideoRequestHandler>();
        adLoadingPanel = GetComponentInChildren<Canvas>().gameObject;
        adLoadingText = adLoadingPanel.GetComponentInChildren<TextMeshProUGUI>();
        adLoadingPanel.gameObject.SetActive(true);
        adLoadingText.text = "Loading Ad..";
        wasPlaying = false;
        
        videoRequestHandler.StartCoroutine(videoRequestHandler.RequestAd());
    }

    private async void OnBecameVisible()
    {
        if (!wasPlaying)
        {
            await PrepareVideoAdAsync();
            videoPlayer.Play();
            wasPlaying = true;
        }
        else
        {
            videoPlayer.Play();
        }
    }

    private void OnBecameInvisible()
    {
        videoPlayer.Pause();
    }

    private async Task PrepareVideoAdAsync()
    {
        await GetVideoLinkAsync();
        await PrepareVideoAsync();

        //Rescale The Ad while maintaining it's aspect ratio
        transform.localScale = new Vector3(videoPlayer.width * rescaleFactor / 100, videoPlayer.height * rescaleFactor / 100, 1f);

        adLoadingPanel.gameObject.SetActive(false);
    }

    private async Task GetVideoLinkAsync()
    {
        //Wait for VideoRequestHandler to prepare the file
        videoPlayer.url = await WaitForLinkAsync();

        //Check if the file already exists locally. If it does - play it from disk. If not - play it directly from URL.
        if (System.IO.File.Exists(videoRequestHandler.EscapedPathToFile))
        {
            videoPlayer.url = videoRequestHandler.EscapedPathToFile;
        }
        else
        {
            return;
        }
    }

    private async Task<string> WaitForLinkAsync()
    {
        while (videoRequestHandler.VideoLink == null)
        {
            await Task.Yield();
        }
        return videoRequestHandler.VideoLink;
    }

    private async Task PrepareVideoAsync()
    {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            await Task.Yield();
        }
    }
}

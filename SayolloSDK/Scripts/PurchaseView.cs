using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(PurchaseViewRequestHandler))]

public class PurchaseView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] TextMeshProUGUI itemTitle;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemPrice;
    [SerializeField] RawImage itemImage;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject itemPanel;
    [SerializeField] GameObject purchasePanel;
    [SerializeField] GameObject purchaseCompletePanel;


    void Awake()
    {
        itemPanel.SetActive(false);
        purchasePanel.SetActive(false);
        purchaseCompletePanel.SetActive(false);
        loadingText.text = "Loading Item..";

        if (!loadingPanel.activeSelf)
        {
            loadingPanel.SetActive(true);
        }
    }

    void OnEnable()
    {
        PurchaseViewRequestHandler.OnImageReceived += DisplayItem;
        PurchaseViewRequestHandler.OnPurchaseResponseFromServer += PurchaseConfimed;
    }

    void OnDisable()
    {
        PurchaseViewRequestHandler.OnImageReceived -= DisplayItem;
        PurchaseViewRequestHandler.OnPurchaseResponseFromServer -= PurchaseConfimed;
    }

    void DisplayItem(Vector2 imageSize, Texture texture, PurchaseViewItem item)
    {
        itemImage.rectTransform.sizeDelta = imageSize;
        itemImage.texture = texture;
        itemTitle.text = item.title;
        itemName.text = item.item_name;
        itemPrice.text = string.Format("{0}{1} ({2})", item.price, item.currency_sign, item.currency);

        itemPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    void PurchaseConfimed(string statusMessage)
    {
        purchaseCompletePanel.GetComponentInChildren<TextMeshProUGUI>().SetText(statusMessage);
        purchasePanel.SetActive(false);
        purchaseCompletePanel.SetActive(true);
    }
}
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(PurchaseViewRequestHandler))]

public class PurchaseView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] TextMeshProUGUI itemTitle;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemPrice;
    [SerializeField] RawImage itemImage;
    [SerializeField] GameObject itemPanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject purchasePanel;
    [SerializeField] GameObject purchaseCompletePanel;

    public PurchaseViewItem item;

    private void Awake()
    {
        itemPanel.SetActive(false);
        purchasePanel.SetActive(false);
        purchaseCompletePanel.SetActive(false);
        loadingText.text = "Loading Item..";
    }

    private void OnEnable()
    {
        PurchaseViewRequestHandler.OnImageReceived += DisplayItem;
        PurchaseViewRequestHandler.OnServerConfirmedOrder += PurchaseConfimed;
    }

    private void OnDisable()
    {
        PurchaseViewRequestHandler.OnImageReceived -= DisplayItem;
        PurchaseViewRequestHandler.OnServerConfirmedOrder -= PurchaseConfimed;
    }

    void DisplayItem(Vector2 imageSize, Texture texture, PurchaseViewItem item)
    {
        itemImage.rectTransform.sizeDelta = imageSize;
        itemImage.texture = texture;
        itemTitle.text = item.title.Substring(1, item.title.Length - 2);
        itemName.text = item.itemName.Substring(1, item.itemName.Length - 2);
        itemPrice.text = string.Format("{0}{1} ({2})", item.price.Substring(1, item.price.Length - 1), item.currencySign.Substring(1, item.currencySign.Length - 2), item.currency.Substring(1, item.currency.Length - 2));
        this.item = item;
        itemPanel.SetActive(true);
        loadingPanel.SetActive(false);
    }

    void PurchaseConfimed()
    {
        purchasePanel.SetActive(false);
        purchaseCompletePanel.SetActive(true);
    }
}

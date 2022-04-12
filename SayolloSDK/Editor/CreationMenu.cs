using UnityEngine;
using UnityEditor;

public class CreationMenu : MonoBehaviour
{
    [MenuItem("/Sayollo/Create Ad Spot", false, 10)]
    static void CreateAdSpot(MenuCommand menuCommand)
    {
        GameObject videoAd = Instantiate(Resources.Load("Prefabs/Video Ad", typeof(GameObject))) as GameObject;
        videoAd.name = "Video Ad";
        Undo.RegisterCreatedObjectUndo(videoAd, "Create " + videoAd.name);
        Selection.activeObject = videoAd;
    }

    [MenuItem("/Sayollo/Create Item Purchase", false, 10)]
    static void CreateItemPurchase(MenuCommand menuCommand)
    {
        GameObject itemPurchase = Instantiate(Resources.Load("Prefabs/Purchase View", typeof(GameObject))) as GameObject;
        itemPurchase.name = "Purchase View";
        Undo.RegisterCreatedObjectUndo(itemPurchase, "Create " + itemPurchase.name);
        Selection.activeObject = itemPurchase; 
    }
}

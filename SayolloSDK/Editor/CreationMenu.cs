using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;


public class CreationMenu : MonoBehaviour
{
    [MenuItem("/Sayollo/Create Ad Spot", false, 1)]
    static void CreateAdSpot()
    {
        GameObject videoAd = Instantiate(Resources.Load("Prefabs/Video Ad", typeof(GameObject))) as GameObject;
        videoAd.transform.position = SceneView.lastActiveSceneView.camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 20f));
        videoAd.transform.rotation = SceneView.lastActiveSceneView.camera.gameObject.transform.rotation;
        videoAd.name = "Video Ad";
        Undo.RegisterCreatedObjectUndo(videoAd, "Create " + videoAd.name);
        Selection.activeObject = videoAd;
    }

    [MenuItem("/Sayollo/Create Item Purchase", false, 2)]
    static void CreateItemPurchase()
    {
        GameObject itemPurchase = Instantiate(Resources.Load("Prefabs/Purchase View", typeof(GameObject))) as GameObject;
        itemPurchase.name = "Purchase View";
        Undo.RegisterCreatedObjectUndo(itemPurchase, "Create " + itemPurchase.name);
        Selection.activeObject = itemPurchase;

        CreateEventSystemIfNoneExist();
    }

    static void CreateEventSystemIfNoneExist()
    {
        if (!FindObjectOfType<EventSystem>())
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }
}

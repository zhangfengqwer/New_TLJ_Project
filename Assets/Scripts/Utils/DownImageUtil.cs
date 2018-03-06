using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownImageUtil : MonoBehaviour
{
    public Image image;
    public string url;

    void Start()
    {
        StartCoroutine(GetImage());
    }

    IEnumerator GetImage()
    {
        UnityWebRequest www = UnityWebRequest.GetTexture(url);
        yield return www.Send();

        if (www.isError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture) www.downloadHandler).texture;
            image.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
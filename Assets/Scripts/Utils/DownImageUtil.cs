using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownImageUtil : MonoBehaviour
{
    public Image image;
    public string url;
    public static Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();

    void Start()
    {
        Texture2D texture;
        if (Images.TryGetValue(url, out texture))
        {
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            StartCoroutine(GetImage());
        }
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
            Texture2D texture = ((DownloadHandlerTexture) www.downloadHandler).texture;
            if (!Images.ContainsKey(url))
            {
                Images.Add(url, texture);
            }

            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
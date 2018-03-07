using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownImageUtil : MonoBehaviour
{
    public Image m_image;
    public string m_url;
    public static Dictionary<string, Texture2D> Images = new Dictionary<string, Texture2D>();

    void Start()
    {
    }

    public void startDown(string url)
    {
        m_url = url;
        m_image = gameObject.GetComponent<Image>();

        Texture2D texture;
        if (Images.TryGetValue(m_url, out texture))
        {
            m_image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            StartCoroutine(GetImage());
        }
    }

    IEnumerator GetImage()
    {
        UnityWebRequest www = UnityWebRequest.GetTexture(m_url);
        yield return www.Send();

        if (www.isError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture) www.downloadHandler).texture;
            if (!Images.ContainsKey(m_url))
            {
                Images.Add(m_url, texture);
            }

            m_image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
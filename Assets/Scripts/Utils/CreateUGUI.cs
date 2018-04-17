using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

class CreateUGUI
{
    public static GameObject createImageObj(Sprite sprite)
    {
        GameObject obj = new GameObject();
        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();
        return obj;
    }

    public static GameObject createImageObj(GameObject parent, Sprite sprite)
    {
        GameObject obj = new GameObject();
        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();

        obj.transform.SetParent(parent.transform);
        obj.transform.localScale = new Vector3(1, 1, 1);

        return obj;
    }

    public static GameObject createButtonObj(UnityAction onClick, Sprite sprite)
    {
        GameObject obj = new GameObject();

        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.SetNativeSize();

        Button btn = obj.AddComponent<Button>();
        btn.onClick.AddListener(onClick);

        return obj;
    }
}
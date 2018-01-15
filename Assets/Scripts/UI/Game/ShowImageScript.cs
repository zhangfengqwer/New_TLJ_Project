using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowImageScript : MonoBehaviour
{
    public static GameObject create(string imagePath,Vector3 vec3)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/ShowImage") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.transform.localPosition = vec3;
        CommonUtil.setImageSprite(obj.GetComponent<Image>(), imagePath);
        obj.GetComponent<Image>().SetNativeSize();

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShowImageScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShowImageScript", "Start", null, null);
            return;
        }

        Invoke("onInvoke", 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onInvoke()
    {
        Destroy(gameObject);
    }
}

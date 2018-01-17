using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHeadButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChangeHeadButtonScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChangeHeadButtonScript_hotfix", "Start", null, null);
            return;
        }

        List<string> list = new List<string>();
        CommonUtil.splitStr(UserData.head, list, '_');

        if (list.Count == 2)
        {
            int myCurHead = int.Parse(list[1]);
            ChangeHeadPanelScript.s_instance.m_choiceHead = myCurHead;

            for (int i = 0; i < 18; i++)
            {
                if (gameObject.transform.parent.Find((i + 1).ToString()).name == myCurHead.ToString())
                {
                    gameObject.transform.parent.GetChild(i).Find("Image").localScale = new Vector3(1, 1, 1);

                    return;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickHeadItem()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChangeHeadButtonScript_hotfix", "onClickHeadItem"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChangeHeadButtonScript_hotfix", "onClickHeadItem", null, null);
            return;
        }

        ChangeHeadPanelScript.s_instance.m_choiceHead = int.Parse(gameObject.transform.name);

        gameObject.transform.Find("Image").localScale = new Vector3(1,1,1);

        for (int i = 0; i < 18; i++)
        {
            if (gameObject.transform.parent.Find((i + 1).ToString()).name != gameObject.transform.name)
            {
                gameObject.transform.parent.GetChild(i).Find("Image").localScale = new Vector3(0,0,0);
            }
        }
    }
}

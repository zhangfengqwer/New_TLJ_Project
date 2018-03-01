using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign30LeiJiPanelScript : MonoBehaviour {

    public static int m_id = -1;
    public Text m_text_title;
    public Button m_btn_lingqujiangli;

    public static GameObject create(int id)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/Sign30LeiJiPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        m_id = id;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_sign30LeiJiPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30LeiJiPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30LeiJiPanelScript_hotfix", "Start", null, null);
            return;
        }

        if (Sign30Data.getInstance().getSign30DataContentList().Count == 0)
        {
            Debug.Log("签到奖励配置表未赋值");
            return;
        }

        Sign30DataContent temp = Sign30Data.getInstance().getSign30DataById(m_id);

        m_text_title.text = ("本月累计签到" + temp.day.ToString() + "天");

        // 领取奖励按钮状态
        {
            // 未达成
            if (temp.day > Sign30RecordData.getInstance().getSign30RecordList().Count)
            {
                CommonUtil.setButtonEnable(m_btn_lingqujiangli, false);
            }
            else
            {
                bool isGet = false;

                for (int i = 0; i < Sign30RecordData.getInstance().getSign30LeiJiRecordList().Count; i++)
                {
                    if (Sign30RecordData.getInstance().getSign30LeiJiRecordList()[i] == m_id)
                    {
                        isGet = true;
                        break;
                    }
                }

                // 达成已领取
                if (isGet)
                {
                    CommonUtil.setButtonEnable(m_btn_lingqujiangli, false);
                    CommonUtil.setImageSprite(m_btn_lingqujiangli.transform.Find("Image").GetComponent<Image>(), "Sprites/Sign30/wz_yilingqu");
                    m_btn_lingqujiangli.transform.Find("Image").GetComponent<Image>().SetNativeSize();
                }
                // 达成未领取
                else
                {
                    CommonUtil.setButtonEnable(m_btn_lingqujiangli, true);
                }
            }
        }

        // 奖励
        {
            List<string> list1 = new List<string>();
            CommonUtil.splitStr(temp.reward_prop, list1, ';');

            for (int i = 0; i < list1.Count; i++)
            {
                List<string> list2 = new List<string>();
                CommonUtil.splitStr(list1[i], list2, ':');

                int prop_id = int.Parse(list2[0]);
                int prop_num = int.Parse(list2[1]);

                GameObject obj = transform.Find("Image_bg/Reward_" + (i + 1).ToString()).gameObject;
                obj.transform.localScale = new Vector3(1, 1, 1);
                CommonUtil.setImageSprite(obj.transform.Find("Image").GetComponent<Image>(), GameUtil.getPropIconPath(prop_id));
                obj.transform.Find("Text").GetComponent<Text>().text = prop_num.ToString();

                obj.transform.localPosition = new Vector3(CommonUtil.getPosX(list1.Count, 130, i, 0), 0, 0);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickLingQuJiangLi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30LeiJiPanelScript_hotfix", "onClickLingQuJiangLi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30LeiJiPanelScript_hotfix", "onClickLingQuJiangLi", null, null);
            return;
        }

        if (m_id == -1)
        {
            return;
        }

        OtherData.s_sign30PanelScript.reqSign(m_id);
    }
}

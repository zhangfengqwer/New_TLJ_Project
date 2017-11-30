﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRewardPanelScript : MonoBehaviour {

    static List<string> s_rewardList = new List<string>();

    public Image m_image_itemContent;
    public Button m_button_share;

    public static void Show(string reward, bool isCanShare)
    {
        if (s_rewardList.Count == 0)
        {
            s_rewardList.Add(reward);

            GameObject prefab = Resources.Load("Prefabs/UI/Panel/ShowRewardPanel") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

            obj.GetComponent<ShowRewardPanelScript>().setData(reward, isCanShare);
        }
        else
        {
            s_rewardList.Add(reward);
        }
    }

    public void setData(string reward, bool isCanShare)
    {
        List<string> list1 = new List<string>();
        CommonUtil.splitStr(reward,list1,';');

        for (int i = 0; i < list1.Count; i++)
        {
            List<string> list2 = new List<string>();
            CommonUtil.splitStr(list1[i], list2, ':');

            int id = int.Parse(list2[0]);
            int num = int.Parse(list2[1]);

            GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_reward") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab, m_image_itemContent.transform);

            CommonUtil.setImageSprite(obj.transform.Find("Image_icon").GetComponent<Image>(),GameUtil.getPropIconPath(id));
            obj.transform.Find("Text_num").GetComponent<Text>().text = "x" + num;

            float x = CommonUtil.getPosX(list1.Count,130,i,0);
            obj.transform.localPosition = new Vector3(x,0,0);
        }

        if (!isCanShare)
        {
            GameUtil.hideGameObject(m_button_share.gameObject);
        }
    }

    public void onClickClose()
    {
        s_rewardList.RemoveAt(0);
        Destroy(gameObject);

        if (s_rewardList.Count > 0)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Panel/ShowRewardPanel") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

            obj.GetComponent<ShowRewardPanelScript>().setData(s_rewardList[0],false);
        }
    }

    public void onClickShare()
    {

    }
}

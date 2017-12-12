using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPokerScript : MonoBehaviour
{
    // 请求自定义牌型
    public static void reqCustomPoker(string uid,List<TLJCommon.PokerInfo> pokerList)
    {
        if (pokerList.Count != 25)
        {
            ToastScript.createToast("张数必须为25");
            return;
        }

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_DebugSetPoker;
        data["uid"] = uid;

        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < pokerList.Count; i++)
            {
                JsonData jd = new JsonData();
                jd["num"] = pokerList[i].m_num;
                jd["pokerType"] = (int)pokerList[i].m_pokerType;
                jarray.Add(jd);
            }
            data["pokerList"] = jarray;
        }

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }
}

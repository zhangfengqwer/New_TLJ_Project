using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PVPEndPanelScript : MonoBehaviour {

    public GameScript m_parentScript;

    public Image m_image_itemContent;
    public Text m_text_mingci;
    public GameObject ShareObject;

    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PVPEndPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<PVPEndPanelScript>().m_parentScript = parentScript;

        return obj;
    }

    // Use this for initialization
    void Start()
    {
//        ShareObject.GetComponent<Button>().onClick.RemoveAllListeners();
//        ShareObject.GetComponent<Button>().onClick.AddListener(() =>
//        {
//            ShareObject.transform.localScale = Vector3.zero;
//        });
    }

    public void setData(int mingci,string pvpreward)
    {
        m_text_mingci.text = mingci.ToString();

        List<string> list1 = new List<string>();
        CommonUtil.splitStr(pvpreward, list1, ';');

        for (int i = 0; i < list1.Count; i++)
        {
            List<string> list2 = new List<string>();
            CommonUtil.splitStr(list1[i], list2, ':');

            int id = int.Parse(list2[0]);
            int num = int.Parse(list2[1]);

            GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_reward") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab, m_image_itemContent.transform);
            obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            CommonUtil.setImageSprite(obj.transform.Find("Image_icon").GetComponent<Image>(), GameUtil.getPropIconPath(id));
            obj.transform.Find("Text_num").GetComponent<Text>().text = "x" + num;

            float x = CommonUtil.getPosX(list1.Count, 130, i, 0);
            obj.transform.localPosition = new Vector3(x, 0, 0);
        }
    }

    public void onClickExit()
    {
        //m_parentScript.onClickExitRoom();
        SceneManager.LoadScene("MainScene");
    }

    public void onClickShare()
    {
        //m_parentScript.onClickExitRoom();
        //        ToastScript.createToast("暂未开放");
        ShareObject.transform.localScale = Vector3.one;
    }

    public void OnClickShareClose()
    {
        ShareObject.transform.localScale = Vector3.zero;
    }

    public void OnClickShareFriends()
    {
        string content = string.Format("我获得了第{0}名", m_text_mingci.text);
        PlatformHelper.WXShareFriends("AndroidCallBack", "OnWxShareFriends", content);
    }

    public void OnClickShareFriendsCircle()
    {
        GameObject go = ShareFreindsCircleScript.create();
        Text Text_Name = go.transform.Find("Text_Name").GetComponent<Text>();
        Text Text_ChangCi = go.transform.Find("Text_ChangCi").GetComponent<Text>();
        Image Image_Ranking = go.transform.Find("Image_Ranking").GetComponent<Image>();

        Text_Name.text = "1124";
        Text_ChangCi.text = "经典场";
        string path = Application.dataPath + "/Resources/ScreenShot1.png";
        print(path);
        Application.CaptureScreenshot(path, 0);
        Destroy(go);
    }
}

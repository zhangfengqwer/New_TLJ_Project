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
    private GameObject ShareObject;

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
        string content = string.Format("我获得了第{0}名", m_text_mingci.text);
        ShareObject = ChoiceShareScript.Create(content,"");
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
        //        string path = Application.dataPath + "/Resources/ScreenShot1.png";
        //        LogUtil.Log(path);
        //        Application.CaptureScreenshot(path, 0);
        StartCoroutine(MyCaptureScreen(go));
       
    }

    IEnumerator MyCaptureScreen(GameObject go)
    {
        //等待所有的摄像机和GUI被渲染完成。
        yield return new WaitForEndOfFrame();
        //创建一个空纹理（图片大小为屏幕的宽高）
        Texture2D tex = new Texture2D(Screen.width, Screen.height);
        //只能在帧渲染完毕之后调用（从屏幕左下角开始绘制，绘制大小为屏幕的宽高，宽高的偏移量都为0）
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        //图片应用（此时图片已经绘制完成）
        tex.Apply();
        //将图片装换成png的二进制格式，保存在byte数组中（计算机是以二进制的方式存储数据）
        byte[] result = tex.EncodeToPNG();
        PlatformHelper.WXShareFriendsCircle("AndroidCallBack", "OnWxShareFriends", result);
        //文件保存，创建一个新文件，在其中写入指定的字节数组（要写入的文件的路径，要写入文件的字节。）
        string path = Application.dataPath + "/Resources/ScreenShot1.png";
        System.IO.File.WriteAllBytes(path, result);
        Destroy(go);
    }
}

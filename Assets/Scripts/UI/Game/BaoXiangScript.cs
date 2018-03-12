using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaoXiangScript : MonoBehaviour {

    public float m_speed = 1;
    public int screen_width = Screen.width;
    public int screen_height = Screen.height;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Game/BaoXiang") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        //m_speed = RandomUtil.getRandom(300, 600) / 100.0f;
        m_speed = Random.Range(300, 600) / 100.0f;
        gameObject.transform.localScale = new Vector3(1,1,1);

        //int width = RandomUtil.getRandom(30,70);
        int width = Random.Range(30, 70);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, width);

        {
            screen_width = Screen.width;
            screen_height = Screen.height;

            float pos_x = RandomUtil.getRandom(-screen_width / 2, screen_width / 2);
            //float pos_y = screen_height / 2 + RandomUtil.getRandom(0, 600);
            float pos_y = screen_height / 2 + Random.Range(0, 600);
            gameObject.transform.localPosition = new Vector3(pos_x, pos_y, 1);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.localPosition -= new Vector3(0,m_speed,0);

        if (gameObject.transform.localPosition.y < (-screen_height / 2))
        {
            Destroy(gameObject);
        }
    }
}

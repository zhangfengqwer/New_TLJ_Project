using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PVPGameRoomDataScript
{
    static PVPGameRoomDataScript s_instance = null;

    List<PVPGameRoomData> m_dataList = new List<PVPGameRoomData>();

    public static PVPGameRoomDataScript getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new PVPGameRoomDataScript();
        }

        return s_instance;
    }

    public void initJson(string json)
    {
        m_dataList.Clear();

        JsonData jsonData = JsonMapper.ToObject(json);
        
        for (int i = 0; i < jsonData["room_list"].Count; i++)
        {
            PVPGameRoomData temp = new PVPGameRoomData();

            temp.id = (int)jsonData["room_list"][i]["id"];
            temp.gameroomtype = (string)jsonData["room_list"][i]["gameroomtype"];
            temp.gameroomname = (string)jsonData["room_list"][i]["gameroomname"];
            temp.kaisairenshu = (int)jsonData["room_list"][i]["kaisairenshu"];
            temp.baomingrenshu = (int)jsonData["room_list"][i]["baomingrenshu"];

            // 报名费
            {
                temp.baomingfei = (string)jsonData["room_list"][i]["baomingfei"];

                if (temp.baomingfei.CompareTo("0") != 0)
                {
                    List<string> list = new List<string>();
                    CommonUtil.splitStr(temp.baomingfei, list, ':');

                    temp.baomingfei_id = int.Parse(list[0]);
                    temp.baomingfei_num = int.Parse(list[1]);
                }
            }

            // 奖励
            {
                temp.reward = (string)jsonData["room_list"][i]["reward"];

                List<string> list = new List<string>();
                CommonUtil.splitStr(temp.reward,list,':');

                temp.reward_id = int.Parse(list[0]);
                temp.reward_num = int.Parse(list[1]);
            }

            // 已报名人数增加点
            temp.baomingrenshu += RandomUtil.getRandom(100, 200);

            m_dataList.Add(temp);
        }
    }

    public List<PVPGameRoomData> getDataList()
    {
        return m_dataList;
    }

    public PVPGameRoomData getDataByRoomType(string gameRoomType)
    {
        PVPGameRoomData temp = null;
        for (int i = 0; i < m_dataList.Count; i++)
        {
            if (m_dataList[i].gameroomtype.CompareTo(gameRoomType) == 0)
            {
                temp = m_dataList[i];
                break;
            }
        }

        return temp;
    }

    public PVPGameRoomData getDataById(int id)
    {
        PVPGameRoomData temp = null;
        for (int i = 0; i < m_dataList.Count; i++)
        {
            if (m_dataList[i].id == id)
            {
                temp = m_dataList[i];
                break;
            }
        }

        return temp;
    }
}

public class PVPGameRoomData
{
    public int id;
    public string gameroomtype;
    public string gameroomname;
    public int kaisairenshu;

    public string baomingfei;
    public int baomingfei_id;
    public int baomingfei_num;

    public string reward;
    public int reward_id;
    public int reward_num;

    public int baomingrenshu;
}
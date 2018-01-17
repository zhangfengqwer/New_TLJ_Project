using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TurntableDataScript
{
    public static TurntableDataScript s_instance = null;

    public List<TurntableData> m_dataList = new List<TurntableData>();

    public static TurntableDataScript getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new TurntableDataScript();
        }

        return s_instance;
    }

    public void clear()
    {
        m_dataList.Clear();
    }

    public void initJson(string json)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntableDataScript_hotfix", "initJson"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntableDataScript_hotfix", "initJson", null, json);
            return;
        }

        m_dataList.Clear();

        {
            JsonData jd = JsonMapper.ToObject(json);
            
            for (int i = 0; i < jd["turntable_list"].Count; i++)
            {
                int id = (int)jd["turntable_list"][i]["id"];
                string reward = (string)jd["turntable_list"][i]["reward"];
                int probability = (int)jd["turntable_list"][i]["probability"];

                TurntableData temp = new TurntableData(id, reward, probability);
                m_dataList.Add(temp);
            }
        }
    }

    public List<TurntableData> getDataList()
    {
        return m_dataList;
    }

    public TurntableData getDataById(int id)
    {
        TurntableData temp = null;
        for (int i = 0; i < m_dataList.Count; i++)
        {
            if (m_dataList[i].m_id == id)
            {
                temp = m_dataList[i];
                break;
            }
        }

        return temp;
    }
}

public class TurntableData
{
    public int m_id;
    public string m_reward;
    public int m_probability;

    public TurntableData(int id, string reward, int probability)
    {
        m_id = id;
        m_reward = reward;
        m_probability = probability;
    }
}
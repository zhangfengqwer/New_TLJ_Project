using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using TLJCommon;
using UnityEngine.SceneManagement;

public class GetSignRecordRequest : Request
{
    private void Awake()
    {
        Tag = Consts.Tag_GetSignRecord;
    }


    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.Uid;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int) jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            LogicEnginerScript.IsSuccessList.Add(true);
            SignData.SignWeekDays = (int) jsonData["signWeekDays"];
            string update = (string) jsonData["updateTime"];
            DateTime updateTime;
            var isSuccess = DateTime.TryParse(update, out updateTime);
            if (isSuccess)
            {
                int updateTimeYear = updateTime.Year;
                int updateTimeMonth = updateTime.Month;
                int updateTimeDay = updateTime.Day;
                int nowYear = DateTime.Now.Year;
                int nowMonth = DateTime.Now.Month;
                int nowDay = DateTime.Now.Day;
                print(updateTimeYear + "-" + updateTimeMonth + "-" + updateTimeDay);
                //通过数据库的更新时间和本地时间作对比，判断是否签到过
                if (updateTimeYear == nowYear && updateTimeMonth == nowMonth && updateTimeDay == nowDay &&
                    SignData.SignWeekDays != 0)
                {
                    SignData.IsSign = true;
                }
                else
                {
                    SignData.IsSign = false;
                }


//                    int offsetTime = nowDay - updateTimeDay;
//                    if (offsetTime == 0 )
//                    {
//                        //客户端不能继续签到
//                        SignData.IsSign = true;
//                        SignData.IsContinuousSign = true;
//
//                    }  else if (offsetTime == 1)
//                    { 
//                        //客户端可以签到
//                        SignData.IsSign = false;
//                        //客户端是连续签到的
//                        SignData.IsContinuousSign = true;
//                       
//                    }
//                    else
//                    {  
//                        //客户端可以签到
//                        SignData.IsSign = false;
//                        //客户端不是连续签到的
//                        SignData.IsContinuousSign = false;
//                    }

                print("SignData.IsSign:" + SignData.IsSign
                      + "\nSignData.SignWeekDays:" + SignData.SignWeekDays);
            }
        }
    }
}
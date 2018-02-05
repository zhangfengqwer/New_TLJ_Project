using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherData
{
    public enum DefaultLoginType
    {
        DefaultLoginType_Default,
        DefaultLoginType_GuanFang,
        DefaultLoginType_QQ,
        DefaultLoginType_WeChat,
    }

    public static int s_defaultLoginType = (int) DefaultLoginType.DefaultLoginType_Default;

    public static string s_channelName = "";            // 渠道号
    public static string s_apkVersion = "1.0.50";       // apk版本号
    public static string s_codeVersion;                 // 代码版本
    public static string s_resVersion;                  // 资源版本

    public static bool s_isFromSetToLogin = false;
    public static bool s_isFirstOpenGame = true;
    public static bool s_isTest = true;
    public static bool s_canDebug = false;
    public static bool s_hasCheckSecondPSW = false;
    public static bool s_canRecharge = false; // 是否开放充值
    public static bool s_hasShouChong = false;

    public static Vector2 s_screenSize;

    // 七牛
    //public static string s_webStorageUrl = "http://p02gqb8lq.bkt.clouddn.com/";

    // web测试服
    static string s_webStorageUrl_test = "http://fwdown.hy51v.com/test/file/";

    // web正式服
    static string s_webStorageUrl = "http://fwdown.hy51v.com/online/file/";

    public static AboutPanelScript s_aboutPanelScript = null;
    public static BagPanelScript s_bagPanelScript = null;
    public static BindPhoneScript s_bindPhoneScript = null;
    public static BuyGoodsPanelScript s_buyGoodsPanelScript = null;
    public static ChangeHeadPanelScript s_changeHeadPanelScript = null;
    public static CheckSecondPSWPanelScript s_checkSecondPSWPanelScript = null;
    public static ChoiceShareScript s_choiceShareScript = null;
    public static EmailPanelScript s_emailPanelScript = null;
    public static ExitGamePanelScript s_exitGamePanelScript = null;
    public static ExplainPanelScript s_explainPanelScript = null;
    public static GetNetEntityFile s_getNetEntityFile = null;
    public static MailDetailScript s_mailDetailScript = null;
    public static MainScript s_mainScript = null;
    public static MedalExplainPanelScript s_medalExplainPanelScript = null;
    public static NoticeDetailScript s_noticeDetailScript = null;
    public static NoticePanelScript s_noticePanelScript = null;
    public static GameLevelChoiceScript s_gameLevelChoiceScript = null;
    public static GameResultPanelScript s_gameResultPanelScript = null;
    public static GameScript s_gameScript = null;
    public static HasInRoomPanelScript s_hasInRoomPanelScript = null;
    public static KeFuPanelScript s_keFuPanelScript = null;
    public static LaBaPanelScript s_laBaPanelScript = null;
    public static LaBaScript s_laBaScript = null;
    public static LoginScript s_loginScript = null;
    public static PropDetailPanelScript s_propDetailPanelScript = null;
    public static PVPChoiceScript s_pvpChoiceScript = null;
    public static PVPEndPanelScript s_pvpEndPanelScript = null;
    public static PVPGameResultPanelScript s_pvpGameResultPanelScript = null;
    public static QueRenBaoMingPanelScript s_queRenBaoMingPanelScript = null;
    public static QueRenExitPanelScript s_queRenExitPanelScript = null;
    public static RankListCaifuScript s_rankListCaifuScript = null;
    public static RankListJifenScript s_rankListJifenScript = null;
    public static RealNameScript s_realNameScript = null;
    public static SetScript s_setScript = null;
    public static SetSecondPswPanelScript s_setSecondPswPanelScript = null;
    public static ShareFreindsCircleScript s_shareFreindsCircleScript = null;
    public static ShopPanelScript s_shopPanelScript = null;
    public static ShouChongPanelScript s_shouChongPanelScript = null;
    public static ShowRewardPanelScript s_showRewardPanelScript = null;
    public static TaskPanelScript s_taskPanelScript = null;
    public static TuoGuanPanelScript s_tuoGuanPanelScript = null;
    public static TurntablePanelScript s_turntablePanelScript = null;
    public static TurntableTipPanelScript s_turntableTipPanelScript = null;
    public static UserAgreeMentScript s_userAgreeMentScript = null;
    public static UserInfoScript s_userInfoScript = null;
    public static VipPanelScript s_vipPanelScript = null;
    public static WaitMatchPanelScript s_waitMatchPanelScript = null;
    public static WaitOtherPlayerScript s_waitOtherPlayerScript = null;
    public static WeeklySignScript s_weeklySignScript = null;
    public static OldPlayerBindPanelScript s_oldPlayerBindPanelScript = null;

    public static string getWebUrl()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherData_hotfix", "getWebUrl"))
        {
            string s = (string)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherData_hotfix", "getWebUrl", null, null);
            return s;
        }

        if (s_isTest)
        {
            return s_webStorageUrl_test;
        }
        else
        {
            return s_webStorageUrl;
        }
    }
}
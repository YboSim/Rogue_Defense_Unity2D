using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeBox : MonoBehaviour
{
    public Button m_ExitBtn;
    public Button m_EasyBtn;
    public Button m_HardBtn;
    public Button m_HellBtn;
    public ScrollRect m_ScoreSR;

    bool m_IsInstantiated = false;

    public Text[] m_NickNameText = new Text[15];
    public Text[] m_ScoreText = new Text[15];

    // Start is called before the first frame update
    void Start()
    {
        m_IsInstantiated = true;

        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(() =>
            {
                Destroy(this.gameObject);

                Chapter_Mgr.m_ModeBoxOn = false;

                Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);
            });

        if (m_HardBtn != null)
            m_HardBtn.onClick.AddListener(() =>
            {
                if (Chapter_Mgr.m_MapIdx == 0)
                {
                    return;
                }
                else if (Chapter_Mgr.m_MapIdx == 1)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("BurialGDMap");
                    else
                        SceneManager.LoadScene("BurialGDMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 2)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("ArcticMap");
                    else
                        SceneManager.LoadScene("ArcticMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 3)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("ForestMap");
                    else
                        SceneManager.LoadScene("ForestMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 4)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("DesertMap");
                    else
                        SceneManager.LoadScene("DesertMap");
                }

                Game_Mgr.m_GameMode = GameMode.Hard;

                Destroy(gameObject);

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_EasyBtn != null)
            m_EasyBtn.onClick.AddListener(() =>
            {
                if (Chapter_Mgr.m_MapIdx == 0)
                {
                    return;
                }
                else if (Chapter_Mgr.m_MapIdx == 1)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("BurialGDMap");
                    else
                        SceneManager.LoadScene("BurialGDMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 2)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("ArcticMap");
                    else
                        SceneManager.LoadScene("ArcticMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 3)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("ForestMap");
                    else
                        SceneManager.LoadScene("ForestMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 4)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("DesertMap");
                    else
                        SceneManager.LoadScene("DesertMap");
                }

                Game_Mgr.m_GameMode = GameMode.Easy;

                Destroy(gameObject);

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_HellBtn != null)
            m_HellBtn.onClick.AddListener(() =>
            {
                if (Chapter_Mgr.m_MapIdx == 0)
                {
                    return;
                }
                else if (Chapter_Mgr.m_MapIdx == 1)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("BurialGDMap");
                    else
                        SceneManager.LoadScene("BurialGDMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 2)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("ArcticMap");
                    else
                        SceneManager.LoadScene("ArcticMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 3)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("ForestMap");
                    else
                        SceneManager.LoadScene("ForestMap");
                }
                else if (Chapter_Mgr.m_MapIdx == 4)
                {
                    if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
                        Fade_Mgr.Inst.SceneOutReserve("DesertMap");
                    else
                        SceneManager.LoadScene("DesertMap");
                }
                Game_Mgr.m_GameMode = GameMode.Hell;

                Destroy(gameObject);

                Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        LoadBestScore(Chapter_Mgr.m_MapIdx);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsInstantiated == true)
        {
            transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            if (transform.localScale.x >= 1.0f)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                m_IsInstantiated = false;

                m_ScoreSR.vertical = true;
            }
        }
    }

    void LoadBestScore(int a_MapIdx)
    {
        if (GlobalValue.g_Unique_ID == "") //로그인 상태에서만...
            return;

        var request = new GetLeaderboardRequest
        {
            StartPosition = 0,      //0번인덱스 즉 1등부터
            StatisticName = "BestScore_" + a_MapIdx.ToString(), //관리자페이지의 순위표 변수 중 "BestScore_n" 기준
            MaxResultsCount = 15,   //15명까지
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true, //닉네임도 요청
            }
        };

        PlayFabClientAPI.GetLeaderboard(request,
            (result) =>
            {  //랭킹 리스트 받아오기 성공
                for (int ii = 0; ii < result.Leaderboard.Count; ii++)
                {
                    var curBoard = result.Leaderboard[ii];

                    //등수 안에 내가 있다면 색 표시
                    if (curBoard.PlayFabId == GlobalValue.g_Unique_ID)
                    {
                        m_ScoreText[ii].color = new Color(1, 0, 0);
                        m_NickNameText[ii].color = new Color(1, 0, 0);
                    }

                    m_NickNameText[ii].text = curBoard.DisplayName;
                    m_ScoreText[ii].text = curBoard.StatValue.ToString() + "Kill";
                }

            },
            (error) =>
            {  //랭킹 리스트 받아오기 실패
                //Debug.Log(error.ErrorMessage);
            }
     );
    }
}

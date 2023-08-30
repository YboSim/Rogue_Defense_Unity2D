using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Text.RegularExpressions;
using System.Globalization;
using System;

public class LogInBox : MonoBehaviour
{
    public GameObject m_LoginPanel;
    public GameObject m_CreateAccountPanel;

    public Button m_ExitBtn;
    [Header("------LogInPanel------")]
    public InputField L_ID_InputField;
    public InputField L_Pw_InputField;
    public Button L_LogIn_Btn;
    public Button L_CreateAccount_Btn;
    [Header("------CreateAccountPanel------")]
    public InputField C_ID_InputField;
    public InputField C_Pw_InputField;
    public InputField C_Nn_InputField;
    public Button C_Cancel_Btn;
    public Button C_CreateAccount_Btn;
    [Header("------Message------")]
    public Text MessageText;
    float ShowMsTimer = 0.0f;

    bool m_IsInstantiated = false;

    bool invalidEmailType = false;       // 이메일 포맷이 올바른지 체크
    bool isValidFormat = false;          // 올바른 형식인지 아닌지 체크

    // Start is called before the first frame update
    void Start()
    {
        m_IsInstantiated = true;

        if (m_ExitBtn != null)
            m_ExitBtn.onClick.AddListener(() =>
            {
                m_IsInstantiated = false;
                Destroy(this.gameObject);
            });

        if (L_LogIn_Btn != null)
            L_LogIn_Btn.onClick.AddListener(LoginBtn);

        if (L_CreateAccount_Btn != null)
            L_CreateAccount_Btn.onClick.AddListener(OpenCreateAccBtn);

        if (C_Cancel_Btn != null)
            C_Cancel_Btn.onClick.AddListener(CreateCancelBtn);

        if (C_CreateAccount_Btn != null)
            C_CreateAccount_Btn.onClick.AddListener(CreateAccountBtn);
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
            }
        }

        if (0.0f < ShowMsTimer)
        {
            ShowMsTimer -= Time.deltaTime;
            if (ShowMsTimer <= 0.0f)
            {
                MessageOnOff("", false); //메세지 끄기
            }
        }//if(0.0f < ShowMsTimer)
    }

    public void LoginBtn()
    {
        string a_IdStr = L_ID_InputField.text;
        string a_PwStr = L_Pw_InputField.text;

        a_IdStr = a_IdStr.Trim();
        a_PwStr = a_PwStr.Trim();

        if (string.IsNullOrEmpty(a_IdStr) == true ||
           string.IsNullOrEmpty(a_PwStr) == true)
        {
            MessageOnOff("ID, PW 빈칸 없이 입력해 주세요.");
            return;
        }

        if (!(6 <= a_IdStr.Length && a_IdStr.Length < 20)) //6 ~ 20
        {
            MessageOnOff("ID는 6글자 이상 20글자 이하로 작성해 주세요.");
            return;
        }

        if (!(6 <= a_PwStr.Length && a_PwStr.Length < 20)) //6 ~ 20
        {
            MessageOnOff("비밀번호는 6글자 이상 20글자 이하로 작성해 주세요.");
            return;
        }

        if (!CheckEmailAddress(a_IdStr))
        {
            MessageOnOff("Email 형식이 맞지 않습니다.");
            return;
        }

        //--- 로그인 성공시 어떤 유저 정보를 가져올지를 설정하는 옵션 객체 생성
        var option = new GetPlayerCombinedInfoRequestParams()
        {
            //--- DisplayName(닉네임)을 가져오기 위한 옵션
            GetPlayerProfile = true,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true,  //DisplayName(닉네임) 가져오기 위한 요청 옵션
                //ShowAvatarUrl = true     //AvatarUrl 을 가져오는 옵션
            },
            //--- DisplayName(닉네임)을 가져오기 위한 옵션

            //--- BestScore 통계값(순위표에 관여하는)을 불러올 수 있는 옵션
            GetPlayerStatistics = true,

            //--- < 플레이어 데이터(타이틀) > 값을 불러올 수 있게 하는 옵션
            GetUserData = true
        };

        var request = new LoginWithEmailAddressRequest
        {
            Email = a_IdStr,
            Password = a_PwStr,
            InfoRequestParameters = option
        };

        PlayFabClientAPI.LoginWithEmailAddress(request,
                                        OnLoginSuccess, OnLoginFailure);

        //SceneManager.LoadScene("scLobby");
    }
    void OnLoginSuccess(LoginResult result)
    {
        MessageOnOff("로그인 성공");

        GlobalValue.g_Unique_ID = result.PlayFabId;

        if (result.InfoResultPayload != null)
        {
            GlobalValue.g_NickName = result.InfoResultPayload.PlayerProfile.DisplayName;
            //닉네임 가져오기

            //--- 유저 통계값(순위표에 관여하는) BestScore(내점수) 불러오기  
            foreach (var eachStat in result.InfoResultPayload.PlayerStatistics)
            {
                if (eachStat.StatisticName == "BestScore_1") //BG
                {
                    GlobalValue.g_BestScore_1 = eachStat.Value;
                }
                else if (eachStat.StatisticName == "BestScore_2") //AC
                {
                    GlobalValue.g_BestScore_2 = eachStat.Value;
                }
                else if (eachStat.StatisticName == "BestScore_3") //FR
                {
                    GlobalValue.g_BestScore_3 = eachStat.Value;
                }
                else if (eachStat.StatisticName == "BestScore_4") //DS
                {
                    GlobalValue.g_BestScore_4 = eachStat.Value;
                }
            }
            //--- 유저 통계값(순위표에 관여하는) BestScore(내점수) 불러오기 
        }

        if (Fade_Mgr.Inst != null && Fade_Mgr.Inst.IsFadeOut == true)
            Fade_Mgr.Inst.SceneOutReserve("ChapterScene");
        else
            SceneManager.LoadScene("ChapterScene");

        m_IsInstantiated = false;
        //Destroy(this.gameObject);
    }

    void OnLoginFailure(PlayFabError error)
    {
        MessageOnOff("로그인 실패");
        Debug.Log("로그인 실패");
    }

    void MessageOnOff(string Mess = "", bool isOn = true)
    {
        if(isOn == true)
        {
            MessageText.text = Mess;
            MessageText.gameObject.SetActive(true);
            ShowMsTimer = 7.0f;
        }
        else
        {
            MessageText.text = "";
            MessageText.gameObject.SetActive(false);
        }
    }

    void OpenCreateAccBtn()
    {
        if (m_LoginPanel != null)
            m_LoginPanel.SetActive(false);

        if (m_CreateAccountPanel != null)
            m_CreateAccountPanel.SetActive(true);
    }

    void CreateCancelBtn()
    {
        if (m_LoginPanel != null)
            m_LoginPanel.SetActive(true);

        if (m_CreateAccountPanel != null)
            m_CreateAccountPanel.SetActive(false);
    }

    void CreateAccountBtn()
    {
        string a_IdStr = C_ID_InputField.text;
        string a_PwStr = C_Pw_InputField.text;
        string a_NickStr = C_Nn_InputField.text;

        a_IdStr = a_IdStr.Trim();
        a_PwStr = a_PwStr.Trim();
        a_NickStr = a_NickStr.Trim();


        if (string.IsNullOrEmpty(a_IdStr) == true ||
           string.IsNullOrEmpty(a_PwStr) == true ||
           string.IsNullOrEmpty(a_NickStr) == true)
        {
            MessageOnOff("ID, PW, 별명은 빈칸 없이 입력해 주세요.");
            return;
        }

        if (!(6 <= a_IdStr.Length && a_IdStr.Length < 20)) //6 ~ 20
        {
            MessageOnOff("ID는 6글자 이상 20글자 이하로 작성해 주세요.");
            return;
        }

        if (!(6 <= a_PwStr.Length && a_PwStr.Length < 20)) //6 ~ 20
        {
            MessageOnOff("비밀번호는 6글자 이상 20글자 이하로 작성해 주세요.");
            return;
        }

        if (!(1 <= a_NickStr.Length && a_NickStr.Length < 4)) //1 ~ 20
        {
            MessageOnOff("별명은 1글자 이상 3글자 이하로 작성해 주세요.");
            return;
        }

        if (!CheckEmailAddress(a_IdStr))
        {
            MessageOnOff("Email 형식이 맞지 않습니다.");
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = a_IdStr,
            Password = a_PwStr,
            DisplayName = a_NickStr,
            RequireBothUsernameAndEmail = false  //Email을 기본 ID로 사용하겠다는 옵션
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);

    }

    void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        MessageOnOff("가입 성공");
    }

    void RegisterFailure(PlayFabError error)
    {
        MessageOnOff("가입 실패");
    }

    //----------------- 이메일형식이 맞는지 확인하는 방법 스크립트
    private bool CheckEmailAddress(string EmailStr)
    {
        if (string.IsNullOrEmpty(EmailStr)) isValidFormat = false;

        EmailStr = Regex.Replace(EmailStr, @"(@)(.+)$", this.DomainMapper, RegexOptions.None);
        if (invalidEmailType) isValidFormat = false;

        // true 로 반환할 시, 올바른 이메일 포맷임.
        isValidFormat = Regex.IsMatch(EmailStr,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase);
        return isValidFormat;
    }

    private string DomainMapper(Match match)
    {
        // IdnMapping class with default property values.
        IdnMapping idn = new IdnMapping();

        string domainName = match.Groups[2].Value;
        try
        {
            domainName = idn.GetAscii(domainName);
        }
        catch (ArgumentException)
        {
            invalidEmailType = true;
        }
        return match.Groups[1].Value + domainName;
    }
    //----------------- 이메일형식이 맞는지 확인하는 방법 스크립트
}

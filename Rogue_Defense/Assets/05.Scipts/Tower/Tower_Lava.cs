using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_Lava : MonoBehaviour
{
    public GameObject m_FireBallPrefab;
    public Transform m_Shootpos;
    public GameObject m_TowerRange;
    public GameObject m_UpgradeGroup;
    public GameObject m_UpgradeBG;
    public Sprite m_FullUpgradeImg;

    public Button m_UpgradeBtn;
    public Button m_AtSpeedUpBtn;
    public Button m_AtPowerUpBtn;
    public Button m_AtRangeUpBtn;
    public Button m_SellBtn;

    public Text m_AtSpeedUpGoldText;
    public Text m_AtPowerUpGoldText;
    public Text m_AtRangeUpGoldText;
    public Text m_SellGoldText;

    public static List<Monster> m_MonListInRange;
    float m_CreateTime = 1.0f;
    float m_AttackSpeed = 5.0f;

    //타워 UpGrade관련 변수
    bool m_UpgradeOnOff = false; //업그레이드OnOff
    [HideInInspector] public int m_TowerValue = 160;   //타워 값어치
    int m_AtSpeedUpIdx = 0;   //공속 업그레이드 횟수를 담을 인덱스
    int m_AtSpeedUpGold = 140; //공석 업그레이드 횟수에 따른 필요 골드값
    int m_AtPowerUpIdx = 0;   //공격력 업그레이드 횟수를 담을 인덱스
    int m_AtPowerUpGold = 140; //공석 업그레이드 횟수에 따른 필요 골드값
    int m_AtRangeUpIdx = 0;   //공격범위 업그레이드 횟수를 담을 인덱스
    int m_AtRangeUpGold = 80; //공격범위 업그레이드 횟수에 따른 필요 골드값
    //타워 UpGrade관련 변수

    // Start is called before the first frame update
    void Start()
    {
        //이전게임에서 업그레이드한 변수들 초기화
        LavaCtrl.m_Damage = 30;
        FireBallCtrl.m_MvSpeed = 1.5f;
        //이전게임에서 업그레이드한 변수들 초기화

        m_MonListInRange = new List<Monster>();

        m_AtSpeedUpGoldText.text = m_AtSpeedUpGold.ToString();
        m_AtPowerUpGoldText.text = m_AtPowerUpGold.ToString();
        m_AtRangeUpGoldText.text = m_AtRangeUpGold.ToString();
        m_SellGoldText.text = (m_TowerValue * 80 / 100).ToString();

        if (m_UpgradeBtn != null)
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                m_UpgradeOnOff = !m_UpgradeOnOff;

                if (m_UpgradeOnOff == true)
                    Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);
                else
                    Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
            });

        if (m_AtSpeedUpBtn != null)
            m_AtSpeedUpBtn.onClick.AddListener(AtSpeedUpBtnClick);

        if (m_AtPowerUpBtn != null)
            m_AtPowerUpBtn.onClick.AddListener(AtPowerUpBtnClick);

        if (m_AtRangeUpBtn != null)
            m_AtRangeUpBtn.onClick.AddListener(AtRangeUpBtnClick);

        if (m_SellBtn != null)
            m_SellBtn.onClick.AddListener(SellBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CreateTime > 0.0f)
        {
            m_CreateTime -= Time.deltaTime;
            if (m_CreateTime <= 0.0f)
            {
                ShootFireBall();

                m_CreateTime = m_AttackSpeed;
            }
        }

        if (m_UpgradeOnOff == true)
        {
            m_TowerRange.GetComponent<SpriteRenderer>().enabled = true; //타워범위 배경 이미지

            m_UpgradeBG.gameObject.SetActive(true); //업그레이드 배경(나침반)

            m_UpgradeGroup.gameObject.SetActive(true); //업그레이드그룹(업그레이드관련 버튼들)
        }
        else
        {
            m_TowerRange.GetComponent<SpriteRenderer>().enabled = false;

            m_UpgradeBG.gameObject.SetActive(false); //업그레이드 배경(나침반)

            m_UpgradeGroup.gameObject.SetActive(false);
        }
    }

    void ShootFireBall()
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        if (m_MonListInRange.Count > 0)
        {
            GameObject a_FireBallObj = Instantiate(m_FireBallPrefab) as GameObject;
            a_FireBallObj.transform.position = m_Shootpos.position;

            Sound_Mgr.Instance.PlayEffSound("magic_03", 1.0f);
        }
    }

    void AtSpeedUpBtnClick()
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        if (m_AtSpeedUpIdx >= 5) //최대 강화 횟수
        {
            Game_Mgr.Inst.ShowMessage("이미 강화 최대치에 도달했습니다.");
            return;
        }

        if (Game_Mgr.Inst.m_Gold - m_AtSpeedUpGold < 0) //보유골드가 필요강화골드보다 적으면
        {
            Game_Mgr.Inst.ShowMessage("보유골드가 강화에 필요한 골드보다 적습니다.");
            return;
        }

        m_AttackSpeed -= 0.5f;                   //공속증가
        Game_Mgr.Inst.AddGold(-m_AtSpeedUpGold); //골드차감

        m_AtSpeedUpGold += 140;
        m_AtSpeedUpGoldText.text = m_AtSpeedUpGold.ToString();

        m_TowerValue += 140;
        m_SellGoldText.text = (m_TowerValue * 80 / 100).ToString();

        m_AtSpeedUpIdx++;
        if (m_AtSpeedUpIdx >= 5)
        {
            m_AtSpeedUpBtn.GetComponent<Image>().sprite = m_FullUpgradeImg;

            m_AtSpeedUpGoldText.text = "Full";
        }

        Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
    }

    void AtPowerUpBtnClick()
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        if (m_AtPowerUpIdx >= 5) //최대 강화 횟수
        {
            Game_Mgr.Inst.ShowMessage("이미 강화 최대치에 도달했습니다.");
            return;
        }

        if (Game_Mgr.Inst.m_Gold - m_AtPowerUpGold < 0) //보유골드가 필요강화골드보다 적으면
        {
            Game_Mgr.Inst.ShowMessage("보유골드가 강화에 필요한 골드보다 적습니다.");
            return;
        }

        LavaCtrl.m_Damage += 5;                 //공격력 증가
        Game_Mgr.Inst.AddGold(-m_AtPowerUpGold); //골드차감

        m_AtPowerUpGold += 140;
        m_AtPowerUpGoldText.text = m_AtPowerUpGold.ToString();

        m_TowerValue += 140;
        m_SellGoldText.text = (m_TowerValue * 80 / 100).ToString();

        m_AtPowerUpIdx++;
        if (m_AtPowerUpIdx >= 5)
        {
            m_AtPowerUpBtn.GetComponent<Image>().sprite = m_FullUpgradeImg;

            m_AtPowerUpGoldText.text = "Full";
        }

        Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
    }

    void AtRangeUpBtnClick()
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        if (m_AtRangeUpIdx >= 5) //최대 강화 횟수
        {
            Game_Mgr.Inst.ShowMessage("이미 강화 최대치에 도달했습니다.");
            return;
        }

        if (Game_Mgr.Inst.m_Gold - m_AtRangeUpGold < 0) //보유골드가 필요강화골드보다 적으면
        {
            Game_Mgr.Inst.ShowMessage("보유골드가 강화에 필요한 골드보다 적습니다.");
            return;
        }

        m_TowerRange.transform.localScale = new Vector3(m_TowerRange.transform.localScale.x + 1.0f,
                                                        m_TowerRange.transform.localScale.y + 1.0f,
                                                        m_TowerRange.transform.localScale.z);

        Game_Mgr.Inst.AddGold(-m_AtRangeUpGold); //골드차감

        m_AtRangeUpGold += 80;
        m_AtRangeUpGoldText.text = m_AtRangeUpGold.ToString();

        m_TowerValue += 80;
        m_SellGoldText.text = (m_TowerValue * 80 / 100).ToString();

        m_AtRangeUpIdx++;
        if (m_AtRangeUpIdx >= 5)
        {
            m_AtRangeUpBtn.GetComponent<Image>().sprite = m_FullUpgradeImg;

            m_AtRangeUpGoldText.text = "Full";
        }

        Sound_Mgr.Instance.PlayGUISound("UIClick1", 0.8f);
    }

    void SellBtnClick()
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        Game_Mgr.Inst.AddGold(m_TowerValue * 80 / 100); //판매 시 타워 값어치에 80%만 돌려받음

        Destroy(gameObject);

        GameObject[] a_Lava = GameObject.FindGameObjectsWithTag("Lava");
        for (int ii = 0; ii < a_Lava.Length; ii++)
        {
            Destroy(a_Lava[ii]);
        }

        //재설치 할때를 위한 변수 초기화
        LavaCtrl.m_Damage = 10;
        m_AttackSpeed = 5.0f;
        TowerInstallMgr a_TowerInstallMgr = GameObject.FindObjectOfType<TowerInstallMgr>();
        SlotScript a_Slot = a_TowerInstallMgr.m_TowerSlots[3].GetComponent<SlotScript>();
        a_Slot.m_IsInstalled = false;
        a_Slot.m_InstalledImg.gameObject.SetActive(false); //X표시 제거
        InstallSlotScript a_InstallSlot = GetComponentInParent<InstallSlotScript>();
        a_InstallSlot.m_IsInstalled = false; // 해당위치에 타워설치여부 false로 변경
        //재설치 할때를 위한 변수 초기화

        Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);
    }
}

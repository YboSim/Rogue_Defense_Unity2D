using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_Ice : MonoBehaviour
{
    ParticleSystem m_IceParticle;       //아이스 방출 파티클
    public SpriteRenderer m_IceEffect;  //아이스 방출 전 범위이펙트
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

    float m_AttackSpeed = 8.0f;
    float m_FreezeRatio = 0.5f;
    float m_RangeRadius = 10.0f;

    //타워 UpGrade관련 변수
    bool m_UpgradeOnOff = false; //업그레이드OnOff
    [HideInInspector] public int m_TowerValue = 160;   //타워 값어치
    int m_AtSpeedUpIdx = 0;   //공속 업그레이드 횟수를 담을 인덱스
    int m_AtSpeedUpGold = 100; //공석 업그레이드 횟수에 따른 필요 골드값
    int m_AtPowerUpIdx = 0;   //공격력 업그레이드 횟수를 담을 인덱스
    int m_AtPowerUpGold = 100; //공석 업그레이드 횟수에 따른 필요 골드값
    int m_AtRangeUpIdx = 0;   //공격범위 업그레이드 횟수를 담을 인덱스
    int m_AtRangeUpGold = 120; //공격범위 업그레이드 횟수에 따른 필요 골드값
    //타워 UpGrade관련 변수

    //타워 동작 이펙트 관련변수 선언
    float m_Timer = 0.0f;
    float m_AniDuring = 1.0f;
    float m_CacTime = 0.0f;
    Color m_Color;

    bool m_IsWork = false;
    float m_CoolTime = 2.0f;
    //타워 동작 이펙트 관련변수 선언

    // Start is called before the first frame update
    void Start()
    {
        m_IceParticle = GetComponentInChildren<ParticleSystem>();

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
        m_CoolTime -= Time.deltaTime;
        if(m_CoolTime <= 0.0f)
        {
            m_IsWork = true;

            m_CoolTime = m_AttackSpeed;
        }

        if(m_IsWork == true)
        {
            TowerWorking(); //타워 이펙트 실행 함수
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

    void TowerWorking()
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        //아이스타워 이펙트 
        if (m_CacTime< 1.0f)
        {
            m_Timer += Time.deltaTime;
            m_CacTime = m_Timer / m_AniDuring;
            m_Color = m_IceEffect.color;
            m_Color.a = Mathf.Lerp(0, 1, m_CacTime);
            m_IceEffect.color = m_Color;

            if(m_CacTime > 1.0f)
            {
                m_IceParticle.Play();

                Sound_Mgr.Instance.PlayEffSound("magic_02", 1.0f);

                Freezing();

                m_Color.a = 0.0f;
                m_IceEffect.color = m_Color;
                m_Timer = 0.0f;
                m_CacTime = 0.0f;

                m_IsWork = false;
            }
        }
        //아이스타워 이펙트 
    }

    void Freezing() //타워로 부터 범위안의 몬스터를 추출
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, m_RangeRadius);
        Monster a_Monster;
        foreach (Collider2D coll in colls)
        {
            a_Monster = coll.GetComponent<Monster>();
            if (a_Monster == null)
                continue;

            a_Monster.Freezed(m_FreezeRatio); //몬스터 얼리기
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

        m_AtSpeedUpGold += 100;
        m_AtSpeedUpGoldText.text = m_AtSpeedUpGold.ToString();

        m_TowerValue += 100;
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

        m_FreezeRatio -= 0.05f;                  //이속저하Value 증가
        Game_Mgr.Inst.AddGold(-m_AtPowerUpGold); //골드차감

        m_AtPowerUpGold += 100;
        m_AtPowerUpGoldText.text = m_AtPowerUpGold.ToString();

        m_TowerValue += 100;
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
        m_RangeRadius += 1;
        Game_Mgr.Inst.AddGold(-m_AtRangeUpGold); //골드차감

        m_AtRangeUpGold += 120;
        m_AtRangeUpGoldText.text = m_AtRangeUpGold.ToString();

        m_TowerValue += 120;
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

        //재설치 할때를 위한 변수 초기화
        TowerInstallMgr a_TowerInstallMgr = GameObject.FindObjectOfType<TowerInstallMgr>();
        SlotScript a_Slot = a_TowerInstallMgr.m_TowerSlots[2].GetComponent<SlotScript>();
        a_Slot.m_IsInstalled = false;
        a_Slot.m_InstalledImg.gameObject.SetActive(false); //X표시 제거
        InstallSlotScript a_InstallSlot = GetComponentInParent<InstallSlotScript>();
        a_InstallSlot.m_IsInstalled = false; // 해당위치에 타워설치여부 false로 변경
        //재설치 할때를 위한 변수 초기화

        Sound_Mgr.Instance.PlayGUISound("UIClick2", 0.8f);
    }
}

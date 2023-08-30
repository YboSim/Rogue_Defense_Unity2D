using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    Animator m_Animator;

    public Image m_HpImg; //Hp바 이미지
    public GameObject m_IcePrefab; //빙결 상태 보여줄 오브젝트
    public Transform m_Rogue_pelvis; //사망 시 회전됨, 풀에 되돌려놓을시 원래대로 되돌려놓음
    GameObject m_IceImage; //m_IcePrefab저장해놓을 변수
    public Transform m_Canvas; //메인 Canvas

    //빙결 관련
    bool m_IsFreezed = false; //현재 몬스터의 빙결 여부
    float m_FreezingTime = 10.0f; //빙결효과 지속시간
    float m_PrevMvSpeed; //본래 이동속도
    float m_FreezeMvSpeed; //빙결 적용시 이동속도
    //빙결 관련

    //중독 관련
    bool m_IsPoisoned = false; //현재 몬스터의 중독 여부
    float m_PoisoningTime = 10.0f; //중독효과 지속시간
    int m_PosionDamage; //중독 상태시 0.1초당 주는 독데미지
    float m_PosionCoolTime = 1.0f; //중독 상태걸린 직후 1초 뒤부터 데미지줌
    //중독 관련


    int m_MaxHp; //최대 Hp (각각의 로그Ctrl 스크립트에서 받아옴)
    int m_CurHp; //현재 Hp (각각의 로그Ctrl 스크립트에서 받아옴)
    bool m_IsDie = false; //몬스터 상태를 담을 변수

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();

        if (this.gameObject.name.Contains("MB") || this.gameObject.name.Contains("FB"))
            HpSetting(); //MB 와 FB는 오브젝트 풀링 사용하지 않기때문에 Start()에서 세팅
                         //나머지 기본Rogue들은 TakeRogueFromPool()에서 호출
    }

    void OnEnable()
    {
        StopAllCoroutines();

        m_IsDie = false;
    }

    void OnDisable()
    {
        //간헐적으로 몬스터 사망시 PathFollower스크립트가 삭제되지않는 버그가 발생 되어 오브젝트풀로 되돌려줄때마다 한번더 실행
        PathFollower a_PathFollower = GetComponent<PathFollower>();
        if (a_PathFollower != null)
            Destroy(a_PathFollower);
        //간헐적으로 몬스터 사망시 PathFollower스크립트가 삭제되지않는 버그가 발생 되어 오브젝트풀로 되돌려줄때마다 한번더 실행
    }

    // Update is called once per frame
    void Update()
    {
        AnimationUpdate(); //몬스터 Walk,Die 애니메이션 Play하는 함수

        if(m_IsPoisoned == true)
        {
            //데미지 주기
            if(m_PosionCoolTime > 0.0f)
            {
                m_PosionCoolTime -= Time.deltaTime;
                if (m_PosionCoolTime <= 0.0f)
                {
                    TakeDamage(m_PosionDamage);

                    m_PosionCoolTime = 0.1f;
                }
            }
            //데미지 주기

            //중독 효과 지속 시간 종료 후 초기화
            m_PoisoningTime -= Time.deltaTime;
            if(m_PoisoningTime <= 0.0f)
            {
                ClearPoisoned();

                m_PoisoningTime = 10.0f;
            }
        }

        if (m_IsFreezed == true)
        {
            //빙결 효과 지속 시간 종료 후 초기화
            m_FreezingTime -= Time.deltaTime;
            if(m_FreezingTime <= 0.0f)
            {
                ClearFreezed();

                m_FreezingTime = 10.0f;
            }
            //빙결 효과 지속 시간 종료 후 초기화
        }

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //몬스터가 죽지않고 마지막지점 까지 도착했을 경우
        if (coll.gameObject.name.Contains("EndPoint") == true)
        {
            if (this.gameObject.name.Contains("FB") == true)
            {
                Destroy(gameObject);

                if (Game_Mgr.m_GameState == GameState.Playing)
                    Game_Mgr.Inst.GameOverBox(GameState.GameOver); //게임오버
            }
            else if(this.gameObject.name.Contains("MB") == true)
            {
                Game_Mgr.Inst.DeathCount(5);

                Destroy(gameObject);
            }
            else
            {
                Game_Mgr.Inst.DeathCount(1);

                StartCoroutine(PushRogueToPool());
            }
        }
        //몬스터가 죽지않고 마지막지점 까지 도착했을 경우
    }

    public void TakeDamage(int a_Damage)
    {
        if (m_CurHp <= 0)
            return;

        m_CurHp -= a_Damage;

        if (m_CurHp > 0) //피격후 몬스터의 현재Hp가 0이아닐경우
        {
            m_HpImg.fillAmount = (float)m_CurHp / (float)m_MaxHp;
        }
        else //몬스터 사망 시
        {
            m_HpImg.fillAmount = 0.0f; //체력바 0으로
            this.gameObject.GetComponent<PathFollower>().speed = 0.0f; //몬스터 제자리에 멈춤

            //사망시 타워범위내 몬스터리스트에서 즉시 제거
            if (Tower_Arrow.m_MonListInRange != null)
            {
                if (Tower_Arrow.m_MonListInRange.Contains(this) == true)
                    Tower_Arrow.m_MonListInRange.Remove(this);
            }
            if (Tower_Bomb.m_MonListInRange != null)
            {
                if (Tower_Bomb.m_MonListInRange.Contains(this) == true)
                    Tower_Bomb.m_MonListInRange.Remove(this);
            }
            if (Tower_Lava.m_MonListInRange != null)
            {
                if (Tower_Lava.m_MonListInRange.Contains(this) == true)
                    Tower_Lava.m_MonListInRange.Remove(this);
            }
            //사망시 타워범위내 몬스터리스트에서 즉시 제거

            m_IsDie = true;// 사망 애니메이션 재생

            StartCoroutine(PushRogueToPool()); //오브젝트 풀로 되돌려놓음

            if (gameObject.name.Contains("Rogue_01") == true)
            {
                Game_Mgr.Inst.AddGold(10);
            }
            else if(gameObject.name.Contains("Rogue_02") == true)
            {
                Game_Mgr.Inst.AddGold(15);
            }
            else if (gameObject.name.Contains("Rogue_03") == true)
            {
                Game_Mgr.Inst.AddGold(20);
            }
            else if (gameObject.name.Contains("Rogue_04") == true)
            {
                Game_Mgr.Inst.AddGold(35);
            }
            else if(gameObject.name.Contains("Rogue_MB") == true)
            {
                Game_Mgr.Inst.AddGold(100);

                Destroy(gameObject);

                //10분 이후 부터 MB잡을 시 FB스폰
                if(Game_Mgr.Inst.m_GameTime > 600.0f)
                {
                    RogueGenerator a_RogueGen = FindObjectOfType<RogueGenerator>();
                    a_RogueGen.SapwnRogue_FB();
                }
                //10분 이후 부터 MB잡을 시 FB스폰
            }
            else if ((gameObject.name.Contains("Rogue_FB") == true))
            {
                //Easy와 노말 모드에는 Ending이 있음
                if(Game_Mgr.m_GameMode == GameMode.Easy || Game_Mgr.m_GameMode == GameMode.Hard)
                {
                    Destroy(gameObject);

                    if (Game_Mgr.m_GameState == GameState.Playing)
                        Game_Mgr.Inst.GameOverBox(GameState.Victory);
                }
                else //Hard에서는 별도의 보상 없이 계속 진행
                {
                    Destroy(gameObject);
                }
            }

            Game_Mgr.Inst.AddKillCount(); //스코어 1Kill ↑
        }
    }

    IEnumerator PushRogueToPool()
    {
        yield return new WaitForSeconds(1.0f); //사망 애니메이션 재생 완료 후 사망처리

        //경로 초기화
        PathFollower a_Path = GetComponent<PathFollower>();
        if (a_Path != null)
            Destroy(a_Path);
        //경로 초기화

        //빙결관련 초기화
        if (m_IsFreezed == true)
        {
            m_IsFreezed = false;
            Destroy(m_IceImage);

            m_FreezingTime = 10.0f;
        }
        //빙결관련 초기화

        //중독 관련 초기화
        if (m_IsPoisoned == true)
        {
            m_IsPoisoned = false;

            m_PoisoningTime = 10.0f;
        }
        //중독 관련 초기화

        //사망으로 흐려지거나 중독상태로 변한 몬스터 원래 색으로 변경
        SpriteRenderer[] a_SR = GetComponentsInChildren<SpriteRenderer>();
        for (int ii = 0; ii < a_SR.Length; ii++)
        {
            if (a_SR[ii].gameObject.name.Contains("Ice") == true) //Ice Prefab제외
                continue;
            else
                a_SR[ii].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        //사망으로 흐려지거나 중독상태로 변한 몬스터 원래 색으로 변경

        m_Rogue_pelvis.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f); //사망한 몬스터 일으키기

        if (this.gameObject.name.Contains("MB") == true || this.gameObject.name.Contains("FB") == true)
            Destroy(gameObject);

        if (this.gameObject != null)
            gameObject.SetActive(false);
    }

    public void Freezed(float a_Ratio) //Ice타워에 피격시
    {
        if (this.gameObject.name.Contains("04") == true) //Rogue_04 빙결 면역
            return;

        if (m_CurHp <= 0)
            return;

        if (m_IsFreezed == true)
            return;

        m_PrevMvSpeed = GetComponent<PathFollower>().speed;
        m_FreezeMvSpeed = m_PrevMvSpeed * a_Ratio;
        GetComponent<PathFollower>().speed = m_FreezeMvSpeed; //속도저하

        //빙결 상태 표시 IcePrefab생성
        GameObject a_IcePrefab = Instantiate(m_IcePrefab) as GameObject;
        a_IcePrefab.transform.SetParent(m_Canvas, true);
        a_IcePrefab.transform.localPosition = new Vector3(1.0f, 4.0f, 0.0f);
        if (this.gameObject.name.Contains("MB") == true)
        {
            a_IcePrefab.transform.localScale = new Vector3(8.0f, 3.0f, 1.0f);
        }
        else if (this.gameObject.name.Contains("FB") == true)
        {
            a_IcePrefab.transform.localScale = new Vector3(8.0f, 3.0f, 1.0f);
        }

        m_IceImage = a_IcePrefab; //오브젝트 풀로 돌려놓을때 지우기위해 저장
        //빙결 상태 표시 IcePrefab생성

        m_IsFreezed = true; // 몬스터에 빙결상태 저장
    }

    void ClearFreezed()
    {
        if (this.gameObject.name.Contains("04") == true) //Rogue_04 빙결 면역
            return;

        if (m_CurHp <= 0)
            return;

        if (m_IsFreezed == false)
            return;

        GetComponent<PathFollower>().speed = m_PrevMvSpeed; //본래이동속도로 돌려놓음

        Destroy(m_IceImage); //빙결이미지 삭제

        m_IsFreezed = false;

    }

    public void Poisoned(int a_Damage)
    {
        if (m_CurHp <= 0)
            return;

        m_PosionDamage = a_Damage;

        if (m_IsPoisoned == true)
            return;

        SpriteRenderer[] a_SR = GetComponentsInChildren<SpriteRenderer>();
        for (int ii = 0; ii < a_SR.Length; ii++)
        {
            if (a_SR[ii].gameObject.name.Contains("Ice") == true)
                continue;
            else
                a_SR[ii].color = new Color(200 / 255, 1, 250 / 255); //몬스터 초록색 계열로 변경
        }

        m_IsPoisoned = true; //몬스터에 중독 상태 저장
    }

    void ClearPoisoned()
    {
        if (m_CurHp <= 0)
            return;

        SpriteRenderer[] a_SR = GetComponentsInChildren<SpriteRenderer>();
        for (int ii = 0; ii < a_SR.Length; ii++)
        {
            if (a_SR[ii].gameObject.name.Contains("Ice") == true)
                continue;
            else
                a_SR[ii].color = new Color(1,1,1); //몬스터 본래 색으로 변경
        }

        m_IsPoisoned = false; //중독 상태 해제
    }

    void AnimationUpdate()
    {
        if (m_IsDie == true)
        {
            m_Animator.SetBool("IsDie", true); //사망 애니메이션 재생

            //몬스터 이미지 흐려지게 만들기
            SpriteRenderer[] a_SR = transform.GetComponentsInChildren<SpriteRenderer>();
            for (int ii = 0; ii<a_SR.Length; ii++)
            {
                if (a_SR[ii].color.a > 0.0f)
                    a_SR[ii].color -= new Color(0.0f, 0.0f, 0.0f, 0.005f);
            }
            //몬스터 이미지 흐려지게 만들기
        }
        else
        {
            m_Animator.SetBool("IsDie", false); //walking 애니메이션 재생
        }
    }

    public void HpSetting()
    {
        if (this.gameObject.name.Contains("01") == true)
        {
            Rogue_1_Ctrl a_Rogue = GetComponent<Rogue_1_Ctrl>();
            m_MaxHp = a_Rogue.m_MaxHp;
            m_CurHp = a_Rogue.m_CurHp;

            m_HpImg.fillAmount = (float)m_CurHp / (float)m_MaxHp;
        }
        else if (this.gameObject.name.Contains("02") == true)
        {
            Rogue_2_Ctrl a_Rogue = GetComponent<Rogue_2_Ctrl>();
            m_MaxHp = a_Rogue.m_MaxHp;
            m_CurHp = a_Rogue.m_CurHp;

            m_HpImg.fillAmount = (float)m_CurHp / (float)m_MaxHp;
        }
        else if (this.gameObject.name.Contains("03") == true)
        {
            Rogue_3_Ctrl a_Rogue = GetComponent<Rogue_3_Ctrl>();
            m_MaxHp = a_Rogue.m_MaxHp;
            m_CurHp = a_Rogue.m_CurHp;

            m_HpImg.fillAmount = (float)m_CurHp / (float)m_MaxHp;
        }
        else if (this.gameObject.name.Contains("04") == true)
        {
            Rogue_4_Ctrl a_Rogue = GetComponent<Rogue_4_Ctrl>();
            m_MaxHp = a_Rogue.m_MaxHp;
            m_CurHp = a_Rogue.m_CurHp;

            m_HpImg.fillAmount = (float)m_CurHp / (float)m_MaxHp;
        }
        else if (this.gameObject.name.Contains("MB") == true)
        {
            Rogue_MB_Ctrl a_Rogue = GetComponent<Rogue_MB_Ctrl>();
            m_MaxHp = a_Rogue.m_MaxHp;
            m_CurHp = a_Rogue.m_CurHp;

            m_HpImg.fillAmount = (float)m_CurHp / (float)m_MaxHp;
        }
        else if (this.gameObject.name.Contains("FB") == true)
        {
            Rogue_FB_Ctrl a_Rogue = GetComponent<Rogue_FB_Ctrl>();
            m_MaxHp = a_Rogue.m_MaxHp;
            m_CurHp = a_Rogue.m_CurHp;

            m_HpImg.fillAmount = (float)m_CurHp / (float)m_MaxHp;
        }
    }
}

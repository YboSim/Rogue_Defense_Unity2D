using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    Animator m_Animator;

    public Image m_HpImg; //Hp�� �̹���
    public GameObject m_IcePrefab; //���� ���� ������ ������Ʈ
    public Transform m_Rogue_pelvis; //��� �� ȸ����, Ǯ�� �ǵ��������� ������� �ǵ�������
    GameObject m_IceImage; //m_IcePrefab�����س��� ����
    public Transform m_Canvas; //���� Canvas

    //���� ����
    bool m_IsFreezed = false; //���� ������ ���� ����
    float m_FreezingTime = 10.0f; //����ȿ�� ���ӽð�
    float m_PrevMvSpeed; //���� �̵��ӵ�
    float m_FreezeMvSpeed; //���� ����� �̵��ӵ�
    //���� ����

    //�ߵ� ����
    bool m_IsPoisoned = false; //���� ������ �ߵ� ����
    float m_PoisoningTime = 10.0f; //�ߵ�ȿ�� ���ӽð�
    int m_PosionDamage; //�ߵ� ���½� 0.1�ʴ� �ִ� ��������
    float m_PosionCoolTime = 1.0f; //�ߵ� ���°ɸ� ���� 1�� �ں��� ��������
    //�ߵ� ����


    int m_MaxHp; //�ִ� Hp (������ �α�Ctrl ��ũ��Ʈ���� �޾ƿ�)
    int m_CurHp; //���� Hp (������ �α�Ctrl ��ũ��Ʈ���� �޾ƿ�)
    bool m_IsDie = false; //���� ���¸� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();

        if (this.gameObject.name.Contains("MB") || this.gameObject.name.Contains("FB"))
            HpSetting(); //MB �� FB�� ������Ʈ Ǯ�� ������� �ʱ⶧���� Start()���� ����
                         //������ �⺻Rogue���� TakeRogueFromPool()���� ȣ��
    }

    void OnEnable()
    {
        StopAllCoroutines();

        m_IsDie = false;
    }

    void OnDisable()
    {
        //���������� ���� ����� PathFollower��ũ��Ʈ�� ���������ʴ� ���װ� �߻� �Ǿ� ������ƮǮ�� �ǵ����ٶ����� �ѹ��� ����
        PathFollower a_PathFollower = GetComponent<PathFollower>();
        if (a_PathFollower != null)
            Destroy(a_PathFollower);
        //���������� ���� ����� PathFollower��ũ��Ʈ�� ���������ʴ� ���װ� �߻� �Ǿ� ������ƮǮ�� �ǵ����ٶ����� �ѹ��� ����
    }

    // Update is called once per frame
    void Update()
    {
        AnimationUpdate(); //���� Walk,Die �ִϸ��̼� Play�ϴ� �Լ�

        if(m_IsPoisoned == true)
        {
            //������ �ֱ�
            if(m_PosionCoolTime > 0.0f)
            {
                m_PosionCoolTime -= Time.deltaTime;
                if (m_PosionCoolTime <= 0.0f)
                {
                    TakeDamage(m_PosionDamage);

                    m_PosionCoolTime = 0.1f;
                }
            }
            //������ �ֱ�

            //�ߵ� ȿ�� ���� �ð� ���� �� �ʱ�ȭ
            m_PoisoningTime -= Time.deltaTime;
            if(m_PoisoningTime <= 0.0f)
            {
                ClearPoisoned();

                m_PoisoningTime = 10.0f;
            }
        }

        if (m_IsFreezed == true)
        {
            //���� ȿ�� ���� �ð� ���� �� �ʱ�ȭ
            m_FreezingTime -= Time.deltaTime;
            if(m_FreezingTime <= 0.0f)
            {
                ClearFreezed();

                m_FreezingTime = 10.0f;
            }
            //���� ȿ�� ���� �ð� ���� �� �ʱ�ȭ
        }

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //���Ͱ� �����ʰ� ���������� ���� �������� ���
        if (coll.gameObject.name.Contains("EndPoint") == true)
        {
            if (this.gameObject.name.Contains("FB") == true)
            {
                Destroy(gameObject);

                if (Game_Mgr.m_GameState == GameState.Playing)
                    Game_Mgr.Inst.GameOverBox(GameState.GameOver); //���ӿ���
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
        //���Ͱ� �����ʰ� ���������� ���� �������� ���
    }

    public void TakeDamage(int a_Damage)
    {
        if (m_CurHp <= 0)
            return;

        m_CurHp -= a_Damage;

        if (m_CurHp > 0) //�ǰ��� ������ ����Hp�� 0�̾ƴҰ��
        {
            m_HpImg.fillAmount = (float)m_CurHp / (float)m_MaxHp;
        }
        else //���� ��� ��
        {
            m_HpImg.fillAmount = 0.0f; //ü�¹� 0����
            this.gameObject.GetComponent<PathFollower>().speed = 0.0f; //���� ���ڸ��� ����

            //����� Ÿ�������� ���͸���Ʈ���� ��� ����
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
            //����� Ÿ�������� ���͸���Ʈ���� ��� ����

            m_IsDie = true;// ��� �ִϸ��̼� ���

            StartCoroutine(PushRogueToPool()); //������Ʈ Ǯ�� �ǵ�������

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

                //10�� ���� ���� MB���� �� FB����
                if(Game_Mgr.Inst.m_GameTime > 600.0f)
                {
                    RogueGenerator a_RogueGen = FindObjectOfType<RogueGenerator>();
                    a_RogueGen.SapwnRogue_FB();
                }
                //10�� ���� ���� MB���� �� FB����
            }
            else if ((gameObject.name.Contains("Rogue_FB") == true))
            {
                //Easy�� �븻 ��忡�� Ending�� ����
                if(Game_Mgr.m_GameMode == GameMode.Easy || Game_Mgr.m_GameMode == GameMode.Hard)
                {
                    Destroy(gameObject);

                    if (Game_Mgr.m_GameState == GameState.Playing)
                        Game_Mgr.Inst.GameOverBox(GameState.Victory);
                }
                else //Hard������ ������ ���� ���� ��� ����
                {
                    Destroy(gameObject);
                }
            }

            Game_Mgr.Inst.AddKillCount(); //���ھ� 1Kill ��
        }
    }

    IEnumerator PushRogueToPool()
    {
        yield return new WaitForSeconds(1.0f); //��� �ִϸ��̼� ��� �Ϸ� �� ���ó��

        //��� �ʱ�ȭ
        PathFollower a_Path = GetComponent<PathFollower>();
        if (a_Path != null)
            Destroy(a_Path);
        //��� �ʱ�ȭ

        //������� �ʱ�ȭ
        if (m_IsFreezed == true)
        {
            m_IsFreezed = false;
            Destroy(m_IceImage);

            m_FreezingTime = 10.0f;
        }
        //������� �ʱ�ȭ

        //�ߵ� ���� �ʱ�ȭ
        if (m_IsPoisoned == true)
        {
            m_IsPoisoned = false;

            m_PoisoningTime = 10.0f;
        }
        //�ߵ� ���� �ʱ�ȭ

        //������� ������ų� �ߵ����·� ���� ���� ���� ������ ����
        SpriteRenderer[] a_SR = GetComponentsInChildren<SpriteRenderer>();
        for (int ii = 0; ii < a_SR.Length; ii++)
        {
            if (a_SR[ii].gameObject.name.Contains("Ice") == true) //Ice Prefab����
                continue;
            else
                a_SR[ii].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        //������� ������ų� �ߵ����·� ���� ���� ���� ������ ����

        m_Rogue_pelvis.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f); //����� ���� ����Ű��

        if (this.gameObject.name.Contains("MB") == true || this.gameObject.name.Contains("FB") == true)
            Destroy(gameObject);

        if (this.gameObject != null)
            gameObject.SetActive(false);
    }

    public void Freezed(float a_Ratio) //IceŸ���� �ǰݽ�
    {
        if (this.gameObject.name.Contains("04") == true) //Rogue_04 ���� �鿪
            return;

        if (m_CurHp <= 0)
            return;

        if (m_IsFreezed == true)
            return;

        m_PrevMvSpeed = GetComponent<PathFollower>().speed;
        m_FreezeMvSpeed = m_PrevMvSpeed * a_Ratio;
        GetComponent<PathFollower>().speed = m_FreezeMvSpeed; //�ӵ�����

        //���� ���� ǥ�� IcePrefab����
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

        m_IceImage = a_IcePrefab; //������Ʈ Ǯ�� ���������� ��������� ����
        //���� ���� ǥ�� IcePrefab����

        m_IsFreezed = true; // ���Ϳ� ������� ����
    }

    void ClearFreezed()
    {
        if (this.gameObject.name.Contains("04") == true) //Rogue_04 ���� �鿪
            return;

        if (m_CurHp <= 0)
            return;

        if (m_IsFreezed == false)
            return;

        GetComponent<PathFollower>().speed = m_PrevMvSpeed; //�����̵��ӵ��� ��������

        Destroy(m_IceImage); //�����̹��� ����

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
                a_SR[ii].color = new Color(200 / 255, 1, 250 / 255); //���� �ʷϻ� �迭�� ����
        }

        m_IsPoisoned = true; //���Ϳ� �ߵ� ���� ����
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
                a_SR[ii].color = new Color(1,1,1); //���� ���� ������ ����
        }

        m_IsPoisoned = false; //�ߵ� ���� ����
    }

    void AnimationUpdate()
    {
        if (m_IsDie == true)
        {
            m_Animator.SetBool("IsDie", true); //��� �ִϸ��̼� ���

            //���� �̹��� ������� �����
            SpriteRenderer[] a_SR = transform.GetComponentsInChildren<SpriteRenderer>();
            for (int ii = 0; ii<a_SR.Length; ii++)
            {
                if (a_SR[ii].color.a > 0.0f)
                    a_SR[ii].color -= new Color(0.0f, 0.0f, 0.0f, 0.005f);
            }
            //���� �̹��� ������� �����
        }
        else
        {
            m_Animator.SetBool("IsDie", false); //walking �ִϸ��̼� ���
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

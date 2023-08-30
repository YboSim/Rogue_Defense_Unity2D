using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCtrl : MonoBehaviour
{
    public static float m_MvSpeed = 0.7f;
    public static int m_Damage = 40;

    public GameObject m_ExplosionPrefab;
    public AnimationCurve m_Curve;
    Transform m_StartPos;

    Monster m_Target;

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = GameObject.FindObjectOfType<Tower_Bomb>().m_Shootpos;

        Destroy(gameObject, 5.0f);  //5���� ����

        CheckTarget();

        StartCoroutine(MoveToTarget(m_Target));
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Target == null) // ȭ���� �����Ǿ����� �������� ������ ȭ�쿡 ���� m_Target�� �׾��� ���
            CheckTarget();    // m_Target�� �ǽð����� ��������
    }

    public IEnumerator MoveToTarget(Monster a_TargetMonster) //Ÿ������ ������ ���͸� ���� �̵�
    {
        if (a_TargetMonster != null)
        {

            float a_Duration = m_MvSpeed;
            float a_Time = 0.0f;
            Vector3 a_StartPos = m_StartPos.position;
            Vector3 a_EndPos = a_TargetMonster.GetComponent<Transform>().position;

            while (a_Time < a_Duration)
            {
                a_Time += Time.deltaTime;
                float a_LinearT = a_Time / a_Duration;
                float a_HeightT = m_Curve.Evaluate(a_LinearT);

                float a_Height = Mathf.Lerp(0.0f, 8.0f, a_HeightT); //��ź�� Ÿ����ġ�� ��������

                Vector3 a_CacPos = Vector2.Lerp(a_StartPos, a_EndPos, a_LinearT) + new Vector2(0.0f, a_Height); //Ŀ�꿡 ���������� ���� ����
                transform.position = a_CacPos;

                yield return null;
            }

            Explosion(); //��ǥ���� ������ ���� ����Ʈ �߻�

            Damage(); //��ǥ�������� �����ȿ� ���͵鿡�� ����������

            Destroy(gameObject); //���߰� ���ÿ� ����
        }
    }

    void CheckTarget()
    {
        if (Tower_Bomb.m_MonListInRange.Count > 0)
        {
            for (int ii = 0; ii < Tower_Bomb.m_MonListInRange.Count; ii++)
            {//for�� ������ ���͸���Ʈ�� ù��° ���͸� Ÿ������ ����
                if (Tower_Bomb.m_MonListInRange[ii] != null)
                {
                    m_Target = Tower_Bomb.m_MonListInRange[ii];
                    return;
                }
            }
        }
    }

    void Explosion()
    {
        GameObject a_ExplosionPrefab = Instantiate(m_ExplosionPrefab) as GameObject;
        a_ExplosionPrefab.transform.position = transform.position;

        Sound_Mgr.Instance.PlayEffSound("Explosion", 1.0f);
    }

    void Damage()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 4.0f);
        Monster a_Monster;
        foreach(Collider2D coll in colls)
        {
            a_Monster = coll.GetComponent<Monster>();
            if (a_Monster == null)
                continue;

            a_Monster.TakeDamage(m_Damage);
        }
    }
}

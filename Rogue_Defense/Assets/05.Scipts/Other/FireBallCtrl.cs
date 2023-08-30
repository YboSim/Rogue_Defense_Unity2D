using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCtrl : MonoBehaviour
{
    public GameObject m_LavaPrefab;
    public GameObject m_ExplosionPrefab;

    public static float m_MvSpeed = 1.5f;
    Vector3 m_CurDir = Vector3.zero;

    public AnimationCurve m_Curve;
    Transform m_StartPos;

    Monster m_Target;

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = GameObject.FindObjectOfType<Tower_Lava>().m_Shootpos;

        Destroy(gameObject, 3.0f);  //3���� ����

        CheckTarget();

        StartCoroutine(MoveToTarget(m_Target));
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Target == null) // FireBall�� �����Ǿ����� �ٸ�Ÿ���鿡 ���� m_Target�� �׾��� ���
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
                Vector3 a_OldPos = transform.position;

                a_Time += Time.deltaTime;
                float a_LinearT = a_Time / a_Duration;
                float a_HeightT = m_Curve.Evaluate(a_LinearT);

                float a_Height = Mathf.Lerp(0.0f, 8.0f, a_HeightT); //FireBall�� Ÿ����ġ�� ��������

                Vector3 a_CacPos = Vector2.Lerp(a_StartPos, a_EndPos, a_LinearT) + new Vector2(0.0f, a_Height); //Ŀ�꿡 ���������� ���� ����
                transform.position = a_CacPos;

                //FireBall ȸ��
                m_CurDir = transform.position - a_OldPos;
                m_CurDir.z = 0.0f;
                m_CurDir.Normalize();

                float a_Angle = Mathf.Atan2(m_CurDir.y, m_CurDir.x) * Mathf.Rad2Deg;
                Quaternion a_Rot = Quaternion.AngleAxis(a_Angle - 90.0f, Vector3.forward);
                transform.rotation = a_Rot;
                //FireBall ȸ��

                yield return null;
            }
            Explosion(); //��ǥ���� ���� �� ���� ����Ʈ ����

            CreateLava(); //��������Ʈ ���� �� �ٴڿ� ������ �ִ� ��ϻ���

            Destroy(gameObject); // ��ϻ��� �� FireBall ����
        }
    }

    void CheckTarget()
    {
        if (Tower_Lava.m_MonListInRange.Count > 0)
        {
            for (int ii = 0; ii < Tower_Lava.m_MonListInRange.Count; ii++)
            {//for�� ������ ���͸���Ʈ�� ù��° ���͸� Ÿ������ ����
                if (Tower_Lava.m_MonListInRange[ii] != null)
                {
                    m_Target = Tower_Lava.m_MonListInRange[ii];
                    return;
                }
            }
        }
    }

    void Explosion()
    {
        GameObject a_ExplosionPrefab = Instantiate(m_ExplosionPrefab) as GameObject;
        a_ExplosionPrefab.transform.position = 
            new Vector3(transform.position.x, transform.position.y + 2.0f, 0.0f);
    }

    void CreateLava()
    {
        GameObject a_LavaObj = Instantiate(m_LavaPrefab) as GameObject;
        a_LavaObj.transform.position = transform.position;
    }
}

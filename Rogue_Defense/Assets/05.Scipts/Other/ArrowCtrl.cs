using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    public static float m_MvSpeed = 0.7f; //ȭ�� �̵��ӵ�
    public static int m_Damgae = 20;     //ȭ�� ������
    Vector3 m_CurDir = Vector3.zero;

    public AnimationCurve m_Curve;
    Transform m_StartPos;

    Monster m_Target;

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = GameObject.FindObjectOfType<Tower_Arrow>().m_Shootpos;

        Destroy(gameObject, 2.0f);  //2���� ����

        CheckTarget();

        StartCoroutine(MoveToTarget(m_Target));
    }

    // Update is called once per frame
    void Update()
    {
        //RotateToTarget();

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
                Vector3 a_OldPos = transform.position;

                a_Time += Time.deltaTime;
                float a_LinearT = a_Time / a_Duration;
                float a_HeightT = m_Curve.Evaluate(a_LinearT);

                float a_Height = Mathf.Lerp(0.0f, 8.0f, a_HeightT); //ȭ���� Ÿ����ġ�� ��������

                Vector3 a_CacPos = Vector2.Lerp(a_StartPos, a_EndPos, a_LinearT) + new Vector2(0.0f, a_Height); //Ŀ�꿡 ���������� ���� ����
                transform.position = a_CacPos;

                //ȭ�� ȸ��
                m_CurDir = transform.position - a_OldPos;
                m_CurDir.z = 0.0f;
                m_CurDir.Normalize();

                float a_Angle = Mathf.Atan2(m_CurDir.y, m_CurDir.x) * Mathf.Rad2Deg;
                Quaternion a_Rot = Quaternion.AngleAxis(a_Angle - 90.0f, Vector3.forward);
                transform.rotation = a_Rot;
                //ȭ�� ȸ��

                yield return null;
            }

            //ȭ�� ��ǥ������ ���� ��(���Ϳ� ȭ���� �¾�����)
            Destroy(gameObject); //ȭ�� ����

            if (a_TargetMonster != null)
                a_TargetMonster.TakeDamage(m_Damgae); //������
            //ȭ�� ��ǥ������ ���� ��(���Ϳ� ȭ���� �¾�����)
        }
    }

    void CheckTarget()
    {
        if (Tower_Arrow.m_MonListInRange.Count > 0)
        {
            for (int ii = 0; ii < Tower_Arrow.m_MonListInRange.Count; ii++)
            {//for�� ������ ���͸���Ʈ�� ù��° ���͸� Ÿ������ ����
                if (Tower_Arrow.m_MonListInRange[ii] != null)
                {
                    m_Target = Tower_Arrow.m_MonListInRange[ii];
                    break;
                }
            }
        }
    }
}

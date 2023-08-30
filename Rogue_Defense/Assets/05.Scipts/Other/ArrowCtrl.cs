using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    public static float m_MvSpeed = 0.7f; //화살 이동속도
    public static int m_Damgae = 20;     //화살 데미지
    Vector3 m_CurDir = Vector3.zero;

    public AnimationCurve m_Curve;
    Transform m_StartPos;

    Monster m_Target;

    // Start is called before the first frame update
    void Start()
    {
        m_StartPos = GameObject.FindObjectOfType<Tower_Arrow>().m_Shootpos;

        Destroy(gameObject, 2.0f);  //2초후 삭제

        CheckTarget();

        StartCoroutine(MoveToTarget(m_Target));
    }

    // Update is called once per frame
    void Update()
    {
        //RotateToTarget();

        if (m_Target == null) // 화살이 생성되었을때 이전번에 생성된 화살에 의해 m_Target이 죽었을 경우
            CheckTarget();    // m_Target을 실시간으로 재조정함
    }

    public IEnumerator MoveToTarget(Monster a_TargetMonster) //타겟으로 설정된 몬스터를 향해 이동
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

                float a_Height = Mathf.Lerp(0.0f, 8.0f, a_HeightT); //화살이 타겟위치로 선형보간

                Vector3 a_CacPos = Vector2.Lerp(a_StartPos, a_EndPos, a_LinearT) + new Vector2(0.0f, a_Height); //커브에 선형보간한 값을 더함
                transform.position = a_CacPos;

                //화살 회전
                m_CurDir = transform.position - a_OldPos;
                m_CurDir.z = 0.0f;
                m_CurDir.Normalize();

                float a_Angle = Mathf.Atan2(m_CurDir.y, m_CurDir.x) * Mathf.Rad2Deg;
                Quaternion a_Rot = Quaternion.AngleAxis(a_Angle - 90.0f, Vector3.forward);
                transform.rotation = a_Rot;
                //화살 회전

                yield return null;
            }

            //화살 목표지점에 도착 시(몬스터에 화살이 맞았을때)
            Destroy(gameObject); //화살 제거

            if (a_TargetMonster != null)
                a_TargetMonster.TakeDamage(m_Damgae); //데미지
            //화살 목표지점에 도착 시(몬스터에 화살이 맞았을때)
        }
    }

    void CheckTarget()
    {
        if (Tower_Arrow.m_MonListInRange.Count > 0)
        {
            for (int ii = 0; ii < Tower_Arrow.m_MonListInRange.Count; ii++)
            {//for문 돌려서 몬스터리스트에 첫번째 몬스터를 타겟으로 설정
                if (Tower_Arrow.m_MonListInRange[ii] != null)
                {
                    m_Target = Tower_Arrow.m_MonListInRange[ii];
                    break;
                }
            }
        }
    }
}

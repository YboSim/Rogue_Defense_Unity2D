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

        Destroy(gameObject, 5.0f);  //5초후 삭제

        CheckTarget();

        StartCoroutine(MoveToTarget(m_Target));
    }

    // Update is called once per frame
    void Update()
    {
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
                a_Time += Time.deltaTime;
                float a_LinearT = a_Time / a_Duration;
                float a_HeightT = m_Curve.Evaluate(a_LinearT);

                float a_Height = Mathf.Lerp(0.0f, 8.0f, a_HeightT); //폭탄이 타겟위치로 선형보간

                Vector3 a_CacPos = Vector2.Lerp(a_StartPos, a_EndPos, a_LinearT) + new Vector2(0.0f, a_Height); //커브에 선형보간한 값을 더함
                transform.position = a_CacPos;

                yield return null;
            }

            Explosion(); //목표지점 도착후 폭발 이펙트 발생

            Damage(); //목표지점에서 범위안에 몬스터들에게 데미지입힘

            Destroy(gameObject); //폭발과 동시에 삭제
        }
    }

    void CheckTarget()
    {
        if (Tower_Bomb.m_MonListInRange.Count > 0)
        {
            for (int ii = 0; ii < Tower_Bomb.m_MonListInRange.Count; ii++)
            {//for문 돌려서 몬스터리스트에 첫번째 몬스터를 타겟으로 설정
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

using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueGenerator : MonoBehaviour
{
    int[] m_MaxPoolSize = { 60, 50, 40, 30 };
    public GameObject[] m_RoguePrefab;

    public Transform m_StartPoint; //로그들의 생성위치
    public PathCreator[] m_Path;   //로그들의 경로
    public Transform m_RoguePool;  //로그들을 담을 부모 오브젝트

    //로그 스폰 주기 관련 변수
    float m_SpawnIdx1;
    float m_SpawnIdx2;
    float m_SpawnIdx3;
    float m_SpawnIdx4;
    float m_SpawnIdxMB;

    float m_SpawnTimer1;
    float m_SpawnTimer2;
    float m_SpawnTimer3;
    float m_SpawnTimer4;
    //로그 스폰 주기 관련 변수

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RogueSpawnTimerSetting());

        //맵 입장시 오브젝트 풀링
        CreateRoguePool(0);
        CreateRoguePool(1);
        CreateRoguePool(2);
        CreateRoguePool(3);
        //맵 입장시 오브젝트 풀링

        m_SpawnIdx1 = 3.0f;
        m_SpawnIdx2 = 3.3f;
        m_SpawnIdx3 = 3.7f;
        m_SpawnIdx4 = 4.1f;
        m_SpawnIdxMB = 120.0f;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnRogue_01(m_SpawnTimer1);

        SpawnRogue_02(m_SpawnTimer2);

        SpawnRogue_03(m_SpawnTimer3);

        SpawnRogue_04(m_SpawnTimer4);

        SpawnRogue_MB();
    }

    IEnumerator RogueSpawnTimerSetting()
    {
        m_SpawnTimer1 = 6.0f;
        m_SpawnTimer2 = 6.0f;
        m_SpawnTimer3 = 8.1f;
        m_SpawnTimer4 = 12.1f;

        yield return new WaitForSeconds(120.0f);

        m_SpawnTimer1 = 3.4f;
        m_SpawnTimer2 = 3.4f;
        m_SpawnTimer3 = 4.0f;
        m_SpawnTimer4 = 6.0f;

        yield return new WaitForSeconds(120.0f);

        m_SpawnTimer1 = 2.7f;
        m_SpawnTimer2 = 2.7f;
        m_SpawnTimer3 = 3.0f;
        m_SpawnTimer4 = 3.4f;

        yield return new WaitForSeconds(120.0f);

        m_SpawnTimer1 = 2.1f;
        m_SpawnTimer2 = 2.3f;
        m_SpawnTimer3 = 2.5f;
        m_SpawnTimer4 = 2.7f;

        yield return new WaitForSeconds(120.0f);

        m_SpawnTimer1 = 1.85f;
        m_SpawnTimer2 = 2.1f;
        m_SpawnTimer3 = 2.2f;
        m_SpawnTimer4 = 2.3f;

    }

    void CreateRoguePool(int a_RogueIdx)
    {
        for (int ii = 0; ii < m_MaxPoolSize[a_RogueIdx]; ii++)
        {
            GameObject a_Rogue = Instantiate(m_RoguePrefab[a_RogueIdx]) as GameObject;

            //위치 지정
            a_Rogue.transform.position = m_StartPoint.position;
            a_Rogue.transform.SetParent(m_RoguePool);
            //위치 지정

            //사이즈 조정
            if (Chapter_Mgr.m_MapIdx == 1 || Chapter_Mgr.m_MapIdx == 2) //Burial, Arcitc Map
            {
                a_Rogue.transform.localScale =
                    new Vector3(0.3f, 0.3f, 1.0f);
                a_Rogue.GetComponentInChildren<RectTransform>().localScale =
                    new Vector3(1.0f, 1.0f, 1.0f);
            }
            else // Forest, Desert Map
            {
                a_Rogue.transform.localScale =
                    new Vector3(-0.3f, 0.3f, 1.0f);
                a_Rogue.GetComponentInChildren<RectTransform>().localScale =
                    new Vector3(-1.0f, 1.0f, 1.0f); //캔버스 반전
            }
            //사이즈 조정

            a_Rogue.gameObject.SetActive(false);
        }
    }

    void TakeRogueFromPool(int a_RogueIdx)
    {
        //게임오버상태일 경우 로그생산 리턴
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        int a_RandomIdx = Random.Range(0, 2); //path 지정용 랜덤 변수

        Monster[] a_Rogues = m_RoguePool.transform.GetComponentsInChildren<Monster>(true);

        foreach (Monster a_Rogue in a_Rogues)
        {
            if (a_Rogue.gameObject.activeSelf == false)
            {
                if (a_Rogue.gameObject.name.Contains("_0" + (a_RogueIdx + 1).ToString()) == true)
                {
                    a_Rogue.GetComponent<Monster>().HpSetting(); //게임 플레이 타임에 따른 로그들 Hp설정

                    a_Rogue.transform.position = m_StartPoint.position; //로그생성후 맵중앙에 잠깐의 프레임동안 깜빡거림을 방지하기위해 카메라 범위 밖에서 생성
                    a_Rogue.gameObject.SetActive(true);

                    //경로지정
                    PathFollower a_Path = a_Rogue.gameObject.AddComponent<PathFollower>();
                    if (a_Path != null)
                    {
                        a_Path.pathCreator = m_Path[a_RandomIdx];
                    }
                    //경로지정

                    return;
                }
            }
        }
    }

    void CreateRogue_MB() //Rogue_MB 스폰 메서드
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        GameObject obj = Instantiate(m_RoguePrefab[4]) as GameObject;
        obj.transform.position = m_StartPoint.position;//스폰위치 지정
        PathFollower a_Path = obj.AddComponent<PathFollower>();

        int a_RandomIdx = Random.Range(0, 2);

        //사이즈 조정
        if (Chapter_Mgr.m_MapIdx == 1 || Chapter_Mgr.m_MapIdx == 2)
        {
            obj.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
            obj.GetComponentInChildren<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            obj.transform.localScale = new Vector3(-0.8f, 0.8f, 1.0f);
            obj.GetComponentInChildren<RectTransform>().localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        //사이즈 조정

        // 경로지정
        if (a_Path.pathCreator == null)
        {
            a_Path.pathCreator = m_Path[a_RandomIdx];
        }
        // 경로지정
    }

    public void SapwnRogue_FB() //Rogue_MB 스폰 메서드
    {
        if (Game_Mgr.m_GameState == GameState.GameOver || Game_Mgr.m_GameState == GameState.Victory)
            return;

        GameObject obj = Instantiate(m_RoguePrefab[5]) as GameObject;
        obj.transform.position = m_StartPoint.position;//스폰위치 지정
        PathFollower a_Path = obj.AddComponent<PathFollower>();

        //사이즈 조정
        if (Chapter_Mgr.m_MapIdx == 1 || Chapter_Mgr.m_MapIdx == 2)
        {
            obj.transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
            obj.GetComponentInChildren<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            obj.transform.localScale = new Vector3(-0.9f, 0.9f, 1.0f);
            obj.GetComponentInChildren<RectTransform>().localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        //사이즈 조정

        // 경로지정
        a_Path.pathCreator = m_Path[0];
        // 경로지정
    }

    void SpawnRogue_01(float a_Index)
    {
        if (m_SpawnIdx1 > 0.0f)
        {
            m_SpawnIdx1 -= Time.deltaTime;
            if (m_SpawnIdx1 <= 0.0f)
            {
                m_SpawnIdx1 = a_Index;

                TakeRogueFromPool(0);
            }
        }
    }

    void SpawnRogue_02(float a_Index)
    {
        if (m_SpawnIdx2 > 0.0f)
        {
            m_SpawnIdx2 -= Time.deltaTime;
            if (m_SpawnIdx2 <= 0.0f)
            {
                m_SpawnIdx2 = a_Index;

                TakeRogueFromPool(1);
            }
        }
    }

    void SpawnRogue_03(float a_Index)
    {
        if (m_SpawnIdx3 > 0.0f)
        {
            m_SpawnIdx3 -= Time.deltaTime;
            if (m_SpawnIdx3 <= 0.0f)
            {
                m_SpawnIdx3 = a_Index;

                TakeRogueFromPool(2);
            }
        }
    }

    void SpawnRogue_04(float a_Index)
    {
        if (m_SpawnIdx4 > 0.0f)
        {
            m_SpawnIdx4 -= Time.deltaTime;
            if (m_SpawnIdx4 <= 0.0f)
            {
                m_SpawnIdx4 = a_Index;

                TakeRogueFromPool(3);
            }

        }
    }

    void SpawnRogue_MB()
    {
        if (m_SpawnIdxMB > 0.0f)
        {
            m_SpawnIdxMB -= Time.deltaTime;
            if (m_SpawnIdxMB <= 0.0f)
            {
                CreateRogue_MB();

                m_SpawnIdxMB = 120.0f;
            }
        }
    }
}

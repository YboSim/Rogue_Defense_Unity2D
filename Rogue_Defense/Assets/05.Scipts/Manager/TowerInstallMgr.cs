using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TowerInstallMgr : MonoBehaviour
{
    [Header("------ TowerInstall ------")]
    public SlotScript[] m_TowerSlots;
    public InstallSlotScript[] m_InstallSlots;
    public Image m_MsObj = null;        //마우스를 따라다녀야 하는 오브젝트
    int m_SaveIndex = -1;               //어떤 타워를 설치하는지 저장해놓을 인덱스

    Color m_OriginColor;
    Color m_RedColor = Color.red;
    bool m_MsObjOnOff = false;

    [Header("------ TowerPrefab ------")]
    public GameObject m_ArrowTwPrefab;
    public GameObject m_BombTwPrefab;
    public GameObject m_IceTwPrefab;
    public GameObject m_LavaTwPrefab;
    public GameObject m_PoisonTwPrefab;

    [Header("------ Button ------")]
    //타워 설치 관련 버튼
    public Transform m_TowerBtnGroup;
    public Button m_Tower_Arrow_Btn;
    public Button m_Tower_Bomb_Btn;
    public Button m_Tower_Ice_Btn;
    public Button m_Tower_Lava_Btn;
    public Button m_Tower_Posion_Btn;
    public Button m_TowerInstall_Btn;
    //타워 설치 관련 버튼

    //타워 설치 버튼 열고닫음 상태 정보 담을 변수
    bool m_TowerInstall_OnOff = false;
    Vector3 m_BtnOffPos = new Vector3(-800.0f, 0.0f, 0.0f);
    Vector3 m_BtnOnPos = new Vector3(0.0f, 0.0f, 0.0f);
    float m_TowerBtnGroupMvSpeed = 9000.0f;
    //타워 설치 버튼 열고닫음 상태 정보 담을 변수

    // Start is called before the first frame update
    void Start()
    {
        //타워 설치 관련
        m_TowerInstall_OnOff = false; //타워 설치 버튼 Off상태로 시작

        m_OriginColor = m_MsObj.transform.Find("MsIconImg").GetComponent<Image>().color;

        if (m_TowerInstall_Btn != null)
            m_TowerInstall_Btn.onClick.AddListener(() =>
            {
                if (Game_Mgr.m_GameState == GameState.Playing)
                {
                    m_TowerInstall_OnOff = !m_TowerInstall_OnOff;

                    Sound_Mgr.Instance.PlayGUISound("TowerInstallSound", 0.7f);
                }
            });

        if (m_Tower_Arrow_Btn != null)
            m_Tower_Arrow_Btn.onClick.AddListener(() =>
            {
                m_TowerInstall_OnOff = false;

                TowerInstallBtnClick();
            });

        if (m_Tower_Bomb_Btn != null)
            m_Tower_Bomb_Btn.onClick.AddListener(() =>
            {
                m_TowerInstall_OnOff = false;

                TowerInstallBtnClick();
            });

        if (m_Tower_Ice_Btn != null)
            m_Tower_Ice_Btn.onClick.AddListener(() =>
            {
                m_TowerInstall_OnOff = false;

                TowerInstallBtnClick();
            });

        if (m_Tower_Lava_Btn != null)
            m_Tower_Lava_Btn.onClick.AddListener(() =>
            {
                m_TowerInstall_OnOff = false;

                TowerInstallBtnClick();
            });

        if (m_Tower_Posion_Btn != null)
            m_Tower_Posion_Btn.onClick.AddListener(() =>
            {
                m_TowerInstall_OnOff = false;

                TowerInstallBtnClick();
            });
        //타워 설치 관련
    }

    // Update is called once per frame
    void Update()
    {
        TowerInstallOnOff_Update(); //TowerInstall 버튼들 위치 이동 함수

        if (m_MsObjOnOff == true)
        {
            TowerInstallIng();  //타워 설치 버튼 클릭 후 InstallSlot 선택중 상태

            InstallSlotOn(); //타워설치 가능위치 보여주는 이미지 켜주는 함수

            if(Input.GetKeyDown(KeyCode.Escape) == true)
            {
                TowerInstallCancel(); //TowerInstall 버튼 클릭 후 ESC누를시 취소
            }
        }
        else
        {
            InstallSlotOff(); //타워설치 가능위치 보여줄 이미지 꺼주는 함수
        }
    }
    void TowerInstallBtnClick() //타워 설치 버튼 클릭
    {
        m_SaveIndex = -1; // 인덱스 초기화

        for (int ii = 0; ii < m_TowerSlots.Length; ii++)
        {
            if (IsTowerSlot(m_TowerSlots[ii]) == true)
            {
                if (m_TowerSlots[ii].GetComponent<SlotScript>().m_IsInstalled == true)
                {
                    Game_Mgr.Inst.ShowMessage("이미 설치된 타워 입니다.");//이미 설치 된 타워 입니다.
                    return;
                }

                m_SaveIndex = ii;

                //보유 골드 확인
                if (m_SaveIndex == 0)
                {
                    if (Game_Mgr.Inst.m_Gold < 100)
                    {
                        Game_Mgr.Inst.ShowMessage("보유 골드가 부족합니다.",2.0f);
                        return;
                    }
                }
                else if (m_SaveIndex == 1)
                {
                    if (Game_Mgr.Inst.m_Gold < 180)
                    {
                        Game_Mgr.Inst.ShowMessage("보유 골드가 부족합니다.",2.0f);
                        return;
                    }
                }
                else if (m_SaveIndex == 2)
                {
                    if (Game_Mgr.Inst.m_Gold < 160)
                    {
                        Game_Mgr.Inst.ShowMessage("보유 골드가 부족합니다.",2.0f);
                        return;
                    }
                }
                else if (m_SaveIndex == 3)
                {
                    if (Game_Mgr.Inst.m_Gold < 160)
                    {
                        Game_Mgr.Inst.ShowMessage("보유 골드가 부족합니다.",2.0f);
                        return;
                    }
                }
                else if (m_SaveIndex == 4)
                {
                    if (Game_Mgr.Inst.m_Gold < 140)
                    {
                        Game_Mgr.Inst.ShowMessage("보유 골드가 부족합니다.",2.0f);
                        return;
                    }
                }
                //보유 골드 확인

                Transform a_ChildImg = m_MsObj.transform.Find("MsIconImg");
                if (a_ChildImg != null)
                    a_ChildImg.GetComponent<Image>().sprite =
                                    m_TowerSlots[ii].SlotImg.sprite;
                if (m_SaveIndex == 4)
                    a_ChildImg.GetComponent<RectTransform>().sizeDelta = new Vector2(110.0f, 130.0f);
                else
                    a_ChildImg.GetComponent<RectTransform>().sizeDelta = new Vector2(88.0f, 140.0f);

                m_MsObj.gameObject.SetActive(true);

                m_MsObjOnOff = true;
                break;
            }
        }

        Sound_Mgr.Instance.PlayEffSound("TowerInstallSound", 0.7f);
    }

    void TowerInstallCancel() //타워 설치 버튼클릭 후 ESC버튼 눌러서 취소하기
    {
        m_SaveIndex = -1; // 인덱스 초기화
        
        m_MsObj.gameObject.SetActive(false);
        m_MsObjOnOff = false;
    }

    void TowerInstallIng() //타워 설치 버튼 클릭 후 InstallSlot 선택중 상태
    {
        if (0 <= m_SaveIndex)
            m_MsObj.transform.position = Input.mousePosition;

        Transform a_ChildImg = m_MsObj.transform.Find("MsIconImg");
        for (int ii = 0; ii < m_InstallSlots.Length; ii++)
        {
            if(IsInstallSlot(m_InstallSlots[ii]) == true) //타워설치 가능 구역
            {
                a_ChildImg.GetComponent<Image>().color = m_OriginColor;

                m_MsObj.transform.position = Camera.main.WorldToScreenPoint(m_InstallSlots[ii].transform.position);
                //InstallSlot에 마우스 가져다 댓을때 설치위치 보여주기

                TowerPosClick(m_InstallSlots[ii]); //타워설치하기

                break;
            }
            else//타워설치 불가능 구역
            {
                a_ChildImg.GetComponent<Image>().color = m_RedColor;

                m_MsObj.transform.position = Input.mousePosition;
            }
        }
    }

    void TowerPosClick(InstallSlotScript a_InstallSlot) // InstallSlot 클릭
    {
        if (m_MsObjOnOff == false)
            return;

        if (m_SaveIndex < 0 || m_TowerSlots.Length <= m_SaveIndex)
            return;

        if(Input.GetMouseButtonDown(0) == true)
        {
            CreateTower(a_InstallSlot);

            m_MsObjOnOff = false;
            m_MsObj.gameObject.SetActive(false);

            a_InstallSlot.m_IsInstalled = true;
            a_InstallSlot.GetComponent<SpriteRenderer>().sortingOrder = -1;
            //다른 타워를 설치 할 때 이미 설치된슬롯에 SlotPos표지판이 타워 앞으로 표시되는 현상 방지

            Sound_Mgr.Instance.PlayGUISound("TowerInstallSound", 0.7f);
        }
    }

    void CreateTower(InstallSlotScript a_InstallSlot)
    {
        GameObject a_Tower;

        if(m_SaveIndex  == 0)
        {
            a_Tower = Instantiate(m_ArrowTwPrefab) as GameObject;
            a_Tower.transform.SetParent(a_InstallSlot.transform, false);
            a_Tower.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);

            //골드 차감
            int a_ArrowTowerValue = 100;
            if (Game_Mgr.Inst.m_Gold - a_ArrowTowerValue >= 0)
                Game_Mgr.Inst.AddGold(-a_ArrowTowerValue);
            //골드 차감

            SlotScript a_Slot = m_TowerSlots[0].GetComponent<SlotScript>();
            a_Slot.m_IsInstalled = true; //해당 타워 설치 된것으로 체크
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //X표시
                a_Slot.m_InstalledImg.gameObject.SetActive(true);
        }
        else if(m_SaveIndex == 1)
        {
            a_Tower = Instantiate(m_BombTwPrefab) as GameObject;
            a_Tower.transform.SetParent(a_InstallSlot.transform, false);
            a_Tower.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);

            int a_BombTowerValue = 180;
            if (Game_Mgr.Inst.m_Gold - a_BombTowerValue >= 0)
                Game_Mgr.Inst.AddGold(-a_BombTowerValue);

            SlotScript a_Slot = m_TowerSlots[1].GetComponent<SlotScript>();
            a_Slot.m_IsInstalled = true; //해당 타워 설치 된것으로 체크
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //X표시
                a_Slot.m_InstalledImg.gameObject.SetActive(true);

        }
        else if (m_SaveIndex == 2)
        {
            a_Tower = Instantiate(m_IceTwPrefab) as GameObject;
            a_Tower.transform.SetParent(a_InstallSlot.transform, false);
            a_Tower.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);

            int a_IceTowerValue = 160;
            if (Game_Mgr.Inst.m_Gold - a_IceTowerValue >= 0)
                Game_Mgr.Inst.AddGold(-a_IceTowerValue);

            SlotScript a_Slot = m_TowerSlots[2].GetComponent<SlotScript>();
            a_Slot.m_IsInstalled = true; //해당 타워 설치 된것으로 체크
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //X표시
                a_Slot.m_InstalledImg.gameObject.SetActive(true);
        }
        else if (m_SaveIndex == 3)
        {
            a_Tower = Instantiate(m_LavaTwPrefab) as GameObject;
            a_Tower.transform.SetParent(a_InstallSlot.transform, false);
            a_Tower.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);

            int a_LavaTowerValue = 160;
            if (Game_Mgr.Inst.m_Gold - a_LavaTowerValue >= 0)
                Game_Mgr.Inst.AddGold(-a_LavaTowerValue);

            SlotScript a_Slot = m_TowerSlots[3].GetComponent<SlotScript>();
            a_Slot.m_IsInstalled = true; //해당 타워 설치 된것으로 체크
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //X표시
                a_Slot.m_InstalledImg.gameObject.SetActive(true);
        }
        else if (m_SaveIndex == 4)
        {
            a_Tower = Instantiate(m_PoisonTwPrefab) as GameObject;
            a_Tower.transform.SetParent(a_InstallSlot.transform, false);
            a_Tower.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);

            int a_PosionTowerValue = 140;
            if (Game_Mgr.Inst.m_Gold - a_PosionTowerValue >= 0)
                Game_Mgr.Inst.AddGold(-a_PosionTowerValue);

            SlotScript a_Slot = m_TowerSlots[4].GetComponent<SlotScript>();
            a_Slot.m_IsInstalled = true; //해당 타워 설치 된것으로 체크
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //X표시
                a_Slot.m_InstalledImg.gameObject.SetActive(true);
        }

    }

    void InstallSlotOn()
    {
        for (int ii = 0; ii < m_InstallSlots.Length; ii++)
        {
            m_InstallSlots[ii].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void InstallSlotOff()
    {
        for (int ii = 0; ii < m_InstallSlots.Length; ii++)
        {
            m_InstallSlots[ii].GetComponent<SpriteRenderer>().enabled = false;
        }
    }


    bool IsTowerSlot(SlotScript a_CkSlot)
    {  //마우스가 UI 슬롯 위에 있는지? 판단하는 함수
        if (a_CkSlot == null)
            return false;

        Vector3[] v = new Vector3[4];
        a_CkSlot.GetComponent<RectTransform>().GetWorldCorners(v);
        //v[0] : 좌측하단  v[1] : 좌측상단  v[2] : 우측상단  v[3] : 우측하단
        //v[0] 좌측 하단이 0, 0 좌표인 스크린좌표(Screen.width, Screen.height)를
        //마우스의 좌표계
        //RectTransform : 즉 UGUI 좌표 기준

        if (v[0].x <= Input.mousePosition.x && Input.mousePosition.x <= v[2].x &&
           v[0].y <= Input.mousePosition.y && Input.mousePosition.y <= v[2].y)
        {            
            return true;
        }

        return false;

    }//bool IsCollSlot(SlotScript a_CkSlot)

    bool IsInstallSlot(InstallSlotScript a_InstSlot)
    {  //마우스가 SpriteRenderer 슬롯 위에 있는지? 판단하는 함수
        if (a_InstSlot == null)
            return false;

        Vector3[] v = new Vector3[2];
        float a_Width = a_InstSlot.GetComponent<SpriteRenderer>().size.x;
        float a_Height = a_InstSlot.GetComponent<SpriteRenderer>().size.y;
        v[0] = new Vector3(a_InstSlot.transform.position.x - a_Width,
                           a_InstSlot.transform.position.y - a_Height, 0.0f); // 좌측하단
        v[1] = new Vector3(a_InstSlot.transform.position.x + a_Width,
                           a_InstSlot.transform.position.y + a_Height, 0.0f); // 우측상단

        Vector3 a_MsPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                             Input.mousePosition.y,
                                                             0.0f)); //현재마우스 위치의 월드포인트 값

        if (v[0].x < a_MsPos.x && a_MsPos.x <= v[1].x &&
            v[0].y <= a_MsPos.y && a_MsPos.y <= v[1].y) //타워 설치 가능 구역
        {
            if (a_InstSlot.m_IsInstalled == false) //해당 슬롯에 이미 타워가 설치되어 있지 않으면
            {
                return true;
            }
        }

        return false;
    }

    void TowerInstallOnOff_Update() //타워버튼 그룹OnOff 함수
    {
        if (m_TowerInstall_OnOff == true) //타워버튼그룹 ON
        {
            m_TowerBtnGroup.localPosition =
                Vector3.MoveTowards(m_TowerBtnGroup.localPosition, m_BtnOnPos,
                                    m_TowerBtnGroupMvSpeed * Time.deltaTime);
        }
        else //if(m_TowerInstall_Onoff == true) //타워버튼그룹 Off
        {
            m_TowerBtnGroup.localPosition =
                Vector3.MoveTowards(m_TowerBtnGroup.localPosition, m_BtnOffPos,
                                    m_TowerBtnGroupMvSpeed * Time.deltaTime);
        }
    }
}

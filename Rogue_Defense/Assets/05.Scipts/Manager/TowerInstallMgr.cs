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
    public Image m_MsObj = null;        //���콺�� ����ٳ�� �ϴ� ������Ʈ
    int m_SaveIndex = -1;               //� Ÿ���� ��ġ�ϴ��� �����س��� �ε���

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
    //Ÿ�� ��ġ ���� ��ư
    public Transform m_TowerBtnGroup;
    public Button m_Tower_Arrow_Btn;
    public Button m_Tower_Bomb_Btn;
    public Button m_Tower_Ice_Btn;
    public Button m_Tower_Lava_Btn;
    public Button m_Tower_Posion_Btn;
    public Button m_TowerInstall_Btn;
    //Ÿ�� ��ġ ���� ��ư

    //Ÿ�� ��ġ ��ư ������� ���� ���� ���� ����
    bool m_TowerInstall_OnOff = false;
    Vector3 m_BtnOffPos = new Vector3(-800.0f, 0.0f, 0.0f);
    Vector3 m_BtnOnPos = new Vector3(0.0f, 0.0f, 0.0f);
    float m_TowerBtnGroupMvSpeed = 9000.0f;
    //Ÿ�� ��ġ ��ư ������� ���� ���� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        //Ÿ�� ��ġ ����
        m_TowerInstall_OnOff = false; //Ÿ�� ��ġ ��ư Off���·� ����

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
        //Ÿ�� ��ġ ����
    }

    // Update is called once per frame
    void Update()
    {
        TowerInstallOnOff_Update(); //TowerInstall ��ư�� ��ġ �̵� �Լ�

        if (m_MsObjOnOff == true)
        {
            TowerInstallIng();  //Ÿ�� ��ġ ��ư Ŭ�� �� InstallSlot ������ ����

            InstallSlotOn(); //Ÿ����ġ ������ġ �����ִ� �̹��� ���ִ� �Լ�

            if(Input.GetKeyDown(KeyCode.Escape) == true)
            {
                TowerInstallCancel(); //TowerInstall ��ư Ŭ�� �� ESC������ ���
            }
        }
        else
        {
            InstallSlotOff(); //Ÿ����ġ ������ġ ������ �̹��� ���ִ� �Լ�
        }
    }
    void TowerInstallBtnClick() //Ÿ�� ��ġ ��ư Ŭ��
    {
        m_SaveIndex = -1; // �ε��� �ʱ�ȭ

        for (int ii = 0; ii < m_TowerSlots.Length; ii++)
        {
            if (IsTowerSlot(m_TowerSlots[ii]) == true)
            {
                if (m_TowerSlots[ii].GetComponent<SlotScript>().m_IsInstalled == true)
                {
                    Game_Mgr.Inst.ShowMessage("�̹� ��ġ�� Ÿ�� �Դϴ�.");//�̹� ��ġ �� Ÿ�� �Դϴ�.
                    return;
                }

                m_SaveIndex = ii;

                //���� ��� Ȯ��
                if (m_SaveIndex == 0)
                {
                    if (Game_Mgr.Inst.m_Gold < 100)
                    {
                        Game_Mgr.Inst.ShowMessage("���� ��尡 �����մϴ�.",2.0f);
                        return;
                    }
                }
                else if (m_SaveIndex == 1)
                {
                    if (Game_Mgr.Inst.m_Gold < 180)
                    {
                        Game_Mgr.Inst.ShowMessage("���� ��尡 �����մϴ�.",2.0f);
                        return;
                    }
                }
                else if (m_SaveIndex == 2)
                {
                    if (Game_Mgr.Inst.m_Gold < 160)
                    {
                        Game_Mgr.Inst.ShowMessage("���� ��尡 �����մϴ�.",2.0f);
                        return;
                    }
                }
                else if (m_SaveIndex == 3)
                {
                    if (Game_Mgr.Inst.m_Gold < 160)
                    {
                        Game_Mgr.Inst.ShowMessage("���� ��尡 �����մϴ�.",2.0f);
                        return;
                    }
                }
                else if (m_SaveIndex == 4)
                {
                    if (Game_Mgr.Inst.m_Gold < 140)
                    {
                        Game_Mgr.Inst.ShowMessage("���� ��尡 �����մϴ�.",2.0f);
                        return;
                    }
                }
                //���� ��� Ȯ��

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

    void TowerInstallCancel() //Ÿ�� ��ġ ��ưŬ�� �� ESC��ư ������ ����ϱ�
    {
        m_SaveIndex = -1; // �ε��� �ʱ�ȭ
        
        m_MsObj.gameObject.SetActive(false);
        m_MsObjOnOff = false;
    }

    void TowerInstallIng() //Ÿ�� ��ġ ��ư Ŭ�� �� InstallSlot ������ ����
    {
        if (0 <= m_SaveIndex)
            m_MsObj.transform.position = Input.mousePosition;

        Transform a_ChildImg = m_MsObj.transform.Find("MsIconImg");
        for (int ii = 0; ii < m_InstallSlots.Length; ii++)
        {
            if(IsInstallSlot(m_InstallSlots[ii]) == true) //Ÿ����ġ ���� ����
            {
                a_ChildImg.GetComponent<Image>().color = m_OriginColor;

                m_MsObj.transform.position = Camera.main.WorldToScreenPoint(m_InstallSlots[ii].transform.position);
                //InstallSlot�� ���콺 ������ ������ ��ġ��ġ �����ֱ�

                TowerPosClick(m_InstallSlots[ii]); //Ÿ����ġ�ϱ�

                break;
            }
            else//Ÿ����ġ �Ұ��� ����
            {
                a_ChildImg.GetComponent<Image>().color = m_RedColor;

                m_MsObj.transform.position = Input.mousePosition;
            }
        }
    }

    void TowerPosClick(InstallSlotScript a_InstallSlot) // InstallSlot Ŭ��
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
            //�ٸ� Ÿ���� ��ġ �� �� �̹� ��ġ�Ƚ��Կ� SlotPosǥ������ Ÿ�� ������ ǥ�õǴ� ���� ����

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

            //��� ����
            int a_ArrowTowerValue = 100;
            if (Game_Mgr.Inst.m_Gold - a_ArrowTowerValue >= 0)
                Game_Mgr.Inst.AddGold(-a_ArrowTowerValue);
            //��� ����

            SlotScript a_Slot = m_TowerSlots[0].GetComponent<SlotScript>();
            a_Slot.m_IsInstalled = true; //�ش� Ÿ�� ��ġ �Ȱ����� üũ
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //Xǥ��
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
            a_Slot.m_IsInstalled = true; //�ش� Ÿ�� ��ġ �Ȱ����� üũ
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //Xǥ��
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
            a_Slot.m_IsInstalled = true; //�ش� Ÿ�� ��ġ �Ȱ����� üũ
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //Xǥ��
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
            a_Slot.m_IsInstalled = true; //�ش� Ÿ�� ��ġ �Ȱ����� üũ
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //Xǥ��
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
            a_Slot.m_IsInstalled = true; //�ش� Ÿ�� ��ġ �Ȱ����� üũ
            if (a_Slot.m_InstalledImg.gameObject.activeSelf == false) //Xǥ��
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
    {  //���콺�� UI ���� ���� �ִ���? �Ǵ��ϴ� �Լ�
        if (a_CkSlot == null)
            return false;

        Vector3[] v = new Vector3[4];
        a_CkSlot.GetComponent<RectTransform>().GetWorldCorners(v);
        //v[0] : �����ϴ�  v[1] : �������  v[2] : �������  v[3] : �����ϴ�
        //v[0] ���� �ϴ��� 0, 0 ��ǥ�� ��ũ����ǥ(Screen.width, Screen.height)��
        //���콺�� ��ǥ��
        //RectTransform : �� UGUI ��ǥ ����

        if (v[0].x <= Input.mousePosition.x && Input.mousePosition.x <= v[2].x &&
           v[0].y <= Input.mousePosition.y && Input.mousePosition.y <= v[2].y)
        {            
            return true;
        }

        return false;

    }//bool IsCollSlot(SlotScript a_CkSlot)

    bool IsInstallSlot(InstallSlotScript a_InstSlot)
    {  //���콺�� SpriteRenderer ���� ���� �ִ���? �Ǵ��ϴ� �Լ�
        if (a_InstSlot == null)
            return false;

        Vector3[] v = new Vector3[2];
        float a_Width = a_InstSlot.GetComponent<SpriteRenderer>().size.x;
        float a_Height = a_InstSlot.GetComponent<SpriteRenderer>().size.y;
        v[0] = new Vector3(a_InstSlot.transform.position.x - a_Width,
                           a_InstSlot.transform.position.y - a_Height, 0.0f); // �����ϴ�
        v[1] = new Vector3(a_InstSlot.transform.position.x + a_Width,
                           a_InstSlot.transform.position.y + a_Height, 0.0f); // �������

        Vector3 a_MsPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                             Input.mousePosition.y,
                                                             0.0f)); //���縶�콺 ��ġ�� ��������Ʈ ��

        if (v[0].x < a_MsPos.x && a_MsPos.x <= v[1].x &&
            v[0].y <= a_MsPos.y && a_MsPos.y <= v[1].y) //Ÿ�� ��ġ ���� ����
        {
            if (a_InstSlot.m_IsInstalled == false) //�ش� ���Կ� �̹� Ÿ���� ��ġ�Ǿ� ���� ������
            {
                return true;
            }
        }

        return false;
    }

    void TowerInstallOnOff_Update() //Ÿ����ư �׷�OnOff �Լ�
    {
        if (m_TowerInstall_OnOff == true) //Ÿ����ư�׷� ON
        {
            m_TowerBtnGroup.localPosition =
                Vector3.MoveTowards(m_TowerBtnGroup.localPosition, m_BtnOnPos,
                                    m_TowerBtnGroupMvSpeed * Time.deltaTime);
        }
        else //if(m_TowerInstall_Onoff == true) //Ÿ����ư�׷� Off
        {
            m_TowerBtnGroup.localPosition =
                Vector3.MoveTowards(m_TowerBtnGroup.localPosition, m_BtnOffPos,
                                    m_TowerBtnGroupMvSpeed * Time.deltaTime);
        }
    }
}

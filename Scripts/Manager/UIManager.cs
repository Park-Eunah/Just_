using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject[] mapImages;

    public GameObject startTimer;
    public GameObject timer;
    public GameObject purchasedImg; //����Ʋ ���ſϷ� �̹���
    public GameObject taskPanel;
    public GameObject guide; //���̵�â. ��������� �»�ܿ� �ִ� ���̵�â, �̴ϰ��ӿ����� �̴ϰ��� ���̵�â
    public GameObject endPanel; //2������ ���� �� �ӽ÷� ����� ���� �ȳ�â

    private GameObject settingImage = null; //����â
    private GameObject mapPanel = null; 
    private GameObject mImg = null;
    private GameObject settingImg = null;

    private bool map = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this);
    }

    void Start()
    {
        FindUI();

        if (SceneManager.GetActiveScene().name.Contains("GameStart") && GameManager.sceneCount > 1) //1������ ������ ������� ���̵�â ������
        {
            guide.SetActive(false);
        }
    }

    void Update()
    {
        InputKey();
    }

    public void InputKey() //�Է°��� ���� �޼��� ȣ��
    {
        //escŰ�� ����â ���� �ݱ�.
        if (settingImage != null && settingImage.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            SettingOpen();
        }
        else if (settingImage != null && settingImage.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            SettingClose();
        }

        //mŰ�� ���� ���� �ݱ�.
        if (mapPanel != null && Input.GetKeyDown(KeyCode.M))
        {
            MapOpenOrClose();
        }

        //nŰ�� ���ϸ���Ʈ ���� ����.
        if(Input.GetKeyDown(KeyCode.N))
        {
            TaskManager.instance.TaskOpenOrClose();
        }
    }

    public void SettingOpen() //����â ���� �޼���
    {
        Time.timeScale = 0f;
        settingImage.SetActive(true);
    }

    public void SettingClose() //����â �ݴ� �޼���
    {
        Time.timeScale = 1f;
        settingImage.SetActive(false);
    }

    public void MapOpenOrClose() //�� ���� �ݴ� �޼���
    {
        if (!map)
        {
            map = true;
            MSetActive(false);
            mapPanel.SetActive(true);
        }
        else
        {
            map = false;
            MSetActive(true);
            mapPanel.SetActive(false);
        }
    }

    public void MapFloorButton(int floor) //���� ������ ���� ��ư�� ������ ȣ���Ǵ� �޼���
    {
        for (int i = 0; i < mapImages.Length; i++)
        {
            if (i == floor)
            {
                mapImages[i].SetActive(true);
            }
            else
                mapImages[i].SetActive(false);
        }
    }

    public void MSetActive(bool trueOrFalse) //������ �ϴ��� m�̹���, ���� �̹��� Ȱ��ȭ/��Ȱ��ȭ �޼���
    {
        if (mImg != null)
        {
            switch (trueOrFalse)
            {
                case true:
                    mImg.SetActive(true);
                    settingImg.SetActive(true);
                    break;
                case false:
                    mImg.SetActive(false);
                    settingImg.SetActive(false);
                    break;
            }
        }
    }

    public void MarketClear() //����Ʋ���� ���� ���ſ� �����ϸ� UI����
    {
        timer.SetActive(false); //Ÿ�̸� �����ְ�
        taskPanel.SetActive(true); //�� �� ����Ʈ ���ְ�
        purchasedImg.SetActive(true); //���ſϷ� �̹��� ����
        Invoke("FadeDownImg", 1f); //���ſϷ� �̹��� �����
    }

    private void FindUI() //ui�� ã�� �����ϴ� �޼���, public���� �ۿ��� �������� �޼���� ���� �����.
    {
        settingImage = transform.Find("SettingView").gameObject;

        if (SceneManager.GetActiveScene().name.Contains("Corridor"))
        {
            mapPanel = transform.Find("MapPanel").gameObject;
            mImg = transform.Find("mapImg").gameObject;
            settingImg = transform.Find("settingImg").gameObject;
        }
    }

    private void FadeDownImg()
    {
        purchasedImg.GetComponent<Animator>().SetBool("FadeDown", true);
        TaskManager.instance.ShowTask(1); //�� �� ä���ֱ�, MarketClear���� �ϸ� ������
    }

    public void RunStart() //����Ʋ �� ����
    {
        startTimer.GetComponent<MiniGame_StartTimer>().enabled = true; //Ÿ�̸� ���ֱ�
        guide.SetActive(false); //���̵�â �����ֱ�
    }

    public void QuitGame() //��������
    {
        SoundManager.instance.ClickSound();
        Application.Quit();
    }

    public void LookRoom() //2������ ���� �� �ӽ÷� ����� ���� �ȳ�â�� �� �ѷ����� ��ư.
    {
        endPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void EndGame() //2������ ���� �� �ӽ÷� ����� ���� �ȳ�â ����, �ð� ���߱�.
    {
        endPanel.SetActive(true);
        Time.timeScale = 0;
    }
}

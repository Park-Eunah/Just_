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
    public GameObject purchasedImg; //빵셔틀 구매완료 이미지
    public GameObject taskPanel;
    public GameObject guide; //가이드창. 등교씬에서는 좌상단에 있는 가이드창, 미니게임에서는 미니게임 가이드창
    public GameObject endPanel; //2일차가 끝난 후 임시로 만들어 놓은 안내창

    private GameObject settingImage = null; //설정창
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

        if (SceneManager.GetActiveScene().name.Contains("GameStart") && GameManager.sceneCount > 1) //1일차가 지나면 등교씬에서 가이드창 지워줌
        {
            guide.SetActive(false);
        }
    }

    void Update()
    {
        InputKey();
    }

    public void InputKey() //입력값에 따른 메서드 호출
    {
        //esc키로 설정창 열고 닫기.
        if (settingImage != null && settingImage.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            SettingOpen();
        }
        else if (settingImage != null && settingImage.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            SettingClose();
        }

        //m키로 맵을 열고 닫기.
        if (mapPanel != null && Input.GetKeyDown(KeyCode.M))
        {
            MapOpenOrClose();
        }

        //n키로 할일리스트 열고 접기.
        if(Input.GetKeyDown(KeyCode.N))
        {
            TaskManager.instance.TaskOpenOrClose();
        }
    }

    public void SettingOpen() //설정창 여는 메서드
    {
        Time.timeScale = 0f;
        settingImage.SetActive(true);
    }

    public void SettingClose() //설정창 닫는 메서드
    {
        Time.timeScale = 1f;
        settingImage.SetActive(false);
    }

    public void MapOpenOrClose() //맵 열고 닫는 메서드
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

    public void MapFloorButton(int floor) //맵의 층수가 적힌 버튼을 누르면 호춣되는 메서드
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

    public void MSetActive(bool trueOrFalse) //오른쪽 하단의 m이미지, 설정 이미지 활성화/비활성화 메서드
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

    public void MarketClear() //빵셔틀에서 과자 구매에 성공하면 UI정리
    {
        timer.SetActive(false); //타이머 지워주고
        taskPanel.SetActive(true); //할 일 리스트 켜주고
        purchasedImg.SetActive(true); //구매완료 이미지 띄우기
        Invoke("FadeDownImg", 1f); //구매완료 이미지 지우기
    }

    private void FindUI() //ui들 찾아 연결하는 메서드, public으로 밖에서 연결할지 메서드로 할지 고민중.
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
        TaskManager.instance.ShowTask(1); //할 일 채워주기, MarketClear에서 하면 오류남
    }

    public void RunStart() //빵셔틀 런 시작
    {
        startTimer.GetComponent<MiniGame_StartTimer>().enabled = true; //타이머 켜주기
        guide.SetActive(false); //가이드창 지워주기
    }

    public void QuitGame() //게임종료
    {
        SoundManager.instance.ClickSound();
        Application.Quit();
    }

    public void LookRoom() //2일차가 끝난 후 임시로 만들어 놓은 안내창의 방 둘러보기 버튼.
    {
        endPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void EndGame() //2일차가 끝난 후 임시로 만들어 놓은 안내창 띄우고, 시간 멈추기.
    {
        endPanel.SetActive(true);
        Time.timeScale = 0;
    }
}

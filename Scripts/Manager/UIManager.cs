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
    public GameObject purchasedImg; //»§¼ÅÆ² ±¸¸Å¿Ï·á ÀÌ¹ÌÁö
    public GameObject taskPanel;
    public GameObject guide; //°¡ÀÌµåÃ¢. µî±³¾À¿¡¼­´Â ÁÂ»ó´Ü¿¡ ÀÖ´Â °¡ÀÌµåÃ¢, ¹Ì´Ï°ÔÀÓ¿¡¼­´Â ¹Ì´Ï°ÔÀÓ °¡ÀÌµåÃ¢
    public GameObject endPanel; //2ÀÏÂ÷°¡ ³¡³­ ÈÄ ÀÓ½Ã·Î ¸¸µé¾î ³õÀº ¾È³»Ã¢

    private GameObject settingImage = null; //¼³Á¤Ã¢
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

        if (SceneManager.GetActiveScene().name.Contains("GameStart") && GameManager.sceneCount > 1) //1ÀÏÂ÷°¡ Áö³ª¸é µî±³¾À¿¡¼­ °¡ÀÌµåÃ¢ Áö¿öÁÜ
        {
            guide.SetActive(false);
        }
    }

    void Update()
    {
        InputKey();
    }

    public void InputKey() //ÀÔ·Â°ª¿¡ µû¸¥ ¸Ş¼­µå È£Ãâ
    {
        //escÅ°·Î ¼³Á¤Ã¢ ¿­°í ´İ±â.
        if (settingImage != null && settingImage.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            SettingOpen();
        }
        else if (settingImage != null && settingImage.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            SettingClose();
        }

        //mÅ°·Î ¸ÊÀ» ¿­°í ´İ±â.
        if (mapPanel != null && Input.GetKeyDown(KeyCode.M))
        {
            MapOpenOrClose();
        }

        //nÅ°·Î ÇÒÀÏ¸®½ºÆ® ¿­°í Á¢±â.
        if(Input.GetKeyDown(KeyCode.N))
        {
            TaskManager.instance.TaskOpenOrClose();
        }
    }

    public void SettingOpen() //¼³Á¤Ã¢ ¿©´Â ¸Ş¼­µå
    {
        Time.timeScale = 0f;
        settingImage.SetActive(true);
    }

    public void SettingClose() //¼³Á¤Ã¢ ´İ´Â ¸Ş¼­µå
    {
        Time.timeScale = 1f;
        settingImage.SetActive(false);
    }

    public void MapOpenOrClose() //¸Ê ¿­°í ´İ´Â ¸Ş¼­µå
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

    public void MapFloorButton(int floor) //¸ÊÀÇ Ãş¼ö°¡ ÀûÈù ¹öÆ°À» ´©¸£¸é È£­„µÇ´Â ¸Ş¼­µå
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

    public void MSetActive(bool trueOrFalse) //¿À¸¥ÂÊ ÇÏ´ÜÀÇ mÀÌ¹ÌÁö, ¼³Á¤ ÀÌ¹ÌÁö È°¼ºÈ­/ºñÈ°¼ºÈ­ ¸Ş¼­µå
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

    public void MarketClear() //»§¼ÅÆ²¿¡¼­ °úÀÚ ±¸¸Å¿¡ ¼º°øÇÏ¸é UIÁ¤¸®
    {
        timer.SetActive(false); //Å¸ÀÌ¸Ó Áö¿öÁÖ°í
        taskPanel.SetActive(true); //ÇÒ ÀÏ ¸®½ºÆ® ÄÑÁÖ°í
        purchasedImg.SetActive(true); //±¸¸Å¿Ï·á ÀÌ¹ÌÁö ¶ç¿ì±â
        Invoke("FadeDownImg", 1f); //±¸¸Å¿Ï·á ÀÌ¹ÌÁö Áö¿ì±â
    }

    private void FindUI() //uiµé Ã£¾Æ ¿¬°áÇÏ´Â ¸Ş¼­µå, publicÀ¸·Î ¹Û¿¡¼­ ¿¬°áÇÒÁö ¸Ş¼­µå·Î ÇÒÁö °í¹ÎÁß.
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
        TaskManager.instance.ShowTask(1); //ÇÒ ÀÏ Ã¤¿öÁÖ±â, MarketClear¿¡¼­ ÇÏ¸é ¿À·ù³²
    }

    public void RunStart() //»§¼ÅÆ² ·± ½ÃÀÛ
    {
        startTimer.GetComponent<MiniGame_StartTimer>().enabled = true; //Å¸ÀÌ¸Ó ÄÑÁÖ±â
        guide.SetActive(false); //°¡ÀÌµåÃ¢ Áö¿öÁÖ±â
    }

    public void QuitGame() //°ÔÀÓÁ¾·á
    {
        SoundManager.instance.ClickSound();
        Application.Quit();
    }

    public void LookRoom() //2ÀÏÂ÷°¡ ³¡³­ ÈÄ ÀÓ½Ã·Î ¸¸µé¾î ³õÀº ¾È³»Ã¢ÀÇ ¹æ µÑ·¯º¸±â ¹öÆ°.
    {
        endPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void EndGame() //2ÀÏÂ÷°¡ ³¡³­ ÈÄ ÀÓ½Ã·Î ¸¸µé¾î ³õÀº ¾È³»Ã¢ ¶ç¿ì°í, ½Ã°£ ¸ØÃß±â.
    {
        endPanel.SetActive(true);
        Time.timeScale = 0;
    }
}

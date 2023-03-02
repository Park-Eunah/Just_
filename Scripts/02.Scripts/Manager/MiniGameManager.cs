using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;

    private static bool isOvered = false; //재도전인지 확인.

    public GameObject gameoverText;
    public GameObject clearText;
    public GameObject[] lockers;

    public Text hpText;

    public Image hpImage;

    public DialogueManager theDm;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Failed()  //실패시 1초후 다시 시작
    {
        Time.timeScale = 0f;
        isOvered = true; //재도전임
        gameoverText.SetActive(true);
        StartCoroutine("Restart");
    }

    public void Failed(bool isMiss) //빵셔틀 실패시 사용. 잘못 선택해서 탈락시 true, 시간 초과로 탈락시 false
    {
        isOvered = true; //재도전임
        Fade.instance.FadeOut();//페이드 아웃
        if (isMiss)
        {
            StoryManager.instance.Story(8);
        }
        else
            StoryManager.instance.Story(7);

    }

    public void GameClear()  //성공시 1초후 다음씬으로 이동
    {
        if (SceneManager.GetActiveScene().name == "Market") //빵셔틀 성공시
        {
            Fade.instance.FadeOut(); //페이드 아웃
            if (!isOvered) //한번에 성공시
            {
                StoryManager.instance.Story(9);
            }
            else //재도전에 성공시
            {
                StoryManager.instance.Story(10);
            }
        }
        else
        {
            Time.timeScale = 0f;
            clearText.SetActive(true);
        }

        isOvered = false; //다음 미니게임 진행을 위해 false로 바꿔줌
    }

    public void MinusHp() //hp 감소시키는 메서드
    {
        hpImage.fillAmount -= 0.25f;
        hpText.text = (int)(hpImage.fillAmount * 100 )  + " / 100";
        if (hpImage.fillAmount <= 0.000f)
        {
            Failed();
        }
    }

    public bool GetIsOvered()
    {
        return isOvered;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public static int sceneCount = 1;//날짜 기준 정수( 런게임 부분만 이어주면 폰찾기 이벤트에서 집으로 넘어갈때 이틀차 대화나옴)

    public Text inputName = null;

    private static string playerName = null;

    private void Awake()//싱글톤
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public string GetName()
    {
        return playerName;
    }

    public void EnterButton() //이름 입력 후 확인버튼 누르면 호출
    {
        SoundManager.instance.ClickSound();
        playerName = inputName.text.ToString();
        //SceneManager.LoadScene("GameStart");
        sceneCount++; //캡스톤견징대회 시연을 위해 임시로 2일차부터 시작.
        SceneManager.LoadScene(2) ; //캡스톤경진대회 시연을 위해 임시로 2일차부터 시작.
    }
}

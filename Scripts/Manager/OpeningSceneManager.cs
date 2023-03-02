using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpeningSceneManager : MonoBehaviour
{

    public Image image;

    public Text inputName = null;
    private float fadeCount = 0;//처음알파값

    public GameObject startBtn;
    public GameObject quitBtn;
    public GameObject nameText;
    public GameObject EnterBtn;
    public GameObject settingBtn;
    private GameObject settingImage = null;
    public void StartGame() //게임시작 버튼을 누르면 호출
    {
        SoundManager.instance.ClickSound();
        startBtn.SetActive(false);
        quitBtn.SetActive(false);
        settingBtn.SetActive(false);
        StartCoroutine(FadeOut());
    }

    public void QuitGame() //나가기버튼 누르면 호출
    {
        SoundManager.instance.ClickSound();
        Application.Quit();
    }

    public void SettingOpen() //설정창 여는 메서드
    {
        settingImage.SetActive(true);
    }

    public void SettingClose() //설정창 닫는 메서드
    {
        settingImage.SetActive(false);
    }

    public void EnterButton() //이름 입력 후 확인버튼 누르면 호출
    {
        SoundManager.instance.ClickSound();
        //playerName = inputName.text.ToString();
        SceneManager.LoadScene("GameStart");
    }
    public IEnumerator FadeOut()
    {
        while (fadeCount < 1.0f) //알파값의 최대값인 1.0까지 반복
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f); //0.01초마다 실행
            image.color = new Color(0, 0, 0, fadeCount); //해당 변수값으로 알파값 지정
        }
        inputName.transform.parent.gameObject.SetActive(true);
        nameText.SetActive(true);
        EnterBtn.SetActive(true);
    }
}

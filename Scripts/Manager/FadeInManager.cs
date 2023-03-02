using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInManager : MonoBehaviour
{
    public static FadeInManager instance = null;
    float fadeCount = 1.0f;
    public Image image;
    // Start is called before the first frame update
    private void Awake()
    {
        image.gameObject.SetActive(true);
    }
    void Start()
    {
        StartCoroutine(StartFadeIn());//씬이 로드되면 페이드인
    }
    public IEnumerator StartFadeIn()//페이드인 함수
    {
       //SoundManager.instance.FadeInBgm();
        while (fadeCount != 0.0f) //알파값의 최대값인 1.0까지 반복
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f); //0.01초마다 실행
            image.color = new Color(0, 0, 0, fadeCount); //해당 변수값으로 알파값 지정
        }
        PlayerCtrl.moveImpossible = false;
        fadeCount = 1.0f;
    }
}

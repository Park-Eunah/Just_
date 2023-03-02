using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public static Fade instance = null;
    public Animator anim;
    private int levelToLoad;
    public bool FadeStart = false;
    public bool isFading = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void FadeToLevel(int levelIndex) //인자값으로 씬넘버 입력받으면 그씬으로 감
    {
        levelToLoad = levelIndex;
        anim.SetTrigger("FadeOut");
    }
    public void OnFadeComplet() //페이드 아웃끝나면 씬이동
    {
        SceneManager.LoadScene(levelToLoad);   
        if(SceneManager.GetActiveScene().name == "Corridor")
        {
            switch(PlayerCtrl.instance.sceneCount) //씬이동 후 플레이어 위치 조정
            {
                case 1:
                    PlayerCtrl.instance.gameObject.transform.position = new Vector3(3, 1, 5);
                    break;
                    //
            }
        }else if(SceneManager.GetActiveScene().name == "GameStart")
        {
            switch (PlayerCtrl.instance.sceneCount)
            {
                case 2:
                    PlayerCtrl.instance.gameObject.transform.position = new Vector3(10, 1, -240);//둘째날
                    break;
            }
        }
        
    }
    public void FadeOut()
    {
        anim.SetTrigger("OnlyFadeOut");
    }
    public void FadeIn()
    {
        anim.SetTrigger("FadeIn");
    }
    public void FadeInOut()
    {
        anim.SetTrigger("FadeInOut");
    }
    public void FadingEnd()//페이드인이끝나야 대화창 호출
    {
        DialogueManager.instance.SettingUI(true);
    }
    public void FadingStart()//페이드인 시작되면 대화창 비활성화 및 대화시작 안함
    {
        DialogueManager.instance.SettingUI(false);
        isFading = true;
    }


}

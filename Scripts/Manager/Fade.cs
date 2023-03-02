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

    public void FadeToLevel(int levelIndex) //���ڰ����� ���ѹ� �Է¹����� �׾����� ��
    {
        levelToLoad = levelIndex;
        anim.SetTrigger("FadeOut");
    }
    public void OnFadeComplet() //���̵� �ƿ������� ���̵�
    {
        SceneManager.LoadScene(levelToLoad);   
        if(SceneManager.GetActiveScene().name == "Corridor")
        {
            switch(PlayerCtrl.instance.sceneCount) //���̵� �� �÷��̾� ��ġ ����
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
                    PlayerCtrl.instance.gameObject.transform.position = new Vector3(10, 1, -240);//��°��
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
    public void FadingEnd()//���̵����̳����� ��ȭâ ȣ��
    {
        DialogueManager.instance.SettingUI(true);
    }
    public void FadingStart()//���̵��� ���۵Ǹ� ��ȭâ ��Ȱ��ȭ �� ��ȭ���� ����
    {
        DialogueManager.instance.SettingUI(false);
        isFading = true;
    }


}

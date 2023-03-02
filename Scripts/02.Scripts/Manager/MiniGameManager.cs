using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;

    private static bool isOvered = false; //�絵������ Ȯ��.

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

    public void Failed()  //���н� 1���� �ٽ� ����
    {
        Time.timeScale = 0f;
        isOvered = true; //�絵����
        gameoverText.SetActive(true);
        StartCoroutine("Restart");
    }

    public void Failed(bool isMiss) //����Ʋ ���н� ���. �߸� �����ؼ� Ż���� true, �ð� �ʰ��� Ż���� false
    {
        isOvered = true; //�絵����
        Fade.instance.FadeOut();//���̵� �ƿ�
        if (isMiss)
        {
            StoryManager.instance.Story(8);
        }
        else
            StoryManager.instance.Story(7);

    }

    public void GameClear()  //������ 1���� ���������� �̵�
    {
        if (SceneManager.GetActiveScene().name == "Market") //����Ʋ ������
        {
            Fade.instance.FadeOut(); //���̵� �ƿ�
            if (!isOvered) //�ѹ��� ������
            {
                StoryManager.instance.Story(9);
            }
            else //�絵���� ������
            {
                StoryManager.instance.Story(10);
            }
        }
        else
        {
            Time.timeScale = 0f;
            clearText.SetActive(true);
        }

        isOvered = false; //���� �̴ϰ��� ������ ���� false�� �ٲ���
    }

    public void MinusHp() //hp ���ҽ�Ű�� �޼���
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

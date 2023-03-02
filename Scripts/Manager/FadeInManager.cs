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
        StartCoroutine(StartFadeIn());//���� �ε�Ǹ� ���̵���
    }
    public IEnumerator StartFadeIn()//���̵��� �Լ�
    {
       //SoundManager.instance.FadeInBgm();
        while (fadeCount != 0.0f) //���İ��� �ִ밪�� 1.0���� �ݺ�
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f); //0.01�ʸ��� ����
            image.color = new Color(0, 0, 0, fadeCount); //�ش� ���������� ���İ� ����
        }
        PlayerCtrl.moveImpossible = false;
        fadeCount = 1.0f;
    }
}

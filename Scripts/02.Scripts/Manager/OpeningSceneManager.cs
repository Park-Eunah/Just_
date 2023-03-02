using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpeningSceneManager : MonoBehaviour
{

    public Image image;

    public Text inputName = null;
    private float fadeCount = 0;//ó�����İ�

    public GameObject startBtn;
    public GameObject quitBtn;
    public GameObject nameText;
    public GameObject EnterBtn;
    public GameObject settingBtn;
    private GameObject settingImage = null;
    public void StartGame() //���ӽ��� ��ư�� ������ ȣ��
    {
        SoundManager.instance.ClickSound();
        startBtn.SetActive(false);
        quitBtn.SetActive(false);
        settingBtn.SetActive(false);
        StartCoroutine(FadeOut());
    }

    public void QuitGame() //�������ư ������ ȣ��
    {
        SoundManager.instance.ClickSound();
        Application.Quit();
    }

    public void SettingOpen() //����â ���� �޼���
    {
        settingImage.SetActive(true);
    }

    public void SettingClose() //����â �ݴ� �޼���
    {
        settingImage.SetActive(false);
    }

    public void EnterButton() //�̸� �Է� �� Ȯ�ι�ư ������ ȣ��
    {
        SoundManager.instance.ClickSound();
        //playerName = inputName.text.ToString();
        SceneManager.LoadScene("GameStart");
    }
    public IEnumerator FadeOut()
    {
        while (fadeCount < 1.0f) //���İ��� �ִ밪�� 1.0���� �ݺ�
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f); //0.01�ʸ��� ����
            image.color = new Color(0, 0, 0, fadeCount); //�ش� ���������� ���İ� ����
        }
        inputName.transform.parent.gameObject.SetActive(true);
        nameText.SetActive(true);
        EnterBtn.SetActive(true);
    }
}

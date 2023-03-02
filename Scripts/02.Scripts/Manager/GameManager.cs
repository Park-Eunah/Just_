using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public static int sceneCount = 1;//��¥ ���� ����( ������ �κи� �̾��ָ� ��ã�� �̺�Ʈ���� ������ �Ѿ�� ��Ʋ�� ��ȭ����)

    public Text inputName = null;

    private static string playerName = null;

    private void Awake()//�̱���
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

    public void EnterButton() //�̸� �Է� �� Ȯ�ι�ư ������ ȣ��
    {
        SoundManager.instance.ClickSound();
        playerName = inputName.text.ToString();
        //SceneManager.LoadScene("GameStart");
        sceneCount++; //ĸ�����¡��ȸ �ÿ��� ���� �ӽ÷� 2�������� ����.
        SceneManager.LoadScene(2) ; //ĸ���������ȸ �ÿ��� ���� �ӽ÷� 2�������� ����.
    }
}

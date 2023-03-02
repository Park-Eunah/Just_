using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGame_Timer : MonoBehaviour
{
    private static float timer = 100f;

    public Text timerText;
    private void OnEnable()
    {
        if (!SceneManager.GetActiveScene().name.Contains("Market")) //���� �ð� �̾��
        {
            timer = 100f;
        }

        timerText.text = ((int)timer).ToString();
    }

    void Update()
    {
        CountDown();
    }

    void CountDown()
    {
        timer -= Time.deltaTime;
        timerText.text = ((int)timer).ToString();

        if(timer <= 0) //���ѽð��� ������ ��
        {
            if (SceneManager.GetActiveScene().name.Contains("Avoiding")) //���ϱ�����̸� ����
            {
                //MiniGameManager.instance.GameClear();
                //���� (���� ������ �Ѿ)
            }
            else //���ϱ� ������ �ƴϸ� ����
            {
                MiniGameManager.instance.Failed(false);
                Destroy(gameObject);
                //MiniGameManager.instance.Failed();
                //���� (�ٽ� �ϱ�)
            }
        }
    }
}

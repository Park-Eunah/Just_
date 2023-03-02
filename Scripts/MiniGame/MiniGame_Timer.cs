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
        if (!SceneManager.GetActiveScene().name.Contains("Market")) //전의 시간 이어가기
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

        if(timer <= 0) //제한시간이 끝났을 때
        {
            if (SceneManager.GetActiveScene().name.Contains("Avoiding")) //피하기게임이면 성공
            {
                //MiniGameManager.instance.GameClear();
                //성공 (다음 씬으로 넘어감)
            }
            else //피하기 게임이 아니면 실패
            {
                MiniGameManager.instance.Failed(false);
                Destroy(gameObject);
                //MiniGameManager.instance.Failed();
                //실패 (다시 하기)
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGame_StartTimer : MonoBehaviour
{
    private float timer = 5f;

    private Text countText;
   
    public GameObject objPool;
    public GameObject timerText;
    public GameObject guideText;
    public GameObject player;


    private void Awake()
    {
        countText = GetComponent<Text>();
    }

    void Update()
    {
        CountDown();
    }

    void CountDown()
    {
        timer -= Time.deltaTime;

        if (4f >= timer && timer > 3f)
        {
            countText.text = "3";

        }
        else if (3f >= timer && timer > 2f)
        {
            countText.text = "2";
        }
        else if (2f >= timer && timer > 1f)
        {
            countText.text = "1";
        }
        else if (1f >= timer && timer > 0f)
        {
            countText.text = "Start!";
        }
        else if (0f >= timer && timer > -1f)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name==("MiniGame_Avoiding"))  //피하기 게임일때 실행
            {
                player.GetComponent<PlayerController_Avoiding>().enabled = true;
                objPool.SetActive(true);
                timerText.SetActive(true);
            }
            else if(scene.name==("MiniGame_TreasureHunt")) //폰찾기 게임일때 실행
            {
                player.GetComponent<PlayerController_TreasureHunt>().enabled = true;
                player.GetComponentInChildren<Cam_TreasureHunt>().enabled = true;
                timerText.SetActive(true);
            }
            else if(scene.name=="MiniGame_TargetShoot") //맞추기 게임일때 실행
            {
                player.GetComponent<PlayerController_TargetShoot>().enabled = true;
                timerText.SetActive(true);
                guideText.SetActive(false);
            }
            else if (scene.name=="MiniGame_RunGame")
            {
                timerText.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}

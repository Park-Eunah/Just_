using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance = null;

    public Text[] tasks = new Text[4]; //static으로 하면 인스펙터창에 노출이 안돼서 OnEnable에서 씬 넘어갈때마다 다시 설정해주는 걸로 함.
    public TextAsset txt; //할일들이 적혀있는 csv파일

    private static int[] curTasks = new int[4]; //다음 씬으로 넘어가도 유지되도록 static으로 함 
    private static string[,] Sentence; //할 일들을 나눠 저장할 배열 [몇줄,몇번째], 다음 씬으로 넘어갔을 때 OnEnble에서 텍스트 설정해주기 위해 static으로 함

    private int lineSize = 0, rowSize = 0; //총 줄 수, 한 줄에 로우단위 수
    private int currentTask = 0;

    private GameObject taskPanelOpen = null; //열린 상태의 할일리스트
    private GameObject taskPanelClose = null; //접힌 상태의 할일리스트

    private bool isOpen = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnEnable() //다음 씬으로 넘어가도 할 일 리스트가 유지되도록 함 
    {
        for (int i = 0; i < curTasks.Length; i++) 
        {
            if (curTasks[i] != 0) //표시해야 할 할 일이 있다면 표시.
            {
                tasks[i].gameObject.SetActive(true);
                tasks[i].text = Sentence[curTasks[i] - 1, 1]; 
            }
        }
    }

    void Start()
    {
        // 엔터단위와 쉼표로 나눠서 배열의 크기 조정
        string currentText = txt.text.Substring(0, txt.text.Length - 1);
        string[] line = currentText.Split('\n');
        lineSize = line.Length;
        rowSize = line[0].Split(',').Length;
        Sentence = new string[lineSize, rowSize];

        // 한 줄에서 쉼표로 나눔
        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split(',');
            for (int j = 0; j < rowSize; j++)
            {
                Sentence[i, j] = row[j];
                Debug.Log(Sentence[i, j]);
            }
        }
    }

    private void Update()
    {
        Arrange();
    }

    public void ShowTask(int num) //몇번째 할 일을 꺼내 보일지 매개변수로 받아 확인 후 할 일 추가하기.
    {
        if (currentTask != num)
        {
            for(int i = 0; i < curTasks.Length; i++)
            {
                if (num == curTasks[i]) //현재 진행중인 할 일이라면 리턴.
                {
                    return;
                }
            }

            currentTask = num;

            for (int i = 0; i < tasks.Length; i++)
            {
                if (!tasks[i].IsActive())
                {
                    curTasks[i] = num;
                    tasks[i].text = Sentence[num - 1, 1];
                    tasks[i].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void TaskComplete(int num) //몇 번째 할일을 완료했는지 매개변수로 받아 확인 후 할 일에서 지워주기.
    {
        for (int i = 0; i < curTasks.Length; i++)
        {
            if (num == curTasks[i]) //현재 진행중인 할 일이라면 지워준다.
            {
                curTasks[i] = 0; 
                tasks[i].text = "";
                tasks[i].gameObject.SetActive(false);
            }
        }
    }

    public void TaskOpenOrClose()  //할일 리스트 열고 접는 메서드
    {
        switch (isOpen)
        {
            case true:
                isOpen = false;
                //할일 접기,  아직 접힌 산태의 할일리스트 이미지 없음.
                //taskPanelClose.SetActive(true);
                //taskPanelOpen.SetActive(false);
                //
                break;
            case false:
                isOpen = true;
                //할일 열기
                //taskPanelOpen.SetActive(true);
                //taskPanelClose.SetActive(false);
                break;
        }
    }

    private void Arrange() //할일 정리
    {
        //할 일을 중간에 빈칸없이 순서대로 나타내기 위한 반복문 
        for (int i = 2; i >= 0; i--) //tasks[2]부터 tasks[0]까지 반복
        {
            if (!tasks[i].IsActive()) //task[i]가 비활성화일 때
            {
                for (int j = i + 1; j < tasks.Length; j++) //task[i+1]부터 끝까지 반복
                {
                    if (tasks[j].IsActive()) // task[i] 뒤에 있는 task[j]가 활성화 되어 있다면
                    {
                        for (int k = i; k <= j; k++)
                        {
                            if (k == i)
                            {
                                tasks[k].gameObject.SetActive(true);
                            }
                            else if (k == j)
                            {
                                //할 일을 앞으로 넘겼으면 지워주기
                                tasks[k].gameObject.SetActive(false);
                                curTasks[k] = 0;
                                return;
                            }
                            //뒤에 있는 할 일 앞으로 넘겨주기 
                            tasks[k].text = tasks[k + 1].text;
                            curTasks[k] = curTasks[k + 1];
                        }
                    }
                }
            }
        }
    }
}

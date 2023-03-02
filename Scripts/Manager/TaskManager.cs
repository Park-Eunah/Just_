using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance = null;

    public Text[] tasks = new Text[4]; //static���� �ϸ� �ν�����â�� ������ �ȵż� OnEnable���� �� �Ѿ������ �ٽ� �������ִ� �ɷ� ��.
    public TextAsset txt; //���ϵ��� �����ִ� csv����

    private static int[] curTasks = new int[4]; //���� ������ �Ѿ�� �����ǵ��� static���� �� 
    private static string[,] Sentence; //�� �ϵ��� ���� ������ �迭 [����,���°], ���� ������ �Ѿ�� �� OnEnble���� �ؽ�Ʈ �������ֱ� ���� static���� ��

    private int lineSize = 0, rowSize = 0; //�� �� ��, �� �ٿ� �ο���� ��
    private int currentTask = 0;

    private GameObject taskPanelOpen = null; //���� ������ ���ϸ���Ʈ
    private GameObject taskPanelClose = null; //���� ������ ���ϸ���Ʈ

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
    private void OnEnable() //���� ������ �Ѿ�� �� �� ����Ʈ�� �����ǵ��� �� 
    {
        for (int i = 0; i < curTasks.Length; i++) 
        {
            if (curTasks[i] != 0) //ǥ���ؾ� �� �� ���� �ִٸ� ǥ��.
            {
                tasks[i].gameObject.SetActive(true);
                tasks[i].text = Sentence[curTasks[i] - 1, 1]; 
            }
        }
    }

    void Start()
    {
        // ���ʹ����� ��ǥ�� ������ �迭�� ũ�� ����
        string currentText = txt.text.Substring(0, txt.text.Length - 1);
        string[] line = currentText.Split('\n');
        lineSize = line.Length;
        rowSize = line[0].Split(',').Length;
        Sentence = new string[lineSize, rowSize];

        // �� �ٿ��� ��ǥ�� ����
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

    public void ShowTask(int num) //���° �� ���� ���� ������ �Ű������� �޾� Ȯ�� �� �� �� �߰��ϱ�.
    {
        if (currentTask != num)
        {
            for(int i = 0; i < curTasks.Length; i++)
            {
                if (num == curTasks[i]) //���� �������� �� ���̶�� ����.
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

    public void TaskComplete(int num) //�� ��° ������ �Ϸ��ߴ��� �Ű������� �޾� Ȯ�� �� �� �Ͽ��� �����ֱ�.
    {
        for (int i = 0; i < curTasks.Length; i++)
        {
            if (num == curTasks[i]) //���� �������� �� ���̶�� �����ش�.
            {
                curTasks[i] = 0; 
                tasks[i].text = "";
                tasks[i].gameObject.SetActive(false);
            }
        }
    }

    public void TaskOpenOrClose()  //���� ����Ʈ ���� ���� �޼���
    {
        switch (isOpen)
        {
            case true:
                isOpen = false;
                //���� ����,  ���� ���� ������ ���ϸ���Ʈ �̹��� ����.
                //taskPanelClose.SetActive(true);
                //taskPanelOpen.SetActive(false);
                //
                break;
            case false:
                isOpen = true;
                //���� ����
                //taskPanelOpen.SetActive(true);
                //taskPanelClose.SetActive(false);
                break;
        }
    }

    private void Arrange() //���� ����
    {
        //�� ���� �߰��� ��ĭ���� ������� ��Ÿ���� ���� �ݺ��� 
        for (int i = 2; i >= 0; i--) //tasks[2]���� tasks[0]���� �ݺ�
        {
            if (!tasks[i].IsActive()) //task[i]�� ��Ȱ��ȭ�� ��
            {
                for (int j = i + 1; j < tasks.Length; j++) //task[i+1]���� ������ �ݺ�
                {
                    if (tasks[j].IsActive()) // task[i] �ڿ� �ִ� task[j]�� Ȱ��ȭ �Ǿ� �ִٸ�
                    {
                        for (int k = i; k <= j; k++)
                        {
                            if (k == i)
                            {
                                tasks[k].gameObject.SetActive(true);
                            }
                            else if (k == j)
                            {
                                //�� ���� ������ �Ѱ����� �����ֱ�
                                tasks[k].gameObject.SetActive(false);
                                curTasks[k] = 0;
                                return;
                            }
                            //�ڿ� �ִ� �� �� ������ �Ѱ��ֱ� 
                            tasks[k].text = tasks[k + 1].text;
                            curTasks[k] = curTasks[k + 1];
                        }
                    }
                }
            }
        }
    }
}

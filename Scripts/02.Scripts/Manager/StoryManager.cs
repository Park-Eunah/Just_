using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public DialogueManager theDm;
    public static StoryManager instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void Story(int index)
    {
        switch (index)
        {
            case 0:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(1,1));//ù�� � ù���(�����ϰڴ�)
                break;
            case 1:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(6, 11));//ù�� ���������
                break;
            case 2:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(12, 12));//�� �ڸ��� ���⿴��
                break;
            case 3:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(27, 27));//������ �������� ������
                break;
            case 4:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(29, 29));//���ΰ� 1���� �ϱ� �� ����
                break;
            case 5:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(30, 32));//2���� �
                break;
            case 6:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(36, 43));//2���� ���ȭ
                break;
            case 7:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(64, 68), "marketTimeover"); //����Ʋ �ð��ʰ��� Ż���� ��ȭ
                break;
            case 8:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(70, 73), "marketMiss"); //����Ʋ ���� �߸� ������ Ż���� ��ȭ
                break;
            case 9:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(61, 63), "marketClear"); //����Ʋ �� ���� ������ ��ȭ
                break;
            case 10:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(74, 74), "marketClear"); //����Ʋ �絵���� ������ ��ȭ
                break;
            case 11:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(76, 77), "RingBell");//�ڵ��� ã���̺�Ʈ ���۴�ȭ
                break;
            case 12:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(78, 80), "BackPack");//�ڵ��� ã���̺�Ʈ ���۴�ȭ
                break;
            case 13:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(81, 82), "BackPack");//�ڵ��� ã���̺�Ʈ ���۴�ȭ
                break;
            case 14:
                theDm.ShowDialogue(DatabaseManager.instance.GetDialogue(83, 94), "FindPhone");//�ڵ��� ã�� ���� �� ��ȭ
                break;
            default:
                Debug.Log("���丮 �ε��� �߸�������");
                break;
        }
                          
    }
    public void Story(RaycastHit hit)
    {
        switch (GameManager.sceneCount)
        {
            case 1: //1����
                theDm.ShowDialogue(hit.transform.GetComponent<interactionEvent>().GetDialogue(), hit.transform.GetComponent<interactionEvent>().dialogue[0].name);
                break;
            case 2: //2����
                //�浹ü�� ��ȭ�̺�Ʈ�� �ϳ����̶�� ù��° ��ȭ ���, �ƴ϶�� �ι�° ��ȭ ���.
                switch (hit.transform.GetComponent<interactionEvent>().dialogue.Length)
                {
                    case 1:
                        theDm.ShowDialogue(hit.transform.GetComponent<interactionEvent>().GetDialogue(), hit.transform.GetComponent<interactionEvent>().dialogue[0].name);
                        break;
                    default:
                        hit.transform.GetComponent<interactionEvent>().showed = true;
                        theDm.ShowDialogue(hit.transform.GetComponent<interactionEvent>().GetDialogue(), hit.transform.GetComponent<interactionEvent>().dialogue[1].name);                
                        break;
                }
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance = null;

    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    Dialogue[] dialogues;

    public bool isDialogue = false;
    bool isNext = false; //����
    string DialogueName;

    [SerializeField] float textDelay = 0.03f; //��簡 ��Ÿ���� �ѹ��� �����°Ծƴ϶� Ÿ�ڰ��������� ������
    private float textTypeSpeed = 0f;
    private float textDelaySkip = 0f;

    int lineCount = 0;
    int contextCount = 0;

    private bool isTyping = false; //���� ��簡 Ÿ�� �������� õõ�� ������ ������ üũ

    private static string username;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        if(username == null) //�̸��� ó�� ������ ���� ����ǵ���.
        {
            username = GameManager.instance.GetName();
        }
    }
    private void Update()
    {
        if (isDialogue)
        {
            if (isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
                {
                    isNext = false;
                    txt_Dialogue.text = "";
                    isTyping = true;
                    textTypeSpeed = textDelay;

                    if (++contextCount < dialogues[lineCount].contexts.Length)
                    {
                        StartCoroutine(TypeWriter());
                    }
                    else
                    {
                        contextCount = 0;
                        if(++lineCount < dialogues.Length)
                        {
                            StartCoroutine(TypeWriter());
                        }
                        else
                        {
                            EndDialogue();
                        }
                    }
                }
            }
        }
    }

    public void ShowDialogue(Dialogue[] p_dialogues) //ȣ��Ǹ� �Ű������� ���� Ex) hit�� ������Ʈ���ִ� interactionEvent�� �̸��� ��ȭ�� ���
    {
        if(SceneManager.GetActiveScene().buildIndex != 7) //
        {
            PlayerCtrl.instance.MoveStop(true);
        }
        else
        {
            PlayerController_TreasureHunt.instance.MoveStop(true);
        }
        //isDialogue = true;
        txt_Dialogue.text = ""; //���� �ִ� ���,�̸� �������� �ʱ�ȭ
        txt_Name.text = "";
        dialogues = p_dialogues;
        StartCoroutine(TypeWriter());
    }
    public void ShowDialogue(Dialogue[] p_dialogues,string DialogueName) //�Ѿ�� ��ȭ�̸��� �޾��ִ� �Ű������߰� �����ε�
    {
        if (SceneManager.GetActiveScene().buildIndex != 7)
        {
            if (!SceneManager.GetActiveScene().name.Contains("Market"))
            {
                PlayerCtrl.instance.MoveStop(true);
            }
        }
        else
        {
            PlayerController_TreasureHunt.instance.MoveStop(true);
        }
        //isDialogue = true;
        txt_Dialogue.text = ""; //���� �ִ� ���,�̸� �������� �ʱ�ȭ
        txt_Name.text = "";
        dialogues = p_dialogues;
        this.DialogueName = DialogueName;
        StartCoroutine(TypeWriter());
    }
    void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        SettingUI(false);
        if (SceneManager.GetActiveScene().buildIndex != 7 && !SceneManager.GetActiveScene().name.Contains("Market"))
        {
            PlayerCtrl.instance.MoveStop(false);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 7)
        {
            PlayerController_TreasureHunt.instance.MoveStop(false);
        }
        switch (this.DialogueName)
        {
            case "greeting":
                TaskManager.instance.ShowTask(1);
                break;

            case "morning": //���� ��ȭ�� ���� ��ȭ�� ���
                Fade.instance.FadeInOut();
                this.DialogueName = ""; //��ȭ�̸� �ʱ�ȭ ����� ���ѷ����� �Ⱥ���
                PlayerCtrl.moveImpossible = true;
                PlayerCtrl.instance.StartCoroutine(PlayerCtrl.instance.CutAway(0));//�÷��̾� �� ī�޶� Ʈ������ �� ����.
                PlayerCtrl.instance.friends.SetActive(true); //ģ���� Ȱ��ȭ
                break;

            case "afterMarket": //���� ��ȭ�� �������� ��ȭ�� ���
                Fade.instance.FadeInOut();
                this.DialogueName = "";
                PlayerCtrl.instance.MoveStop(true);
                PlayerCtrl.instance.StartCoroutine(PlayerCtrl.instance.CutAway(1));
                SoundManager.instance.BreakTimeBellSound();
                ShowDialogue(DatabaseManager.instance.GetDialogue(21, 26));
                break;

            case "gohome": //������ ���鳡�� �� 1���� �ϱ�(������ �̵�)
                SceneManager.LoadScene("Home");
                break;

            case "monologue": //������ ���� �� 2���� �
                GameManager.sceneCount++;
                Fade.instance.FadeToLevel(1);
                break;

            case "morning2":
                Fade.instance.FadeInOut();
                this.DialogueName = ""; //��ȭ�̸� �ʱ�ȭ ����� ���ѷ����� �Ⱥ���
                PlayerCtrl.moveImpossible = true;
                PlayerCtrl.instance.StartCoroutine(PlayerCtrl.instance.CutAway(0));
                SoundManager.instance.BreakTimeBellSound();
                PlayerCtrl.instance.friends.SetActive(true);
                break;

            case "afterMarket2":
                Fade.instance.FadeToLevel(6);
                break;

            case "marketTimeover": //����Ʋ ���� ��ȭ�� ���� ��
            case "marketMiss":
                Fade.instance.FadeToLevel(6); //����Ʋ �ٽ� ����
                break;

            case "marketClear": //����Ʋ ���� ��ȭ�� ���� �� 
                SceneManager.LoadScene(7); //�� ã������� �̵� 
                break;

            case "RingBell":
                PlayerController_TreasureHunt.instance.MoveStop(false);
                break;

            case "BackPack": //��������� ��ȭ���� ��
                //�����ڵ� �����Ҹ�
                break;

            case "FindPhone":
                Fade.instance.FadeToLevel(5); //������ �̵�
                break;

            case "monologue2": //��Ʋ�� ������ ���ƿ� ���� ��
                UIManager.instance.EndGame();
                break;

        }
        StartCoroutine(JumpDelay());
    }
    IEnumerator TypeWriter()
    {
   /*     if (Fade.instance.isFading)
        {
            yield return new WaitForSeconds(2.0f);
            Fade.instance.isFading = false;
        }*/
            SettingUI(true);
            txt_Name.text = dialogues[lineCount].name;
            if (txt_Name.text.Equals("���ΰ�"))
            {
                txt_Name.text = username;
            }
            string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
            t_ReplaceText = t_ReplaceText.Replace("���ΰ�", username);
            //t_ReplaceText = t_ReplaceText.Replace("�ΰ�", username_TwoWord);//�� ���� �θ���
            t_ReplaceText = t_ReplaceText.Replace("'", ","); //��ǥ�� ǥ�����ֱ����� text�� replace����
                                                             // '��� Ư�����ڸ� ������ ��ǥ�� ���÷��̽����ش�.
            for (int i = 0; i < t_ReplaceText.Length; i++)//�ؽ�Ʈ ������ȿ��
            {
                //Debug.Log(isTyping);
                txt_Dialogue.text += t_ReplaceText[i];

                yield return new WaitForSeconds(textTypeSpeed);

                //��� Ÿ������ ������ ���� �����̽��ٸ� �� �� �� ������ ���� Ÿ���� ���� ��簡 ������ ���� �ʰ� ����.
                //�Ǵٰ� �ȵǴٰ� ��.
                if (Input.GetKeyDown(KeyCode.Space) && isTyping)
                {
                    textTypeSpeed = textDelaySkip;
                    //Debug.Log(textTypeSpeed);
                }
            }
            //��簡 �� ���� �� 0.5�� �Ŀ� ���� ���� �Ѿ �� ����. (�����̽��ٸ� �� ������ �� �Ѿ�� �ʱ� ����)
            yield return new WaitForSeconds(0.3f);
            isTyping = false;
            isNext = true;
        
    }
    IEnumerator JumpDelay()
    {
        PlayerCtrl.instance.jumppossible = false;
        yield return new WaitForSeconds(0.5f);
        PlayerCtrl.instance.jumppossible = true;
    }



    public void SettingUI(bool p_flag) //��ȭâUI�� TypeWriter�� ���۵Ǹ� true , EndDialogue�� ����Ǹ� false
    {
        isDialogue = p_flag;
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
        //return p_flag;
    }
}

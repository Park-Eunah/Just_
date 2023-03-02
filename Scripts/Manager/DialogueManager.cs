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
    bool isNext = false; //다음
    string DialogueName;

    [SerializeField] float textDelay = 0.03f; //대사가 나타날때 한번에 나오는게아니라 타자가쳐지듯이 나오게
    private float textTypeSpeed = 0f;
    private float textDelaySkip = 0f;

    int lineCount = 0;
    int contextCount = 0;

    private bool isTyping = false; //현재 대사가 타자 쳐지듯이 천천히 나오는 중인지 체크

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
        if(username == null) //이름을 처음 지정할 때만 실행되도록.
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

    public void ShowDialogue(Dialogue[] p_dialogues) //호출되면 매개변수로 받은 Ex) hit된 오브젝트에있는 interactionEvent의 이름과 대화를 출력
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
        txt_Dialogue.text = ""; //전에 있던 대사,이름 공백으로 초기화
        txt_Name.text = "";
        dialogues = p_dialogues;
        StartCoroutine(TypeWriter());
    }
    public void ShowDialogue(Dialogue[] p_dialogues,string DialogueName) //넘어온 대화이름도 받아주는 매개변수추가 오버로딩
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
        txt_Dialogue.text = ""; //전에 있던 대사,이름 공백으로 초기화
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

            case "morning": //끝난 대화가 조례 대화일 경우
                Fade.instance.FadeInOut();
                this.DialogueName = ""; //대화이름 초기화 해줘야 무한루프에 안빠짐
                PlayerCtrl.moveImpossible = true;
                PlayerCtrl.instance.StartCoroutine(PlayerCtrl.instance.CutAway(0));//플레이어 및 카메라 트랜스폼 값 조정.
                PlayerCtrl.instance.friends.SetActive(true); //친구들 활성화
                break;

            case "afterMarket": //끝난 대화가 매점가자 대화일 경우
                Fade.instance.FadeInOut();
                this.DialogueName = "";
                PlayerCtrl.instance.MoveStop(true);
                PlayerCtrl.instance.StartCoroutine(PlayerCtrl.instance.CutAway(1));
                SoundManager.instance.BreakTimeBellSound();
                ShowDialogue(DatabaseManager.instance.GetDialogue(21, 26));
                break;

            case "gohome": //교무실 독백끝난 후 1일차 하교(집으로 이동)
                SceneManager.LoadScene("Home");
                break;

            case "monologue": //집에서 독백 후 2일차 등교
                GameManager.sceneCount++;
                Fade.instance.FadeToLevel(1);
                break;

            case "morning2":
                Fade.instance.FadeInOut();
                this.DialogueName = ""; //대화이름 초기화 해줘야 무한루프에 안빠짐
                PlayerCtrl.moveImpossible = true;
                PlayerCtrl.instance.StartCoroutine(PlayerCtrl.instance.CutAway(0));
                SoundManager.instance.BreakTimeBellSound();
                PlayerCtrl.instance.friends.SetActive(true);
                break;

            case "afterMarket2":
                Fade.instance.FadeToLevel(6);
                break;

            case "marketTimeover": //빵셔틀 실패 대화가 끝난 후
            case "marketMiss":
                Fade.instance.FadeToLevel(6); //빵셔틀 다시 시작
                break;

            case "marketClear": //빵셔틀 성공 대화가 끝난 후 
                SceneManager.LoadScene(7); //폰 찾기씬으로 이동 
                break;

            case "RingBell":
                PlayerController_TreasureHunt.instance.MoveStop(false);
                break;

            case "BackPack": //가방뒤지고 대화끝난 뒤
                //가해자들 웃음소리
                break;

            case "FindPhone":
                Fade.instance.FadeToLevel(5); //집으로 이동
                break;

            case "monologue2": //이틀차 집으로 돌아온 독백 후
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
            if (txt_Name.text.Equals("주인공"))
            {
                txt_Name.text = username;
            }
            string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
            t_ReplaceText = t_ReplaceText.Replace("주인공", username);
            //t_ReplaceText = t_ReplaceText.Replace("인공", username_TwoWord);//성 떼고 부르기
            t_ReplaceText = t_ReplaceText.Replace("'", ","); //쉼표도 표현해주기위해 text를 replace해줌
                                                             // '라는 특수문자를 만나면 쉼표로 리플레이스해준다.
            for (int i = 0; i < t_ReplaceText.Length; i++)//텍스트 딜레이효과
            {
                //Debug.Log(isTyping);
                txt_Dialogue.text += t_ReplaceText[i];

                yield return new WaitForSeconds(textTypeSpeed);

                //대사 타이핑이 쳐지는 동안 스페이스바를 한 번 더 누르면 현재 타이핑 중인 대사가 딜레이 되지 않고 나옴.
                //되다가 안되다가 함.
                if (Input.GetKeyDown(KeyCode.Space) && isTyping)
                {
                    textTypeSpeed = textDelaySkip;
                    //Debug.Log(textTypeSpeed);
                }
            }
            //대사가 다 나온 후 0.5초 후에 다음 대사로 넘어갈 수 있음. (스페이스바를 막 눌러도 막 넘어가지 않기 위해)
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



    public void SettingUI(bool p_flag) //대화창UI가 TypeWriter가 시작되면 true , EndDialogue가 실행되면 false
    {
        isDialogue = p_flag;
        go_DialogueBar.SetActive(p_flag);
        go_DialogueNameBar.SetActive(p_flag);
        //return p_flag;
    }
}

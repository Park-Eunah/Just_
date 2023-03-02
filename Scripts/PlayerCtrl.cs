using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl instance = null;

    public static bool moveImpossible = false; //다음 씬으로 이동할때 플레이어 움직임 방지
    public static bool isInteracting = false;
    public  bool jumppossible = true;

    public int sceneCount = 1;//1일차부터 시작이라 1로설정

    public GameObject arrow; //교실 들어가면 플레이어의 자리를 가리키는 화살표
    public GameObject friends; //1일차에 조례 후 대화할때 나타나는 친구들

    public Transform[] cutAwayPos; //같은 씬 내에서 장면 전환을 위해 이동할 위치값, 나중에 더 생길 수 있기 때문에 배열로 해둠

    [SerializeField] private float walkSpeed = 10.0f;
    [SerializeField] private float runSpeed = 15.0f;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float jumpPower;

    [SerializeField] Camera cam = null;

    [SerializeField] GameObject txt_Tutorial;

    private float moveX, moveZ;
    private float applySpeed;
    private float cameraLimit = 40;

    private bool isGround = true;
    //private bool isInteract = false;

    private GameObject prefab;
    private GameObject interactObj;
    private GameObject prevInteractObj;

    private Vector2 createPoint;
    private Rigidbody rigid;
    private RaycastHit hit;
    private Scene currentScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        //Instantiate(prefab, createPoint, Quaternion.identity, GameObject.Find("Canvas").transform);
        applySpeed = walkSpeed;
        rigid = GetComponent<Rigidbody>();
        //Debug.Log("게임시작");
        if (currentScene.name.Contains("GameStart"))
        {
            switch (GameManager.sceneCount)
            {
                case 1:
                    StoryManager.instance.Story(0);//1일차 등교씬 스토리
                    break;
                case 2:
                    StoryManager.instance.Story(5);//2일차 등교씬 스토리
                    break;
            }
        }
        else if (currentScene.name.Contains("Corridor"))
        {
            switch (GameManager.sceneCount)
            {
                case 1:
                    StoryManager.instance.Story(1); //1일차 학교 입장 후 대화
                    TaskManager.instance.TaskComplete(1); //학교 들어가기 완수
                    TaskManager.instance.ShowTask(2); //교실 들어가기 시작
                    break;

            }
        }

        //GameManager.sceneCount++;//2일차 디버깅
    }

    void Update()
    {
        if (moveImpossible == false) // false 일때만 캐릭터 움직임 가능
        {
            CharacterRotation();
            CharacterMove();
            CameraRotation();
            Run();
            if (jumppossible)
            {
                Jump();
            }
        }
        
        if (!isInteracting) //상호작용중이 아닐때만 레이발사( Ray 중복방지 )
        {
            HitTheRay();
        }
    }

    public void CharacterMove() //캐릭터이동
    {
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime * applySpeed;
        moveZ = Input.GetAxis("Vertical") * Time.deltaTime * applySpeed;
        transform.Translate(moveX, 0, moveZ);
    }
    public void CharacterRotation() //캐릭터회전
    {
        float RotationY = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        transform.Rotate(0, RotationY, 0);
    }
    public void CameraRotation() // X축의회전은 Player가 아닌 카메라만 회전
    {
        float RotationX = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        RotationX = Mathf.Clamp(RotationX, -cameraLimit, cameraLimit); //Clamp로 각도의 최소값,최대값 설정
        cam.transform.eulerAngles -= new Vector3(RotationX, 0, 0);
    }
    private void Run() //달리기
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }

    }

    private void Running() 
    {
        applySpeed = runSpeed; //LeftShift키를 누르면
                               //CharacterMover에 곱해준 applySpeed를 runSpeed로 변경
    }

    private void RunningCancel()
    {
        applySpeed = walkSpeed;//LeftShift키를 떼면
                               //runSpeed였던 applySpeed를 다시 walkSpeed로 변경
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround==true)//더블점프 제한
        {
            rigid.AddForce(Vector3.up * jumpPower,ForceMode.Impulse);
            isGround = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ground")) //ground 닿아야 true
        {
            isGround = true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Class")) //교실
        {
            if (GameManager.sceneCount == 1) //1일차에만 실행
            {
                other.tag = "Finished";
                TaskManager.instance.TaskComplete(2); //교실 들어가기 할 일 완수      
                TaskManager.instance.ShowTask(3); //자리 찾아 앉기 할 일 추가
                arrow.SetActive(true); //플레이어의 자리를 가리키는 화살표 활성화
                StoryManager.instance.Story(2);//자리 어딘지 찾는 대화창 
            }
            arrow.SetActive(true); //캡스톤경진대회 시연으로 임시로 해둠.
        } 
        else if (other.CompareTag("Counsel")) //상담실
        {
            if (GameManager.sceneCount == 1) //1일차에만 실행
            {
                other.tag = "Finished";
                StoryManager.instance.Story(3);// 상담실에서 선생님이 앉으렴이라고 하는 대화창뜨게 함
            }
        }
        else if(other.CompareTag("Story_Two"))
        {
            if (GameManager.sceneCount == 2)
            {
                other.tag = "Finished";
                StoryManager.instance.Story(6); //2일차 등교대화
            }
        }
    }
    public void HitTheRay() //Raycast충돌시 처리함수
    {
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue, 0.3f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))//다음스테이지 판별
        {
            if (hit.collider.CompareTag("door") && Input.GetKeyDown(KeyCode.Q))
            {
                //moveImpossible = true;
                if (currentScene.name.Contains("GameStart") && hit.transform.name == "School"&& GameManager.sceneCount == 1) //1일차에 학교에 들어가면 첫번째 할일 완수.
                {
                    TaskManager.instance.TaskComplete(1); //학교 들어가기 완수
                    TaskManager.instance.ShowTask(2); //교실 들어가기 시작
                }
                else if (currentScene.name.Contains("Market")) //빵셔틀 끝난 후 문으로 나가면 폰찾기 이벤트.
                {
                    MiniGameManager.instance.GameClear();
                    return;
                }
                Fade.instance.FadeToLevel(2);// 씬번호2번으로 페이드아웃
                return;
            }
            else if (hit.collider.CompareTag("interactObj")) //상호작용오브젝트와 충돌시 대화시작
            {
                if (interactObj != null)
                {
                    prevInteractObj = interactObj;
                    prevInteractObj.GetComponent<Outline>().enabled = false;
                }
                interactObj = hit.collider.gameObject;
                interactObj.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    MoveStop(true);
                    StoryManager.instance.Story(hit);
                }
            }
            else if (hit.collider.CompareTag("corridorStory"))//Corridor씬 입장했을때 대사
            {  
                switch (GameManager.sceneCount)
                {
                    case 1:
                        hit.transform.tag = "Finished";
                        MoveStop(true);
                        StoryManager.instance.Story(1);
                        break;
                    default:
                        MoveStop(false);
                        break;
                }

            }
            else if (hit.collider.CompareTag("PlayerSeat") && Input.GetKeyDown(KeyCode.Q)) //플레이어가 자리를 찾아 q를 누르면 조례.
            {
                Fade.instance.FadeInOut();
                arrow.SetActive(false); //플레이어의 자리를 가리키는 화살표 비활성화
                TaskManager.instance.TaskComplete(3); //자리찾기 할 일 완수
                hit.transform.tag = "Finished"; //대화한번 후 태그 바꿔줌으로써 중복방지
                MoveStop(true);
                StartCoroutine(CutAway(2));        
            }
            else if(hit.collider.CompareTag("PlayerSeat2") && Input.GetKeyDown(KeyCode.Q))//선생님 대사
            {
                Fade.instance.FadeOut();
                hit.transform.tag = "Finished";
                MoveStop(true);
                StoryManager.instance.Story(hit);
            }
            else if (hit.collider.CompareTag("friend") && moveImpossible == true)
            {
                isInteracting = true;
                StoryManager.instance.Story(hit);
            }
            else if (hit.collider.CompareTag("monologue"))
            {
                hit.transform.tag = "Finished";
                StoryManager.instance.Story(hit);
            }
            else //상호작용 오브젝트 아웃라인 지워주기
            {
                if (interactObj != null)
                {
                    prevInteractObj = interactObj;
                    prevInteractObj.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
    public void MoveStop(bool flag)
    {
        moveImpossible = flag;
        isInteracting = flag;
        switch (flag)
        {
            case true:
                UIManager.instance.MSetActive(false);
                break;
            case false:
                UIManager.instance.MSetActive(true);
                break;
        }
    }
    public IEnumerator CutAway(int index) //같은 씬 내에서 장면 전환을 위해 플레이어의 위치를 조정하는 메서드
    {
         yield return new WaitForSeconds(1.0f);//페이드 아웃끝난뒤 위치변경
            transform.position = cutAwayPos[index].position;
            transform.rotation = cutAwayPos[index].rotation;
        switch (index)
        {
            case 0:
                cam.transform.localRotation = Quaternion.Euler(20, 0, 0);//매점대화
                break;
            case 1:
                cam.transform.localRotation = Quaternion.Euler(0, 0, 0);//5교시이후
                break;
            case 2:
                cam.transform.localRotation = Quaternion.Euler(0, 0, 0);//조례
                StoryManager.instance.Story(hit);
                break;

        }
    }
}
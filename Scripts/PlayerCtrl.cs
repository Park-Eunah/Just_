using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl instance = null;

    public static bool moveImpossible = false; //���� ������ �̵��Ҷ� �÷��̾� ������ ����
    public static bool isInteracting = false;
    public  bool jumppossible = true;

    public int sceneCount = 1;//1�������� �����̶� 1�μ���

    public GameObject arrow; //���� ���� �÷��̾��� �ڸ��� ����Ű�� ȭ��ǥ
    public GameObject friends; //1������ ���� �� ��ȭ�Ҷ� ��Ÿ���� ģ����

    public Transform[] cutAwayPos; //���� �� ������ ��� ��ȯ�� ���� �̵��� ��ġ��, ���߿� �� ���� �� �ֱ� ������ �迭�� �ص�

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
        //Debug.Log("���ӽ���");
        if (currentScene.name.Contains("GameStart"))
        {
            switch (GameManager.sceneCount)
            {
                case 1:
                    StoryManager.instance.Story(0);//1���� ��� ���丮
                    break;
                case 2:
                    StoryManager.instance.Story(5);//2���� ��� ���丮
                    break;
            }
        }
        else if (currentScene.name.Contains("Corridor"))
        {
            switch (GameManager.sceneCount)
            {
                case 1:
                    StoryManager.instance.Story(1); //1���� �б� ���� �� ��ȭ
                    TaskManager.instance.TaskComplete(1); //�б� ���� �ϼ�
                    TaskManager.instance.ShowTask(2); //���� ���� ����
                    break;

            }
        }

        //GameManager.sceneCount++;//2���� �����
    }

    void Update()
    {
        if (moveImpossible == false) // false �϶��� ĳ���� ������ ����
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
        
        if (!isInteracting) //��ȣ�ۿ����� �ƴҶ��� ���̹߻�( Ray �ߺ����� )
        {
            HitTheRay();
        }
    }

    public void CharacterMove() //ĳ�����̵�
    {
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime * applySpeed;
        moveZ = Input.GetAxis("Vertical") * Time.deltaTime * applySpeed;
        transform.Translate(moveX, 0, moveZ);
    }
    public void CharacterRotation() //ĳ����ȸ��
    {
        float RotationY = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        transform.Rotate(0, RotationY, 0);
    }
    public void CameraRotation() // X����ȸ���� Player�� �ƴ� ī�޶� ȸ��
    {
        float RotationX = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        RotationX = Mathf.Clamp(RotationX, -cameraLimit, cameraLimit); //Clamp�� ������ �ּҰ�,�ִ밪 ����
        cam.transform.eulerAngles -= new Vector3(RotationX, 0, 0);
    }
    private void Run() //�޸���
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
        applySpeed = runSpeed; //LeftShiftŰ�� ������
                               //CharacterMover�� ������ applySpeed�� runSpeed�� ����
    }

    private void RunningCancel()
    {
        applySpeed = walkSpeed;//LeftShiftŰ�� ����
                               //runSpeed���� applySpeed�� �ٽ� walkSpeed�� ����
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround==true)//�������� ����
        {
            rigid.AddForce(Vector3.up * jumpPower,ForceMode.Impulse);
            isGround = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ground")) //ground ��ƾ� true
        {
            isGround = true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Class")) //����
        {
            if (GameManager.sceneCount == 1) //1�������� ����
            {
                other.tag = "Finished";
                TaskManager.instance.TaskComplete(2); //���� ���� �� �� �ϼ�      
                TaskManager.instance.ShowTask(3); //�ڸ� ã�� �ɱ� �� �� �߰�
                arrow.SetActive(true); //�÷��̾��� �ڸ��� ����Ű�� ȭ��ǥ Ȱ��ȭ
                StoryManager.instance.Story(2);//�ڸ� ����� ã�� ��ȭâ 
            }
            arrow.SetActive(true); //ĸ���������ȸ �ÿ����� �ӽ÷� �ص�.
        } 
        else if (other.CompareTag("Counsel")) //����
        {
            if (GameManager.sceneCount == 1) //1�������� ����
            {
                other.tag = "Finished";
                StoryManager.instance.Story(3);// ���ǿ��� �������� �������̶�� �ϴ� ��ȭâ�߰� ��
            }
        }
        else if(other.CompareTag("Story_Two"))
        {
            if (GameManager.sceneCount == 2)
            {
                other.tag = "Finished";
                StoryManager.instance.Story(6); //2���� ���ȭ
            }
        }
    }
    public void HitTheRay() //Raycast�浹�� ó���Լ�
    {
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue, 0.3f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))//������������ �Ǻ�
        {
            if (hit.collider.CompareTag("door") && Input.GetKeyDown(KeyCode.Q))
            {
                //moveImpossible = true;
                if (currentScene.name.Contains("GameStart") && hit.transform.name == "School"&& GameManager.sceneCount == 1) //1������ �б��� ���� ù��° ���� �ϼ�.
                {
                    TaskManager.instance.TaskComplete(1); //�б� ���� �ϼ�
                    TaskManager.instance.ShowTask(2); //���� ���� ����
                }
                else if (currentScene.name.Contains("Market")) //����Ʋ ���� �� ������ ������ ��ã�� �̺�Ʈ.
                {
                    MiniGameManager.instance.GameClear();
                    return;
                }
                Fade.instance.FadeToLevel(2);// ����ȣ2������ ���̵�ƿ�
                return;
            }
            else if (hit.collider.CompareTag("interactObj")) //��ȣ�ۿ������Ʈ�� �浹�� ��ȭ����
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
            else if (hit.collider.CompareTag("corridorStory"))//Corridor�� ���������� ���
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
            else if (hit.collider.CompareTag("PlayerSeat") && Input.GetKeyDown(KeyCode.Q)) //�÷��̾ �ڸ��� ã�� q�� ������ ����.
            {
                Fade.instance.FadeInOut();
                arrow.SetActive(false); //�÷��̾��� �ڸ��� ����Ű�� ȭ��ǥ ��Ȱ��ȭ
                TaskManager.instance.TaskComplete(3); //�ڸ�ã�� �� �� �ϼ�
                hit.transform.tag = "Finished"; //��ȭ�ѹ� �� �±� �ٲ������ν� �ߺ�����
                MoveStop(true);
                StartCoroutine(CutAway(2));        
            }
            else if(hit.collider.CompareTag("PlayerSeat2") && Input.GetKeyDown(KeyCode.Q))//������ ���
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
            else //��ȣ�ۿ� ������Ʈ �ƿ����� �����ֱ�
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
    public IEnumerator CutAway(int index) //���� �� ������ ��� ��ȯ�� ���� �÷��̾��� ��ġ�� �����ϴ� �޼���
    {
         yield return new WaitForSeconds(1.0f);//���̵� �ƿ������� ��ġ����
            transform.position = cutAwayPos[index].position;
            transform.rotation = cutAwayPos[index].rotation;
        switch (index)
        {
            case 0:
                cam.transform.localRotation = Quaternion.Euler(20, 0, 0);//������ȭ
                break;
            case 1:
                cam.transform.localRotation = Quaternion.Euler(0, 0, 0);//5��������
                break;
            case 2:
                cam.transform.localRotation = Quaternion.Euler(0, 0, 0);//����
                StoryManager.instance.Story(hit);
                break;

        }
    }
}
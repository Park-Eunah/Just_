using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Extra_Moving : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;//현재오브젝트가 이동할 타겟위치

    private void Start()
    {
        target = GameObject.Find("School").transform;//프리팹화 시키면 연결이끊어지고 null로 변하는 것을 해결
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        agent.SetDestination(target.position);//학생프리팹생성시 target으로 이동       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "door")
        {
            Debug.Log("닿음");//NavMeshAgent 와 OnTriggerEnter가 동기화가 안되는문제 NavMesh오브젝트에 RigidBody추가 후 is Kinematic을 true로 설정해줌으로 해결
            ObjectPool_Student.objectPool.returnPool(gameObject);//ObjectPooling스크립트의 returnPool메소드 인수로 현재 게임오브젝트(생성된학생) 를 넣어줌
        }
    }
}

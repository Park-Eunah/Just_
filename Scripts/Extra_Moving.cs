using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Extra_Moving : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;//���������Ʈ�� �̵��� Ÿ����ġ

    private void Start()
    {
        target = GameObject.Find("School").transform;//������ȭ ��Ű�� �����̲������� null�� ���ϴ� ���� �ذ�
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        agent.SetDestination(target.position);//�л������ջ����� target���� �̵�       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "door")
        {
            Debug.Log("����");//NavMeshAgent �� OnTriggerEnter�� ����ȭ�� �ȵǴ¹��� NavMesh������Ʈ�� RigidBody�߰� �� is Kinematic�� true�� ������������ �ذ�
            ObjectPool_Student.objectPool.returnPool(gameObject);//ObjectPooling��ũ��Ʈ�� returnPool�޼ҵ� �μ��� ���� ���ӿ�����Ʈ(�������л�) �� �־���
        }
    }
}

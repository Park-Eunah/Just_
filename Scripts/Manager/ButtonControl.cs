using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{ 
    public Text inputName = null;
    void Update()
    {
        if (inputName.text.Length < 2) //ó�� �̸��Է� ���͹�ư �Է�ĭ�� �α����̻� �Ⱦ��� ��Ȱ��ȭ
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}

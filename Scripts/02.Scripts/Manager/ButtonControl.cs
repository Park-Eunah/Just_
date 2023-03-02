using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{ 
    public Text inputName = null;
    void Update()
    {
        if (inputName.text.Length < 2) //처음 이름입력 엔터버튼 입력칸에 두글자이상 안쓸시 비활성화
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}

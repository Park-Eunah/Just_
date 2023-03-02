using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //대화리스트생성
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //csv파일 가져와서 담아줌

        string[] data = csvData.text.Split(new char[]{ '\n' }); //넣어준 csv파일(엑셀) 기준에서 일단 엔터기준으로 쪼개줌
        for(int i = 1; i<data.Length;) //0번째에는 지금은 사용하지않을 ID 값이 들어가기있기에 1부터 시작
        {
            string[] row = data[i].Split(new char[] { ',' }); //첫번째 줄을 다시 쉼표 기준으로 row 단위(엑셀에서의 셀)로 쪼개줌
            
            Dialogue dialogue = new Dialogue(); //처음 생성했던 Dialogue 객체를 생성

            dialogue.name = row[1];  //name에 row[1]번째있는 string데이터 넣어줌(이름)
            List<string> contextList = new List<string>();//context 리스트 생성(대사는 이름과 달리 하나가아니라 이름하나에 여러개있기에 따로 context 리스트생성)


            do //row단위로 짤라서 리스트에 넣어줌
            {
                contextList.Add(row[2]); //대사1개는 무조건있으니 dowhile문으로 대사리스트에 row[2] 즉 csv파일첫번째줄에서 쉼표단위로 짜른것중 두번째에 있는(첫번째대사) 녀석을 가져와서 대사리스트에 넣어줌
                if (++i < data.Length)
                {
                   row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray(); //잘라준 녀석들을 Dialogue클래스를 인스턴스화시킨 dialogue의 contexts에 배열로 변경해서 넣어줌

            dialogueList.Add(dialogue);//이름과 각 대사들이 한세트로 넣어진 완성된 대화목록을 dialogueList에 넣어줌
        }
        return dialogueList.ToArray(); //List형식을 Dialogue[]와 같은형식인 배열형식으로 바꿔준뒤 리턴
    }
}

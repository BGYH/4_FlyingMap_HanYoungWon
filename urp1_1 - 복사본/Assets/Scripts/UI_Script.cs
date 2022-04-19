using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class UI_Script : MonoBehaviour
{
    public Text chatText;
    
   
    public string writerText = "";
    public int next_Panel = 0;
    public XRController controller = null;
   
    void Start()
    {
        StartCoroutine(Text1());
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    IEnumerator NormalChat(string chat)
    {
        int a = 0;
 
        writerText = "";

        //텍스트 타이핑 효과
        for (a = 0; a < chat.Length; a++)
        {
            writerText += chat[a];
            chatText.text = writerText;
            yield return null;
        }

        //키를 다시 누를때까지 무한정 대기
        while (true)
        {

            controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool AButton);
            if (AButton == true)
            {
                Debug.Log("press A");//A 버튼 누르면
                break;
            }
            yield return null;

        }

    }

    IEnumerator Text1()
    {//타이핑 효과를 줄 대사를 이곳에 입력
        yield return StartCoroutine(NormalChat("넌 이제부터 하늘을 날아볼거야"));
        yield return StartCoroutine(NormalChat("네 앞에 있는 친구가 그걸 도와줄거고,"));
        yield return StartCoroutine(NormalChat("일단 넌 어떤 곳을 탐험하고 싶은지 알려줄래?"));
        next_Panel = 1; //다 누르면 패널번호 1로 바꾸기
        yield return StartCoroutine(NormalChat(" "));





    }
}
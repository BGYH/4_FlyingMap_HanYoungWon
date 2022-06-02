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
        // StartCoroutine(Text1());
        Invoke(nameof(doText), 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void doText()
    {
        Debug.Log("Function started");
        StartCoroutine(Text1());
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
        yield return StartCoroutine(NormalChat("어떻게 하늘을 날아야하는지 알려줄테니까 잘 기억해야해,"));
        yield return StartCoroutine(NormalChat("이제 드래곤이 나타날거야"));
        yield return StartCoroutine(NormalChat("드래곤이 나타나면 가까이 다가가봐"));
        yield return StartCoroutine(NormalChat("가까이 가서 A버튼을 누르면 드래곤에 탈 수 있어"));
        yield return StartCoroutine(NormalChat("아, 가까이 가려면 왼쪽 조이스틱으로 움직여야하는거 잊지말고!"));
        yield return StartCoroutine(NormalChat("드래곤에 타고 나서 B버튼을 누르면 하늘을 날게 될거야."));
        yield return StartCoroutine(NormalChat("날고 싶으면 B버튼은 계속 누르고 있어야 해"));
        yield return StartCoroutine(NormalChat("버튼을 누르면서 조이스틱을 움직여봐!"));
        yield return StartCoroutine(NormalChat("음 그리고 혹시나 드래곤에서 내려오고 싶으면 A버튼을 길게 다시 누르면 돼!"));
        next_Panel = 1; //다 누르면 패널번호 1로 바꾸기
        yield return StartCoroutine(NormalChat("다 이해했지? 기억해야 할 건 A, B, 조이스틱이야!"));
        yield return StartCoroutine(NormalChat("자, B버튼을 눌러봐 드래곤이 나타날거야!"));
    }
}
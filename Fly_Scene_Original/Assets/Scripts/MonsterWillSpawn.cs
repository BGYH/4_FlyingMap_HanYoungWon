using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//씬 관리를 위해 추가
using UnityEngine.UI;

public class MonsterWillSpawn : MonoBehaviour
{
    //public GameObject player;//player


    public float animTime = 2f;         // Fade 애니메이션 재생 시간 (단위:초).  

    private Image fadeImage;            // UGUI의 Image컴포넌트 참조 변수.  

    private float start = 1f;           // Mathf.Lerp 메소드의 첫번째 값.  
    private float end = 0f;             // Mathf.Lerp 메소드의 두번째 값.  
    private float time = 0f;            // Mathf.Lerp 메소드의 시간 값. 


    public bool stopIn = false; //false일때 실행되는건데, 초기값을 false로 한 이유는 게임 시작할때 페이드인으로 들어가려고...그게 싫으면 true로 하면됨.
    public bool stopOut = true;


    // Start is called before the first frame update
    void Start()
    {
        // Image 컴포넌트를 검색해서 참조 변수 값 설정.  
        fadeImage = GetComponent<Image>();
        Invoke("MonsterSpawnHere", 30.0f);//30초 후 함수실행
    }



    // Update is called once per frame
    void Update()
    {/*
        if (player.transform.position.z >= new Vector3(-636, 42, 184).z)
        {
            // Debug.Log("Monster Spawn!");
            MonsterSpawnHere();
        }
        */

        // 투명해지는 = FadeIn 애니메이션 재생.  
        if (stopIn == false && time <= 2)
        {
            PlayFadeIn();
        }
        if (stopOut == false && time <= 2)
        {
            PlayFadeOut();
        }
        if (time >= 2 && stopIn == false)
        {
            stopIn = true;
            time = 0;
            Debug.Log("StopIn");
        }
        if (time >= 2 && stopOut == false)
        {
            stopIn = false; //하얗게 전환되고 나서 씬 전환 후 다시 풀거라 넣었다. 그냥 게임 끝낼거면 넣을 필요 없음.
            stopOut = true;
            time = 0;
            Debug.Log("StopOut");
        }

    }

    // 흰색->투명
    void PlayFadeIn()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        time += Time.deltaTime / animTime;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = fadeImage.color;
        // 알파 값 계산.  
        color.a = Mathf.Lerp(start, end, time);
        // 계산한 알파 값 다시 설정.  
        fadeImage.color = color;
        // Debug.Log(time);
    }


    // Fade 애니메이션 함수.  
    void PlayFadeOut()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        time += Time.deltaTime / animTime;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = fadeImage.color;
        // 알파 값 계산.  
        color.a = Mathf.Lerp(end, start, time);
        // 계산한 알파 값 다시 설정.  
        fadeImage.color = color;
    }
    public void MonsterSpawnHere()
    {
        //몬스터와 만나게됨
        Debug.Log("Monster Spawn!");
      /*  if (stopOut == false && time <= 2)
        {
            PlayFadeOut();
        }
      */
        SceneManager.LoadScene (0);

       
    }
}

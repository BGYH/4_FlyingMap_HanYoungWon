using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;


public class Change_Dragon : MonoBehaviour
{
   //나타나게 할 드래곤
    public GameObject changed1;
    public GameObject changed2;
    public GameObject changed3;
    public GameObject changed4;
    public GameObject changed5;
    public GameObject changed6;


    // Start is called before the first frame update
    void Start()
    {
        set_active(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void set_active(bool a)
    {
        string fileName = "TestJson";//제이슨 파일명
        string path = Application.dataPath + "/" + fileName + ".Json";//제이슨이 있는 경로

        FileStream filestream = new FileStream(path, FileMode.Open);
        byte[] data = new byte[filestream.Length];
        filestream.Read(data, 0, data.Length);
        filestream.Close();
        string json = Encoding.UTF8.GetString(data);

        PlayerState myplayerState = JsonUtility.FromJson<PlayerState>(json);
        switch (myplayerState.dragon)
        {
            case "Red":
                Debug.Log("Dragon_Red uploaded");
                changed1.SetActive(a);
                //transform.Find("BoostPoint").gameObject.SetActive(a);
                break;

            case "Green":
                Debug.Log("Dragon_Green uploaded");
                changed2.SetActive(a);
                break;

            case "Black":
                Debug.Log("Dragon_Black uploaded");
                changed3.SetActive(a);
                //transform.Find("BoostPoint").gameObject.SetActive(a);
                break;

            case "Yellow":
                Debug.Log("Dragon_Yellow uploaded");
                changed4.SetActive(a);
                //transform.Find("BoostPoint").gameObject.SetActive(a);
                break;

            case "Red_small":
                Debug.Log("Dragon_Red_small uploaded");
                changed5.SetActive(a);
                //transform.Find("BoostPoint").gameObject.SetActive(a);
                break;

            case "Green_small":
                Debug.Log("Dragon_Green_small uploaded");
                changed6.SetActive(a);
                //transform.Find("BoostPoint").gameObject.SetActive(a);
                break;
        }
    }
}

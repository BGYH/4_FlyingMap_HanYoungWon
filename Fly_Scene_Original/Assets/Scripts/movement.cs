using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class movement : MonoBehaviour
{
    public XRController controller = null;// 컨트롤러

    Ray ray;
    RaycastHit hit;



    void Start()
    {
        StartCoroutine(actions());
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    IEnumerator Choose()
    {
        while (true)
        {
            controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool AButton);

            if (AButton == true)//A 버튼 누르면
            {
                Debug.Log("A button pressed. check.");
                break;
            }
            yield return null;
        }
    }

    IEnumerator actions(){

        yield return StartCoroutine(Choose());
}
}
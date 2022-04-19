using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class movement : MonoBehaviour
{
    public XRController controller = null;// 컨트롤러
   // private CharacterController character;
   // private GameObject _camera;
   // private XRRig xrrig1;
    Ray ray;
    RaycastHit hit;
   
    /*
    private void Awake()
    {
        character = GetComponent<CharacterController>();
        //_camera = GetComponent<XRRig>().cameraGameObject;
        _camera = GetComponent<XRRig>().cameraGameObject;
    }
    */

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            Choose();
        
    }


    private void Choose()
    {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool AButton))
            if(AButton == true)//A 버튼 누르면
            {
                Debug.Log("select");
            }
    }
}
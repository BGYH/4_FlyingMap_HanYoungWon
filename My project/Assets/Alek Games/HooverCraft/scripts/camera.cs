using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlekGames.Systems
{
    public class camera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        [SerializeField] private Transform cam;
        [SerializeField] private Transform camPos;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            moveCamera();
        }

        private void moveCamera()
        {
            Vector3 configuratedCamPos = new Vector3(camPos.position.x, camPos.position.y, camPos.position.z);
            float distance = Vector3.Distance(camPos.position, cam.position);
            cam.position = Vector3.MoveTowards(cam.position, configuratedCamPos, speed * Time.deltaTime * distance);

            cam.LookAt(target);

        }
    }
}

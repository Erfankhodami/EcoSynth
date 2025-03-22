using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class paralloxController : MonoBehaviour
{
    private GameObject mainCam;
    private Vector3 previousPos;
    private Vector3 camVelocity;
    [SerializeField] private float movingSpeed;
    private GameObject sizeSetter;
    private Vector3 siezeSetterCurrentOffset;
    private GameObject backGround;
    void Start()
    {
        backGround = GameObject.Find("BackGround");
        sizeSetter = GameObject.Find("SizeSetter");
        siezeSetterCurrentOffset = sizeSetter.transform.localPosition;
        mainCam = Camera.main.gameObject;
        previousPos = mainCam.transform.position;
    }
    void Update()
    {
        //get the velocity of camera
        camVelocity = (mainCam.transform.position - previousPos) / Time.deltaTime;
        previousPos = mainCam.transform.position;
        
        transform.Translate(-camVelocity*movingSpeed*Time.deltaTime);
        if (mainCam.transform.position.x > sizeSetter.transform.position.x-20)
        {
            GameObject gm=Instantiate(backGround, Vector3.zero, quaternion.identity,transform);
            gm.transform.localPosition = new Vector3(sizeSetter.transform.localPosition.x,0,0);
            sizeSetter.transform.localPosition += siezeSetterCurrentOffset;
        }
    }
}

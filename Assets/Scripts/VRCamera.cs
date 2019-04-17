using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.VR;

public class VRCamera : MonoBehaviour {

    void Start()
    {
        while (KinectAvater.Eye == null)
        {

        }
        Vector3 pos = KinectAvater.Eye.transform.position;
        this.transform.position = pos;
    }

	void Update ()
    {
        Vector3 pos = KinectAvater.Eye.transform.position;
        this.transform.position = pos;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    private float m_zoomDistance = -5.0f; //set the zoom distance

    public float GetZoomDistance() //public method that returns the zoom distance variable
    {
        return m_zoomDistance; 
    }

	void Update () {
        float delta = Input.GetAxis("Mouse ScrollWheel");//set variable to the mouse scroll wheel
        m_zoomDistance += delta;//set zoom distance variable to mouse wheel input
        transform.Translate(new Vector3(0, 0, delta));//move camera position along the z axis using mouse input
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RightClickFunctions : NetworkBehaviour
{

    public GameObject rightClickMenuPanelPrefab;

    private Camera m_camera;
    private Ray m_ray; //create ray
    private RaycastHit m_hit; //create the raycast hit
    private Quaternion m_hitAngle;
    private GameObject m_rightClickMenuPanel;
    private GameObject m_newCommentMarkerClone;
    private GameObject m_rightClickMenuCanvas;

    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_rightClickMenuCanvas = GameObject.Find("RightClickMenuCanvas");     
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
 
        if (Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out m_hit)) //If ray has hit something in the scene
        {
            if (Input.GetMouseButtonDown(1))//if the right click mouse button has been pressed
            {
                m_rightClickMenuPanel = Instantiate(rightClickMenuPanelPrefab, m_rightClickMenuCanvas.transform) as GameObject;//create the UI panel using the panel prefab and set its parent as the canvas           
                m_rightClickMenuPanel.transform.position = Input.mousePosition + new Vector3(83f, -47f, 0f);//offset the position of the panel so it is not directly over the mouse

                var rcm = m_rightClickMenuPanel.GetComponentInChildren<RightClickMenuFunctions>();
                rcm.spawner = GetComponent<NewCommentSpawner>();

                m_hitAngle = Quaternion.LookRotation(m_hit.normal);//fetch the rotation of the raycast hit
                Color m_newCommentMarkerColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);//generate a random colour for the comment marker

                GetComponent<NewCommentSpawner>().getRaycastData(m_newCommentMarkerColor, m_hitAngle, m_hit.point);//run the getRayCastData method from the NewCommentSpawner script and pass in the random colour, angle and rotation of raycast hit
               
            }
        }

    }



}
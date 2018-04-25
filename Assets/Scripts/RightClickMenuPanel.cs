using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickMenuPanel : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))//if the user right clicks then destroy the comment UI
        {
            Destroy(gameObject);
        }
         if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())//if the user left clicks and is not over a gameobject then destroy the comment UI
        {
            Destroy(gameObject);
        }

    }
}

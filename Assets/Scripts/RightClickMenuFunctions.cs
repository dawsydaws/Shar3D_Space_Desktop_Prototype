using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenuFunctions : MonoBehaviour {

    public Text m_rightClickInputField;
    public NewCommentSpawner spawner;

	// Use this for initialization
	void Start () {
        
	}

    void Update ()
    {
        if (spawner != null)
        {
			if (m_rightClickInputField.text != "") {
				spawner.commentText = m_rightClickInputField.text;//if input field is not empty then set the comment text to the input field text
				if (Input.GetKeyDown (KeyCode.Return)) { //if user presses enter then run method
					GetInputFieldText ();
					m_rightClickInputField.text = ""; //reset the input field to be empty
				}
			}
        }
    }

    public void GetInputFieldText()
    {
		if (m_rightClickInputField.text != "") {
			spawner.spawnNewComment (); //spawn the new comment and then destroy the comment UI
			Destroy (gameObject); 
		}
    }

	public void Cancel(){
		Destroy (gameObject);//If user presses cancel on UI then destroy the gameobject
	}
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommentDeleter : NetworkBehaviour {

	private string commentName;
	private GameObject currentObj;

	public void deleteComment(GameObject obj)//public function which requires gameobject to be passed as parametre
	{

		currentObj = GameObject.Find ("NetworkedObject");//set private variable to shared object
        commentName = obj.name;//set comment name variable to the name of the passed in object

		Destroy (GameObject.Find(commentName));//find and destroy the gameobject with that name

		GameObject marker = currentObj.transform.Find(commentName).gameObject;//find the gameobject with that name
		Destroy (marker);//destroy the attached named marker


		CmdDeleteComment (commentName);//run the network command passing in the name of the object to be destroyed
	}

	[Command]//Runs the Command passing in name of the object
	void CmdDeleteComment(string commentName){
		RpcDeleteComment (commentName);//Runs the Client Rpc, passing in name of object
	}


	[ClientRpc]//Runs Client Rpc passing in name of object
	void RpcDeleteComment(string commentName){
		Destroy (GameObject.Find(commentName));//Client Rpc finds and destroys game object with name accross network
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NewCommentSpawner : NetworkBehaviour {
    
    public string commentText;

    private GameObject m_currentObject;
    private GameObject m_commentParentPanel;
    private GameObject m_newCommentMarkerClone;
    private Color m_newCommentMarkerColor;
    private RaycastHit m_hit;
    private Quaternion m_hitAngle;
    private Vector3 m_pos;
	private int m_newCommentCount = 0;
	private int m_commentMarkerCount = 0;

    public GameObject newCommentPrefab;
    public GameObject newCommentMarkerPrefab;
  

    void Start()
    {  
        m_commentParentPanel = GameObject.Find("commentContainer");//sets private variable to UI element "commentContainer"
        m_currentObject = GameObject.Find("NetworkedObject");//sets the private variable to be the shared object
    }

    public void getRaycastData(Color commentMarkerColor, Quaternion rightClickHitAngle, Vector3 rightClickPos)//public method that requires parametres
    {
        m_newCommentMarkerColor = commentMarkerColor;//sets private variable to passed in colour
        m_hitAngle = rightClickHitAngle;//sets private variable to passed in quarternion angle
        m_hit.point = rightClickPos;//sets private raycast hit to the passed in vector3
    }

    public void spawnNewComment()
    {
        if (!isLocalPlayer)//Checks to see if the user accessing this script is the local player
        {
            return;//if they are not they cannot proceed
        }
		CmdSpawnNewComment(commentText, m_newCommentMarkerColor);//Runs network command that creates the new comment, passing in public string commentText and private colour variable
        CmdSpawnMarker(m_newCommentMarkerColor, m_hit.point, m_hitAngle);//Runs network command that creates the comment marker, passing in the colour variable and raycast hit position and angle

    }

    [Command]
	void CmdSpawnNewComment(string newCommentText, Color m_newCommentMarkerColor)
    {
		RpcSpawnNewComment(newCommentText, m_newCommentMarkerColor);//passes parametres to the Client Rpc
    }

    [ClientRpc]
	void RpcSpawnNewComment(string newCommentText, Color m_newCommentMarkerColor)
    {
		GameObject newComment = Instantiate (newCommentPrefab, m_commentParentPanel.transform) as GameObject;//Creates a new comment using the new comment prefab and sets the UI element as its parent

		var text = newComment.transform.GetChild(1).GetComponentInChildren<Text>();//fetches the text element from the comment

		if(text != null)
        {
			if (newCommentText != "")//if the text field is not empty...
            {
				text.text = newCommentText;//then set the new comment text to that of the text field
			}
        }

		newComment.transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>().color = m_newCommentMarkerColor;//set the colour of the new comment text colour identifier to that of the passed in colour variable
	
		newComment.name = "comment_" + m_commentMarkerCount;//set the name of the comment to "comment" plus the private integer 
		m_commentMarkerCount++;//increment the private integer by 1


    }

    [Command]
    public void CmdSpawnMarker(Color commentMarkerColor, Vector3 pos, Quaternion rot)
    {
        RpcSpawnMarker(commentMarkerColor, pos, rot);//passes parametres of the new comment marker to the Client Rpc
    }

    [ClientRpc]
    void RpcSpawnMarker(Color commentMarkerColor, Vector3 pos, Quaternion rot)
    {
        m_newCommentMarkerClone = Instantiate(newCommentMarkerPrefab, pos, rot, m_currentObject.transform) as GameObject;//Create a new comment marker using the prefab with the position of the raycast hit and set the parent to be the shared object
        m_newCommentMarkerClone.transform.localScale = new Vector3(0.002f, 0.002f, 0.002f);//set the scale of the new comment marker
        m_newCommentMarkerClone.GetComponent<Renderer>().material.color = commentMarkerColor;//set the colour of the new comment marker to the same private colour variable

		m_newCommentMarkerClone.name = "comment_" + m_newCommentCount;//set the name of the comment marker to "comment" plus the private integer
        m_newCommentCount++;//increment the private integer by 1
    }

}

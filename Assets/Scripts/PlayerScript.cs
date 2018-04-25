   using UnityEngine.Networking; //Need to call the namespace in order to access network functions
using UnityEngine;

public class PlayerScript : NetworkBehaviour //Allows use of network behaviours
{
    public float RotationSpeed = 5.0f; //Set the speed for the object rotation

    //Set private variables
    private GameObject m_currentObject; //A private gameobject to store the shared object in the scene
    private Camera m_camera; //A private Camera object to store the player's camera
    private bool m_mouseDown = false; //Boolean which identifies whether the mouse has been pressed
    private CameraZoom m_cameraZoom; //A custom variable to store the camera zoom value

    private void Start()
    {
        //Find and store the appropriate game components in their private variables
        m_cameraZoom = GetComponent<CameraZoom>();
        m_currentObject = GameObject.Find("NetworkedObject");
        m_camera = GetComponent<Camera>();
    }

    private void Update()
    {

        if(!isLocalPlayer)//Check to see if the player accessing this script is the local player
        {
            return;//If they are not, then go no further
        }

        
        if(m_mouseDown && Input.GetKey(KeyCode.LeftControl))//Check to see if the left mouse button is pressed and the ctrl key is also pressed
        {

            if (m_currentObject)//If the raycast is on the shared object
            {   
                var worldPos = m_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -m_cameraZoom.GetZoomDistance()));//Then store the mouse position in the variable worldPos
                CmdMove(m_currentObject, worldPos);//Run move object script and pass the shared object and mouse position as parametres
            }
        }

        if (m_mouseDown && !Input.GetKey(KeyCode.LeftControl))//If the mouse is pressed and the left crtl is not pressed
        {
            RotateObject();//then run the Rotate Object function
        }

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(m_camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f))))//Check to see if left mouse button is down and the raycast has hit the shared object
        {       
            m_mouseDown = true;//if true then set boolean
        }
        else if (Input.GetMouseButtonUp(0))//Check to see if left mouse button is up
        {
            m_mouseDown = false; //if false and the mouse button is up, then set false
        }
    }

    void RotateObject()//Function to rotate shared object based on user mouse position
    {
        if (!Input.GetMouseButton(0)) return;//If the mouse button isn't down then go no further
    
        float x = Input.GetAxis("Mouse X") * RotationSpeed * Mathf.Deg2Rad;//If it's still down (being held down) then set floats to mouse position, multiplied by rotation and then the degrees in radians

        float y = Input.GetAxis("Mouse Y") * RotationSpeed * Mathf.Deg2Rad;

        CmdRotate(x, y); //Run move rotate script and pass the mouse positions as parametres
    }

    [Command]//Command runs from user to server it's expecting to receieve two float values which are stored as x and y
    void CmdRotate(float x, float y)
    {
        m_currentObject.transform.RotateAround(Vector3.down, x);//applies the movement to the object on x axis

        m_currentObject.transform.RotateAround(Vector3.right, y);//applies the movement to the object on y axis

        RpcRotate(m_currentObject.transform.rotation.eulerAngles);//Runs Client RPC with the updated object rotation
    }

    
    [ClientRpc]//ClientRpc runs from server to client, it will accept a Vector3
    void RpcRotate(Vector3 rotation)
    {
        m_currentObject.transform.rotation = Quaternion.Euler(rotation);//Updates the shared object's rotation based on the new Vector3
    }

    [Command]//Command runs from user to server
    void CmdMove(GameObject obj, Vector3 position)
    {
        RpcMove(obj, position);//In this instance the command just passes the parametres on to the Client Rpc
    }

    [ClientRpc]//ClientRpc runs from server to client
    void RpcMove(GameObject obj, Vector3 position)
    {
        obj.transform.position = position;//Sets the objects new position over the network
    }
}

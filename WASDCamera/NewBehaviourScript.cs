using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public Vector2 mouseSensitivity = new Vector2(0.01f, 0.01f); //rajoitukset
    public Vector2 mouseLookLimitY = new Vector2(-85.0f, 85.0f);
    public Vector2 currentRotation = new Vector2(0.0f, 0.0f); //cameran asento

    const float moveSpeed = 0.1f; // liikkumisnopeus
    Quaternion originalRotation;  //                            x   y   z   
                                  //                            1   0   0   0   right
                                  //                            0   1   0   0   up
                                  //                            0   0   1   0   dir
                                  //                            5   -2  9   1   pos
    const float jumpSpeed = 5.0f;
    private bool jumping = false;
    private float jumpPhase = 0.0f;   // millä kohdalla hyppyä ollaan menossa
    private float jumpHeight = 0.0f;  // millä korkeudella ollaan hypyssä
            
    // Use this for initialization
	void Start () {
        originalRotation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        MouseLook();
        WasdMovement();
        HandleJump();
	}

    private void MouseLook()
    {
        //hiiren akseli
        currentRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity.x;
        currentRotation.y += Input.GetAxis("Mouse Y") * mouseSensitivity.y; // * Time.deltaTime;

        currentRotation.y = Mathf.Clamp(currentRotation.y, mouseLookLimitY.x, mouseLookLimitY.y); // rajaus

        Quaternion q1 = Quaternion.AngleAxis(currentRotation.x, Vector3.up);
        Quaternion q2 = Quaternion.AngleAxis(currentRotation.y, -Vector3.right);
        transform.localRotation = originalRotation * q1 * q2;
    }

    private void WasdMovement()
    {
        transform.Translate(Vector3.right * moveSpeed * Input.GetAxis("Horizontal"));
        transform.Translate(Vector3.forward * moveSpeed * Input.GetAxis("Vertical"));

        //sidotaan kamera y:hyn
        Vector3 pos = transform.position;
        pos.y = 2.0f + jumpHeight;
        transform.position = pos;
    }

    private void HandleJump()
    {
        if (jumping)
        {
            jumpPhase += Time.deltaTime * jumpSpeed;
            if (jumpPhase > Mathf.PI)   //hyppy piihin asti
            {
                jumpPhase = 0.0f;
                jumping = false;
            }

            jumpHeight = Mathf.Sin(jumpPhase) * 5.0f; //hyppy sinikäyrällä
        }

        if (!jumping && Input.GetMouseButtonDown(0))
        {
            jumpPhase = 0.0f;
            jumping = true;
        }
    }
}

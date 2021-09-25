using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class BoardController : MonoBehaviour
{
    private Quaternion oldRotation;
    private float xInput, zInput;
    public float steeringSensitivity = 10f; 
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        oldRotation = transform.rotation;
        Vector3 normal = Utils.getNormal(transform.position, true); 
        transform.Rotate(0.0f, Input.GetAxis("Horizontal") * steeringSensitivity * Time.deltaTime, 0.0f);
    }


    private void ProcessInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical"); //use this for rotation only in snowboard game
    }

}


using UnityEngine;
using UnityUtils;

public class BoardController : MonoBehaviour
{
    private float xInput, zInput;
    public float steeringSensitivity = 20f; //rotation speed
    
    public float gravaty = 9.80f; //scaler for the down hill forces. 
    public float frictionCoefficient;
    public float angle;

    public float dragCoefficient; 

    // meters/second. magnitured
    public float V; 

  

    private void Start()
    {
        //convert to rad
        angle = angle * (Mathf.PI / 180);
    }
    void FixedUpdate()
    {
        Vector3 normal = this.NormalUnderneath();
        Vector3 fallLine = FallLine(Vector3.up, normal);


        float downHillAlingment = Vector3.Dot(transform.forward, fallLine);

        this.simGravity(downHillAlingment);

        //delta distance  unit: meters
        Vector3 F = transform.forward * V  *  Time.deltaTime;

        Move(F);

    }


    private void simGravity(float downHillAlingment)
    {

        //to do, lose speed when going uphill. stop speed increas when goin sidewards. 

        //acceleration unit: m/s^2
        float Ax = gravaty * (Mathf.Sin(angle) - frictionCoefficient * Mathf.Cos(angle));

        //velocity unit: m/s
        V += Ax * Time.deltaTime;
    }





    private void Update()
    {
        ProcessInputs();
        transform.Rotate(0.0f, xInput * steeringSensitivity * Time.deltaTime, 0.0f);
    }




    private void ProcessInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical"); 
    }


    //move the board 
    private void Move(Vector3 force)  
    {
        transform.position += force; 
    }


    //shoots ray from board to ground. returns the normal from the object underneath. 
    private Vector3 NormalUnderneath()
    {
        return Utils.getNormal(this.transform.position, true).normalized;
    }


    //Get Vector from top to bottom of the slope or hill.
    private Vector3 FallLine(Vector3 globalUp, Vector3 slopeNormal)
    {
        //The cross product of two vectors is the third vector that is perpendicular to the two original vectors.
        Vector3 accross = Vector3.Cross(globalUp, slopeNormal);
        return Vector3.Cross(accross, slopeNormal).normalized; // fliped to point down slope 
    }



}


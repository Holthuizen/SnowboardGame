using UnityEngine;
using UnityUtils;

public class BoardController : MonoBehaviour
{
    private float xInput, zInput;
    public float steeringSensitivity = 20f; //rotation speed
    public float speed = 20f; //scaler for the foward force of the board

    // Update is called once per frame
    void Update()
    {
        ProcessInputs(); 
        transform.Rotate(0.0f, xInput * steeringSensitivity * Time.deltaTime, 0.0f);

        Vector3 normal = this.NormalUnderneath();
        Debug.Log("Normal :" + normal);
        Vector3 forwardForce = this.FowardVelocityMagitude(normal, GetDownHillVector(Vector3.up, normal));
        Utils.drawVector(forwardForce, this.transform.position, speed);
        this.ApplyMoveForce(forwardForce);

        Debug.Log(forwardForce);
    }

    private void ProcessInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical"); //use this for rotation only in snowboard game
    }

    //move the board
    private void ApplyMoveForce(Vector3 forwardForce)  
    {
        transform.position += forwardForce * Time.deltaTime; 
    }

    //shoots ray from board to ground. returns the normal from the object underneath. 
    private Vector3 NormalUnderneath()
    {
        return Utils.getNormal(this.transform.position, true);
    }

    //calculate the snow boards forward foce, based on alingment with the slope. 
    private Vector3 FowardVelocityMagitude(Vector3 surfaceNormal, Vector3 downHillDirection)
    {
        // dot product of the slopes down hill vector and snowboards forward vector.
        // Returns a value between 1 & -1.  1 when going straigt down, -1 when roted 180 degrees. 0 for 90 degrees. 
        float alignment = Vector3.Dot(downHillDirection, transform.forward);
        Debug.Log("down Hill Vector: " + downHillDirection);
        Debug.Log("alingment: " + alignment);
        Vector3 v =  alignment * transform.forward * speed;
        //extend Vector3 class
        return v.Round(1); 
    }


    //Get Vector from top to bottom of the slope or hill
    private Vector3 GetDownHillVector(Vector3 globalUp, Vector3 slopeNormal)
    {
        //The cross product of two vectors is the third vector that is perpendicular to the two original vectors.
        Vector3 accross = Vector3.Cross(globalUp, slopeNormal);
        return Vector3.Cross(accross, slopeNormal); // fliped to point down slope 
    }

}


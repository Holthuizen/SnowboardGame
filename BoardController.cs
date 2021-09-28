using UnityEngine;
using UnityUtils;

public class BoardController : MonoBehaviour
{
    private float xInput, zInput;
    public float steeringSensitivity = 20f; //rotation speed
    public float gravaty = 20f; //scaler for the down hill forces. 
    public float drag = 1f; 
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs(); 
        transform.Rotate(0.0f, xInput * steeringSensitivity * Time.deltaTime, 0.0f);

        Vector3 normal = this.NormalUnderneath();
        Vector3 fallLine = FallLine(Vector3.up, normal);
        Vector3 forwardForce = this.FowardForce(normal, fallLine);
        Vector3 slidingForce = this.SlidingForce(Vector3.up, normal, fallLine);

        this.ApplyForce(forwardForce);
        this.ApplyForce(slidingForce);

        Utils.drawVector(forwardForce, this.transform.position, 2);
        Utils.drawVector(slidingForce, this.transform.position, 2);

        Debug.Log("slope angle: " + fallLine); 


        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            //jump
            rb.AddForce(new Vector3(0f, 200f, 0));  
        }

    }

    private void ProcessInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical"); 
    }

    //move the board
    private void ApplyForce(Vector3 forwardForce)  
    {
        transform.position += forwardForce * Time.deltaTime; 
    }

    //shoots ray from board to ground. returns the normal from the object underneath. 
    private Vector3 NormalUnderneath()
    {
        return Utils.getNormal(this.transform.position, true);
    }

    //calculate the snowboards forward foce, based on alingment with the slope. 
    private Vector3 FowardForce(Vector3 surfaceNormal, Vector3 downHillDirection)
    {
        // dot product of the slopes down hill vector and snowboards forward vector.
        // Returns a value between 1 & -1.  1 when going straigt down, -1 when roted 180 degrees. 0 for 90 degrees. 
        float alignment = Vector3.Dot(downHillDirection, transform.forward);
        //direction, magintured
        Vector3 v =   (transform.forward * alignment) * gravaty;

        return v; 
    }

    //calculates the sliding force that is allinged  with the fall line of the slope. 
    private Vector3 SlidingForce(Vector3 gobalUp, Vector3 slopeNormal, Vector3 fallLine)
    {
        //The cross product of two vectors is the third vector that is perpendicular to the two original vectors.
        Vector3 accross = Vector3.Cross(gobalUp, slopeNormal);
        float alignment = Vector3.Dot(accross, transform.forward);
        if (alignment < 0) alignment *= -1; //allway postive to prevent sliding up hill
        Vector3 x = alignment * fallLine * gravaty/ drag;
        return x;
    }


    //Get Vector from top to bottom of the slope or hill.
    private Vector3 FallLine(Vector3 globalUp, Vector3 slopeNormal)
    {
        //The cross product of two vectors is the third vector that is perpendicular to the two original vectors.
        Vector3 accross = Vector3.Cross(globalUp, slopeNormal);
        return Vector3.Cross(accross, slopeNormal); // fliped to point down slope 
    }

    //needs testing
    private float slopeAngle(Vector3 fallLine)
    {
        //this function should also work for mesh terain. 
        //world forward, this might not work in all situations. 
        float alpha = Vector3.Angle(Vector3.forward, fallLine);
        return alpha; 
    }

}


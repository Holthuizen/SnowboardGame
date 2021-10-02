using UnityEngine;
using UnityUtils;

public class PhysicsBasedBoardController : MonoBehaviour
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



    private void Update()
    {
        ProcessInputs();
        //transform.Rotate(0.0f, xInput * steeringSensitivity * Time.deltaTime, 0.0f);
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            //jump
            rb.AddForce(new Vector3(0f, 200f, 0));
        }
    }

    void FixedUpdate()
    {

        Vector3 normal = this.NormalUnderneath();
        Vector3 fallLine = FallLine(Vector3.up, normal);

        
        if(xInput > 0) {
            MoveRight(fallLine);
        }

        if (xInput < 0)
        {
            MoveLeft(fallLine);
        }

        if (xInput == 0)
        {
            //AutoAline(Vector3.Cross(Vector3.up, normal));
            //rb.AddForce(fallLine * -11f);
        }





    }


    //run while xInput > 0
    private void MoveRight(Vector3 fallLine)
    {
        //rotate
        rb.AddTorque(new Vector3(0f, steeringSensitivity  , 0f));

        //calucate "angle" beta; using the dot product of forward with acrossSlope. 
        //For normalized vectors Dot returns 1 if they point in exactly the same direction, -1 if they point in completely opposite directions and zero if the vectors are perpendicular.
        float B  =  Vector3.Dot(transform.forward, fallLine.normalized); // 0 is down hill, 1 or -1 is allinged with the accross line. so the bord is horizontal

        //forward force should increase with B. 
        Vector3 forwardForce = B * transform.forward * gravaty;

        //right force, relative to forward force, but roted 90 degrees, to point against gravaty. aka 180 degrees agains the fall line
        Vector3 rightForce = transform.right * (Vector3.Magnitude(forwardForce));

        rb.AddForce(forwardForce);
        //rb.AddForce(rightForce);
        //Utils.drawVector(forwardForce, transform.position); 
        //Utils.drawVector(rightForce, transform.position);


        //brake 
        //slow the fuck down, stop sliding like a bitch 
        Vector3 brakeForce = rb.velocity.normalized + transform.right * 10f;
        Utils.drawVector(brakeForce, transform.position); 



    }



    private void MoveLeft(Vector3 fallLine)
    {
        //rotate
        rb.AddTorque(new Vector3(0f, steeringSensitivity * -1, 0f));
        
        //calucate "angle" beta; using the dot product of forward with acrossSlope. 
        //For normalized vectors Dot returns 1 if they point in exactly the same direction, -1 if they point in completely opposite directions and zero if the vectors are perpendicular.
        float B = Vector3.Dot(transform.forward, fallLine.normalized); // 0 is down hill, 1 or -1 is allinged with the accross line. so the bord is horizontal

        //forward force should increase with B. 
        Vector3 forwardForce = B * transform.forward * gravaty;

        //left steering/counter force, relative to forward force, but roted 90 degrees, to point against gravaty. aka 180 degrees agains the fall line
        Vector3 leftForce = transform.right.normalized * (Vector3.Magnitude(forwardForce)) * -1;

        rb.AddForce(forwardForce*Time.deltaTime, ForceMode.Acceleration);
        rb.AddForce(leftForce * Time.deltaTime, ForceMode.Acceleration);

        Utils.drawVector(forwardForce, transform.position);
        Utils.drawVector(leftForce, transform.position);
    }


    public float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    private void AutoAline(Vector3 accrossSlope)
    {
        float A = Vector3.Dot(transform.forward, accrossSlope.normalized); // 0 is down hill, 1 or -1 is allinged with the accross line. so the bord is horizontal
        Debug.Log("angle A: " + A);
        Debug.Log("Sing of A: " + Mathf.Sign(A));


        if (Round(A,3) != 0)
        {
            rb.AddTorque(new Vector3(0f, 2f * Mathf.Sign(A) *-1, 0f) *Time.deltaTime,ForceMode.VelocityChange);
            Debug.Log("TEST");
        }
    }
     



    private void ProcessInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
    }

    //move the board 
    private void ApplyForce(Vector3 force)
    {
        //transform.position += force * Time.deltaTime;
        rb.AddForce(force * Time.deltaTime); 
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
        Vector3 v = (transform.forward * alignment) * gravaty;

        return v;
    }

    //calculates the sliding force that is allinged  with the fall line of the slope. 
    private Vector3 SlidingForce(Vector3 gobalUp, Vector3 slopeNormal, Vector3 fallLine)
    {
        //The cross product of two vectors is the third vector that is perpendicular to the two original vectors.
        Vector3 accross = Vector3.Cross(gobalUp, slopeNormal);
        float alignment = Vector3.Dot(accross, transform.forward);
        if (alignment < 0) alignment *= -1; //allway postive to prevent sliding up hill
        Vector3 x = alignment * fallLine * gravaty / drag;
        return x;
    }


    //Get Vector from top to bottom of the slope or hill.
    private Vector3 FallLine(Vector3 globalUp, Vector3 slopeNormal)
    {
        //The cross product of two vectors is the third vector that is perpendicular to the two original vectors.
        Vector3 accross = Vector3.Cross(globalUp, slopeNormal);
        return Vector3.Cross(accross, slopeNormal); // fliped to point down slope 
    }


}


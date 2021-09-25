using UnityEngine;
using UnityUtils;

public class test : MonoBehaviour
{


    private Vector3 vel;
    private Vector3 pos; 

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        Vector3 normal = getNormal(); 
        Vector3 moveForce = calcVel();
        Utils.drawVector(moveForce, pos, 10); 
        this.Move(moveForce); 
    }


    private void Move(Vector3 vel)  
    {
        transform.position += vel * Time.deltaTime; 
    }



    private Vector3 calcVel()
    {
        float speed = 10f;
        Vector3 normal = this.getNormal();
        Vector3 down = this.calculateDownSlope(Vector3.up, normal);
        float dot = Vector3.Dot(down, transform.forward); 
        Vector3 moveForce = dot * transform.forward * speed;
        return moveForce; 
    }

    private Vector3 getNormal()
    {
        return Utils.getNormal(pos, true); 
    }

    private Vector3 calculateDownSlope(Vector3 globalUp, Vector3 slopeNormal)
    {
        //The cross product of two vectors is the third vector that is perpendicular to the two original vectors.
        Vector3 accross = Vector3.Cross(globalUp, slopeNormal);
        return Vector3.Cross(accross, slopeNormal); // fliped to point down slope 
    }


  


}


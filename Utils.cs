

using UnityEngine;
namespace UnityUtils
{

    public class Utils
    {
        public static Vector3 getNormal(Vector3 origin, bool debug)
        {
            //https://docs.unity3d.com/ScriptReference/RaycastHit-normal.html
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 10f))
            {
                if (debug) drawVector(hit.normal, origin, 10f);
                return hit.normal;
            }
            return default;
        }

        public static void drawVector(Vector3 v, Vector3 origin, float scaler = 1)
        {
            Debug.DrawLine(origin, origin + v * scaler, Color.green);
        }




    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleRayReflection : MonoBehaviour
{
    public int Reflections;
    public float maxLength;

    private float remainingLenght;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;

    private void Start()
    {
        
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        ray = new Ray(this.transform.position, this.transform.forward);

        float remainingLenght = maxLength;

        for (int i = 0; i < Reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLenght))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(ray.origin, hit.point);

                remainingLenght -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class TESTREVERB : MonoBehaviour
{
    public int Reflections;
    public float maxLength;

    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        ray = new Ray(transform.position, transform.forward);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLenght = maxLength;

        for (int i = 0; i < Reflections; i++)
        {
            if(Physics.Raycast(ray.origin, ray.direction, out hit, remainingLenght))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLenght -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));

                if (hit.collider.tag != "Untagged")
                {
                    print("Break");
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLenght);
            }

        }

    }
}

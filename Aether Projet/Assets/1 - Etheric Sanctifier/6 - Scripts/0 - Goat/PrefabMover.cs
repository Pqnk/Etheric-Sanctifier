using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMover : MonoBehaviour
{
    private Transform target; 
    private float moveSpeed;
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void SetNameAndMaterial(string name, Material mat)
    {
        this.transform.gameObject.name = name;

        foreach (Transform t in transform.GetChild(1).transform) 
        { 
            if(t.GetComponent<MeshRenderer>() == true)
            {
                t.GetComponent<MeshRenderer>().material = mat;  
            }
            else
            {
                t.GetComponent<SkinnedMeshRenderer>().material = mat;  
            }
        }
    }


    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
            }


            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}


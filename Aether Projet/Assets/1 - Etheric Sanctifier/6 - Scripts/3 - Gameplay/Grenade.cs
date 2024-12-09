using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Grenade : MonoBehaviour
{
    [SerializeField] float rangeExplode;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Throwable>().attachmentOffset = GameObject.Find("Grab_Point").transform;
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<SphereCollider>().radius = rangeExplode;
    }

    public void Action_ThrowGrenade()
    {
        GetComponent<MeshCollider>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {        
        GetComponent<SphereCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.GetChild(0).gameObject.SetActive(false);

        if (other.gameObject.tag == "Ghost")
        {
            Destroy(other.gameObject);
        }

        StartCoroutine(DestroyGrenade());
    }

    IEnumerator DestroyGrenade()
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        yield return new WaitForSeconds(4);

        Destroy(gameObject);
    }
}

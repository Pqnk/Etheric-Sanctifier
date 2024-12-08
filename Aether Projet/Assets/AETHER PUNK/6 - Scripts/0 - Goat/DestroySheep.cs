using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DestroySheep : MonoBehaviour
{
    [Header("Sons")]
    public GameObject hit;

    public Material swordMat;
    public Mesh cube;
    public float force;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("Camera").GetComponent<GameManager>();
        MeshFilter meshFilter = transform.GetChild(5).gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = cube;
        MeshRenderer meshRenderer = transform.GetChild(5).gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = swordMat;
        transform.GetChild(5).gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sheep")
        {
            Debug.LogWarning("Rouge Bon");
            gameManager.AddHit();
            GameObject sound = Instantiate(hit, other.gameObject.transform.position, other.gameObject.transform.rotation);
            sound.transform.SetParent(other.gameObject.transform);
            other.gameObject.AddComponent<Destroy>();
            Vector3 collisionDirection = (other.transform.position - transform.position).normalized;
            other.attachedRigidbody.AddForce(collisionDirection * force, ForceMode.Impulse);

        }

    }
}

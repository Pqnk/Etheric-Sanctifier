using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class S_Teleportation_Camera_Rig : MonoBehaviour
{
    public GameObject player;
    public GameObject VRCasque;

    [Header("Mesh Baton")]
    public GameObject rightStick;
    public GameObject leftStick;

    //booléen pour savoir si le player est en train de faire une TP
    public bool isTeleported = false;
    private bool canChangeTP = true;

    //hauteur pour activer la TP
    public float detectionHigh = 0;

    //ciblage au sol lors de la TP
    public S_Target target;
    private Vector3 endPosition;

    //ligne de "pêche"
    public GameObject line;
    public GameObject rayTarget;

    //Points de départ et d'arrivée
    public GameObject start;
    public GameObject end;

    //Temps d'attente entre la validation de la TP et la TP réel
    public float waitTime = 1;

    //distance max de TP
    public float distanceMax = 5;

    //  Prefabs pour les FX de TP
    public GameObject tpDeparture;
    private GameObject _tpDepartureRef;
    public GameObject tpArrival;
    private GameObject _tpArrivalRef;

    private s_DetectionColision detectionColision;
    [SerializeField] MeshRenderer meshRendererRight;
    [SerializeField] MeshRenderer meshRendererLeft;

    private void Start()
    {
        detectionColision = GetComponent<s_DetectionColision>();
        StartCoroutine(LoadMeshRenderer());
    }


    void Update()
    {
        if (SuperManager.instance.tutoManager.isTpLearned)
        {
            if (this.transform.position.y - VRCasque.transform.position.y > detectionHigh)
            {
                if (!isTeleported && canChangeTP)
                {
                    // right 1
                    isTeleported = true;
                    meshRendererRight.SetMaterials(detectionColision.matMoon_1_On);
                    canChangeTP = false;
                    SuperManager.instance.soundManager.PlaySound(SoundType.TeleportReady, .5f, this.gameObject.transform.position);
                    Debug.Log("teleportation ready");
                }
                else if (isTeleported && canChangeTP)
                {
                    isTeleported = false;
                    meshRendererRight.SetMaterials(detectionColision.matMoon);
                    canChangeTP = false;
                    SuperManager.instance.soundManager.PlaySound(SoundType.TeleportCanceled, .5f, this.gameObject.transform.position);
                    Debug.Log("annulation ready");
                }
            }
        }

        if (this.transform.position.y < VRCasque.transform.position.y && !canChangeTP)
        {
            canChangeTP = true;
            //Debug.Log("change TP ready");
        }

        if (isTeleported)
        {
            Debug.DrawRay(rayTarget.transform.position, transform.TransformDirection(Vector3.up) * distanceMax, Color.yellow);

            if (Physics.Raycast(rayTarget.transform.position, transform.TransformDirection(Vector3.up), out RaycastHit hit, distanceMax))
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.tag == "Ground")
                {
                    target.gameObject.SetActive(true);
                    meshRendererLeft.SetMaterials(detectionColision.matSun_1_On);
                    //line.SetActive(true);
                    target.transform.position = hit.point;
                }
                else if (hit.collider.gameObject.tag == "Object")
                {
                    //line.SetActive(false);
                    target.gameObject.SetActive(false);
                    meshRendererLeft.SetMaterials(detectionColision.matSun);
                }
            }
            else
            {
                //line.SetActive(false);
                target.gameObject.SetActive(false);
            }
        }
        else
        {
            //line.SetActive(false);
            target.gameObject.SetActive(false);
        }
    }

    IEnumerator LoadMeshRenderer()
    {
        yield return new WaitForSeconds(1);
        meshRendererRight = rightStick.GetComponent<MeshRenderer>();
        meshRendererLeft = leftStick.GetComponent<MeshRenderer>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (target.canTP)
        {
            if (collision.gameObject.tag == "LeftSword" && isTeleported)
            {
                Debug.Log("Teleportation done");
                SuperManager.instance.soundManager.PlaySound(SoundType.TeleportValidated, .5f, this.gameObject.transform.position);
                StartCoroutine(Teleportation());
            }
        }
        else
        {
            LoseLife();
        }

    }

    private void LoseLife()
    {
        return;
    }

    private IEnumerator Teleportation()
    {
        SuperManager.instance.soundManager.PlaySound(SoundType.Teleporting, .5f, this.gameObject.transform.position);

        //InstantiateTPVisuals();

        endPosition = target.transform.position;
        isTeleported = false;
        yield return new WaitForSeconds(waitTime);

        player.transform.position = endPosition;

        meshRendererRight.SetMaterials(detectionColision.matMoon);
        meshRendererLeft.SetMaterials(detectionColision.matSun);

        //yield return new WaitForSeconds(1.5f);
        //DestroyTPVisuals();
    }


    private void InstantiateTPVisuals()
    {
        _tpDepartureRef =  Instantiate(tpArrival, target.transform.position, Quaternion.identity);
        _tpArrivalRef = Instantiate(tpDeparture, player.transform.position, Quaternion.identity);
    }

    private void DestroyTPVisuals()
    {
        Destroy(_tpArrivalRef);
        Destroy(_tpDepartureRef);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class s_DetectionColision : MonoBehaviour
{
    [SerializeField] GameObject detectionEffectPrefab;
    [SerializeField] float timeDestroy;
    [SerializeField] bool itsRightSword;

    [Header("Cooldown Collision")]
    public float cooldown = 1f;
    private float currentCooldown;
    public bool readyToSpawn = true;

    [Header("Generation Cube")]
    public Material swordMat;
    public Mesh cube;

    [Header("Search Target")]
    private float currentCooldownSearch;
    public bool readyToSearch = true;

    [Header("Sceptre Moon")]
    [SerializeField] Mesh meshMoon;
    public List<Material> matMoon;
    public List<Material> matMoon_1_On;
    public List<Material> matMoon_2_On;

    [Header("Sceptre Sun")]
    [SerializeField] Mesh meshSun;
    public List<Material> matSun;
    public List<Material> matSun_1_On;
    public List<Material> matSun_2_On;

    private const int maxObjects = 2;
    private S_Teleportation_Camera_Rig teleportation;

    [SerializeField] GameObject meshRendererRight;
    [SerializeField] GameObject meshRendererLeft;

    private void OnEnable()
    {

        if (itsRightSword)
        {
            teleportation = GetComponent<S_Teleportation_Camera_Rig>();
        }
    }

    private void Start()
    {
        currentCooldown = cooldown;
        currentCooldownSearch = SuperManager.instance.gameManagerAetherPunk.cooldownSearch;

        MeshFilter meshFilter = transform.GetChild(5).gameObject.AddComponent<MeshFilter>();
        if (itsRightSword)
        {
            meshFilter.mesh = meshMoon;
        }
        else
        {
            meshFilter.mesh = meshSun;
        }

        if (itsRightSword)
        {
            meshRendererRight.AddComponent<MeshRenderer>();
            meshRendererLeft.AddComponent<MeshRenderer>();
            meshRendererRight.GetComponent<MeshRenderer>().SetMaterials(matMoon);
            meshRendererLeft.GetComponent<MeshRenderer>().SetMaterials(matSun);
        }
        transform.GetChild(5).gameObject.SetActive(true);
    }

    void Update()
    {
        currentCooldown -= Time.deltaTime;
        currentCooldownSearch -= Time.deltaTime;

        if (currentCooldown <= 0)
        {
            currentCooldown = 0;
            readyToSpawn = true;
        }

        if (currentCooldownSearch <= 0)
        {
            currentCooldownSearch = 0;
            readyToSearch = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Collision pour onde de choc
        if (SuperManager.instance.tutoManager.isCollisionLearned)
        {
            if (collision.gameObject.tag == "Object" && readyToSpawn && SuperManager.instance.gameManagerAetherPunk.detectionEffects.Count < maxObjects)
            {
                if (collision.contacts.Length > 0)
                {
                    Vector3 contactPoint = collision.contacts[0].point;
                    SuperManager.instance.soundManager.PlaySound(SoundType.Collision, .5f, contactPoint);
                    GameObject obj = Instantiate(detectionEffectPrefab, contactPoint, Quaternion.identity);
                    obj.AddComponent<s_DetectionEffect>();
                    obj.GetComponent<s_DetectionEffect>().detectionColision = this;
                    obj.GetComponent<s_DetectionEffect>().timeBeforeDestroy = timeDestroy;

                    // boule
                    if (itsRightSword)
                    {
                        meshRendererRight.GetComponent<MeshRenderer>().SetMaterials(matMoon_2_On);
                    }
                    else
                    {
                        meshRendererLeft.GetComponent<MeshRenderer>().SetMaterials(matSun_2_On);
                    }

                    StopCoroutine(TimeBeforeGetOff());
                    StartCoroutine(TimeBeforeGetOff());

                    int newID = -1;
                    for (int i = 0; i <= 2; i++)
                    {
                        bool asId = false;

                        foreach (var item in SuperManager.instance.gameManagerAetherPunk.detectionEffects)
                        {
                            if (item.GetComponent<UpdateMaterialPosition>().sphereID == i)
                            {
                                asId = true;
                                break;
                            }
                        }

                        if (!asId)
                        {
                            newID = i;
                            break;
                        }
                    }

                    if (newID != -1)
                    {
                        obj.GetComponent<UpdateMaterialPosition>().sphereID = newID;
                    }

                    SuperManager.instance.gameManagerAetherPunk.detectionEffects.Add(obj);

                    currentCooldown = cooldown;
                    readyToSpawn = false;
                }
            }
        }

        // Collision pour rechercher l'objet
        if (SuperManager.instance.tutoManager.isDetectionLearned)
        {
            if (itsRightSword)
            {
                if (collision.gameObject.tag == "LeftSword" && readyToSearch && teleportation.isTeleported == false)
                {
                    if (collision.contacts.Length > 0)
                    {
                        Vector3 contactPoint = collision.contacts[0].point;
                        SuperManager.instance.soundManager.PlaySound(SoundType.SearchingObjective, .5f, contactPoint);

                        // boule
                        if (itsRightSword)
                        {
                            meshRendererRight.GetComponent<MeshRenderer>().SetMaterials(matMoon_2_On);
                            meshRendererLeft.GetComponent<MeshRenderer>().SetMaterials(matSun_2_On);
                        }

                        StopCoroutine(TimeBeforeGetOff());
                        StartCoroutine(TimeBeforeGetOff());

                        foreach (Transform item in SuperManager.instance.gameManagerAetherPunk.artefactParent.transform)
                        {
                            float dist = Vector3.Distance(this.gameObject.transform.position, item.position);

                            if (dist <= SuperManager.instance.gameManagerAetherPunk.distanceSearch)
                            {
                                if (!item.GetComponent<ObjectToFind>().GetIsFound())
                                {
                                    item.GetComponent<ObjectToFind>().SetObjectIsFound();
                                }
                            }
                        }

                        currentCooldownSearch = SuperManager.instance.gameManagerAetherPunk.cooldownSearch;
                        readyToSearch = false;
                    }
                }
            }
        }
    }

    public void RemoveEffect(GameObject effect)
    {
        if (SuperManager.instance.gameManagerAetherPunk.detectionEffects.Contains(effect))
        {
            SuperManager.instance.gameManagerAetherPunk.detectionEffects.Remove(effect);
        }
    }

    IEnumerator TimeBeforeGetOff()
    {
        yield return new WaitForSeconds(2);

        meshRendererRight.GetComponent<MeshRenderer>().SetMaterials(matMoon);
        meshRendererLeft.GetComponent<MeshRenderer>().SetMaterials(matSun);

    }
}

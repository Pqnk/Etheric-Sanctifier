using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToFind : MonoBehaviour
{
    private bool _isFound = false;
    public Transform target;

    private void Start()
    {
        target = transform.GetChild(0);
    }

    public bool GetIsFound()
    {
        return _isFound;
    }

    public void SetObjectIsFound()
    {
        _isFound = true;

        SetMaterialObjectFound();
    }

    private void SetMaterialObjectFound()
    {
        Vector3 pos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        GameObject obj = Instantiate(SuperManager.instance.gameManagerAetherPunk.appearArea, pos, Quaternion.identity);

        SuperManager.instance.soundManager.PlaySound(SoundType.FindingObjective, 1f, this.gameObject.transform.position);

        this.gameObject.GetComponent<MeshRenderer>().material = SuperManager.instance.gameManagerAetherPunk.artefactMat;

        GameObject objParticule = Instantiate(SuperManager.instance.gameManagerAetherPunk.particulFollow, this.gameObject.transform.position, Quaternion.identity);
        objParticule.GetComponent<s_ParticuleFollow>().Set_Target(target);

        GameObject.Find("--RADAR--").GetComponent<Radar>().UpdateIndexObjective();
    }
}

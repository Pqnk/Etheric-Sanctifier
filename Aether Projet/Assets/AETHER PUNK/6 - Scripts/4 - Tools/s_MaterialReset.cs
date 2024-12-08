using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class s_MaterialReset : MonoBehaviour
{
    public Material objectMaterial;

    private void Start()
    {
        if (objectMaterial != null)
        {
            objectMaterial.SetFloat("_SphereRadius0", 0);
            objectMaterial.SetFloat("_SphereRadius1", 0f);
            objectMaterial.SetFloat("_SphereRadius2", 0f);
            objectMaterial.SetFloat("_SphereRadius3", 0f);

            objectMaterial.SetVector("_SphereCenter0", new Vector3(0, 0, 0));
            objectMaterial.SetVector("_SphereCenter1", new Vector3(0, 0, 0));
            objectMaterial.SetVector("_SphereCenter2", new Vector3(0, 0, 0));
            objectMaterial.SetVector("_SphereCenter3", new Vector3(0, 0, 0));
        }
    }

    public void ResetMat()
    {
        if (objectMaterial != null)
        {
            objectMaterial.SetFloat("_SphereRadius0", 0);
            objectMaterial.SetFloat("_SphereRadius1", 0f);
            objectMaterial.SetFloat("_SphereRadius2", 0f);
            objectMaterial.SetFloat("_SphereRadius3", 0f);

            objectMaterial.SetVector("_SphereCenter0", new Vector3(0, 0, 0));
            objectMaterial.SetVector("_SphereCenter1", new Vector3(0, 0, 0));
            objectMaterial.SetVector("_SphereCenter2", new Vector3(0, 0, 0));
            objectMaterial.SetVector("_SphereCenter3", new Vector3(0, 0, 0));
        }
    }
}

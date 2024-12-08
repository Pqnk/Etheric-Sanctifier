using UnityEngine;

public class UpdateMaterialPosition : MonoBehaviour
{
    // Référence au matériau
    public Material m_BoolShader;
    public int sphereID;
    // Nom de la propriété du shader (par exemple, "_ObjectPosition")
    public string position0PropertyName = "_SphereCenter0";
    public string Scale0PropertyName = "_SphereRadius0";

    public string position1PropertyName = "_SphereCenter1";
    public string Scale1PropertyName = "_SphereRadius1";

    public string position2PropertyName = "_SphereCenter2";
    public string Scale2PropertyName = "_SphereRadius2";

    public string position3PropertyName = "_SphereCenter3";
    public string Scale3PropertyName = "_SphereRadius3";

    void Update()
    {
        if (sphereID == 0)
        {
            if (m_BoolShader != null && !string.IsNullOrEmpty(position0PropertyName))
            {
                // Récupère la position actuelle du GameObject
                Vector3 objectPosition = transform.position;

                // Transmet la position au matériau
                m_BoolShader.SetVector(position0PropertyName, objectPosition);
            }

            if (m_BoolShader != null && !string.IsNullOrEmpty(Scale0PropertyName))
            {
                // Récupère la position actuelle du GameObject
                float objectScale = transform.localScale.x;
                //Debug.Log(objectScale);

                // Transmet la position au matériau
                m_BoolShader.SetFloat(Scale0PropertyName, objectScale);
            }
        }

        if (sphereID == 1)
        {
            if (m_BoolShader != null && !string.IsNullOrEmpty(position1PropertyName))
            {
                // Récupère la position actuelle du GameObject
                Vector3 objectPosition = transform.position;

                // Transmet la position au matériau
                m_BoolShader.SetVector(position1PropertyName, objectPosition);
            }

            if (m_BoolShader != null && !string.IsNullOrEmpty(Scale1PropertyName))
            {
                // Récupère la position actuelle du GameObject
                float objectScale = transform.localScale.x;
                //Debug.Log(objectScale);

                // Transmet la position au matériau
                m_BoolShader.SetFloat(Scale1PropertyName, objectScale);
            }
        }

        if (sphereID == 2)
        {
            if (m_BoolShader != null && !string.IsNullOrEmpty(position2PropertyName))
            {
                // Récupère la position actuelle du GameObject
                Vector3 objectPosition = transform.position;

                // Transmet la position au matériau
                m_BoolShader.SetVector(position2PropertyName, objectPosition);
            }

            if (m_BoolShader != null && !string.IsNullOrEmpty(Scale2PropertyName))
            {
                // Récupère la position actuelle du GameObject
                float objectScale = transform.localScale.x;
                //Debug.Log(objectScale);

                // Transmet la position au matériau
                m_BoolShader.SetFloat(Scale2PropertyName, objectScale);
            }
        }

        if (sphereID == 3)
        {
            if (m_BoolShader != null && !string.IsNullOrEmpty(position3PropertyName))
            {
                // Récupère la position actuelle du GameObject
                Vector3 objectPosition = transform.position;

                // Transmet la position au matériau
                m_BoolShader.SetVector(position3PropertyName, objectPosition);
            }

            if (m_BoolShader != null && !string.IsNullOrEmpty(Scale3PropertyName))
            {
                // Récupère la position actuelle du GameObject
                float objectScale = transform.localScale.x;
                //Debug.Log(objectScale);

                // Transmet la position au matériau
                m_BoolShader.SetFloat(Scale3PropertyName, objectScale);
            }
        }
    }

    private void OnDrawGizmos()
    {
        float objectScale = transform.localScale.x;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position ,objectScale);
    }
}

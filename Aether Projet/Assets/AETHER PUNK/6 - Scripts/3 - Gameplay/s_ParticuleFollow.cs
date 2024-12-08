using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class s_ParticuleFollow : MonoBehaviour
{
    private Transform target;

    private VisualEffect visualEffect;
    private NavMeshAgent agent;

    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
        visualEffect.SetInt("NbParticule", 200);
        agent = GetComponent<NavMeshAgent>();

        NavMeshHit hit;
        // Vérifie la position la plus proche sur le NavMesh
        if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            transform.position = hit.position; // Place l'objet sur le NavMesh
        }
        else
        {
            Debug.LogError("Impossible de placer l'objet sur le NavMesh !");
        }
    }

    public void Set_Target(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(target.position); 

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    Debug.Log("Target atteinte !");
                    visualEffect.SetInt("NbParticule", 0);
                    StartCoroutine(Destroy());
                }
            }
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2);

        Destroy(this.gameObject);
    }
}

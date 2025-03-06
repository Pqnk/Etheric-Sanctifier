using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlayTuto : MonoBehaviour
{
    public TutoManager tutoManager;
    public TutoButton etatTuto;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandNaked"))
        {
            tutoManager.AffichageTutoButton(etatTuto);
        }
    }
}

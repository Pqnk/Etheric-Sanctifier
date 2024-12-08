using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    public TutoType tutoType;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LeftSword" || other.gameObject.tag == "RightSword")
        {
            switch (tutoType)
            {
                case TutoType.Tp:
                    //SuperManager.instance.tutoManager.PlayTpDialogue();
                    break;

                case TutoType.Collision:
                    //SuperManager.instance.tutoManager.PlayCollisionDialogue();
                    break;

                case TutoType.Detection:
                    //SuperManager.instance.tutoManager.PlayDetectionDialogue();
                    break;

                case TutoType.PasTouche:
                    //SuperManager.instance.tutoManager.PlayDestructionVue();
                    break;
            }

        }
    }
}

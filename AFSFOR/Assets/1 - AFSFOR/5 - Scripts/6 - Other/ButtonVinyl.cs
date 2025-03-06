using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum ButtonVinylType
{
    Play,
    Next,
    Previous
}

public class ButtonVinyl : MonoBehaviour
{
    [SerializeField] private ButtonVinylType _typeButton;
    [SerializeField] private Vinyl _vinyl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandNaked"))
        {
            switch (_typeButton)
            {
                case ButtonVinylType.Play:
                    _vinyl.PlayOrStop();
                    break;
                case ButtonVinylType.Next:
                    _vinyl.SwitchToNext();

                    break;
                case ButtonVinylType.Previous:
                    _vinyl.SwitchToPrevious();
                    break;
            }
        }
    }

}

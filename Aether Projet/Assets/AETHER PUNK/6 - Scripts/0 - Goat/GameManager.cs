using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Sons")]
    public TMP_Text kill;
    public Slider life;
    public int maxLife;
    public GameObject explosion;

    private int currentKill;
    private int currentLife;

    // Start is called before the first frame update
    void Start()
    {
        life.maxValue = maxLife;
        currentLife = maxLife;
        life.value = currentLife;

        kill.text = currentKill.ToString();
    }


    public void AddHit()
    {
        currentKill++;
        kill.text = currentKill.ToString();
    }

    public void GetHit()
    {
        currentLife--;
        life.value = currentLife;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sheep")
        {
            GetHit();
            GameObject sound = Instantiate(explosion, transform.position, transform.rotation);
            sound.transform.SetParent(other.gameObject.transform);
            sound.gameObject.AddComponent<Destroy>();
            Destroy(other.gameObject);
        }
    }
}

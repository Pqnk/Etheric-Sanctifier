using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSheep : MonoBehaviour
{
    [Header("Prefabs and Zones")]
    public GameObject[] prefabs; 
    public Transform[] spawnZones;
    public Transform[] targets;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    private float _moveSpeed;
    public float moveSpeedMin = 2f;
    public float moveSpeedMax = 100f;

    public string[] names;
    public Material[] mat;

    private void Start()
    {
        StartCoroutine(SpawnPrefabs());
    }

    IEnumerator SpawnPrefabs()
    {
        while (true)
        {
            // Génération d'un prefab aléatoire dans une zone aléatoire
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            Transform zone = spawnZones[Random.Range(0, spawnZones.Length)];

            // Instanciation à la position de la zone
            Vector3 spawnPosition = zone.position;
            GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);

            // Choisir une cible aléatoire parmi les cibles disponibles
            Transform randomTarget = targets[Random.Range(0, targets.Length)];

            // Ajout du script de mouvement
            PrefabMover mover = instance.AddComponent<PrefabMover>();
            mover.SetTarget(randomTarget);

            _moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
            mover.SetSpeed(_moveSpeed);

            int index = Random.Range(0, names.Length);
            mover.SetNameAndMaterial(names[index], mat[index]);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
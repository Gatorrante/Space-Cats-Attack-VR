using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag; // Identificador del pool.
        public GameObject prefab; // Prefab que se va a usar.
        public int size; // Tamaño inicial del pool.
    }

    public List<Pool> pools; // Lista de pools configurados.
    public Dictionary<string, Queue<GameObject>> poolDictionary; // Diccionario para acceder a los pools.

    public static ObjectPooler Instance { get; internal set; }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Inicializar cada pool.
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); // Desactiva el objeto.
                objectPool.Enqueue(obj); // Agrega al pool.
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool con etiqueta {tag} no existe.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}

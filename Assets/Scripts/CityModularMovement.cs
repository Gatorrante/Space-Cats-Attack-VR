using UnityEngine;

public class CityModularMovement : MonoBehaviour
{
    public string poolTag; // Etiqueta del pool para identificar el prefab a generar.
    public float speed = 2f; // Velocidad del movimiento en el eje X.
    public float objectWidth = 10f; // Ancho del espacio entre objetos adyacentes.
    
    private ObjectPooler objectPooler;
    private GameObject lastInstance; // Referencia al último objeto generado.

    void Start()
    {
        // Referencia al Object Pooler.
        objectPooler = ObjectPooler.Instance;

        // Genera el primer módulo.
        lastInstance = objectPooler.SpawnFromPool(poolTag, transform.position, Quaternion.identity);
    }

    void Update()
    {
        // Mueve los objetos hacia la izquierda.
        foreach (Transform child in transform)
        {
            child.Translate(Vector3.left * speed * Time.deltaTime);

            // Si un objeto está fuera del rango visible, desactívalo.
            if (child.position.x < -objectWidth * 2)
            {
                child.gameObject.SetActive(false);
            }
        }

        // Genera un nuevo módulo cuando sea necesario.
        if (lastInstance.transform.position.x <= transform.position.x)
        {
            Vector3 newPosition = new Vector3(
                lastInstance.transform.position.x + objectWidth,
                lastInstance.transform.position.y,
                lastInstance.transform.position.z
            );

            lastInstance = objectPooler.SpawnFromPool(poolTag, newPosition, Quaternion.identity);
        }
    }
}

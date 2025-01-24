using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Gun : MonoBehaviour
{
    public InputActionProperty triggerAction;
    public float raycastDistance = 100f;
    public LayerMask enemyLayer;
    public Transform barrelLocation;
    public GameObject lineRendererPrefab;
    public ParticleSystem reloadParticle;
    public TextMeshPro bulletCounterText;

    private int bullets = 10;
    private bool isReloading = false;
    private Color initialColor = new Color(0.2f, 1f, 0.75f);
    private Color finalColor = new Color(1f, 0.6f, 0.6f);

    void Start()
    {
        UpdateBulletCounter();
        reloadParticle.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isReloading) return;

        if (triggerAction.action.WasPressedThisFrame() && bullets > 0)
        {
            Shoot();
        }

        if (bullets == 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        bullets--;
        UpdateBulletCounter();

        Ray ray = new Ray(barrelLocation.position, barrelLocation.forward);
        RaycastHit hit;

        GameObject liner = Instantiate(lineRendererPrefab);
        LineRenderer lineRenderer = liner.GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        if (Physics.Raycast(ray, out hit, raycastDistance, enemyLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit: " + hit.collider.name);

                lineRenderer.SetPositions(new Vector3[] { barrelLocation.position, hit.point });

                // Aplica daño al enemigo
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
        }
        else
        {
            lineRenderer.SetPositions(new Vector3[] { barrelLocation.position, barrelLocation.position + barrelLocation.forward * raycastDistance });
        }

        Destroy(liner, 0.2f);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        reloadParticle.gameObject.SetActive(true);
        Debug.Log("Recargando...");
        yield return new WaitForSeconds(3f);
        reloadParticle.gameObject.SetActive(false);
        bullets = 10;
        UpdateBulletCounter();
        isReloading = false;
    }

    void UpdateBulletCounter()
    {
        if (bulletCounterText != null)
        {
            bulletCounterText.text = bullets.ToString();
            float t = (float)bullets / 10f;
            bulletCounterText.color = Color.Lerp(finalColor, initialColor, t);
        }
    }
}
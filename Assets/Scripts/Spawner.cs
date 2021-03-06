using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToTrigger;

    [SerializeField]
    private GameObject somethingToSpawn = null;
    [SerializeField]
    private int spawnCounter = 10;
    [SerializeField]
    private float spawningCooldown = 0f;
    private float lastSomethingSpawned;

    [SerializeField]
    private float activeRange = 50f;
    [SerializeField]
    private bool showRange = false;

    [SerializeField]
    private Transform spawnArea = null;

    void Update()
    {
        if (objectToTrigger != null && Vector3.Distance(objectToTrigger.transform.position, transform.position) < activeRange)
        {
            if (Time.time > lastSomethingSpawned + spawningCooldown && spawnCounter > 0)
            {
                lastSomethingSpawned = Time.time;

                Vector3 spawnPosition;
                if (spawnArea != null)
                {
                    // get random point in circle with radius of 1
                    Vector2 random = Random.insideUnitCircle * spawnArea.lossyScale.x * 0.5f;
                    spawnPosition = transform.position + new Vector3(random.x, 0f, random.y);
                }
                else
                {
                    spawnPosition = transform.position;
                }

                GameObject newBullet = GameObject.Instantiate(somethingToSpawn, spawnPosition, transform.rotation);
                spawnCounter--;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (showRange)
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, activeRange);
        }
    }
#endif
}

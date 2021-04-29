using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public GameObject iconPrefab = null;
    public Transform iconHolder = null;
    public Vector3 iconOffset = Vector3.zero;
    private List<GameObject> iconInstances = new List<GameObject>();

    public void SetHealth(int health)
    {
        UpdateHearthIconCount(health);
    }

    private void UpdateHearthIconCount(int health)
    {
        int points = health / 2;
        bool half = health % 2 > 0;
        // if less icon are there as should be
        if (!half && iconInstances.Count > 0)
        {
            GameObject iconInstance = iconInstances[iconInstances.Count - 1];
            iconInstance.GetComponent<Image>().fillAmount = 1f;
        }
        while (points > iconInstances.Count)
        {
            Vector3 position = iconHolder.position + iconOffset * iconInstances.Count;
            GameObject iconInstance = Instantiate(iconPrefab, position, Quaternion.identity, iconHolder);
            iconInstances.Add(iconInstance);
        }
        if (half)
        {
            Vector3 position = iconHolder.position + iconOffset * iconInstances.Count;
            GameObject iconInstance = Instantiate(iconPrefab, position, Quaternion.identity, iconHolder);
            iconInstance.GetComponent<Image>().fillAmount = 0.5f;
            iconInstances.Add(iconInstance);
        }
        // if more icons are there as should be
        while (points + half.ToInt() < iconInstances.Count)
        {
            GameObject iconInstance = iconInstances[iconInstances.Count - 1];
            iconInstances.RemoveAt(iconInstances.Count - 1);
            Destroy(iconInstance);
        }
        if (half)
        {
            GameObject iconInstance = iconInstances[iconInstances.Count - 1];
            iconInstance.GetComponent<Image>().fillAmount = 0.5f;
        }
    }
}

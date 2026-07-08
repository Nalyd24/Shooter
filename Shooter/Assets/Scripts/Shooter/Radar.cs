using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Radar : MonoBehaviour
{
    public Transform player;
    public RectTransform radarContainer;
    public GameObject enemyIconPrefab;

    public float radarRange = 50f;
    public float radarScale = 2f;
    private readonly List<Transform> enemies = new List<Transform>();
    private readonly List<GameObject> enemyIcons = new List<GameObject>();

    void Start()
    {
        // Automatically find all enemies tagged "Enemy"
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in foundEnemies)
        {
            enemies.Add(enemy.transform);
            GameObject icon = Instantiate(enemyIconPrefab, radarContainer);
            enemyIcons.Add(icon);
        }
    }

    void Update()
    {
        UpdateRadar();
    }

    void UpdateRadar()
    {
        if (player == null || radarContainer == null) return;

        for (int i = 0; i < enemies.Count; i++)
        {
            Transform enemy = enemies[i];
            GameObject icon = enemyIcons[i];

            if (enemy == null)
            {
                icon.SetActive(false);
                continue;
            }

            Vector3 offset = enemy.position - player.position;
            offset.y = 0; // Ignore height

            if (offset.magnitude > radarRange)
            {
                icon.SetActive(false);
                continue;
            }

            icon.SetActive(true);

            // Rotate radar relative to player direction (optional, realistic)
            offset = Quaternion.Euler(0, -player.eulerAngles.y, 0) * offset;

            // Convert to radar position
            Vector2 radarPos = new Vector2(offset.x, offset.z) / radarRange * (radarContainer.sizeDelta.x / 2f);
            icon.GetComponent<RectTransform>().anchoredPosition = radarPos;
        }
    }
}

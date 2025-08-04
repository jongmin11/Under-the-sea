using UnityEngine;

public class AllySpawner : MonoBehaviour
{
    public GameObject allyPrefab;
    public int allyCount = 5;
    public Transform player;

    public void SpawnAllies()
    {
        for (int i = 0; i < allyCount; i++)
        {
            Vector3 spawnPos = GetRandomScreenPosition();
            GameObject ally = Instantiate(allyPrefab, spawnPos, Quaternion.identity);
            AllyFollower script = ally.GetComponent<AllyFollower>();
            script.MoveToPlayer(player.position);
            script.StartOrbit(player, Random.Range(1.8f, 2.8f), Random.Range(30f, 70f));
        }
    }

    private Vector3 GetRandomScreenPosition()
    {
        Vector2 screenPos = new Vector2(
            Random.Range(0.1f, 0.9f) * Screen.width,
            Random.Range(0.1f, 0.9f) * Screen.height
        );

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;
        return worldPos;
    }
}

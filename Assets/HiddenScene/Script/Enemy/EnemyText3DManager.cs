using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyText3DManager : MonoBehaviour
{
    public GameObject enemyTextPrefab;
    public Transform player;
    public Vector3 spawnOrigin = Vector3.zero;

    void Start()
    {
        StartCoroutine(LoadAndSpawn());
    }

    IEnumerator LoadAndSpawn()
    {
        TextAsset json = Resources.Load<TextAsset>("enemy_texts");
        if (json == null)
        {
            Debug.LogError("enemy_texts.json 로드 실패");
            yield break;
        }

        EnemyTextData[] list = JsonHelper.FromJson<EnemyTextData>(json.text);
        System.Array.Sort(list, (a, b) => a.spawnTime.CompareTo(b.spawnTime));

        float timer = 0f;

        foreach (var d in list)
        {
            float wait = d.spawnTime - timer;
            if (wait > 0f) yield return new WaitForSeconds(wait);

            // 화면 위 (posY + 5)에서 생성
            Vector3 spawnPos = spawnOrigin + new Vector3(d.posX, d.posY + 5f, 0);
            GameObject obj = Instantiate(enemyTextPrefab, spawnPos, Quaternion.identity);

            var ctrl = obj.GetComponent<EnemyText3DAIController>();
            if (ctrl != null)
            {
                Color c = new Color(1f, 1f, 1f, d.alpha);
                EnemyTextAIType aiType = EnemyTextAIType.Straight;
                System.Enum.TryParse(d.aiType, true, out aiType);

                ctrl.Setup(
                    d.content,
                    d.fontSize,
                    c,
                    d.downSpeed,
                    d.attackAngle,
                    d.hp,
                    aiType,
                    player
                );
            }

            timer = d.spawnTime;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private EnemyData[] _enemyExamples;
    [SerializeField] private Transform[] _enemyRoutePoints;
    [SerializeField] private int _spawnMultiplier;
    private int _waveNum = 1;
    private int _enemyIndex = 0;

    private void Start()
    {
        CreateTrackWave();
    }

    private void Update()
    {
        //CreateTrackWave();
    }

    private void CreateTrackWave()
    {
        //if (_activeTracks.Count == 0)
        //{
            int randomEnemyExampleIndex = Random.Range(0, _enemyExamples.Length);
            int enemyCount = 0;
            if (randomEnemyExampleIndex == 0)
            {
            enemyCount = _waveNum * _spawnMultiplier;
            }
            else if (randomEnemyExampleIndex == 1)
            {
            enemyCount = _waveNum;
            }
            StartCoroutine(CreateTracks(randomEnemyExampleIndex, enemyCount));
            _waveNum++;
        //}
    }

    private IEnumerator CreateTracks(int EnemyExampleIndex, int enemyCount)
    {
        var currentEnemyExample = _enemyExamples[EnemyExampleIndex];

        float delay = currentEnemyExample.CreationDelay;

        for (int i = 0; i < enemyCount; i++)
        {
            var spawnPosition = new Vector3(_enemyRoutePoints[0].position.x, _enemyRoutePoints[0].position.y, 0);
            var enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

            enemy.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = currentEnemyExample.Sprite;

            enemy.GetComponent<EnemyMovement>().SetMovementParameters(_enemyRoutePoints, currentEnemyExample.Speed);
            enemy.GetComponent<EnemyRotation>().SetRotationParameters
                (_enemyRoutePoints, currentEnemyExample.Speed, currentEnemyExample.RotationSpeed, currentEnemyExample.RotateStartDistance);
            enemy.GetComponent<EnemyPointer>().EnemyIndex = _enemyIndex;
            enemy.GetComponent<HealthComponent>().MaxHealthAmount = currentEnemyExample.MaxHealthAmount;

            _enemyIndex++;

            yield return new WaitForSeconds(delay);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyHandler _enemyHandler;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private EnemyData[] _enemyExamples;
    [SerializeField] private Transform[] _enemyRoutePoints;
    [SerializeField] private int _spawnMultiplier;

    private int _waveNum = 1;
    private int _enemyId = 0;

    private int _availableEnemyCount = 0;

    private void Update()
    {
        CreateTrackWave();
    }

    private void CreateTrackWave()
    {
        if (_availableEnemyCount == 0)
        {
            int randomEnemyExampleIndex = Random.Range(0, _enemyExamples.Length);
            int enemyCount = _waveNum * _enemyExamples[randomEnemyExampleIndex].SpawnMultiplier;

            StartCoroutine(CreateEnemies(randomEnemyExampleIndex, enemyCount));
            _availableEnemyCount = enemyCount;
            _waveNum++;
        }
    }

    private IEnumerator CreateEnemies(int EnemyExampleIndex, int enemyCount)
    {
        var currentEnemyExample = _enemyExamples[EnemyExampleIndex];

        float delay = currentEnemyExample.CreationDelay;

        for (int i = 0; i < enemyCount; i++)
        {
            var spawnPosition = new Vector3(_enemyRoutePoints[0].position.x, _enemyRoutePoints[0].position.y, 0);
            var enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

            var enemyContainer = enemy.GetComponent<EnemyContainer>();

            enemyContainer.EnemySpriteRenderer.sprite = currentEnemyExample.Sprite;

            enemyContainer.EnemyMovementComponent.SetMovementParameters(_enemyRoutePoints, currentEnemyExample.Speed);
            enemyContainer.EnemyRotationComponent.SetRotationParameters
                (_enemyRoutePoints, currentEnemyExample.Speed, currentEnemyExample.RotationSpeed, currentEnemyExample.RotateStartDistance);
            enemyContainer.EnemyPointerComponent.EnemyExampleIndex = EnemyExampleIndex;
            enemyContainer.EnemyIDComponent.EnemyId = _enemyId;
            enemyContainer.HealthComponent.MaxHealthAmount = currentEnemyExample.MaxHealthAmount;

            enemyContainer.Reward = currentEnemyExample.Reward;

            _enemyHandler.SubscribeEnemyHealthEvents(enemyContainer.HealthComponent);

            _enemyId++;

            yield return new WaitForSeconds(delay);
        }
    }

    public void RemoveOneEnemyFromCount()
    {
        _availableEnemyCount--;
    }
}

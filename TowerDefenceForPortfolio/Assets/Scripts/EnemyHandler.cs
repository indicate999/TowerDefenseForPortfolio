using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private MoneyService _moneyService;
    [SerializeField] private SoundEffector _soundEffector;
    public void SubscribeEnemyHealthEvents(HealthComponent enemyHealth)
    {
        enemyHealth.GotHurt += OnEnemyGotHurt;
        enemyHealth.Died += OnEnemyDied;
    }

    private void UnsubscribeEnemyHealthEvents(HealthComponent enemyHealth)
    {
        enemyHealth.GotHurt -= OnEnemyGotHurt;
        enemyHealth.Died -= OnEnemyDied;
    }

    private void OnEnemyGotHurt(GameObject enemy, float healthAmount, float maxHealthAmount)
    {
        enemy.GetComponent<HealthBar>().SetBarValue(healthAmount, maxHealthAmount);
    }

    private void OnEnemyDied(GameObject enemy)
    {
        _soundEffector.PLayTrackExposionSound();
        _moneyService.AddCoins(enemy.GetComponent<EnemyContainer>().Reward);

        enemy.SetActive(false);

        _enemySpawner.RemoveOneEnemyFromCount();

        UnsubscribeEnemyHealthEvents(enemy.GetComponent<HealthComponent>());
    }
}

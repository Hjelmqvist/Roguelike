using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GenScript _levelGenerator;
    [SerializeField] Player _player;
    [SerializeField] float _secondsBeforeMakingMoves = 1;
    [SerializeField] float _secondsBetweenEnemyMoves = 1;
    [SerializeField] float _secondsBeforeEndingTurn = 1;

    [SerializeField] List<Enemy> _enemies = new List<Enemy>();

    public UnityEvent OnEnemyTurnEnded;
    

    public void AddEnemy(Enemy enemy)
    {
        if (enemy)
        {
            _enemies.Add(enemy);
            enemy.OnEntityDeath.AddListener(OnEntityDeath);
        }
    }

    public void ClearEnemyList()
    {
        _enemies.Clear();
    }

    private void OnEntityDeath(Entity entity)
    {
        if (entity is Enemy enemy)
        {
            _player.ModifyGold(enemy.goldValue);
            _enemies.Remove(enemy);
        }
    }

    public void StartEnemyTurn(Entity player)
    {
        StartCoroutine(MakeEnemyMoves(player));
    }

    private void EndEnemyTurn()
    {
        OnEnemyTurnEnded.Invoke();
    }

    IEnumerator MakeEnemyMoves(Entity player)
    {
        yield return new WaitForSeconds(_secondsBeforeMakingMoves);
        foreach (Enemy enemy in _enemies)
        {
            enemy.MakeMove(_levelGenerator.Tiles, player);
            yield return new WaitForSeconds(_secondsBetweenEnemyMoves);
        }
        yield return new WaitForSeconds(_secondsBeforeEndingTurn);
        EndEnemyTurn();
    }
}
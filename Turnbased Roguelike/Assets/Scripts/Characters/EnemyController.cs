using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float _secondsBeforeMakingMoves = 1;
    [SerializeField] float _secondsBetweenEnemyMoves = 1;
    [SerializeField] float _secondsBeforeEndingTurn = 1;

    public static List<Enemy> _enemies = new List<Enemy>();

    public UnityEvent OnEnemyTurnEnded;

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
            enemy.MakeMove(player);
            yield return new WaitForSeconds(_secondsBetweenEnemyMoves);
        }
        yield return new WaitForSeconds(_secondsBeforeEndingTurn);
        EndEnemyTurn();
    }
}
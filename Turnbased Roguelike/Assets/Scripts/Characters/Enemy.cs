using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private void OnEnable() => Player.OnPlayerMoved += Player_OnPlayerMoved;

    private void OnDisable() => Player.OnPlayerMoved -= Player_OnPlayerMoved;

    private void Player_OnPlayerMoved()
    {
        // do movement stuff
    }
}

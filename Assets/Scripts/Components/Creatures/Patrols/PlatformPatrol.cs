using Components.Creatures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPatrol : Patrol
{
    [SerializeField] private LayerCheck _groundCheck;
    [SerializeField] private LayerCheck _obstacleCheck;

    private Creature _creature;
    private Vector2 _direction;


    private void Awake()
    {
        _creature = GetComponent<Creature>();
        _direction = Vector2.left;
        _direction.y = 0;
    }

    public override IEnumerator DoPatrol()
    {
        while(enabled)
        {
            if(_groundCheck.IsTouchingLayer && !_obstacleCheck.IsTouchingLayer)
            {
                _creature.SetDirection(_direction);
            } else
            {
                _direction = _direction * -1;
                _creature.SetDirection(_direction);
            }

            yield return null;
        }
    }
}

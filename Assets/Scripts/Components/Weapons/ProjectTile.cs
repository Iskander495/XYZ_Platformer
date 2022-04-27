using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Weapons
{
    public class ProjectTile : BaseProjectTile
    {
        protected override void Start()
        {
            base.Start();

            var force = new Vector2(_direction * _speed, 0);
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
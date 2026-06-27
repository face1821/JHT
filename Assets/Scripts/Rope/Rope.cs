using System.Collections;
using System.Collections.Generic;
using Game.Player;
using Game.Tool;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStateMachine playerStateMachine = InstanceFinder.Player.StateMachine;
        
    }
}

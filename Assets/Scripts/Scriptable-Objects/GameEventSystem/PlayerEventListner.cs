using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventListner : GameEventListener
{
    public UnityEvent<Player> PlayerResponse;
    override public void OnEventRaised()
    {
        PlayerResponse.Invoke(new Player()); ;
    }

}

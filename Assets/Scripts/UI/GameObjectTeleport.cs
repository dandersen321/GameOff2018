using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTeleport : MonoBehaviour {

    public GameObject objectToMove;
    public GameObject locationToMoveTo;

    public void OnNightBeat(int nightBeat)
    {
        if (nightBeat == 0)
        {
            objectToMove.transform.position = locationToMoveTo.transform.position;
            objectToMove.transform.rotation = locationToMoveTo.transform.rotation;
        }
    }

    private void OnEnable()
    {
        References.GetEnemySpawnerManager().OnNightBeat += OnNightBeat;

    }

    private void OnDisable()
    {
        References.GetEnemySpawnerManager().OnNightBeat -= OnNightBeat;
    }

}

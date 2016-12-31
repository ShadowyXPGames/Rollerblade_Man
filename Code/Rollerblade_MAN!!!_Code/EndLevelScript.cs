using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class EndLevelScript : MonoBehaviour{

    public delegate void LevelWinEventHandler();
    public event LevelWinEventHandler OnLevelWin;

    void CallOnLevelWin() => OnLevelWin?.Invoke();

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            CallOnLevelWin();
        }
    }
}
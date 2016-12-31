using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class GameOverHandler : MonoBehaviour {

    private EndLevelScript levelEndItem;

    private float startLevel;
    private float timeTook;

    private void OnEnable() {
        levelEndItem = FindObjectOfType<EndLevelScript>();
        if(levelEndItem == null) {
            Debug.LogError("No level end script! the player wont be able to finish! CHANGE IT");
        }
        levelEndItem.OnLevelWin += LevelWon;
    }

    private void OnDisable() {
        levelEndItem.OnLevelWin -= LevelWon;
    }

    private void Start() {
        startLevel = Time.time;
    }

    void LevelWon() {
        timeTook = Time.time - startLevel;
        Debug.Log($"you won the level in: {timeTook} seconds!");
    }
}
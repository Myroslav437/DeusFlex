using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnSetup : MonoBehaviour 
{
    public bool isRandom = false;
    public float randomFrom = -10f;
    public float randomTo = 10f;
    public float randomY = 5f;

    public Transform[] RedSpawnPoints;
    public Transform[] BlueSpawnPoints;

    public Vector3 GetSpawnPoint(int actorNum = 0) {
        if (isRandom) {
            Vector3 pos = new Vector3(0f, randomY, 0f);
            pos.x = Random.Range(randomFrom, randomTo);
            return pos;
        }
        else {
            int redIdx = 0, blueIdx = 0;
            foreach (KeyValuePair<int, TeamController.PlayerData> p in TeamController.TC.playersData) {
                if (p.Key == actorNum) {
                    if (p.Value.team == "Red") {
                        return RedSpawnPoints[redIdx % RedSpawnPoints.Length].position;
                    }
                    else if (p.Value.team == "Blue")  {
                        return BlueSpawnPoints[blueIdx % BlueSpawnPoints.Length].position;
                    }
                }
                else {
                    if (p.Value.team == "Red") { 
                        redIdx++;
                    }
                    else if(p.Value.team == "Blue") {
                        blueIdx++;
                    }
                }
            }

            return new Vector3(0, 0, 0);
        }
        
    }

}
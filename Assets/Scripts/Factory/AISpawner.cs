using System;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    // Beginner mode
    public void SpawnRacers(ConcreteAIRacerFactory factory, Transform[] spawnPoints, CustomLinkedList<Transform> waypointsList)
    {
        Debug.Log("Spawning Beginner Racers");
        
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            AIRacer racerBase = factory.CreateRacer();
            GameObject racerGO = racerBase.gameObject;
         

            racerGO.transform.position = spawnPoints[i].position;

            BeginnerRaceLogic  ai = racerGO.AddComponent<BeginnerRaceLogic>();
            if (ai != null)
            {
                ai.waypoints = waypointsList;
                ai.BeginRace();
            }
            else
            {
                Debug.LogWarning(racerGO.name + " is missing RaceLogicCore!");
            }
         
            racerGO.GetComponent<PositionTracker>().CurrentWaypoint = waypointsList[0];
        }
    }
    
    
// Advanced mode
    public void SpawnRacers(ConcreteAIRacerFactory factory, Transform[] spawnPoints, CustomGraph<Transform> waypointsGraph, Transform startWaypoint)
    {
        Debug.Log("Spawning Advanced Racers");
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            AIRacer racerBase = factory.CreateRacer();
            GameObject racerGO = racerBase.gameObject;
            racerGO.transform.position = spawnPoints[i].position;

            AdvancedRaceLogic ai = racerGO.AddComponent<AdvancedRaceLogic>();
            ai.waypoints = waypointsGraph;
            ai.BeginRace(startWaypoint);

            racerGO.GetComponent<PositionTracker>().CurrentWaypoint = startWaypoint;
        }
    }
}
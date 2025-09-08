using UnityEngine;

public class ConcreteAIRacerFactory : AIRacerFactory
{
    //Accessible Game Object components for each AIRacer subclasses
    [SerializeField] private GameObject racerTopSpeed;
    [SerializeField] private GameObject racerAccelerator;
    [SerializeField] private GameObject racerTurner;
    
    public override AIRacer CreateRacer()
    {
        //Picks a random int which will determine the racer subclass being spawned
        int choice = Random.Range(0,3);
        GameObject prefab = null;
        
        //Assigns the subclass of the racer based on the int
        switch (choice)
        {
            case 0: prefab = racerTopSpeed; break;
            case 1: prefab = racerAccelerator; break;
            case 2: prefab = racerTurner; break;
        }
        
        if (prefab == null)
        {
            Debug.LogError("No prefab assigned for racer type: " + choice);
            return null;
        }
        
        //Instantiate the racer into the scene
        GameObject instance = GameObject.Instantiate(prefab);
        //Gets the behaviours of that racer
        AIRacer racer = instance.GetComponent<AIRacer>();
        
        if (racer == null)
        {
            Debug.LogError("Spawned prefab missing AIRacer component!");
        }
        
        //Sets the behaviours of that racer
        racer?.Initialise();
        return racer;
    }
}

using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using System;

public class PigeonAIController : MonoBehaviour {
	public PigeonCharacterScript3 pigeonCharacter;

    //Ai states
    enum PigeonState
    {
       //TODO: make states here
    }

    //current state of the AI
    PigeonState currentState;

    //Time the player has been eating
    DateTime eatTime = DateTime.Now;
    List<GameObject> foodList;
    //radius in which the pigeon gets scared. 
    public float scareRadius;

    void Start () {
		pigeonCharacter = GetComponent<PigeonCharacterScript3> ();

        //initalize the eat time.
        eatTime = DateTime.Now;

        //Find all the food on the map
        foodList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        //Select a random food item.
        pigeonCharacter.food = foodList[UnityEngine.Random.Range(0, foodList.Count)];
        //Set initial state.
        //TODO: Initial state
    }

    /// <summary>
    /// Update loop, called every frame.
    /// </summary>
    void FixedUpdate()
    {
        //TODO: place something here
    }

    /// <summary>
    /// Checks if player is close and sets state accordingly
    /// </summary>
    private void CheckForPlayer()
    {
      
    }

    /// <summary>
    /// decide what to do based on state
    /// </summary>
    private void ResolveState()
    {
        
    }

    void SeekFood()
    {
        //Find relative position of target.
        Vector3 targetRelPos = pigeonCharacter.food.transform.position - transform.position;
        //only regard 2d distance, not height.
        targetRelPos.y = 0;
        //If the pigeon is flying and the target is close, start landing.
        if (targetRelPos.sqrMagnitude < 10f && pigeonCharacter.isFlying && !pigeonCharacter.soaring)
        {

            pigeonCharacter.tryingToLand = true;
            pigeonCharacter.SetForwardAcceleration(-1.5f);
            pigeonCharacter.SetUpSpeed(-1.5f);
        }
        //If the pigeon is not flying and the target is far away, soar.
        else if (targetRelPos.sqrMagnitude > 30f && !pigeonCharacter.isFlying && !pigeonCharacter.tryingToLand)
        {
            pigeonCharacter.Soar();

        }
        //if the target is really close, start eating.
        else if (targetRelPos.sqrMagnitude < .083f)
        {
            pigeonCharacter.SetForwardAcceleration(0f);
            pigeonCharacter.Eat();

            eatTime = DateTime.Now.AddSeconds(UnityEngine.Random.Range(10, 60));
            //TODO: set state here

        }
        //Accelerate towards the target.
        else
        {
            pigeonCharacter.SetForwardAcceleration(.5f);
        }
        //Face the target
        targetRelPos.Normalize();
        pigeonCharacter.turnSpeed = -Vector3.Dot(targetRelPos, transform.forward);
    }

    /// <summary>
    /// Check if there is a player close to the pigeon
    /// </summary>
    /// <returns></returns>
    private bool playerClose()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scareRadius);
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject.GetComponent<ThirdPersonCharacter>())
                {
                    return true;
                }
            }
        }
        return false;
    }
}

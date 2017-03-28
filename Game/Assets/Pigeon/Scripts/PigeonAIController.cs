using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;
using System;

public class PigeonAIController : MonoBehaviour {
	public PigeonCharacterScript3 pigeonCharacter;

    enum PigeonState
    {
        eating,
        walking,
        seekingfood,
        scared
    }

    PigeonState currentState;
    DateTime eatTime = DateTime.Now;
    List<GameObject> foodList;
    public float scareRadius;

    void Start () {
		pigeonCharacter = GetComponent<PigeonCharacterScript3> ();
        currentState = PigeonState.eating;
        eatTime = DateTime.Now;
       foodList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        
        pigeonCharacter.food = foodList[UnityEngine.Random.Range(0, foodList.Count)];
	}

	void SeekFood(){
		Vector3 targetRelPos = pigeonCharacter.food.transform.position - transform.position;
        targetRelPos.y = 0;
        //Vector3 targetRelHzPos = pigeonCharacter.food.transform.position - transform.position; //new Vector3(targetRelPos.x, 0, targetRelPos.z);
		if (targetRelPos.sqrMagnitude < 10f && pigeonCharacter.isFlying && !pigeonCharacter.soaring) {
            
			pigeonCharacter.tryingToLand = true;
			pigeonCharacter.SetForwardAcceleration (-1.5f);
			pigeonCharacter.SetUpSpeed (-1.5f);
		} else if (targetRelPos.sqrMagnitude > 30f && !pigeonCharacter.isFlying && !pigeonCharacter.tryingToLand) {
			pigeonCharacter.Soar ();
		} else if (targetRelPos.sqrMagnitude < .083f) {
			pigeonCharacter.SetForwardAcceleration(0f);
			pigeonCharacter.Eat();


            eatTime = DateTime.Now.AddSeconds(UnityEngine.Random.Range(10, 60));
            currentState = PigeonState.eating;

		}else{
            //if(pigeonCharacter.transform.position.y >1)
            //{
            //    pigeonCharacter.upSpeed = 0;
            //}
           
            pigeonCharacter.SetForwardAcceleration(.5f);
        }
        targetRelPos.Normalize();
		pigeonCharacter.turnSpeed = -Vector3.Dot (targetRelPos, transform.forward);
	}

    private void ResolveState()
    {
        //Debug.Log(string.Format("Current Pigeon state = {0}", currentState));
        switch(currentState)
        {
            case PigeonState.eating:
                if(DateTime.Now > eatTime)
                {
                    pigeonCharacter.food = foodList[UnityEngine.Random.Range(0, foodList.Count)];
                    currentState = PigeonState.seekingfood; 
                }
                break;
            case PigeonState.seekingfood:
                SeekFood();
                break;
            case PigeonState.scared:
                pigeonCharacter.Soar();
                break;
            default:
                break;
        }
    }

    private void CheckForPlayer()
    {
        bool playerFound = false;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scareRadius);
        {
            foreach(Collider collider in hitColliders)
            {
                if (collider.gameObject.GetComponent<ThirdPersonCharacter>()){
                    playerFound = true;
                    
                }
            }
        }
        if(playerFound)
        {
            currentState = PigeonState.scared;
        }
        else if(currentState == PigeonState.scared)
        {
            pigeonCharacter.food = foodList[UnityEngine.Random.Range(0, foodList.Count)];
            currentState = PigeonState.seekingfood;
        }
    }

	void FixedUpdate(){
        CheckForPlayer();
        ResolveState();
	}
}

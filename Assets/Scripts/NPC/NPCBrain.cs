using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

public class NPCBrain : MonoBehaviour
{
    public bool debug = false;

    [Tooltip("The text files that contain the behaviors for this NPC to perform.")]
    [SerializeField]
    private TextAsset[] behaviors;
    [Tooltip("The behavior script currently being executed by the NPC.")]
    [SerializeField]
    private int selectedBehavior = 0;
    [Tooltip("Whether or not to select a behavior script at random each time the current one is completed. If true, then select a random script. If false, then loop the selected script.")]
    [SerializeField]
    private bool randomScript = false;

    private string[] actions;
    private int currentAction = 0;
    private bool skipToEndIf = false;

    private ObjectTargeting targeting;
    private NPCAim aim;
    private NPCShoot shoot;
    private NPCMove move;
    private Health hp;
    private NPCTalk talk;

    private void Start(){
        targeting = GetComponent<ObjectTargeting>();
        aim = GetComponent<NPCAim>();
        shoot = GetComponent<NPCShoot>();
        move = GetComponent<NPCMove>();
        hp = GetComponent<Health>();
        talk = GetComponent<NPCTalk>();  

        hp.SetHealth(hp.GetMaxHealth());
        StartAI();
    }

    private void StartAI(){
        // If desired, select a random behavior
        if(randomScript){
            selectedBehavior = Random.Range(0, behaviors.Length);
        }

        // Split each line in the behavior script and store them in an array
        actions = behaviors[selectedBehavior].text.Split('\n');

        // Begin executing the actions
        StartCoroutine(ExecuteActions());
    }

    private void RandomScript(){
        // Select a random script
        selectedBehavior = Random.Range(0, behaviors.Length);

        // Split each line in the script and put them in an array
        actions = behaviors[selectedBehavior].text.Split('\n');
    }

    private bool ResolveBoolean(string[] boolean){
        // Ensure proper length
        //if(boolean.Length >= 3) return false;
        
        // Comparing health of the current target
        if(boolean[0].Contains("targethp")){
            // Set the values on the left and right sides of the comparison operator
            int targetHP = targeting.GetCurrentTarget().GetComponent<Health>().GetHealth();
            int compVal = 0;
            int.TryParse(boolean[2], out compVal);

            // Determine and perform the comparison operator
            if(boolean[1].Contains("<=")){
                return targetHP <= compVal;
            }else if(boolean[1].Contains(">=")){
                return targetHP >= compVal;
            }else if(boolean[1].Contains("!=")){
                return targetHP != compVal;
            }else if(boolean[1].Contains("<")){
                return targetHP < compVal;
            }else if(boolean[1].Contains(">")){
                return targetHP > compVal;
            }else if(boolean[1].Contains("=") || boolean[1].Contains("==")){
                return targetHP == compVal;
            }
        }
        // Comparing health of this NPC
        else if(boolean[0].Contains("npchp")){
            // Set the values on the left and right sides of the comparison operator
            int npcHP = hp.GetHealth();
            int compVal = 0;
            int.TryParse(boolean[2], out compVal);

            // Determine and perform the comparison operator
            if(boolean[1].Contains("<=")){
                return npcHP <= compVal;
            }else if(boolean[1].Contains(">=")){
                return npcHP >= compVal;
            }else if(boolean[1].Contains("!=")){
                return npcHP != compVal;
            }else if(boolean[1].Contains("<")){
                return npcHP < compVal;
            }else if(boolean[1].Contains(">")){
                return npcHP > compVal;
            }else if(boolean[1].Contains("=") || boolean[1].Contains("==")){
                return npcHP == compVal;
            }
        }
        // Comparing the distance to the current target 
        else if(boolean[0].Contains("distancetotarget") || boolean[0].Contains("disttotarget")){
            // Set the values on the left and right sides of the comparison operator
            float targetDist = targeting.DistanceToCurrentTarget();
            float compVal = 0f;
            float.TryParse(boolean[2], out compVal);

            // Determine and perform the comparison operator
            if(boolean[1].Contains("<=")){
                return targetDist <= compVal;
            }else if(boolean[1].Contains(">=")){
                return targetDist >= compVal;
            }else if(boolean[1].Contains("!=")){
                return targetDist != compVal;
            }else if(boolean[1].Contains("<")){
                return targetDist < compVal;
            }else if(boolean[1].Contains(">")){
                return targetDist > compVal;
            }else if(boolean[1].Contains("=") || boolean[1].Contains("==")){
                return targetDist == compVal;
            }
        }
        // Comparing the distance remaining on the current path
        else if(boolean[0].Contains("pathdistance") || boolean[0].Contains("pathdist")){
            // Set the values on the left and right sides of the comparison operator
            float targetDist = move.CalculatePathLength();
            float compVal = 0f;
            float.TryParse(boolean[2], out compVal);
            // Determine and perform the comparison operator
            if(boolean[1].Contains("<=")){
                return targetDist <= compVal;
            }else if(boolean[1].Contains(">=")){
                return targetDist >= compVal;
            }else if(boolean[1].Contains("!=")){
                return targetDist != compVal;
            }else if(boolean[1].Contains("<")){
                return targetDist < compVal;
            }else if(boolean[1].Contains(">")){
                return targetDist > compVal;
            }else if(boolean[1].Contains("=") || boolean[1].Contains("==")){
                return targetDist == compVal;
            }
        }
        // Comparing the distance to a given firepoint on the target
        else if(boolean[0].Contains("distancetofirepoint") || boolean[0].Contains("disttofirepoint")){
            // Get the index of the firepoint to compare
            int index = 0;
            int.TryParse(boolean[1], out index);
            // Set the values on the left and right sides of the comparison operator
            float targetDist = Vector3.Distance(shoot.GetFirePoint(index).position.WithY(0f), targeting.GetCurrentTarget().transform.position.WithY(0f));
            float compVal = 0f;
            float.TryParse(boolean[3], out compVal);

            // Determine and perform the comparison operator
            if(boolean[2].Contains("<=")){
                return targetDist <= compVal;
            }else if(boolean[2].Contains(">=")){
                return targetDist >= compVal;
            }else if(boolean[2].Contains("!=")){
                return targetDist != compVal;
            }else if(boolean[2].Contains("<")){
                return targetDist < compVal;
            }else if(boolean[2].Contains(">")){
                return targetDist > compVal;
            }else if(boolean[2].Contains("=") || boolean[2].Contains("==")){
                return targetDist == compVal;
            }
        }
        // Comparing amount of shots left in the current burst
        else if(boolean[0].Contains("shots")){
            // Get the amount of bullets we're comparing
            int shots = shoot.GetRemainingBullets();
            int compVal = 0;
            int.TryParse(boolean[2], out compVal);
            

            // Determine and perform the comparison operator
            if(boolean[1].Contains("<=")){
                return shots <= compVal;
            }else if(boolean[1].Contains(">=")){
                return shots >= compVal;
            }else if(boolean[1].Contains("!=")){
                return shots != compVal;
            }else if(boolean[1].Contains("<")){
                return shots < compVal;
            }else if(boolean[1].Contains(">")){
                return shots > compVal;
            }else if(boolean[1].Contains("=") || boolean[2].Contains("==")){
                return shots == compVal;
            }
        }
        // Is the NPC in a conversation
        else if(boolean[0].Contains("inconversation"))
            return talk.isInConversation();
        // Is the NPC not in a conversation
        else if(boolean[0].Contains("notinconversation"))
            return !talk.isInConversation();

        // Given boolean statement not supported
        // Skip to the endif
        if(debug) Debug.Log("Boolean statement /'" + actions[currentAction] + "/' not supported.");
        return false;
    }

    IEnumerator ExecuteActions(){
        // Perform actions until death
        while(hp.GetHealth() > 0){
            // yield return to satisfy need for all paths returning
            // Waits 0 seconds, thus doing nothing
            // DO NOT REMOVE
            yield return StartCoroutine(Wait(0f));

            // The current behavior script is finished executing
            if(currentAction >= actions.Length){
                // Return to first instruction
                currentAction = 0;
                // No longer skip to endif
                skipToEndIf = false;
                // Select a new random script if desired
                if(randomScript) RandomScript();
            }

            // Convert current action to lower and spit it by words, ignore empty
            string[] act = actions[currentAction].ToLower().Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            // Skip empty lines and comments
            if(act.Length == 0 || act[0] == "//"){
                currentAction++;
                continue;
            }

            // An if resulted in false
            // Skip all statements until an endif is found
            // Or until the end of the behavior script is reached
            if(skipToEndIf && !act[0].Contains("endif")){
                if(debug) Debug.Log(currentAction + ": SKIPPED " + actions[currentAction]);
                currentAction++;
                continue;
            }

            if(debug) Debug.Log(currentAction + ": " + actions[currentAction]);

            if(act[0].Contains("aim")){ // ** Aim actions **
                // Ensure proper length
                if(act.Length == 3){
                    // Set the idle type of the NPC
                    if(act[1].Contains("idletype")){
                        if(act[2].Contains("movementdirection") || act[2].Contains("movedir")){
                            aim.SetIdleType(NPCAim.IdleType.MovementDirection);
                        }else if(act[2].Contains("randomdirection") || act[2].Contains("randdir")){
                            aim.SetIdleType(NPCAim.IdleType.RandomDirection);
                        }else if(act[2].Contains("clockwise") || act[2].Contains("clock")){
                            aim.SetIdleType(NPCAim.IdleType.Clockwise);
                        }else if(act[2].Contains("counterclockwise") || act[2].Contains("cntrclock")){
                            aim.SetIdleType(NPCAim.IdleType.CounterClockwise);
                        }else if(act[2].Contains("lastdirection") || act[2].Contains("lastdir")){
                            aim.SetIdleType(NPCAim.IdleType.LastDirection);
                        }
                    }
                    // Set whether or not to track the NPC's target
                    // If not tracking then uses idle aim behavior
                    // Also stops an override direction
                    else if(act[1].Contains("track")){
                        if(act[2].Contains("true") || act[2].Contains("1"))
                            aim.AimAtTarget(true);
                        else if(act[2].Contains("false") || act[2].Contains("0"))
                            aim.AimAtTarget(false);
                    }
                } else if(act.Length == 5){
                    // Aim in a specified direction
                    if(act[1].Contains("direction") || act[1].Contains("dir")){
                        float x = 0f;
                        float y = 0f;
                        float z = 0f;
                        float.TryParse(act[2], out x);
                        float.TryParse(act[3], out y);
                        float.TryParse(act[4], out z);
                        aim.AimAtDirection(new Vector3(x, y, z));
                    }
                }
            }else if(act[0].Contains("move")){ // ** Move actions **
                // Ensure proper length
                if(act.Length == 3){
                    // Set the speed of directional movement
                    if(act[1].Contains("directionspeed") || act[1].Contains("dirspeed")){
                        float speed = 0f;
                        float.TryParse(act[2], out speed);
                        move.SetAuxMovementSpeed(speed);
                    }
                    // Set the speed of pathing movement
                    else if(act[1].Contains("pathingspeed") || act[1].Contains("pathspeed")){
                        float speed = 0f;
                        float.TryParse(act[2], out speed);
                        move.SetPathMovementSpeed(speed);
                    }
                    // Set whether or not to move along path or direction
                    // True for path, false for direction
                    else if(act[1].Contains("path")){
                        if(act[2].Contains("true") || act[2].Contains("1"))
                            move.SetTraversePath(true);
                        else if(act[2].Contains("false") || act[2].Contains("0"))
                            move.SetTraversePath(false);
                    }
                    // Set whether or not the NPC will stop or not
                    // True for stopping, false for moving
                    else if(act[1].Contains("stop")){
                        if(act[2].Contains("true") || act[2].Contains("1"))
                            move.SetIsStopped(true);
                        else if(act[2].Contains("false") || act[2].Contains("0"))
                            move.SetIsStopped(false);
                    }
                    // Set the direction of directional movement
                    else if(act[1].Contains("direction") || act[1].Contains("dir")){
                        if(act[2].Contains("forwardleft"))
                            move.SetMovement(NPCMove.Direction.forwardLeft);
                        else if(act[2].Contains("forwardright"))
                            move.SetMovement(NPCMove.Direction.forwardRight);
                        else if(act[2].Contains("backwardleft"))
                            move.SetMovement(NPCMove.Direction.backwardLeft);
                        else if(act[2].Contains("backwardright"))
                            move.SetMovement(NPCMove.Direction.backwardRight);
                        else if(act[2].Contains("forward"))
                            move.SetMovement(NPCMove.Direction.forward);
                        else if(act[2].Contains("backward"))
                            move.SetMovement(NPCMove.Direction.backward);
                        else if(act[2].Contains("left"))
                            move.SetMovement(NPCMove.Direction.left);
                        else if(act[2].Contains("right"))
                            move.SetMovement(NPCMove.Direction.right);
                    }
                    // Set the destination of the NPC
                    else if(act[1].Contains("destination") || act[1].Contains("dest")){
                        // Destination is current target
                        if(act[2].Contains("target")){
                            move.ChangeDestination();
                        }
                        // Destination is a target other than the current
                        else{
                            int target = 0;
                            int.TryParse(act[2], out target);
                            move.ChangeDestination(target);
                        }
                    }
                }
            }else if(act[0].Contains("shoot")){ // ** Shoot actions **
                // Ensure proper length
                if(act.Length == 2){
                    // Stop firing, even if midburst
                    if(act[1].Contains("stop")){
                        shoot.StopBurst();
                    }
                    // Reset the cooldowns for firing
                    else if(act[1].Contains("reset")){
                        shoot.ResetCooldowns();
                    }
                    // Shoot specified number of bullets
                    else{
                        int amnt = 0;
                        int.TryParse(act[1], out amnt);
                        shoot.FireBullets(amnt);
                    }
                }
                else if(act.Length == 3){
                    // Set a new burst cooldown
                    if(act[1].Contains("burstcooldown")){
                        float newCooldown = shoot.GetBurstCooldown();
                        float.TryParse(act[2], out newCooldown);
                        shoot.SetBurstCooldown(newCooldown);
                    }
                    // Set a new cooldown
                    else if(act[1].Contains("cooldown")){
                        float newCooldown = shoot.GetCooldown();
                        float.TryParse(act[2], out newCooldown);
                        shoot.SetCooldown(newCooldown);
                    }
                    // Shoot specified number of specified bullets
                    else{
                        int amnt = 0;
                        int bulletIndex = 0;
                        int.TryParse(act[1], out amnt);
                        int.TryParse(act[2], out bulletIndex);
                        shoot.FireBullets(amnt, bulletIndex);
                    }
                }
                // Shoot specified number of specified bullets from specified fire point
                else if(act.Length == 4){
                    int amnt = 0;
                    int bulletIndex = 0;
                    int fpIndex = 0;
                    int.TryParse(act[1], out amnt);
                    int.TryParse(act[2], out bulletIndex);
                    int.TryParse(act[3], out fpIndex);
                    shoot.FireBullets(amnt, bulletIndex, fpIndex);
                }
            }else if(act[0].Contains("target")){ // ** Target actions **
                // Ensure proper length
                if(act.Length == 2){
                    // Change target to the first index
                    if(act[1].Contains("first")){
                        targeting.ChangeTarget(0);
                    }
                    // Change target to the last index
                    else if(act[1].Contains("last")){
                        targeting.ChangeTarget(targeting.GetNumberOfTargets() - 1);
                    }
                }else if(act.Length == 3){
                    // Change target according to an index
                    if(act[1].Contains("index")){
                        int index = 0;
                        int.TryParse(act[2], out index);
                        targeting.ChangeTarget(index);
                    }
                    // Add a new target according to its name
                    else if(act[1].Contains("addname")){
                        targeting.AddNewTarget(GameObject.Find(act[2]));
                    }
                }
            }else if(act[0].Contains("behavior")){ // ** Behavior actions **
                // Ensure proper length
                if(act.Length == 3){
                    if(act[1].Contains("change")){
                        if(act[2].Contains("random") || act[2].Contains("rand")){
                            // Return to first instruction
                            currentAction = 0;
                            // No longer skip to endif
                            skipToEndIf = false;
                            // Select random script
                            RandomScript();
                        }
                        int bIndex = 0;
                        int.TryParse(act[2], out bIndex);
                        ChangeBehaviorScript(bIndex);
                    }
                }
            }else if(act[0].Contains("talk")){ // ** Speech actions **
                // Say the given text
                if(act[1].Contains("say")){
                    string[] line = actions[currentAction].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    string speech = "";
                    for(int i = 2; i < line.Length; i++){
                        // Add each word to the speech
                        speech += line[i];
                        // Add spaces between all but the last word
                        if(i < line.Length - 1)
                            speech += " ";
                    }
                    talk.Say(speech);
                }
                // Ensure proper length
                else if(act.Length == 3){
                    // Set whether or not this NPC can talk
                    if(act[1].Contains("cantalk"))
                    {
                        if(act[2].Contains("true") || act[2].Contains("1"))
                            talk.canTalk = true;
                        else if(act[2].Contains("false") || act[2].Contains("0"))
                            talk.canTalk = false;
                    }
                    // Set whether or not this NPC can converse
                    else if(act[1].Contains("canconverse")){
                        if(act[2].Contains("true") || act[2].Contains("1"))
                            talk.canConverse = true;
                        else if(act[2].Contains("false") || act[2].Contains("0"))
                            talk.canConverse = false;
                    }
                    // Set the max distance for conversations
                    else if(act[1].Contains("distance") || act[1].Contains("dist")){
                        int talkDist = 0;
                        int.TryParse(act[2], out talkDist);
                        talk.maxConversationDistance = talkDist;
                    }
                }
                // Ensure proper length
                else if(act.Length == 2){
                    // Silence the NPC
                    if(act[1].Contains("silence")){
                        talk.Say("");
                    }
                }
            }else if(act[0].Contains("endif")){ // ** EndIF action **
                skipToEndIf = false;
            }else if(act[0].Contains("if")){ // ** If action ** 
                // Set whether or not to skip to the endif based upon the given boolean statment
                if(act.Length == 4){
                    string[] boolStatement = {act[1], act[2], act[3]};
                    // Whether or not to skip is the opposite of the boolean's value
                    skipToEndIf = !ResolveBoolean(boolStatement);
                }
                else if(act.Length == 5){
                    string[] boolStatement = {act[1], act[2], act[3], act[4]};
                    // Whether or not to skip is the opposite of the boolean's value
                    skipToEndIf = !ResolveBoolean(boolStatement);
                }
            }else if(act[0].Contains("wait")){ // ** Wait action **
                // Wait a given amount of time before performing the next action
                if(act.Length == 2){
                    float time = 0f;
                    float.TryParse(act[1], out time);
                    yield return StartCoroutine(Wait(time));
                }
                // Wait until the given boolean statement is true
                else if(act.Length == 4){
                    string[] boolStatement = {act[1], act[2], act[3]};
                    // Continue without moving to the next action if boolean statement is false
                    if(!ResolveBoolean(boolStatement))
                        continue;
                }
            }

            // Move to next action
            currentAction++;
        }
    }

    IEnumerator Wait(float time){
		yield return new WaitForSeconds (time);
	}

     public void ChangeBehaviorScript(int i){
         // Ensure given index is within bounds
         if(i >= behaviors.Length) return;

        // Set the new behavior
         selectedBehavior = i;
         // Return to first instruction
        currentAction = 0;
        // No longer skip to endif
        skipToEndIf = false;
        // Split each line in the script and put them in an array
        actions = behaviors[selectedBehavior].text.Split('\n');
     }
}

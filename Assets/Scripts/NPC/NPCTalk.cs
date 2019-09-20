using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{
    public bool debug = false;
    [Tooltip("Can this NPC talk or not. Setting to false will disable all talking behaviour.")]
    public bool canTalk = true;
    [Tooltip("Does this NPC have conversations or simply say greetings, goodbyes, pain explamations and last words. When false this NPC will not scroll through conversation dialouge.")]
    public bool canConverse = true;
    [Tooltip("The max distance to their conversation target that this NPC must be to begin greeting/conversing.")]
    public float maxConversationDistance = 2f;
    [Tooltip("The speed at which characters will appear when the NPC speaks. Measures time in seconds between each letter. Smaller values mean faster text speeds. Zero or less specifies text appearing instantly.")]
    public float textSpeed = 0.025f;
    
    [Tooltip("The tag of the game object this NPC will converse with. Sets the conversation target if not target is specified.")]
    [SerializeField]
    private string conversationTargetTag = "Player";
    [Tooltip("The game object this NPC will converse with.")]
    [SerializeField]
    private Transform conversationTarget;
    [SerializeField]
    private TextAsset[] conversations;
    [Tooltip("The current conversation this NPC will have.")]
    [SerializeField]
    private int currentConversation = 0;
    [Tooltip("Will this NPC select its current conversation at random. If false then conversations will be selected sequentially.")]
    [SerializeField]
    private bool chooseRandomConversation = false;

    private bool inConversation = false;
    private bool isListener = false;
    private bool isPrinting = false;
    private bool justSayIt = false;
    private string currSpeech = ""; // What the NPC is currently saying

    private string[][] conversationLines;
    private int currConvLine = 0;
    private Text speechText;
    private Image speechBubble;

    private void Start(){
        // If there is a text component in the children of this game object
        if(GetComponentInChildren<Text>()){
            // Find speech text
            speechText = GetComponentInChildren<Text>();
        } else { // Couldn't find text and speech bubble, so can't talk
            canTalk = false;
            if(debug) Debug.Log("Speech text not found on: " + gameObject.name);
        }

        // If there is a text component in the children of this game object
        if(GetComponentInChildren<Image>()){
            speechBubble = GetComponentInChildren<Image>();
        } else{
            if(debug) Debug.Log("Speech bubble not found on: " + gameObject.name);
        }

        if(!conversationTarget && GameObject.FindWithTag(conversationTargetTag)){
            conversationTarget = GameObject.FindWithTag(conversationTargetTag).transform;
        }

        //Split conversations into arrays, using a 2D array
        if(conversations.Length > 0){
            conversationLines = new string[conversations.Length][];
            for(int i = 0; i < conversationLines.Length; i++){
                conversationLines[i] = conversations[i].text.Split('\n');
            }
        }
    }

    private void Update(){
        // Player's interact ability won't be instantly available
        // Check each frame until it is
        // Then set up a listener method for starting a conversation
        if(canConverse && InteractAbility.interact && !isListener && !isPrinting){
            isListener = true;        
            InteractAbility.interact.onInteractPressed += Converse;
        }

        // Player is too far, end conversation
        if(inConversation && Vector3.Distance(transform.position, conversationTarget.position) > maxConversationDistance){
            if(debug) Debug.Log(gameObject.name + "'s conversation target moved away. Ending conversation.");
            inConversation = false;
            currConvLine = 0;
            speechText.text = "";

        }

        // If the NPC is mid conversation and is set to no longer converse
        // then end conversation
        if(inConversation && !canConverse){
            if(debug) Debug.Log(gameObject.name + " can't converse anymore. Ending conversation early.");
            inConversation = false;
            currConvLine = 0;
            speechText.text = "";
        }

        // Speech bubble should only be visible when NPC has speech text
        if(speechBubble && speechText.text == "")
            speechBubble.enabled = false;
        else if(speechBubble)
            speechBubble.enabled = true;
    }

    public void Converse(){
        // Remove this method as a listener
        if(this == null){
            InteractAbility.interact.onInteractPressed -= Converse;
            return;
        }
        // If the NPC can't talk or conversation target is too far, then don't start conversation
        if(!canConverse || Vector3.Distance(transform.position, conversationTarget.position) > maxConversationDistance){
            return;
        }

        // If this function is called while printing, then just display the text
        // For the impatient people
        if(isPrinting){
            if(debug) Debug.Log("Just saying it.");
            justSayIt = true;
            return;
        }

        // Cycle conversation lines
        if(currConvLine < conversationLines[currentConversation].Length && !isPrinting){
            if(debug) Debug.Log(gameObject.name + " Converses: " + conversationLines[currentConversation][currConvLine]);
            inConversation = true;
            //speechText.text = conversationLines[currentConversation][currConvLine];
            StartCoroutine(PrintSpeech(conversationLines[currentConversation][currConvLine]));
            currConvLine++;
        }
        // End conversation
        else if(currConvLine >= conversationLines[currentConversation].Length && !isPrinting){
            if(debug) Debug.Log("Ending Conversation");
            inConversation = false;
            currConvLine = 0;
            speechText.text = "";
            
            // Determine which conversation to use next
            if(chooseRandomConversation){ // Random conversation
                currentConversation = Random.Range(0, conversations.Length);
            }else{ // Cycle conversations
                currentConversation++;
                currentConversation %= conversations.Length;
            }
        }
    }

    public void Say(string speech){
        // Can't say other things if in a conversation
        if(inConversation){
            if(debug) Debug.Log(gameObject.name + " can't say something else because they are in a conversation.");
            return;
        }

        // Don't say the same thing twice
        if(speech == currSpeech){
            if(debug) Debug.Log(gameObject.name + " won't say the same thing twice.");
            return;
        }

        // Can't talk
        if(!canTalk){
            speechText.text = "";
            return;
        }

        
        //speechText.text = speech;
        if(isPrinting) return;
        StartCoroutine(PrintSpeech(speech));
    }

    public bool isInConversation(){
        return inConversation;
    }

    IEnumerator PrintSpeech(string speech){
        isPrinting = true;
        // Set what the NPC is currently trying to say
        currSpeech = speech;
        // Don't bother saying empty strings or strings of all spaces
        if(string.IsNullOrWhiteSpace(speech)){
            if(debug) Debug.Log(gameObject.name + " is trying to say an empty string.");
            speechText.text = "";
            isPrinting = false;
            yield break;
        }

        if(debug) Debug.Log(gameObject.name + " says: " + speech);

        // Instantly say text if text speed is 0 or less
        if(textSpeed <= 0f){
            isPrinting = false;
            speechText.text = speech;
            yield break;
        }
        // Say the first character
        speechText.text = "" + speech[0];

        // Next character that will appear
        int currChar = 1;

        // Until all characters are printed
        while(currChar < speech.Length){
            // Wait before printing the next character
            yield return new WaitForSeconds(textSpeed);

            // If text suddenly clears then this npc is being silenced
            // So stop printing text and exit
            if(speechText.text == ""){
                isPrinting = false;
                yield break;
            }

            // Just say it without printing
            if(justSayIt){
                justSayIt = false;
                isPrinting = false;
                speechText.text = speech;
                yield break;
            }

            // Add the new character to speechText and increment to next character
            speechText.text += speech[currChar];
            currChar++;
        }

        justSayIt = false;
        isPrinting = false;
        yield return null;
    }
}

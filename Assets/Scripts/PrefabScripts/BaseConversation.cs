using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//Conversation I.D.:
//Character I.D, Story Progression I.D., Conversation Sub-Index Number

[System.Serializable]
public struct Response
{

    [TextArea(2, 5)]

    public string dialogue;
    public BaseCharacter character;

}
[System.Serializable]
public struct Choice
{

    [TextArea(2, 5)]

    public string dialogue;
    public Response[] responses;
}
[System.Serializable]
public struct Line
{
    public BaseCharacter character;

    [TextArea(2, 5)]
    
    public string dialogue;
    public string nameText;
    public Choice[] choices;
}




[CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation")]
public class BaseConversation : ScriptableObject
{
    public BaseCharacter[] presentCharacters;
    public Line[] lines;

    public int charNum;
    public int storyIDNum;
    public int convoNum;
}

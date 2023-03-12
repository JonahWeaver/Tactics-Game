using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    public BaseConversation conversation;
    public BaseConversation[] conversations;
    
    public int charMax;
    public int storyMax;
    public int convoMax;

    [SerializeField]
    public BaseConversation[,,] allConversations;

    public BaseCharacter[] speakers;
    public SpeakerUI speakerUI;

    private int activeLineIndex = 0;
    public static InteractPreview ip;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        allConversations = new BaseConversation[charMax, storyMax, convoMax];
        foreach (BaseConversation con in conversations)
        {
            for (int x = 0; x < charMax; x++)
            {
                for (int y = 0; y < storyMax; y++)
                {
                    for (int z = 0; z < convoMax; z++)
                    {
                        if (x == con.charNum - 1 && y == con.storyIDNum - 1 && z == con.convoNum - 1)
                        {
                            allConversations[x, y, z] = con;
                            x = charMax;
                            y = storyMax;
                            z = convoMax;
                        }
                    }
                }
            }

        }
        ip = player.GetComponent<InteractPreview>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ip.identity != null)
        {
            conversation = allConversations[ip.identity.characterID - 1, 0, 0];
            speakers = conversation.presentCharacters;
            if (Input.GetKeyDown(KeyCode.P))
            {
                AdvanceConversation();
            }
        }
        else if(activeLineIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                AdvanceConversation();
            }
        }
        else
        {
            speakers = null;
            conversation = null;
        }
    }

    public void RefreshArray()
    {
        BaseConversation[,,] newConversations = new BaseConversation[charMax, storyMax, convoMax];



    }

    void AdvanceConversation()
    {
        if(activeLineIndex< conversation.lines.Length)
        {
            ip.col.enabled= false;
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            
            activeLineIndex = 0;
            speakerUI.Hide();
            ip.col.enabled = true;
        }
                
    }

    void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        BaseCharacter character = line.character;

        SetDialogue(speakerUI, character.name, line.dialogue);
    }

    void SetDialogue(SpeakerUI activeSpeakerUI, string nameText, string dialogue)
    {
        activeSpeakerUI.Dialogue = dialogue;
        activeSpeakerUI.FullName = nameText;
        activeSpeakerUI.Show();
    }
}

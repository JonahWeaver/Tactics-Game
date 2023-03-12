using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConversationController : Controller
{
    public BaseConversation conversation;
    public BaseConversation[] conversations;

    public Choice choice;
    public Choice[] choices;

    public int charMax;
    public int storyMax;
    public int convoMax;

    [SerializeField]
    public BaseConversation[,,] allConversations;

    public BaseCharacter[] speakers;
    public SpeakerUI speakerUI;
    public GameObject panel;

    private int activeLineIndex = 0;
    private int activeQuestionIndex = 0;
    public static InteractPreview ip;

    public GameObject player;
    public float intDelay = .1f;
    public float intDelayRef;

    public bool conversationStarted;
    public bool questionsStarted;
    public bool chosen;
    public bool resume;

    public Button button;

    public int buttonSpacing = 100;

    public float mVert;
    public float mHor;

    public bool interact;

    public override void ReadInput(InputData data)
    {
        RegisterInput(data);

        newInput = true;
    }
 
    protected override void Start()
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
        intDelayRef = intDelay;
        chosen = true;
        resume = true;
        panel.SetActive(false);
    }

    void Update()
    {
        if (ip.identity != null)
        {
            conversation = allConversations[ip.identity.characterID - 1, 0, 0];
            speakers = conversation.presentCharacters;
            StartConversation();
            if (interact && intDelay < 0f)
            {
                AdvanceLine();
                intDelay = intDelayRef;
            }
            else
            {
                intDelay -= Time.deltaTime;
            }
        }
        else if (activeLineIndex != 0)
        {
            if (interact)
            {
                AdvanceLine();
                Debug.Log(activeLineIndex);
            }
        }
        else
        {
            EndConversation();
        }
        
        

        newInput = false;
        Reset(inputData);
        RegisterInput(inputData);
    }


    void Initialize()
    {
        conversationStarted = true;
        activeLineIndex = 0;
        speakerUI.Speaker = conversation.lines[activeLineIndex].character;
    }

    void AdvanceLine()
    {
        if(conversation!=null)
        {
            if (!conversationStarted)
            {
                Initialize();
            }
            if (chosen)
            {
                if (activeLineIndex < conversation.lines.Length)
                {
                    DisplayLine();
                }
                else
                {
                    activeLineIndex = 0;
                    ip.col.enabled = true;
                    EndConversation();
                }
            }
            else if(!questionsStarted)
            {
                CreateChoices(button, conversation.lines[activeLineIndex-1].choices );
            }
            else if(!resume)
            {
                if (activeQuestionIndex < choice.responses.Length)
                {
                    DisplayLineForQuestion();
                }
                else
                {
                    EndQuestions();
                    activeQuestionIndex = 0;
                }
            }
        }
    }

    void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        BaseCharacter character = line.character;

        if (chosen)
        {
            SetDialogue(speakerUI, character.name, line.dialogue);
            activeLineIndex += 1;
        }
        CheckForChoices(line);
    }


    void DisplayLineForQuestion()
    {
        Response response = choice.responses[activeQuestionIndex];
        BaseCharacter character = response.character;
        SetDialogue(speakerUI, character.name, response.dialogue);
        activeQuestionIndex += 1;
    }

    void SetDialogue(SpeakerUI activeSpeakerUI, string nameText, string dialogue)
    {
        activeSpeakerUI.Dialogue = dialogue;
        activeSpeakerUI.FullName = nameText;
        activeSpeakerUI.Show();
    }
    void StartConversation()
    {
        if (activeLineIndex==0 &&InputManager.ins.controller==this)
        {
            DisplayLine();
            conversationStarted = true;
        }
    }

    void EndConversation()
    {
        conversation = null;
        conversationStarted = false;
        speakers = null;
        speakerUI.Hide();
        cam.GetComponent<OverworldCameraMovement>().enabled = true;
        switcher.Switch(switcher.controllers[0]);
    }

    void EndChoices()
    {
        RemoveChoices(button);
        DisplayLineForQuestion();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void EndQuestions()
    {
        resume = true;
        chosen = true;
        questionsStarted = false;
        DisplayLine();
    }

    void CheckForChoices(Line line)
    {
        if(line.choices.Length>0)
        {
            chosen = false;
        }
        else
        {
            chosen = true;
        }
    }

    public void Choose()
    {
        Button chosenOption = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        choice = choices[chosenOption.GetComponent<ButtonIdentity>().Identity];

        resume = false;
        EndChoices();
    }
    
    void CreateChoices(Button buttonTemplate, Choice[] choices)
    {
        buttonSpacing = 100;
        panel.SetActive(true);
        buttonTemplate.gameObject.SetActive(true);
        this.choices = choices;

        for (int index=0; index < choices.Length; index++)
        {
            Button button = Instantiate(buttonTemplate);
            button.transform.SetParent(buttonTemplate.transform.parent);
            button.transform.localScale = Vector3.one;
            button.transform.localPosition = new Vector3(0, index * buttonSpacing, 0);
            button.name = "Choice " + (index + 1);
            button.gameObject.SetActive(true);
            button.GetComponent<ButtonIdentity>().Identity = index;
            button.GetComponentInChildren<Text>().text = choices[index].dialogue;
        }

        buttonTemplate.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        questionsStarted = true;

    }

    void RemoveChoices(Button buttonTemplate)
    {
        Transform parent = buttonTemplate.transform.parent;
        foreach (Transform t in parent)
        {
            if(t!=button.transform)
            {
                Destroy(t.gameObject);
                
            }
        }
    }

    void RegisterInput(InputData data)
    {
        if(data.axes!=null)
        {
            inputData = data;

            mVert = data.axes[0];
            mHor = data.axes[1];

            interact = data.buttons[2];
        }
    }
}

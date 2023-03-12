using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerUI : MonoBehaviour
{
    //public Image portrait;
    public Text fullName;
    public Text dialogue;

    private BaseCharacter currentSpeaker;

    public BaseCharacter Speaker
    {
        get { return currentSpeaker; }
        set
        {
            currentSpeaker = value;
            //portrait.sprite = speaker.portrait;
        }
    }

    public string Dialogue
    {
        set { dialogue.text = value; }
    }
    public string FullName
    {
        set { fullName.text = value; }
    }

    public bool HasSpeaker()
    {
        return currentSpeaker != null;
    }

    public bool SpeakerIs(BaseCharacter character)
    {
        return currentSpeaker == character;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}

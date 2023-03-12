using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConversationController))]
public class ConversationDirectory : Editor
{
    public static BaseConversation[,,] AllConversations { get; set; }

    [SerializeField]
    public BaseConversation[,,] allConversations;

    int highX = 0;
    int highY = 0;
    int highZ = 0;
    Object[] conversations;
    BaseConversation[] convo;

    void Process()
    {
        conversations = Resources.LoadAll("AllConversations", typeof(BaseConversation));
        highX = 0;
        highY = 0;
        highZ = 0;

        foreach (BaseConversation conversation in conversations)
        {
            highX = FindHigher(conversation.charNum, highX);
            highY = FindHigher(conversation.storyIDNum, highY);
            highZ = FindHigher(conversation.convoNum, highZ);
        }

        int charNum=highX;
        int storyIDNum=highY;
        int convoNum=highZ;

        convo = new BaseConversation[conversations.Length];
        int i = 0;
        foreach(BaseConversation conversation1 in conversations)
        {
            if(conversation1!=null)
            {
                convo[i] = conversation1;

            }
            i++;
        }

        allConversations = new BaseConversation[charNum, storyIDNum, convoNum];

        foreach (BaseConversation conversation in conversations)
        {
            for (int x = 0; x < charNum; x++)
            {
                for (int y = 0; y < storyIDNum; y++)
                {
                    for (int z = 0; z < convoNum; z++)
                    {
                        if (x == conversation.charNum-1 && y == conversation.storyIDNum-1 && z == conversation.convoNum-1)
                        {
                            allConversations[x, y, z] = conversation;
                            x = charNum;
                            y = storyIDNum;
                            z = convoNum;
                        }
                    }
                }
            }

        }

        AllConversations = allConversations;
    }

    int FindHigher(int num, int king)
    {
        if (num>king)
        {
            return num;
        }
        else
        {
            return king;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Process();
        ConversationController cc = (ConversationController)target;
        if (GUILayout.Button("Refresh Arrays"))
        {
            cc.allConversations = new BaseConversation[highX,highY,highZ];
            cc.charMax = highX;
            cc.storyMax = highY;
            cc.convoMax = highZ;
            cc.allConversations = allConversations;
            cc.conversations = convo;
            Debug.Log(cc.allConversations[0, 0, 0]);
        }
    }
}

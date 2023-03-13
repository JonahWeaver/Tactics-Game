using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
//needs to be integrated with prefeb generation into some master manager or something in the future
public class ConversationID : MonoBehaviour
{
    void UpdateIDs()
    {
        Object[] conversations = Resources.LoadAll("AllConversations", typeof(BaseConversation));
        foreach (BaseConversation conversation in conversations)
        {
            string newName = conversation.charNum + "." + conversation.storyIDNum + "." + conversation.convoNum + ".";
            string assetPath = AssetDatabase.GetAssetPath(conversation.GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, newName);
            AssetDatabase.SaveAssets();
        }
    }
}

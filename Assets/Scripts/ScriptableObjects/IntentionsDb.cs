using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "IntentionsDb", menuName = "Olympus/Intentions Db")]
public class IntentionsDb : GenericData<IntentionsDb>
{
    [System.Serializable]
    public class IntentionInfo
    {
        public string clientID;
        public Intention intentionType;
        public Sprite icon;
    }

    public IntentionInfo[] intentions;



    public IntentionInfo FindById(string clientId)
    {
        for (int i = 0; i < intentions.Length; i++)
        {
            IntentionInfo intention = intentions[i];
            if (intention.clientID == clientId)
            {
                return intention;
            }
        }

        return null;
    }

    public IntentionInfo FindByType(Intention intentionType)
    {
        for (int i = 0; i < intentions.Length; i++)
        {
            IntentionInfo intention = intentions[i];
            if (intention.intentionType == intentionType)
            {
                return intention;
            }
        }

        return null;
    }
}
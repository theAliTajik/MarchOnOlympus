using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemiesDb;



[CreateAssetMenu(fileName = "Mechanics Db", menuName = "Olympus/Mechanics Db")]
public class MechanicsDb : GenericData<MechanicsDb>
{
    [System.Serializable]
    public class MechanicInfo
    {
        public string clientID;
        public MechanicType type;
        public Sprite icon;
    }


    [ContextMenu("Populate")]
    void populate()
    {
        int count = (int)MechanicType.IMPROVISE;
        allMechanics = new MechanicInfo[count];
        for (int i = 0; i < count; i++)
        {
            MechanicInfo info = new MechanicInfo();
            info.type = (MechanicType)i;
            info.clientID = info.type.ToString().ToLower();

            allMechanics[i] = info;
        }
    }


    public MechanicInfo[] allMechanics;


    public MechanicInfo FindById(string clientId)
    {
        for (int i = 0; i < allMechanics.Length; i++)
        {
            MechanicInfo mechanic = allMechanics[i];
            if (mechanic.clientID == clientId)
            {
                return mechanic;
            }
        }

        return null;
    }

    public MechanicInfo FindByType(MechanicType type)
    {
        for (int i = 0; i < allMechanics.Length; i++)
        {
            MechanicInfo mechanic = allMechanics[i];
            if (mechanic.type == type)
            {
                return mechanic;
            }
        }

        return null;
    }
}
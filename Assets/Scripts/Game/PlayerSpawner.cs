
using UnityEditor;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform m_spawnPoint;
    
    public Fighter SpawnPlayer()
    {
        Fighter prefab = GetPlayerPrefab();
        
        Fighter player = Instantiate(prefab, m_spawnPoint);

        if (player == null)
        {
            CustomDebug.LogError("Prefab couldn't be spawned.", Categories.Fighters.Player.Root);
        }
        
        // SetPlayerPosition(player);
        
        return player;
    }

    private void SetPlayerPosition(Fighter player)
    {
        if (m_spawnPoint != null)
        {
            player.transform.position = m_spawnPoint.position - player.GetRootPosition();
            return;
        }

        CustomDebug.LogError("Spawn point was null", Categories.Fighters.Player.Root);
    }

    private Fighter GetPlayerPrefab()
    {
        string id = GameSessionParams.CharacterId;
        
        if (string.IsNullOrEmpty(id))
        {
            CustomDebug.LogError($"char Id in session params was null", Categories.Fighters.Player.Root);
        }
        
        CharacterInfo info = CharactersDb.Instance.FindById(id);
        
        if (info.Prefab == null)
        {
            CustomDebug.LogError("Player prefab not found", Categories.Fighters.Player.Root);
        }
        
        return info.Prefab;
    }
}

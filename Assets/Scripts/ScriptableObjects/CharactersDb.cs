using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public struct CharacterInfo
{
    public string Id;
    public string Name;
    public Fighter Prefab;
    public Sprite Icon;
    public Sprite HeroImage;
    public Sprite NameImage;
}

[CreateAssetMenu(fileName = "CharactersDb", menuName = "Olympus/Characters Db")]
public class CharactersDb : GenericData<CharactersDb>, IEnumerable<CharacterInfo>
{
    [SerializeField] private List<CharacterInfo> characters = new List<CharacterInfo>();
    
    public IEnumerator<CharacterInfo> GetEnumerator()
    {
        return characters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public CharacterInfo FindById(string id)
    {
        return characters.Find(x => x.Id == id);
    }
}

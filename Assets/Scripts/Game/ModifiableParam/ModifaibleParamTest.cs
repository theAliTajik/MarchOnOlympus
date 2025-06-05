
using System.Collections;
using Game.ModifiableParam;
using UnityEngine;

public class ModifaibleParamTest : MonoBehaviour
{
    public BaseCardData TestCardData;

    private void Start()
    {
        ModifiableParam<int> param = new ModifiableParam<int>(1);
        Debug.Log($"param energy: {param.Value}");
        IParamModifier<int> energySetter = new SetValueModifier<int>(10);
        param.AddModifier(energySetter);
        Debug.Log($"param energy: {param.Value}");
        StartCoroutine(testLoad(param));

    }

    private IEnumerator testLoad(ModifiableParam<int> param)
    {
        SaveToJson(param);
        yield return new WaitForSeconds(1);
        ModifiableParam<int> loadedParam = LoadFromJson();
        Debug.Log(loadedParam.Value);
    }

    private string m_path = "/Test/modifyTest.text";
    private void SaveToJson(ModifiableParam<int> param)
    {
        m_path = Application.persistentDataPath + m_path;
        JsonHelper.SaveAdvanced(param, m_path);
    }

    private ModifiableParam<int> LoadFromJson()
    {
        ModifiableParam<int> param = JsonHelper.LoadAdvanced<ModifiableParam<int>>(m_path);
        return param;
    }
}

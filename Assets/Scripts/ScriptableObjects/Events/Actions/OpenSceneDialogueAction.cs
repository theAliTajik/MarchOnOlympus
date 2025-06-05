
using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "OpenSceneAction", menuName = "Events/DialogueActions/OpenSceneAction")]
public class OpenSceneDialogueAction : DialogueAction
{
    [SerializeField] private Scenes scene;
    
    public override void Execute(DialogueContex context)
    {
        base.Execute(context);
        SceneController.Instance.LoadScene(scene);
    }
}

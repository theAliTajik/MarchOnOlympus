using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyIntentionTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text decText;

    public void SetDescription(string text)
    {
        decText.text = text;
    }

}

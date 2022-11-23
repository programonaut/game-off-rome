using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Card", menuName = "Card", order = 0)]
public class CardObject : ScriptableObject {

    public string cardName;
    [TextArea] public string cardDescription;
    public Sprite cardBackground;
    public Sprite cardFrame;

    [Title("Card Action")]
    [InlineEditor(InlineEditorObjectFieldModes.Foldout)] public Action action;
}

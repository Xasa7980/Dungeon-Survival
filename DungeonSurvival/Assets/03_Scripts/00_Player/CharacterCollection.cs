using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[DisallowMultipleComponent]
public class CharacterCollection : MonoBehaviour
{
    private void Start()
    {
        Destroy(this);
    }

    enum Category
    {
        Head,
        Torso,
        Hips,
        Legs
    }


    [SerializeField, HideInInspector] Category category = Category.Head;

    [SerializeField, ShowIf("@equipmentCategory == Category.Head && onBody")] CharacterPart[] head;
    [SerializeField, ShowIf("@equipmentCategory == Category.Torso && onBody")] CharacterPart[] torso;
    [SerializeField, ShowIf("@equipmentCategory == Category.Hips && onBody")] CharacterPart[] hips;
    [SerializeField, ShowIf("@equipmentCategory == Category.Legs && onBody")] CharacterPart[] legs;

    [SerializeField, HideInInspector] bool onBody = true;

    [SerializeField, ShowIf("@equipmentCategory == Category.Head && !onBody")] CharacterPart[] headElements;
    [SerializeField, ShowIf("@equipmentCategory == Category.Torso && !onBody")] CharacterPart[] torsoElements;
    [SerializeField, ShowIf("@equipmentCategory == Category.Hips && !onBody")] CharacterPart[] hipsElements;
    [SerializeField, ShowIf("@equipmentCategory == Category.Legs && !onBody")] CharacterPart[] legsElements;

    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar")]
    [Button("Head")]
    void SetHead() => category = Category.Head;
    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar")]
    [Button("Torso")]
    void SetTorso() => category = Category.Torso;
    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar")]
    [Button("Hips")]
    void SetHips() => category = Category.Hips;
    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar")]
    [Button("Legs")]
    void SetLegs() => category = Category.Legs;

    [PropertyOrder(-1)]
    [HorizontalGroup("Layer")]
    [Button("Body")]
    void SetBody() => onBody = true;

    [PropertyOrder(-1)]
    [HorizontalGroup("Layer")]
    [Button("Attachments")]
    void SetElements() => onBody = false;

    [System.Serializable]
    class CharacterPart
    {
        [FoldoutGroup("@name")]
        [PropertyOrder(-2)]
        [SerializeField] string name;

        [SerializeField, HideInInspector] bool disabled;

        //[FoldoutGroup("@name")]
        [PropertyOrder(-1)]
        [Button("@disabled?\"Enable\":\"Disable\"", ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        public void Toggle()
        {
            disabled = !disabled;
            if (currentPart) currentPart.SetActive(!disabled);
        }

        [FoldoutGroup("@name")]
        [ListDrawerSettings(IsReadOnly = true)]
        [SerializeField] List<GameObject> parts;

        [FoldoutGroup("@name")]
        [SerializeField, ReadOnly] GameObject currentPart;
        [SerializeField, HideInInspector] int currentPartIndex = 0;

        [HideIf("disabled"), Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        void Previous()
        {
            if (disabled) return;

            if (currentPart) currentPart.SetActive(false);

            if (currentPartIndex > 0)
                currentPartIndex--;
            else
                currentPartIndex = parts.Count - 1;

            currentPart = parts[currentPartIndex];
            currentPart.SetActive(true);
        }

        [HideIf("disabled"), Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        void Next()
        {
            if (disabled) return;

            if(currentPart)currentPart.SetActive(false);

            if (currentPartIndex < parts.Count - 1)
                currentPartIndex++;
            else
                currentPartIndex = 0;

            currentPart = parts[currentPartIndex];
            currentPart.SetActive(true);
        }
    }
}

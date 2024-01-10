using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Dungeon Survival/Enemy/MonsterData")]
public class MonstersDataSO : ScriptableObject
{
    private enum MonsterCategory
    {
        Undeath,
        Beast,
        Humanoid,
        Spirit
    }
    public enum MonsterRank
    {
        Common,
        Uncommon,
        Rare,
        Elite,
        Boss,
        EliteBoss
    }
    
    MonsterCategory monsterCategory = MonsterCategory.Undeath;
    public MonsterRank monsterRank;
    
    public string monsterName;
    public AnimationClipContainerSO animationClipContainerSO;
    public CombatBehaviourSO combatBehaviourSO;



    #region EditorTools

    [PropertyOrder(-2)]
    [HorizontalGroup("MonsterCategoryBar")]
    [Button("Undeath")]
    private void SetCategory_Undeath() => monsterCategory = MonsterCategory.Undeath;

    [PropertyOrder(-2)]
    [HorizontalGroup("MonsterCategoryBar")]
    [Button("Beast")]
    private void SetCategory_Beast() => monsterCategory = MonsterCategory.Beast;

    [PropertyOrder(-2)]
    [HorizontalGroup("MonsterCategoryBar")]
    [Button("Humanoid")]
    private void SetCategory_Humanoid ( ) => monsterCategory = MonsterCategory.Humanoid;

    [PropertyOrder(-2)]
    [HorizontalGroup("MonsterCategoryBar")]
    [Button("Spirit")]
    private void SetCategory_Spirit ( ) => monsterCategory = MonsterCategory.Spirit;

    #endregion
    [SerializeField, ShowIf("@monsterCategory == MonsterCategory.Undeath")] private MonsterWeaponClass[] undeath; 
    [SerializeField, ShowIf("@monsterCategory == MonsterCategory.Beast")] private MonsterWeaponClass[] beast;
    [SerializeField, ShowIf("@monsterCategory == MonsterCategory.Humanoid")] private MonsterWeaponClass[] humanoid;
    [SerializeField, ShowIf("@monsterCategory == MonsterCategory.Spirit")] private MonsterWeaponClass[] spirit;
    [System.Serializable]
    class MonsterWeaponClass
    {
        [FoldoutGroup("@name")]
        [PropertyOrder(-2)]
        [SerializeField] string name;

        [SerializeField, HideInInspector] bool disabled;

        //[FoldoutGroup("@name")]
        [PropertyOrder(-1)]
        [Button("@disabled?\"Enable\":\"Disable\"", ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        public void Toggle ( )
        {
            disabled = !disabled;
            if (currentWeapon) currentWeapon.SetActive(!disabled);
        }

        [FoldoutGroup("@name")]
        [ListDrawerSettings(IsReadOnly = true)]
        [SerializeField] List<GameObject> weapon;

        [FoldoutGroup("@name")]
        [SerializeField, ReadOnly] GameObject currentWeapon;
        [SerializeField, HideInInspector] int currentPartIndex = 0;

        [HideIf("disabled"), Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        void Previous ( )
        {
            if (disabled) return;

            if (currentWeapon) currentWeapon.SetActive(false);

            if (currentPartIndex > 0)
                currentPartIndex--;
            else
                currentPartIndex = weapon.Count - 1;

            currentWeapon = weapon[currentPartIndex];
            currentWeapon.SetActive(true);
        }

        [HideIf("disabled"), Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        void Next ( )
        {
            if (disabled) return;

            if (currentWeapon) currentWeapon.SetActive(false);

            if (currentPartIndex < weapon.Count - 1)
                currentPartIndex++;
            else
                currentPartIndex = 0;

            currentWeapon = weapon[currentPartIndex];
            currentWeapon.SetActive(true);
        }
    }
}

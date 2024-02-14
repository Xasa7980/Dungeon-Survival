using DamageNumbersPro;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum DamageType
{
    NormalDamage,
    CriticalDamage,
    PoisonDamage,
    FireDamage,
    BleedDamage,
    Experience,
    Gold,
    Mana,
    Health,
}
public class GUI_Pool_Manager : MonoBehaviour
{
    public static GUI_Pool_Manager Instance { get; private set; }
    public ObjectPool NormalDamagePool => normalDamagePool;
    [SerializeField] private ObjectPool normalDamagePool;

    public ObjectPool CriticalDamagePool => criticalDamagePool;
    [SerializeField] private ObjectPool criticalDamagePool;

    public ObjectPool BleedDamagePool => bleedDamagePool;
    [SerializeField] private ObjectPool bleedDamagePool;

    public ObjectPool FireDamagePool => fireDamagePool;
    [SerializeField] private ObjectPool fireDamagePool;

    public ObjectPool PoisonDamagePool => poisonDamagePool;
    [SerializeField] private ObjectPool poisonDamagePool;

    public ObjectPool HealthPool => healthPool;
    [SerializeField] private ObjectPool healthPool;

    public ObjectPool ManaPool => manaPool;
    [SerializeField] private ObjectPool manaPool;

    public ObjectPool ExperiencePool => experiencePool;
    [SerializeField] private ObjectPool experiencePool;

    public ObjectPool GoldPool => goldPool;
    [SerializeField] private ObjectPool goldPool;

    [SerializeField] private DamageNumber normalPool;
    [SerializeField] private DamageNumber critPool;

    [SerializeField] private Camera uiCamera;
    [PropertyOrder(0)]
    [Button("SetCameraToCanvas")]
    public void SetCameraToCanvas ( ) { GetComponent<Canvas>().worldCamera = uiCamera; }
    private string suffixText;
    private string prefixText;
    private void Awake ( )
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // Recapacitar de si voy a tener mas escenas o diferentes
        SetCameraToCanvas();
    }
    public ObjectPool GetPoolBasedOnDamageType ( DamageType poolType )
    {
        switch (poolType)
        {
            case DamageType.NormalDamage:

                return normalDamagePool;

            case DamageType.CriticalDamage:

                 return criticalDamagePool;

            case DamageType.PoisonDamage:

                return poisonDamagePool;

            case DamageType.FireDamage:

                return fireDamagePool;

            case DamageType.BleedDamage:

                return bleedDamagePool;

            case DamageType.Experience:

                return experiencePool;

            case DamageType.Gold:

                return goldPool;

            case DamageType.Mana:

                return manaPool;

            case DamageType.Health:

                return healthPool;

            default:

                return normalDamagePool;
        }
    }
    public void CreateNumberTexts ( DamageType poolType, int value, Vector3 position )
    {
        switch (poolType)
        {
            case DamageType.NormalDamage:

                if (value < 0) prefixText = "+";
                else prefixText = "-";
                
                break;

            case DamageType.CriticalDamage:

                if (value < 0) prefixText = "+";
                else prefixText = "-";
                suffixText = "!!";
                
                break;

            case DamageType.PoisonDamage:

                if (value < 0) prefixText = "+";
                else prefixText = "-";

                break;

            case DamageType.FireDamage:

                if (value < 0) prefixText = "+";
                else prefixText = "-";
                
                break;

            case DamageType.BleedDamage:

                if (value < 0) prefixText = "+";
                else prefixText = "-";
                
                break;

            case DamageType.Experience:

                if (value < 0) prefixText = "+";
                else prefixText = "-";
                suffixText = "XP";
                break;

            case DamageType.Gold:

                if (value < 0) prefixText = "+";
                else prefixText = "-";
                suffixText = "G";
                break;

            case DamageType.Mana:

                if (value < 0) prefixText = "+";
                else prefixText = "-";
                suffixText = "MP";

                break;

            case DamageType.Health:

                if (value < 0) prefixText = "+";
                else prefixText = "-";
                suffixText = "HP";

                break;

            default:
                return;
        }

        GameObject gO_Requested = GetPoolBasedOnDamageType(poolType).RequestGameObject();
        
        if (gO_Requested.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI damageIndicator))
        {
            UI_Animations uI_Animations = gO_Requested.GetComponent<UI_Animations>();
            uI_Animations.originalPosition = position; 
            uI_Animations.SetTargetValue(position);
            damageIndicator.text = prefixText + value.ToString() + suffixText;
            damageIndicator.transform.position = position;
        }
    }
}
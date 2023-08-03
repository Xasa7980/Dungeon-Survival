using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractObjectType //pasar scriptable object
{
    Trap,
    Information,
    Resource,
    Quest,
    Workbench,
}
[CreateAssetMenu(fileName = "New Interaction Object", menuName = "Dungeon Library/Inventory/Interactuable")]
public class InteractionScriptableObject : ScriptableObject
{
    public InteractObjectType type;
    public AnimationClip interactionAnimClip;
    public GameObject prefab;
    public float interactionTime;
    public virtual void ReplaceInteractAnim()
    {
        AnimationContainer.instance.SetInteractuableAnimation(interactionAnimClip);
    }

}

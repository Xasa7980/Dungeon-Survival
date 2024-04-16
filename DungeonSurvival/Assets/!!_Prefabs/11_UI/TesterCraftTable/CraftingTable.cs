using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CraftingTable : Interactable
{
    public bool locked;
    public bool interacting { get; private set; }

    [Title("")]
    [SerializeField] Transform _craftingPosition;
    public Transform craftingPosition => _craftingPosition;
    [SerializeField] Transform[] _partsSlots;
    public Transform selectedInteractionPoint;
    public Transform[] partsSlots => _partsSlots;

    [Title("")]
    [SerializeField] Transform cameraPosition;

    [Title("")]
    [SerializeField] Canvas craftingInterface;
    [SerializeField] Sprite emptySlotSprite;
    [SerializeField] Sprite unaviableSlotSprite;
    [SerializeField] Image[] slotsIconImages;
    public Item[] setParts { get; private set; }
    int currentSlotIndex = -1;
    public Item[] requiredPieces { get; private set; }
    public bool isReadyToCraft
    {
        get
        {
            if(requiredPieces.Length > 0)
            {
                foreach (Item piece in requiredPieces)
                {
                    if (piece == null) return false;
                }

                return true;
            }
            return false;
        }
    }

    bool jumpFrame = false;
    protected void Update ( )
    {
        if (jumpFrame)
        {
            jumpFrame = false;
            return;
        }

        if (locked) return;

        if (Input.GetKeyDown(KeyCode.Escape) && interacting)
        {
            StopInteraction();
        }
        //Test only
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //PlayerComponents.instance.anim.SetTrigger("StartCraft");
        }
        if (interacting)
        {
            PlayerLocomotion.current.transform.rotation = Quaternion.Lerp(PlayerLocomotion.current.transform.rotation,
            selectedInteractionPoint.rotation, Time.deltaTime * 5);
        }

    }

    public override void StartInteraction ( )
    {
        base.StartInteraction();
        PlayerComponents.instance.anim.SetFloat("Speed", 0);
        //Hotbar.SetLockingState(true); Examinar
        PlayerComponents.instance.lockedAll = true;
        CameraController.current.GrabCamera(cameraPosition);
        interacting = true;

        UI_CraftingTable.current.gameObject.SetActive(true);

        //PlayerComponents.instance.anim.SetFloat("Speed", 0);
        //PlayerComponents.instance.anim.SetBool("Crafting", true);

        craftingInterface.gameObject.SetActive(true);

        foreach (Image i in slotsIconImages)
        {
            i.sprite = emptySlotSprite;
        }
        UI_CraftingTable.currentTable = this;

        jumpFrame = true;
    }

    public override void StopInteraction ( )
    {
        base.StopInteraction();

        //Hotbar.SetLockingState(false);

        CameraController.current.ReleaseCamera(cameraPosition);
        interacting = false;
        //PlayerComponents.instance.anim.SetBool("Crafting", false);
        //PlayerComponents.instance.anim.SetBool("Working", false);

        UI_CraftingTable.current.gameObject.SetActive(false);

        if (craftingPosition.childCount > 0)
        {
            Transform[] childs = craftingPosition.GetComponentsInChildren<Transform>();
            foreach (Transform child in childs)
            {
                if (child == craftingPosition) continue;
                Destroy(child.gameObject);
            }
        }
    }
    #region UI Methods
    public void OpenRecipeWindow ( )
    {
        UI_CraftingTable.current.OpenRecipeWindow();
    }

    public void PrepareTable ( Item_Recipe itemRecipe )
    {
        requiredPieces = new Item[itemRecipe.ingredients.Count];

        for (int i = 0; i < slotsIconImages.Length; i++)
        {
            if (i < itemRecipe.ingredients.Count)
            {
                slotsIconImages[i].sprite = itemRecipe.ingredients[i].icon;
                slotsIconImages[i].GetComponentInParent<Button>().interactable = true;
                if(i < requiredPieces.Length)
                {
                    requiredPieces[i] = itemRecipe.ingredients[i].item;
                }
            }
            else
            {
                slotsIconImages[i].sprite = unaviableSlotSprite;
                slotsIconImages[i].GetComponentInParent<Button>().interactable = false;
            }
        }

        setParts = new Item[itemRecipe.ingredients.Count];
        for (int i = 0; i < setParts.Length; i++)
        {
            setParts[i] = itemRecipe.ingredients[i].item;
        }

    }

    public void ResetTable ( )
    {
        foreach (Image i in slotsIconImages)
        {
            i.sprite = emptySlotSprite;
        }

        foreach (Transform slot in partsSlots)
        {
            if (slot.childCount > 0)
            {
                Transform[] ts = slot.GetComponentsInChildren<Transform>();
                foreach (Transform child in ts)
                {
                    if (child == slot) continue;

                    Destroy(child.gameObject);
                }
            }
        }

        if (craftingPosition.childCount > 0)
        {
            Transform[] ts = craftingPosition.GetComponentsInChildren<Transform>();
            foreach (Transform child in ts)
            {
                if (child == craftingPosition) continue;

                Destroy(child.gameObject);
            }
        }
    }

    public void ClearTable ( )
    {
        ResetTable();
        UI_CraftingTable.current.ClearRecipe();
    }

    public void SelectPartSlot ( int slot )
    {
        UI_CraftingTable.current.FindCraftingPiece(setParts[slot], partsSlots[slot]);
        currentSlotIndex = slot;
    }

    public void SelectPiece ( Item piece )
    {
        requiredPieces[currentSlotIndex] = piece;

        if (partsSlots[currentSlotIndex].childCount > 0)
        {
            Transform[] ts = partsSlots[currentSlotIndex].GetComponentsInChildren<Transform>();
            foreach (Transform t in ts)
            {
                if (t == partsSlots[currentSlotIndex]) continue;

                Destroy(t.gameObject);
            }
        }

        GameObject pieceGO = Instantiate(piece.visualizationModel);

        MeshFilter filter = pieceGO.GetComponentInChildren<MeshFilter>();
        Vector3 center = GeometryTool.GetCenterOf(filter, pieceGO.transform, GeometryTool.CenterType.Geometrical);
        Transform container = new GameObject("Container").transform;
        container.position = center;
        pieceGO.transform.parent = container;
        container.transform.parent = partsSlots[currentSlotIndex];
        container.transform.localPosition = Vector3.zero;
        container.transform.Rotate(Vector3.up, Random.Range(0, 360));
    }

    public void CreateItem ( )
    {
        if (isReadyToCraft)
        {
            UI_CraftingTable.current.OpenCreateItemWindow();
        }
        else
        {
            Debug.LogError("Still some setParts missing. Impossible to craft.");
        }
    }
    #endregion

}

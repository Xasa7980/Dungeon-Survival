using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_RecipeInfo : MonoBehaviour
{
    [SerializeField] Text recipeName;
    [SerializeField] Image recipeImage;

    [SerializeField] Image[] ingredientsImage;
    [SerializeField] Text[] ingredientsName;

    public void Configure ( Item_Recipe recipe, Vector3 position )
    {
        this.gameObject.SetActive(true);
        this.transform.position = new Vector3(transform.position.x, position.y, transform.position.z);

        recipeName.text = recipe.itemToCraft.displayName;
        recipeImage.sprite = recipe.itemIcon;

        for (int i = 0; i < 4; i++)
        {
            if (i < recipe.ingredients.Count)
            {
                ingredientsImage[i].transform.parent.gameObject.SetActive(true);
                ingredientsImage[i].sprite = recipe.ingredients[i].icon;
                ingredientsName[i].text = recipe.ingredients[i].name + " x" + recipe.ingredients[i].amount;
            }
            else
            {
                ingredientsImage[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void Configure ( iItemData item, Vector3 position )
    {
        this.gameObject.SetActive(true);
        this.transform.position = new Vector3(transform.position.x, position.y, transform.position.z);

        recipeName.text = item.displayName;
        recipeImage.sprite = item.icon;

        for (int i = 0; i < 4; i++)
        {
            if (i < item.resultingPieces.Length)
            {
                ingredientsImage[i].transform.parent.gameObject.SetActive(true);
                ingredientsImage[i].sprite = item.resultingPieces[i].icon;
                ingredientsName[i].text = item.resultingPieces[i].displayName;
            }
            else
            {
                ingredientsImage[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
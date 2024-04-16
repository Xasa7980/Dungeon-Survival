using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_PieceInfo : MonoBehaviour
{
    [SerializeField] Text pieceName;
    [SerializeField] Image pieceImage;

    [SerializeField] Transform statsContainer;
    [SerializeField] UI_Stat_Layout statLayoutPrefab;

    Item itemPiece;

    public void Configure ( Item piece, Vector3 position )
    {
        this.gameObject.SetActive(true);
        this.transform.position = new Vector3(transform.position.x, position.y, transform.position.z);

        this.itemPiece = piece;

        pieceName.text = piece.name;
        pieceImage.sprite = piece.icon;
    }

    public void Select ( )
    {
        //UI_CraftingTable.currentTable.SelectPiece(this.itemPiece);
    }
}
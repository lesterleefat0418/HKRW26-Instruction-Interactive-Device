using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchCell : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    private RawImage cellImage = null;
    public Sprite[] cellSprites;
    public Color32 defaultColor = Color.black;
    public Color32 selectedColor = Color.white;
    public int row;
    public int col;
    public bool isSelected = false;

    void Start()
    {
        this.cellImage = GetComponent<RawImage>();
        this.cellImage.color = this.defaultColor;
    }

    public void SetButtonColor(Color _color = default)
    {
        if (_color != default(Color))
            this.cellImage.color = _color;
        else
            this.cellImage.color = Color.white;
    }

    public void Selected()
    {
        this.isSelected = true;
        this.cellImage.color = this.defaultColor;
    }

    public void DisSelected()
    {
        this.isSelected = false;
        this.cellImage.color = this.selectedColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelected && !TouchPointConnector.Instance.IsConnectWord)
        {
            TouchPointConnector.Instance.StartConnection();
            TouchPointConnector.Instance.SelectCell(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TouchPointConnector.Instance.IsConnectWord) // Check if drawing is active
        {
            TouchPointConnector.Instance.SelectCell(this);
        }
    }

}

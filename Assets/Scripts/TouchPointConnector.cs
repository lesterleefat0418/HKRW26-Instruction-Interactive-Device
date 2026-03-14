using System.Collections.Generic;
using UnityEngine;

public class TouchPointConnector : MonoBehaviour
{
    public static TouchPointConnector Instance = null;
    public Canvas canvas;                    // The canvas containing the points
    public RectTransform[] points;           // Assign point RectTransforms in inspector
    public LineDrawer lineDrawer;
    public bool IsConnectWord = false;
    private List<TouchCell> selectedCells = new List<TouchCell>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartConnection()
    {
        this.IsConnectWord = true; // Start drawing
        this.lineDrawer?.StartDrawing();
    }

    public void SelectCell(TouchCell cell)
    {
        cell.Selected();
        this.selectedCells.Add(cell);
        this.lineDrawer?.AddPoint(cell.transform.position);
        //AudioController.Instance?.PlayAudio(0);
    }

    public void StopConnection()
    {
        //Check Answer
        if (this.IsConnectWord)
        {
            this.IsConnectWord = false;

            // Reset the line drawer so the LineRenderer is cleared and ready for next stroke
            this.lineDrawer?.FinishDrawing();

            // Reset selected cells' state and clear the list
            for (int i = 0; i < this.selectedCells.Count; i++)
            {
                var c = this.selectedCells[i];
                if (c != null)
                {
                    c.DisSelected();
                }
            }
            this.selectedCells.Clear();
        }
    }

    private void Update()
    {
        // Handle mouse input
        if (Input.GetMouseButtonUp(0))
        {
            this.StopConnection();
        }

        // Handle touch input
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        if (Physics.Raycast(ray, out RaycastHit hit))
                        {
                            TouchCell cell = hit.collider.GetComponent<TouchCell>();
                        }
                        this.StartConnection();
                        this.HandleTouch(touch.position);
                        break;
                    case TouchPhase.Moved:
                        if (this.IsConnectWord)
                        {
                            this.HandleTouch(touch.position);
                        }
                        break;
                    case TouchPhase.Ended:
                        this.StopConnection();
                        break;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            this.HandleMouse();
        }
    }

    private void HandleMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        CheckCell(ray);
    }

    private void HandleTouch(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        CheckCell(ray);
    }

    private void CheckCell(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TouchCell cell = hit.collider.GetComponent<TouchCell>();
            if (cell != null)
            {
                if (!cell.isSelected)
                {
                    this.SelectCell(cell);
                }
            }
        }
    }
}

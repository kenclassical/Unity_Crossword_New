using UnityEngine;

public class UIGrid : MonoBehaviour
{
    public GameObject[,] GirdUI;
    public bool CheckGrid = false;
    private int RowUI = 9;
    private int ColUI = 9;
    public GameObject CellPrefab;
    public bool IsIntialized { get; private set; }

    public void Initialize()
    {
        IsIntialized = true;
        GirdUI = new GameObject[RowUI, ColUI];
        var gridSize = GetComponent<RectTransform>().rect;
        var xSize = gridSize.width / ColUI * gameObject.transform.lossyScale.x;
        var ySize = gridSize.height / RowUI * gameObject.transform.lossyScale.y;
        var cellTransform = CellPrefab.gameObject.GetComponent<RectTransform>();
        if (CheckGrid)
        {
            if (xSize > ySize)
                xSize = ySize;
            else ySize = xSize;
        }
        cellTransform.sizeDelta = new Vector2(xSize, ySize);
        var xStart = transform.position.x + (cellTransform.rect.width - ColUI * xSize) / 2;
        var curPosition = new Vector3(xStart, transform.position.y + (cellTransform.rect.height - RowUI * ySize) / 2);
        for (var i = 0; i < RowUI; i++)
        {
            for (var j = 0; j < ColUI; j++)
            {
                var curCell = Instantiate(CellPrefab);
                curCell.transform.SetParent(gameObject.transform);
                curCell.transform.position = curPosition;
                GirdUI[i, j] = curCell;
                curPosition.x += xSize;
            }
            curPosition.y += ySize;
            curPosition.x = xStart;
        }
    }
    public void AddElement(int row, int column, GameObject element, float padding = 0, bool isSquare = false, bool preserveSize = false)//element should have anchors in middle and centre
    {
        element.transform.position = new Vector3(GirdUI[row, column].transform.position.x, GirdUI[row, column].transform.position.y);
        if (preserveSize)
            return;
        if (!isSquare)
            element.gameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2((1 - padding) * GirdUI[row, column].GetComponent<RectTransform>().rect.width,
                    (1 - padding) * GirdUI[row, column].GetComponent<RectTransform>().rect.height);
        else
        {
            var min = (GirdUI[row, column].GetComponent<RectTransform>().rect.width <
                      GirdUI[row, column].GetComponent<RectTransform>().rect.height
                ? GirdUI[row, column].GetComponent<RectTransform>().rect.width
                : GirdUI[row, column].GetComponent<RectTransform>().rect.height) * (1 - padding);
            element.gameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2(min, min);
        }
    }

    public void AddElement(int upperRow, int upperColumn, int lowerRow, int lowerColumn, GameObject element, float padding = 0, bool isSquare = false, bool preserveSize = false)
    {
        element.transform.position = new Vector3((GirdUI[upperRow, upperColumn].transform.position.x + GirdUI[lowerRow, lowerColumn].transform.position.x) / 2,
            (GirdUI[upperRow, upperColumn].transform.position.y + GirdUI[lowerRow, lowerColumn].transform.position.y) / 2);
        if (preserveSize)
            return;
        var ySize = (upperRow - lowerRow + 1) * GirdUI[0, 0].GetComponent<RectTransform>().rect.height * (1 - padding);
        var xSize = (lowerColumn - upperColumn + 1) * GirdUI[0, 0].GetComponent<RectTransform>().rect.width * (1 - padding);
        if (isSquare)
        {
            if (xSize < ySize)
                ySize = xSize;
            else xSize = ySize;
        }
        element.gameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2(xSize, ySize);
    }
}

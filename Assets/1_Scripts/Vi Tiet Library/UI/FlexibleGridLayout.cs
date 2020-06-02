using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRow,
        FixedColumn
    }
    
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private FitType fitType;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private Vector2 spacing;
    [SerializeField] private bool fitX;
    [SerializeField] private bool fitY;


    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (transform.childCount <= 0) return;

        float sqrt = Mathf.Sqrt(transform.childCount);

        if (fitType == FitType.Uniform || fitType == FitType.Width || fitType == FitType.Height)
        {
            fitX = fitY = true;
            rows = columns = Mathf.CeilToInt(sqrt);
        }

        if (fitType == FitType.Height || fitType == FitType.FixedColumn)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }

        if (fitType == FitType.Width || fitType == FitType.FixedRow)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / columns) - (spacing.x / columns * (columns - 1)) - (padding.left / columns) - (padding.right / columns);
        float cellHeight = (parentHeight / rows) - (spacing.y / rows * (rows - 1)) - (padding.top / rows) - (padding.bottom / rows);

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            RectTransform item = rectChildren[i];
            float xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            float yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}

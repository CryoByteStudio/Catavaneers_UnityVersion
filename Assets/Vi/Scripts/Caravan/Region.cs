using CustomMathLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegionData
{
    public int numberOfPoints;
    public float radius;
    public Transform parent;
    public List<Point> pointsList;
}

public class Region : MonoBehaviour
{
    [Header("Inner Region Settings")]
    [SerializeField] private RegionData innerRegion = new RegionData();
    [Header("Outer Region Settings")]
    [SerializeField] private RegionData outerRegion = new RegionData();

    public RegionData InnerRegion { get { return innerRegion; } }
    public RegionData OuterRegion { get { return outerRegion; } }

    private string pointName = "Point ";

    // Start is called before the first frame update
    private void Start()
    {
        CreatRegionPoints(innerRegion);
        CreatRegionPoints(outerRegion);
    }

    private void CreatRegionPoints(RegionData regionData)
    {
        regionData.pointsList = new List<Point>();
        GameObject go;

        for (int i = 0; i < regionData.numberOfPoints; i++)
        {
            go = new GameObject(pointName + (i + 1));
            go.transform.parent = regionData.parent;

            go.transform.position = 
                CustomMathf.GetEvenlySpacingPositionAroundAxis(regionData.numberOfPoints, i, CustomMathf.Axis.Y)
                * regionData.radius + transform.position;

            regionData.pointsList.Add(go.AddComponent<Point>());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, innerRegion.radius);
        Gizmos.DrawWireSphere(transform.position, outerRegion.radius);
        DrawPoints(innerRegion.pointsList);
        DrawPoints(outerRegion.pointsList);
    }

    private static void DrawPoints(List<Point> pointsList)
    {
        if (pointsList.Count > 0)
        {
            for (int i = 0; i < pointsList.Count; i++)
            {
                Gizmos.DrawSphere(pointsList[i].Position, 0.3f);
            }
        }
    }
}

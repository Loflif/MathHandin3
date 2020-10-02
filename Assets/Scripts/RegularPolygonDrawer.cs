using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegularPolygonDrawer : MonoBehaviour
{
    public static float TAU = 6.28318f;
    [SerializeField] private int SideCount = 4;
    [SerializeField] private int Density = 1;

    public TextMeshPro AreaText = null;

    private Vector2[] ConstructVerts()
    {
        Vector2[] verts = new Vector2[SideCount];
        for (int i = 0; i < SideCount; i++)
        {
            verts[i] = AngleToDirection(i * TAU / SideCount);
        }
        return verts;
    }

    private float CalculateAreaOfPolygon(Vector2[] Vertices)
    {
        float totalArea = 0.0f;

        Vector2 objectPos = transform.position;

        for (int i = 0; i < Vertices.Length; i++)
        {
            Vector2 vertOne = Vertices[i];
            Vector2 vertTwo = Vertices[(i+1)%SideCount];
            Vector2 vertThree = objectPos;

            float sideOne = (vertTwo - vertOne).magnitude;
            float sideTwo = (vertThree - vertOne).magnitude;

            float area = (sideTwo * sideOne) / 2;
            totalArea += area;
        }
        
        return totalArea;
    }

    private Vector2 AngleToDirection(float AngleRadians)
    {
        Vector2 direction;
        direction.x = Mathf.Cos(AngleRadians);
        direction.y = Mathf.Sin(AngleRadians);
        return direction;
    }
    
    private void OnDrawGizmos()
    {
        Vector2[] polyGonVerts = ConstructVerts();
        for (int i = 0; i < polyGonVerts.Length; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(polyGonVerts[i], polyGonVerts[(i+Density)%SideCount]);
        }

        for (int i = 0; i < polyGonVerts.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(polyGonVerts[i], 0.03f);
        }

        AreaText.text = "Total Area = \n" + CalculateAreaOfPolygon(polyGonVerts) + "m²";
    }
}

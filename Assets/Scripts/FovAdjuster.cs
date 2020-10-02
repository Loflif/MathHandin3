using UnityEngine;

public class FovAdjuster : MonoBehaviour
{
    public FovPoint[] FocusPoints;


    private void OnDrawGizmos()
    {
        Camera camera = GetComponent<Camera>();

        Vector2 cameraDirectionProjectedSpace = Vector2.right;
        Vector2 cameraPosition = camera.transform.position;

        float angleCameraToBounds = float.MinValue;

        foreach (FovPoint p in FocusPoints)
        {
            Vector3 pointInCameraSpace = camera.transform.InverseTransformPoint(p.transform.position);

            Vector2 pointInProjectedSpace = new Vector2(pointInCameraSpace.z, pointInCameraSpace.y);
            
            float distanceToPoint = pointInProjectedSpace.magnitude;
            Vector2 directionToPoint = pointInProjectedSpace /distanceToPoint; //same as normalizing but faster 

            float angleCameraToPoint = Mathf.Acos(Vector2.Dot(cameraDirectionProjectedSpace, directionToPoint));
            float anglePointToBounds = Mathf.Asin(p.BoundingCircleRadius / distanceToPoint);

            float totalAngle = angleCameraToPoint + anglePointToBounds;

            if (totalAngle > angleCameraToBounds)
                angleCameraToBounds = totalAngle;
        }

        camera.fieldOfView = angleCameraToBounds * 2 * Mathf.Rad2Deg;
        
        DrawLinesToPoints();
        DrawPointBounds();
    }

    private void DrawLinesToPoints()
    {
        Vector3 cameraPos = transform.position;
        foreach (FovPoint p in FocusPoints)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(cameraPos, p.transform.position);
        }
    }

    private void DrawPointBounds()
    {
        foreach (FovPoint p in FocusPoints)
        {
            Gizmos.DrawWireSphere(p.transform.position, p.BoundingCircleRadius);
        }
    }
}

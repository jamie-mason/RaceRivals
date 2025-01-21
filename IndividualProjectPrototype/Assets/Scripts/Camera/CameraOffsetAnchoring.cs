using UnityEngine;

public class CameraOffsetAnchoring
{
    private GameObject playerCar;
    public Vector3 offset {get; set;}

    public CameraOffsetAnchoring(GameObject playerCar, Camera cam)
    {
        this.playerCar = playerCar;
        Vector3 displacement = cam.transform.position - playerCar.transform.position;
        this.offset = displacement; // Offset in local space (x: behind, y: above, z: lateral)
    }
    public CameraOffsetAnchoring(GameObject playerCar, Vector3 offset)
    {
        this.playerCar = playerCar;
        this.offset = offset; // Offset in local space (x: behind, y: above, z: lateral)
    }

    // Get the camera's target position in world space
    public Vector3 GetOffsetPosition()
    {
        // Transform the local offset into world space
        Vector3 worldOffset = 
            offset.x * playerCar.transform.right + // Backward (negative forward)
            offset.y * playerCar.transform.up +      // Upward
            offset.z * playerCar.transform.forward;    // Lateral

        // Add the world offset to the car's position
        return worldOffset;
    }
}

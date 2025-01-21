using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private GameObject minimap;
    private GameObject followPoint;
    private GameObject body;

    Camera mini;
    private bool isMinimapYChange = false;


    [SerializeField] private float height;
    [SerializeField] private Vector3 ViewportOffset;
    float old;



    private void Start()
    {
        InstanceMinimapVariables();

        ChangeMinimapPosition();


    }
    private void InstanceMinimapVariables()
    {
        if (minimap == null)
        {
            minimap = GameObject.FindWithTag("Minimap");
            mini = minimap.GetComponent<Camera>();
        }
        if (followPoint == null)
        {
            followPoint = GameObject.FindWithTag("Player");
        }
        if (body == null)
        {
            body = followPoint.transform.Find("Body").gameObject;
        }

    }

    void ChangeMinimapPosition()
    {
        if (followPoint != null && minimap != null)
        {
            // Set the minimap position to the player's position plus the offset
            Vector3 playerWorldPos = followPoint.transform.position;

            // Convert the player's world position to viewport space
            Vector3 playerViewportPos = mini.WorldToViewportPoint(playerWorldPos);

            // Get the follow point's dimensions (assuming it's a renderer or collider)
            Bounds followBounds = body.GetComponent<MeshRenderer>().bounds;
            Vector3 followSize = followBounds.size;

            // Calculate the size of the follow point in viewport space (considering the size of the object in world space)
            Vector3 viewportSize = mini.WorldToViewportPoint(playerWorldPos + followSize) - mini.WorldToViewportPoint(playerWorldPos);

            // Apply the viewport offset and adjust the clamping to account for the object's size
            playerViewportPos.x = Mathf.Clamp(playerViewportPos.x + ViewportOffset.x, viewportSize.x / 2f, 1f - viewportSize.x / 2f);
            playerViewportPos.y = Mathf.Clamp(playerViewportPos.y + ViewportOffset.y, viewportSize.y / 2f, 1f - viewportSize.y / 2f);

            // Convert the adjusted viewport position back to world space
            Vector3 newWorldPos = mini.ViewportToWorldPoint(playerViewportPos);

            // Set the minimap camera's position to the adjusted world position
            // Keep the Y (height) coordinate of the minimap fixed
            minimap.transform.position = new Vector3(newWorldPos.x, minimap.transform.position.y, newWorldPos.z);


        }
    }

    private void FixedUpdate()
    {
        ViewportOffset = new Vector3(Mathf.Clamp(ViewportOffset.x,-0.5f,0.5f), Mathf.Clamp(ViewportOffset.y, -0.5f, 0.5f), ViewportOffset.z);
        ChangeMinimapPosition();

    }
}

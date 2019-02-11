using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform charLocation;
    private float distanceFromChar = 30.0f;
    Camera curCamera;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = charLocation.position + new Vector3(0, 3, -distanceFromChar);
        curCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var topRightBoundary = curCamera.ScreenToWorldPoint(new Vector3(curCamera.scaledPixelWidth * .75f, curCamera.scaledPixelHeight * .7f, transform.position.z));
        var bottomLeftBoundary  = curCamera.ScreenToWorldPoint(new Vector3(curCamera.scaledPixelWidth * .25f, curCamera.scaledPixelHeight * .3f, transform.position.z));

        float deltaX = 0;
        float deltaY = 0;
        if (charLocation.position.x > topRightBoundary.x)
        {
            deltaX = charLocation.position.x - topRightBoundary.x;
        } else if (charLocation.position.x < bottomLeftBoundary.x)
        {
            deltaX = charLocation.position.x - bottomLeftBoundary.x;
        }

        if (charLocation.position.y > topRightBoundary.y)
        {
            deltaY = charLocation.position.y - topRightBoundary.y;
        } else if (charLocation.position.y < bottomLeftBoundary.y)
        {
            deltaY = charLocation.position.y - bottomLeftBoundary.y;
        }

        if (deltaX != 0 || deltaY != 0)
        {
            Vector3 toPos = new Vector3(transform.position.x + deltaX, transform.position.y + deltaY, -distanceFromChar);
            transform.position = toPos;

            transform.rotation = Quaternion.Lerp(transform.rotation, charLocation.rotation, Time.deltaTime * 5);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform charLocation;
    private float distanceFromChar = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = charLocation.position + new Vector3(0, 2, -distanceFromChar);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toPos = charLocation.position + new Vector3(0, 2, -distanceFromChar);
        transform.position = toPos;

        transform.rotation = Quaternion.Lerp(transform.rotation, charLocation.rotation, Time.deltaTime * 5);

    }
}

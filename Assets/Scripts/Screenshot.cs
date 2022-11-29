using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

public class Screenshot : MonoBehaviour
{
    [SerializeField]
    int screenshotCount = 1;
    int sizeMultiplier = 4;
    [SerializeField]
    string path = "Assets/_Cards/screenshots/screenshot";

    public float speed = 2f;
    public float rotationSpeed = 2f;
    public float zoomSpeed = 2f;

    [Button]
    public void TakeScreenshot()
    {
        Debug.Log("Taking screenshot");
        ScreenCapture.CaptureScreenshot($"{Directory.GetCurrentDirectory()}/{path}-{screenshotCount}.png", sizeMultiplier);
        screenshotCount++;
    }

    // Move camera with wasd
    // Rotate camera with mouse and pressed ctrl left
    // Zoom with mouse scroll
    // Take screenshot with space
    // Move up and down with q and e
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * 10 * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime * 10 * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Time.deltaTime * 10 * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * 10 * speed;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += transform.up * Time.deltaTime * 10 * speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position -= transform.up * Time.deltaTime * 10 * speed;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            TakeScreenshot();
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.GetChild(0).Rotate(new Vector3(-Input.GetAxis("Mouse Y") * rotationSpeed, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * rotationSpeed, 0));
        }

        transform.Translate(new Vector3(0, 0, Input.mouseScrollDelta.y * zoomSpeed));
    }
}

using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Information")]
    public Vector2 rotation;
    public float zoom = 1;

    [Header("Attributes")]
    public float sensitivity = 2;
    public float moveSpeed = 70;
    public float zoomSpeed = 2;
    public Vector2 mixMaxRotation = new Vector2(-30, 30);
    public Vector2 mixMaxZoom = new Vector2(0.5f, 4);

    void Update()
    {
        //rotating
        rotation.x += Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
        rotation.y -= Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        if (Input.GetMouseButton(1))
        {
            rotation.x -= sensitivity * Input.GetAxis("Mouse Y");
            rotation.y += sensitivity * Input.GetAxis("Mouse X");
        }

        //zooming
        zoom -= Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
        if (Input.GetKey(KeyCode.E)) zoom -= 0.2f * Time.deltaTime * zoomSpeed;
        if (Input.GetKey(KeyCode.Q)) zoom += 0.2f * Time.deltaTime * zoomSpeed;

        //SAFTY CLAMPING
        zoom = Mathf.Clamp(zoom, mixMaxZoom.x, mixMaxZoom.y);
        rotation.x = Mathf.Clamp(rotation.x, mixMaxRotation.x, mixMaxRotation.y);

        //APPLYING
        transform.localScale = new Vector3(zoom, zoom, zoom);
        transform.eulerAngles = new Vector2(rotation.x, rotation.y);
    }
}

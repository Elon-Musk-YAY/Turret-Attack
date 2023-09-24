using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 80f;
    [SerializeField]
    private Vector3 targetPosition;

    [SerializeField]
    private float smoothFactor = 2f;




    private void Update()
    {
        if (GameManager.gameOver || GameManager.winState || GameManager.inTextBox)
        {
            this.enabled = false;
            return;
        }
        if ((Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) && transform.position.x < 69.2)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if ((Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) && transform.position.z < 108)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if ((Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) && transform.position.x > -99)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if ((Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) && transform.position.z > -171)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (targetPosition != null && targetPosition.y >= minY && targetPosition.y != transform.position.y)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothFactor);
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
 
        pos.y -= scroll * 500 * scrollSpeed * Time.deltaTime * SettingsManager.scrollSensitivity;
        pos.y = Mathf.Clamp(pos.y, minY,maxY);
        targetPosition = pos;

        //transform.position = pos;


    }
}

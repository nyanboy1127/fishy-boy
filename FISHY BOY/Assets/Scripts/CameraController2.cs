using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public GameObject character;
    public GameObject cameraCenter;
    public float yOffset = 1f;
    public float sensitivity = 3f;
    public Camera cam;

    public float scrollSensivity = 5f;
    public float scrollDampening = 6f;

    public float zoomMin = 3.5f;
    public float zoomMax = 15f;
    public float zoomDefault = 10f;

    [SerializeField]
    public float zoomDistance;

    public float collisionSensivity = 4.5f;

    private RaycastHit _camHit;
    private Vector3 _camDist;

    private void Start()
    {
        _camDist = cam.transform.localPosition;
        zoomDistance = zoomDefault;
        _camDist.z = zoomDistance;

        Cursor.visible = false;
    }


    private void Update()
    {
        cameraCenter.transform.position = new Vector3(character.transform.position.x,
            character.transform.position.y + yOffset, character.transform.position.z);

        var rotation = Quaternion.Euler(
            cameraCenter.transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity / 2,
            cameraCenter.transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivity,
            cameraCenter.transform.rotation.eulerAngles.z
            );
        cameraCenter.transform.rotation = rotation;

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            var scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensivity;
            scrollAmount *= zoomDistance * 0.3f;
            zoomDistance += -scrollAmount;
            zoomDistance = Mathf.Clamp(zoomDistance, zoomMin, zoomMax);
        }

        if (_camDist.z != -zoomDistance)
        {
            _camDist.z = Mathf.Lerp(_camDist.z, -zoomDistance, Time.deltaTime * scrollDampening);

        }

        cam.transform.localPosition = _camDist;

        GameObject obj = new GameObject();
        obj.transform.SetParent(cam.transform.parent);
        obj.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z);

        if (Physics.Linecast(cameraCenter.transform.position, obj.transform.position, out _camHit))
        {
            cam.transform.position = _camHit.point;
            var localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z );
            cam.transform.localPosition = localPosition;
        }

        Destroy(obj);

        if (cam.transform.localPosition.z > -1f)
        {
            cam.transform.localPosition =
                new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, -1f);
        }
    }
}

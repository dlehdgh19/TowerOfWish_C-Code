using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 1.5f, -10f);
    private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    public static bool CameraControlbool = false;
    public GameObject[] LeftBound;
    public GameObject[] RightBound;

    public static int PortalInt;

    // Start is called before the first frame update
    void Start()
    {
        CameraControlbool = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if(CameraControlbool == false)
        {
            if (LeftBound[PlayPortalManager.MapInt].transform.position.x <= target.position.x && RightBound[PlayPortalManager.MapInt].transform.position.x >= target.position.x)
            {
                Vector3 targetPosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
            if (LeftBound[PlayPortalManager.MapInt].transform.position.x > target.position.x)
            {
                Vector3 targetPosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, 
                    new Vector3(LeftBound[PlayPortalManager.MapInt].transform.position.x, targetPosition.y, targetPosition.z), ref velocity, smoothTime);
            }
            if (RightBound[PlayPortalManager.MapInt].transform.position.x < target.position.x)
            {
                Vector3 targetPosition = target.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, 
                    new Vector3(RightBound[PlayPortalManager.MapInt].transform.position.x, targetPosition.y, targetPosition.z), ref velocity, smoothTime);
            }
        }

        if(CameraControlbool == true)
        {
            
        }

        if(PlayPortalManager.MapInt != 16)
        {
            offset = new Vector3(0f, 1.5f, -10f);
        }

        if (PlayPortalManager.MapInt == 16)
        {
            offset = new Vector3(0f, 3f, -10f);
        }
    }
}

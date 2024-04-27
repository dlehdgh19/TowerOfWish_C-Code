using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField]
    private int serialNum;

    public static int staticintNum;

    // Start is called before the first frame update
    void Start()
    {
        staticintNum = serialNum;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

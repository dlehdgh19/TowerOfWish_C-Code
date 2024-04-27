using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTransparent : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] Portals;

    public int PortalNum;
    private float progresstime;
    // Start is called before the first frame update
    void Start()
    {
        progresstime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        progresstime += Time.deltaTime;
        for (int i = 0; i < PortalNum; i++)
        {
            if(progresstime > 0 && progresstime < 0.5f)
            {
                Portals[i].color = new Color(92 / 255, 1, 1, 0.1f + progresstime * 1.8f);
            }
            if (progresstime >= 0.5f && progresstime < 1f)
            {
                Portals[i].color = new Color(92 / 255, 1, 1, 1 - 1.8f * (progresstime - 0.5f));
            }
            if (progresstime > 1)
            {
                progresstime = 0;
            }
        }
    }
}

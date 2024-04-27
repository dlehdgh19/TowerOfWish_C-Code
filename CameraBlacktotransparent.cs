using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraBlacktotransparent : MonoBehaviour
{
    public static bool screenbool;
    private float screentime;
    public SpriteRenderer screencolor;
    // Start is called before the first frame update
    void Start()
    {
        screenbool = false;
        screentime = 0;
        screencolor.color = new Color(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(screenbool == false)
        {
            screentime += Time.deltaTime;
            if(screentime > 1 && screentime < 2)
            {
                screencolor.color = new Color(0, 0, 0, (2 - screentime));
            }
            if(screentime >= 2)
            {
                MainCharacterController.CharacterCantMovebool = false;
                screencolor.color = new Color(0, 0, 0, 0);
                screentime = 0;
                screenbool = true;
            }
        }

        if(ToturialPortalManager.Screenbool == true)
        {
            MainCharacterController.CharacterCantMovebool = true;
            screentime += Time.deltaTime;
            if(screentime > 0 && screentime < 1)
            {
                screencolor.color = new Color(0, 0, 0, screentime);
            }
            if(screentime >= 1)
            {
                screencolor.color = new Color(0, 0, 0, 1);
                ToturialPortalManager.Screenbool = false;
                screentime = 0;
                SaveThings.SceneSerialNum = 2;
                screenbool = false;

                SceneManager.LoadScene("PlayScene");
            }
        }

        if(PlayButtonManager.Scenebool == true)
        {
            MainCharacterController.CharacterCantMovebool = true;
            screentime += Time.deltaTime;
            if (screentime > 0 && screentime < 1)
            {
                screencolor.color = new Color(0, 0, 0, screentime);
            }
            if (screentime >= 1)
            {
                screencolor.color = new Color(0, 0, 0, 1);
                PlayButtonManager.Scenebool = false;
                screentime = 0;
                SaveThings.SceneSerialNum = 0;
                screenbool = false;
                SaveThings.SaveString = "Saved";
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}

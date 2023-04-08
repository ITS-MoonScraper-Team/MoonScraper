using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EditorTools : EditorWindow
{

    [MenuItem("EditorTools/Open Scene/Start_Menu")]
    public static void OpenMainMenuScene()
    {
        EditorSceneManager.OpenScene("Assets/MyScenes/START_MENU.unity");
        //SceneManager.LoadScene(0);
    }

    [MenuItem("EditorTools/Open Scene/Game_Scene")]
    public static void OpenGameScene()
    {
        EditorSceneManager.OpenScene("Assets/MyScenes/GAME_SCENE.unity");
    }

    [MenuItem("EditorTools/MODs/EasyMode ON-OFF")]
    public static void EasyModeOnOff()
    {
        if (MainMenu.easyMode)
        {
            MainMenu.easyMode = false;
            Debug.LogWarning("EASY MODE OFF");
        }
        else
        {
            MainMenu.easyMode = true;
            Debug.LogWarning("EASY MODE ON");
        }
    }

    [MenuItem("EditorTools/MODs/Mushroom ON-OFF")]
    public static void MushroomOnOff()
    {
        //PlayerController player = FindObjectOfType<PlayerController>();
        if (/*player.mushroomJumper*/PlayerController.mushroomJumper)
        {
            PlayerController.mushroomJumper=false;
            //player.mushroomJumper = false;
            Debug.LogWarning($"MUSH OFF: {PlayerController.mushroomJumper}");
        }
        else
        {
            PlayerController.mushroomJumper = true;
            //player.mushroomJumper = true;
            Debug.LogWarning($"MUSH ON: {PlayerController.mushroomJumper}");
        }
    }

    [MenuItem("EditorTools/Cheats/Unlimited Max")]
    public static void UnlimitedFuel()
    {
        PlayerController.secretNumber = 666;
        Debug.LogWarning($"THE BEAST IS UNLEASHED: {PlayerController.secretNumber}");
    }




}

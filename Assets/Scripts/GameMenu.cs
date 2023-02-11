using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class GameMenu : MonoBehaviour
{
    //public static GameMenu InstanceGameMenu;

    //private void Awake()
    //{
    //    InstanceGameMenu = this;
    //}
    // Start is called before the first frame update
    public void BackToMain()
    {
        //get Scenes to play from builder
        SceneManager.LoadScene(0/*,parameterers */);
    }
    //public void QuitGame()
    //{
    //    Application.Quit();
    //    Debug.Log("QUIT!");
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public static MainMenu InstanceMenu;

    public TMP_Text lifeMeterTXT;
    public Slider maxLivesSlider;

    private int livesMax;
    public int LivesMax
    { get; set; }

    private void Awake()
    {
        InstanceMenu = this;
    }
    public void PlayGame()
    {
        //get Scenes to play from builder
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1/*,parameterers */);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT!");
    }
    private void Start()
    { }

    void Update()
    {
        LivesMax = (int)maxLivesSlider.value;
        if (LivesMax == 11)
        { lifeMeterTXT.text = "eternal"; }
        else
        {
            lifeMeterTXT.text = LivesMax.ToString();
        }
        Debug.Log($"VITE {LivesMax}");
    }
}

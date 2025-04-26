//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 11 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class PlayerEffector : MonoBehaviour
{
    public FirstPersonController myController;

    private void Start()
    {
        if(myController == null)
            myController = GetComponent<FirstPersonController>();
    }

    public void ChangeMoveSpeed(float delta)
    {
        myController.MoveSpeed += delta;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        myController.MoveSpeed = newSpeed;
    }

    public void OnRestartApp()
    {
        SceneManager.LoadScene(0);
    }

    public void OnRestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}

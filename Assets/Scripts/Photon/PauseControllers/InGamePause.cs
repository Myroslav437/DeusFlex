using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePause : MonoBehaviour
{
    public Canvas canvas;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas.gameObject.activeSelf)
            {
                canvas.gameObject.SetActive(false);
            }
            else
            {
                canvas.gameObject.SetActive(true);
            }
        }
    }

    public void onContinueButtonPressed()
    {
        canvas.gameObject.SetActive(false);
    }

    public void onExitGameButtonPressed()
    {
        Application.Quit();
    }
}

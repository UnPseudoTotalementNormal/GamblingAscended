using System;
using UnityEngine;
using UnityEngine.UI;

public class GL_QuitButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Application.Quit();
    }
}

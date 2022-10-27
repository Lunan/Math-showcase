using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PrisonersClick()
    {
        SceneManager.LoadScene("Prisoners");
    }

    public void DrunkClick()
    {
        SceneManager.LoadScene("Drunk");
    }

    public void QuitClick()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        Application.Quit();
#endif
    }
}

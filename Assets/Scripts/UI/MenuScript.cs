using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public Texture2D cursor;

    public void PlaySandbox()
    {
        //SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Console.WriteLine("QUIT");
    }

    private void Start()
    {
        Vector2 cursorOffset = new Vector2(cursor.width / 2, cursor.height / 2);
        Cursor.SetCursor(cursor, cursorOffset, CursorMode.Auto);
    }
}

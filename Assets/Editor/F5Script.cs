//-------------------------------
// Created by Lee Elliott
// 15/11/2018
//
// A script designed to start and
// end playback within the editor.
//-------------------------------

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class F5Script : ScriptableObject
{
	// shortcut key F5 to Play (and exit playmode also)
    [MenuItem("Edit/Run _F5")] 
    static void PlayGame()
    {
        if(!Application.isPlaying)
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "", false);
        }

        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

}
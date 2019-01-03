//-------------------------------
// Created by Lee Elliott
// 15/11/2018
//
// A script designed to load
// a specific phrase from
// a CSV file.
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromCSV : MonoBehaviour
{

	// Use this for initialization
	public string LoadSpeech(int row, int column)
    {
        TextAsset speechData = Resources.Load<TextAsset>("Speech");

		// Split file into individual lines
        string[] data = speechData.text.Split(new char[] { '\n' });

		// Split lines into individual phrases
        string[] words = data[row].Split(new char[] { ',' });

		// Return the phrase requested
        return words[column];
	}
}

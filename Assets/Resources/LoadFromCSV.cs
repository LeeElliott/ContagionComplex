﻿//-------------------------------
// Created by Lee Elliott
// 15/11/2018
//
// Edited by Lee Elliott (delimiter change and extension)
// 20/01/2019
//
// A script designed to load
// a specific phrase from
// a CSV file.
//
//-------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromCSV : MonoBehaviour
{

	// Use this for reading speech phrases
	public string LoadSpeech(int row, int column)
    {
        TextAsset speechData = Resources.Load<TextAsset>("Speech");

		// Split file into individual lines
        string[] data = speechData.text.Split(new char[] { '~' });

		// Split lines into individual phrases
        string[] words = data[row].Split(new char[] { '|' });

        // Return the phrase requested
        if (column < words.Length)
        {
            return words[column];
        }
        else
        {
            return "Error, index out of range";
        }
    }

    // Use this for generating names
    public string LoadName(int row, int column)
    {
        TextAsset speechData = Resources.Load<TextAsset>("Names");

        // Split file into individual lines
        string[] data = speechData.text.Split(new char[] { '~' });

        // Split lines into individual phrases
        string[] words = data[row].Split(new char[] { '|' });

        // Return the phrase requested
        return words[column];
    }

    // Use this for loading virus information
    public string LoadData(int row, int column)
    {
        TextAsset speechData = Resources.Load<TextAsset>("VirusData");

        // Split file into individual lines
        string[] data = speechData.text.Split(new char[] { '~' });

        // Split lines into individual phrases
        string[] words = data[row].Split(new char[] { '|' });

        // Return the phrase requested
        if(column < words.Length)
        {
            return words[column];
        }
        else
        {
            return "Error, index out of range";
        }
        
    }
}

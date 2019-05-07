//-------------------------------
// Created by Lee Elliott
// 15/11/2018
//
// Edited by Lee Elliott (delimiter change and extension)
// 20/01/2019
//
// Edited by Lee Elliott (conversion to one flexible function)
// 03/05/2019
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

    // Use this for reading text phrases
    public string LoadTextFromFile(int row, int column, string filename)
    {
        TextAsset speechData = Resources.Load<TextAsset>(filename);

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
}

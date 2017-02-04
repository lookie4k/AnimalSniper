using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magazine : MonoBehaviour {

    public int maxSize;
    public int currentSize;

    void Start()
    {
        UpdateDisplaySize();
    }

    private void UpdateDisplaySize()
    {
        GameObject.Find("Text").GetComponent<Text>().text = currentSize.ToString();
    }

	public bool Increase()
    {
        currentSize++;

        if (currentSize > maxSize)
        {
            currentSize = maxSize;
            UpdateDisplaySize();
            return false;
        }
        UpdateDisplaySize();
        return true;
    }

    public bool Decrease()
    {
        currentSize--;

        if (currentSize <= 0)
        {
            currentSize = 0;
            UpdateDisplaySize();
            return false;
        }
        UpdateDisplaySize();
        return true;
    }

    public bool IsRemaining()
    {
        UpdateDisplaySize();
        return currentSize > 0;
    }
}

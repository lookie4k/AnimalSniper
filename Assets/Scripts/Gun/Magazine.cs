using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour {

    public int maxSize;
    public int currentSize;

	public bool Increase()
    {
        currentSize++;

        if (currentSize > maxSize)
        {
            currentSize = maxSize;
            return false;
        }

        return true;
    }

    public bool Decrease()
    {
        currentSize--;

        if (currentSize <= 0)
        {
            currentSize = 0;
            return false;
        }
        return true;
    }

    public bool IsRemaining()
    {
        return currentSize > 0;
    }
}

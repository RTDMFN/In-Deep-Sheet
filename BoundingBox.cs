using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoundingBox
{
    public GameObject box;

    public Vector2 topLeftCorner;
    public Vector2 topRightCorner;
    public Vector2 bottomLeftCorner;
    public Vector2 bottomRightCorner;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindData : MonoBehaviour
{
    public Sprite sprite;
    public Quaternion rotation;
    public Vector3 position;
    public Vector3 scale;

    public RewindData(Sprite sprite, Quaternion rotation, Vector3 position, Vector3 scale)
    {
        this.sprite = sprite;
        this.rotation = rotation;
        this.position = position;
        this.scale = scale;
    }
}

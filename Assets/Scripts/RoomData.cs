using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    public Vector3 spawnPoint = new Vector3();
    public float diameter;

    public RoomData(Vector3 sp, float d)
    {
        this.spawnPoint = sp;
        this.diameter = d;
    }

}

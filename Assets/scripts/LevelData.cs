using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelDataSpace
{
    public class Colors
    {
        public static List<Color> colors = new List<Color>() {
            new Color(0.6f,0.39f,0.01f), // orange
            new Color(0.01f,0.25f,0.74f), // blue
            new Color(0.39f,0.01f,0.6f), // violet
            new Color(0.25f,0.74f,0.01f), // green
        };
    }
    [Serializable]
    public class LevelData 
    {
        public List<RoomData> cells;
    }
    [Serializable]
    public class RoomData
    {
        public enum RoomType
        {
            passible,
            impassible,
            leftWin,
            rightWin,
            upWin,
            downWin,
        }

        public RoomType type = RoomType.passible;
        public Vector3 smokeDirection = Vector3.zero;
        public int prefabIndex;
    }
}
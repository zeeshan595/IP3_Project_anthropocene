﻿using UnityEngine;

public enum TeamType
{
    Red,
    Blue
}

public class PlayerStats : MonoBehaviour
{
    public float gunRange = 50;
    public TeamType team;
    public float speed = 10;
    public float gravity = 0.98f;
}
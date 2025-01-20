using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static int GetSign(int value)
    {
        return value < 0 ? -1 : 1;
    }

    public static float GetSign(float value)
    {
        return value < 0 ? -1 : 1;
    }

    public static float RandomSignF()
    {
        return Random.value >= 0.5 ? 1 : -1;
    }
    public static int RandomSignI()
    {
        return Random.value >= 0.5 ? 1 : -1;
    }

    public static Vector3 RandomVector()
    {
        return new Vector3(RandomSignF() * Random.value, RandomSignF() * Random.value, RandomSignF() * Random.value);
    }
}

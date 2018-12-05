using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialCircle
{

    public readonly Vector3 Center;
    public float Radius;
    public int MemberCount;

    public SocialCircle(Vector3 center)
    {
        Center = center;
        Radius = 1.5f;
        MemberCount = 0;
    }

    public void Join()
    {
        MemberCount++;
    }

    public void Leave()
    {
        MemberCount--;
    }
}

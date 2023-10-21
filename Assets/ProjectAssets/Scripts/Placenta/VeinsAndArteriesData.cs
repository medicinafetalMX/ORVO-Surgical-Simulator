using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct FetoscopyTargetPointData
{
    public Vector3 position;
    public CirculationIntersectionType type;
}

public enum CirculationIntersectionType
{
    ArterieArterie,
    ArterieVein,
    VeinVein
}

public enum CirculationType
{
    Arterie,
    Vein
}
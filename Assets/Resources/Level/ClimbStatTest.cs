using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chien/Climb Stat Test")]
public class ClimbStatTest : ScriptableObject
{
    [Range(1, 32)] public float forceValue;
    [Range(0.25f, 0.5f)] public float disableTime;
    [Range(0.02f, 0.2f)] public float deltaDisableTime;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SeatStats : MonoBehaviour
{
    public int index;//index of each seat(lowest seat=0, highest seat=3--depending on each row ofc)
    public bool hasSpawnedPassenger = false;//true when a passenger is spawned there
}

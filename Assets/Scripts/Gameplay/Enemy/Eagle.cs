using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class Eagle : EnemyMove
{
    private float time;
    private float maxTime = 2f;
    [SerializeField] private GameObject originalEgg;
    [SerializeField] private Transform shootingPoint;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (distance <= rangeCheck)
        {
            DropEgg();
        }
    }

    private void DropEgg()
    {
        time += Time.fixedDeltaTime;
        if (time >= maxTime)
        {
            GameObject newEgg = Instantiate(originalEgg, shootingPoint.position, Quaternion.identity);
            newEgg.SetActive(true);
            MasterAudio.PlaySound(Constants.Audio.SOUND_EAGLE_SHOOTING);
            time = 0;
        }
    }
}

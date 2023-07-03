using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public bool isDie;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            if (!isDie)
            {
                GameData.mapExtra = TypeMapExtra.MiniWater;
                if (!PlayerMovement.instance.groundCheck.isGround)
                    PlayerMovement.instance.OnWaterEnter();
                MasterAudio.PlaySound(Constants.Audio.SOUND_DIVEIN_WATER);
            }
            PlayerMovement.instance.body.GetComponent<MeshRenderer>().sortingLayerName = "Default";
        }
        if (collision.CompareTag(Constants.TAG.BOMB))
        {
            MasterAudio.PlaySound(Constants.Audio.SOUND_DIVEIN_WATER);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            if (!isDie)
            {
                GameData.mapExtra = TypeMapExtra.None;
                PlayerMovement.instance.anim.SetBool("isSwimming", false);
            }
            PlayerMovement.instance.body.GetComponent<MeshRenderer>().sortingLayerName = "Ground";
        }
    }
}

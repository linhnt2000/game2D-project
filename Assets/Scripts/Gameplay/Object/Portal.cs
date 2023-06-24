using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform endDestination;
    [SerializeField] private Transform startDestination;
    [SerializeField] private bool startPortal;
    private int startDir;
    private int endDir;

    private void Start()
    {
        if (startDestination.transform.eulerAngles.y == 0)
        {
            startDir = 1;
        }
        else startDir = -1;

        if (endDestination.transform.eulerAngles.y == 0)
        {
            endDir = 1;
        }
        else endDir = -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            //DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.SOUND_PORTAL);
            if (startPortal)
            {
                if (startDestination.transform.eulerAngles.z == 0)
                {
                    PlayerMovement.instance.transform.position = new Vector2(endDestination.position.x + endDir * 0.75f, endDestination.position.y - 0.5f);
                }
                else
                {
                    PlayerMovement.instance.MoveToEndPortal(endDestination);
                }
            }
            else
            {
                if (startDestination.transform.eulerAngles.z == 0)
                {
                    PlayerMovement.instance.transform.position = new Vector2(startDestination.position.x - startDir * 0.75f, startDestination.position.y - 0.5f);
                }
                else
                {
                    PlayerMovement.instance.MoveToStartPortal(startDestination);
                }
            }
        }
    }
}

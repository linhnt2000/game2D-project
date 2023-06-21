using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    private enum BrickType
    {
        Default,
        Ocean,
        Desert,
        Ice
    }
    [SerializeField] private BrickType brickType;

    public void Disappear()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.BULLET))
        {
            Disappear();
            Boom();
            PlayerMovement.instance.playerAction.BreakBlockToKillEnemy(gameObject);
        }
    }
    public void Boom()
    {
        //if (brickType == BrickType.Ocean)
        //{
        //    GameController.instance.GetPoolBreakFx(GameController.BrickType.Ocean).Boom(transform.position);
        //}
        //else if (brickType == BrickType.Desert)
        //{
        //    GameController.instance.GetPoolBreakFx(GameController.BrickType.Desert).Boom(transform.position);
        //}
        //else if (brickType == BrickType.Ice)
        //{
        //    GameController.instance.GetPoolBreakFx(GameController.BrickType.Ice).Boom(transform.position);
        //}
        GameController.instance.GetPoolBreakFx(GameController.BrickType.Default).Boom(transform.position);
    }
}

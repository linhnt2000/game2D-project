using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.skeletonMecanim.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Default";
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG.PLAYER))
        {
            PlayerMovement.instance.skeletonMecanim.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Ground";
        }
    }
}

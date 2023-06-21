using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Magnet : MonoBehaviour
{
    [SerializeField] private GameObject coinDetector;
    private float duration = Constants.MAGNET_DURATION;
    private BoxCollider2D magnetCol;

    private void Start()
    {
        coinDetector.SetActive(false);
        magnetCol = GetComponent<BoxCollider2D>();
        magnetCol.enabled = false;
        StartCoroutine(Helper.StartAction(() => magnetCol.enabled = true, 0.1f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            StartCoroutine(ActivateMagnet());
        }
    }

    IEnumerator ActivateMagnet()
    {
        //ItemTabController.instance.magnetIcon.SetActive(false);
        //ItemTabController.instance.magnetIcon.SetActive(true);
        coinDetector.SetActive(true);
        MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);
        transform.SetParent(PlayerMovement.instance.body);
        transform.localPosition = Vector3.zero;
        //if (transform.localScale.x < 0)
        //{
        //    transform.localScale *= -1;
        //}
        GetComponent<SkeletonAnimation>().Skeleton.SetColor(Vector4.zero);
        Destroy(GetComponent<Rigidbody2D>());
        magnetCol.enabled = false;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class GemConvert : MonoBehaviour
{
    [SerializeField] private float speed;
    private SpriteRenderer mySprite;

    [SerializeField] private bool star;

    private void OnEnable()
    {
        mySprite = GetComponent<SpriteRenderer>();
        MasterAudio.PlaySound(Constants.Audio.SOUND_GEM_COLLECT);
        StartCoroutine(Helper.StartAction(() => Disappear(), 1f));
    }

    private void Disappear()
    {
        mySprite.DOFade(0, 0.5f).OnComplete(() =>
        {
            if (!star)
            {
                UIGameController.instance.gemUI.transform.DOScale(new Vector2(1f, 1f), 0.3f).OnComplete(() =>
                {
                    UIGameController.instance.gemUI.transform.transform.localScale = new Vector2(0.8f, 0.8f);
                    gameObject.SetActive(false);
                });
            }
            else gameObject.SetActive(false);
        });
    }
}

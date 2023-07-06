using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClaimCoinFx : MonoBehaviour {
    List<Transform> coins;
    private void Start() {

    }
    public void PlayCoin(Transform posEnd) {
        Time.timeScale = 1;
        coins = new List<Transform>();
        foreach (Transform child in transform) {
            coins.Add(child);
        }
        StartCoroutine(PlayAnim(posEnd));
    }
    IEnumerator PlayAnim(Transform posEnd) {
        for (int i = 0; i < coins.Count; i++) {
            Vector2 target = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            coins[i].transform.DOMove(target, 0.4f).SetEase(Ease.OutBack);
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < coins.Count; i++) {
            coins[i].DOMove(posEnd.position, 1.2f);
            coins[i].GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1);
        }
        yield return new WaitForSeconds(1.2f);
        gameObject.SetActive(false);
    }

}

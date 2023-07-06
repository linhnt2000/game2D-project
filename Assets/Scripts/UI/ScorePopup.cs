using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScorePopup : MonoBehaviour {
    public TextMeshPro textMesh;
    private void OnEnable() {
        transform.DOMoveY(transform.position.y + 1f, 1f).OnComplete(() => {
            textMesh.DOFade(0, 0.5f).OnComplete(() => {
                gameObject.SetActive(false);
                textMesh.color = new Color(1, 1, 1, 1);
            });
        });
    }
}

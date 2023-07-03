using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public ProCamera2D proCamera2D;
    [SerializeField] ProCamera2DShake proCamera2DShake;
    public GameObject bossLimit;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        proCamera2D = GetComponent<ProCamera2D>();
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        proCamera2D.AddCameraTarget(PlayerMovement.instance.transform);
    }
    public void Move(Vector3 movePos)
    {
        transform.DOMove(movePos, 1f);
        proCamera2D.enabled = false;
    }
    public void EnableShake()
    {
        proCamera2DShake.enabled = true;
        proCamera2DShake.Shake(0);
    }
}

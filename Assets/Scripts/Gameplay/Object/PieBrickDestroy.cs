using DarkTonic.MasterAudio;
using DG.Tweening;
using UnityEngine;

public class PieBrickDestroy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] rb;

    [SerializeField] private GameObject[] obj;

    [SerializeField] private BrickPiece[] stoneSprite;
    Vector3[] posStart;
    void Start()
    {
        posStart = new Vector3[obj.Length];
        for (int i = 0; i < obj.Length; i++)
        {
            posStart[i] = obj[i].transform.localPosition;
        }
    }

    void ResetPiece()
    {
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i].transform.localPosition = posStart[i];
            obj[i].SetActive(false);
            stoneSprite[i].spriteRenderer.DOKill();
            stoneSprite[i].spriteRenderer.color = new Color(1, 1, 1, 1);
        }
        gameObject.SetActive(false);
    }
    public void Boom(Vector3 posBoom)
    {
        transform.position = new Vector2(posBoom.x, posBoom.y + 0.5f);
        for (int i = 0; i < rb.Length; i++)
        {
            obj[i].SetActive(true);
        }
        MasterAudio.PlaySound(Constants.Audio.SOUND_DESTROY_BRICK);
        rb[0].AddForce(new Vector2(-45, 200));
        rb[1].AddForce(new Vector2(-15, 250));
        rb[2].AddForce(new Vector2(15, 250));
        rb[3].AddForce(new Vector2(45, 200));
        Invoke("ResetPiece", 3);
    }
}

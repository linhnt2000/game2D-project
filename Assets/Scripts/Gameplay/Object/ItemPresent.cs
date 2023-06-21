using DarkTonic.MasterAudio;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class ItemPresent : MonoBehaviour
{
    private Rigidbody2D itemRb;

    private bool hitFirstTime;
    public bool canKillEnemy;

    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceHeight;
    private Vector2 originalPosition;
    //public Animator anim;

    public ResourceType itemType;

    private void Start()
    {
        originalPosition = transform.position;
        hitFirstTime = true;
        canKillEnemy = true;
    }
    public virtual void BounceAndItemAppear()
    {
        if (hitFirstTime)
        {
            hitFirstTime = false;
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<SkeletonAnimation>().enabled = false;
            MasterAudio.PlaySound(Constants.Audio.SOUND_DESTROY_ITEM_BLOCK);
            if (itemType == ResourceType.LadderItem)
            {
                //LadderItem ladder = SpawnItem().GetComponent<LadderItem>();
                //ladder.ScaleLadder();
            }
            else if (itemType != ResourceType.Coin)
            {
                itemRb = SpawnItem().GetComponent<Rigidbody2D>();
                itemRb.bodyType = RigidbodyType2D.Dynamic;
                itemRb.gravityScale = 2;
                itemRb.drag = 1;
                StartCoroutine(Helper.StartAction(() => itemRb.GetComponent<Collider2D>().isTrigger = false, 0.2f));
                itemRb.AddForce(new Vector2(Random.Range(-50, 50), 450));
                StartCoroutine(Bounce());
            }
            else
            {
                SpawnItem().GetComponent<ElementObject>().AddCoinHitBlock();
                StartCoroutine(Bounce());
            }
        }
    }
    IEnumerator Bounce()
    {
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + bounceSpeed * Time.deltaTime);
            if (transform.position.y >= originalPosition.y + bounceHeight)
            {
                break;
            }
            yield return null;
        }
        while (true)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - bounceSpeed * Time.deltaTime);
            if (transform.position.y <= originalPosition.y)
            {
                transform.position = originalPosition;
                break;
            }
            yield return null;
        }
    }

    private GameObject SpawnItem()
    {
        return Instantiate(Resources.Load<GameObject>("Items/" + itemType.ToString()), transform.position, Quaternion.identity);
    }
}

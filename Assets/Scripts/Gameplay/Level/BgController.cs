using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bg1;
    [SerializeField] private SpriteRenderer bgMapExtra;
    [SerializeField] private float speed;
    [SerializeField] private Transform lstBg;
    private float x;
    private void Start()
    {
        if (LevelController.instance.typeMap == TypeMap.Desert)
        {
            transform.position = new Vector2(transform.position.x, 1);
        }
        else if (LevelController.instance.typeMap == TypeMap.Water)
        {
            transform.position = new Vector2(transform.position.x, 0);
        }
        else if (LevelController.instance.typeMap == TypeMap.Mine)
        {
            transform.position = new Vector2(transform.position.x, 1.2f);
        }
        else transform.position = new Vector2(transform.position.x, -1);
        bg1.sprite = BgMapData.Instance.bgMap[LevelController.instance.typeMap].bgFirst;
        for (int i = 0; i < lstBg.childCount; i++)
        {
            SpriteRenderer sprite = lstBg.GetChild(i).GetComponent<SpriteRenderer>();
            sprite.sprite = BgMapData.Instance.bgMap[LevelController.instance.typeMap].bgSecond;
            x = sprite.sprite.bounds.size.x * 0.79f;
            if (i == 1)
            {
                sprite.flipX = true;
            }
            if (i < lstBg.childCount - 1)
                lstBg.GetChild(i + 1).position = lstBg.GetChild(i).position + new Vector3(x, 0, 0);
        }
        StartCoroutine(SetTemp());
    }
    IEnumerator SetTemp()
    {
        yield return new WaitForSeconds(1);
    }

    private void FixedUpdate()
    {
        if (PlayerMovement.instance != null)
        {
            if (PlayerMovement.instance.transform.position.x > lstBg.GetChild(1).position.x)
            {
                lstBg.GetChild(0).position = lstBg.GetChild(2).position + new Vector3(x, 0, 0);
                lstBg.GetChild(0).GetComponent<SpriteRenderer>().flipX = !lstBg.GetChild(2).GetComponent<SpriteRenderer>().flipX;
                lstBg.GetChild(0).SetAsLastSibling();
            }
            if (PlayerMovement.instance.transform.position.x < lstBg.GetChild(0).position.x)
            {
                lstBg.GetChild(2).position = lstBg.GetChild(0).position - new Vector3(x, 0, 0);
                lstBg.GetChild(2).GetComponent<SpriteRenderer>().flipX = !lstBg.GetChild(0).GetComponent<SpriteRenderer>().flipX;
                lstBg.GetChild(2).SetAsFirstSibling();
            }
        }       
    }
    public void SetBgMapNormal()
    {
        bg1.gameObject.SetActive(true);
        bg1.sprite = BgMapData.Instance.bgMap[LevelController.instance.typeMap].bgFirst;
    }

}

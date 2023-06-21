using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public TypeMap typeMap;
    public TypeMapExtra mapExtra;
    public Transform posMinX;
    public Transform posMaxX;
    public Transform posMinY;
    public Transform posMaxY;
    public static LevelController instance;
    public Transform posPlayer;
    public bool useTop;
    public Transform posFlag;

    public bool bossLevel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        GameData.typeMap = typeMap;
        GameData.mapExtra = mapExtra;
    }
    private void Start()
    {
        if (typeMap == TypeMap.Springs)
        {
            GameObject fx = Instantiate(Resources.Load<GameObject>("Fx/Leaf drop 1"));
            fx.transform.SetParent(transform);
        }
        else if (typeMap == TypeMap.Water)
        {
            GameObject fx = Instantiate(Resources.Load<GameObject>("Fx/Bubble"));
            fx.transform.SetParent(transform);
        }

        if (mapExtra == TypeMapExtra.RunLevel)
        {
            StartCoroutine(Helper.StartAction(() =>
            {
                GameObject bees = Instantiate(Resources.Load<GameObject>("Fx/BeeRun"));
                bees.transform.position = new Vector3(PlayerMovement.instance.transform.position.x - 2, PlayerMovement.instance.transform.position.y + 1.5f, 0);
            }, 0.2f));
        }

        if (bossLevel)
        {
            Flag.instance.gameObject.SetActive(false);
        }
    }

    //void SetTotalScore()
    //{
    //    List<ElementObject> scoreObjects = FindObjectsOfType<ElementObject>().ToList();
    //    for (int i = 0; i < scoreObjects.Count; i++)
    //    {
    //        totalScore += scoreObjects[i].score;
    //    }
    //    totalScoreForProgress = (int)(totalScore * threeStar);
    //}
}

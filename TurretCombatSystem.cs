using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class TurretCombatSystem : MonoBehaviour {
    // Use this for initialization
    //GameObject[] players;
    GameObject closest;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    float closestDistance;
    public float radius = 5;
    //public string targetTag="player";
    public string[] targetTags;
    private int battleState = 0;  // 0 == no battle, 1== battle going on

    public int damage = 10;

    public int fireInterval = 60;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (closest != null && closestDistance <radius)
        {
            transform.LookAt(closest.transform.position + new Vector3(0, 0, 0));
        }

        if (Time.frameCount % fireInterval == 0)
        {
            checkBattleCondition();
            if (battleState == 1 && closest != null)
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        GameObject temp = Instantiate(bullet, bulletSpawnPoint.transform.position, transform.rotation);
    }

    void checkBattleCondition()
    {
        closest = findClosestEnemy();
        if (closest == null)
        {
            closestDistance = Mathf.Infinity;
        }
        else
        {
            closestDistance = distanceFromSelf(closest);

        }
        if (closestDistance < radius)
        {
            battleState = 1;
        }
        else
        {
            battleState = 0;
        }
        //Debug.Log("conditionCheck");
    }

    GameObject findClosestEnemy()
    {

        GameObject[] players = { };

        for (int i = 0; i < targetTags.Length; i++)
        {
            players = players.Concat(GameObject.FindGameObjectsWithTag(targetTags[i])).ToArray();
        }

        if (gameObject.tag == "enemy")
        {
            players = players.Concat(GameObject.FindGameObjectsWithTag("essentialSite")).ToArray();
        }

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject player in players)
        {
            Vector3 diff = player.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = player;
                distance = curDistance;
            }
        }
        return closest;
    }

    float distanceFromSelf(GameObject obj)
    {
        Vector3 diff = obj.transform.position - transform.position;
        float curDistance = diff.sqrMagnitude;
        return curDistance;
    }
}

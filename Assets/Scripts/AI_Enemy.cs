using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    public CombatController cc;
    public PlayerBehaviour pb;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float d1;
    [SerializeField]
    private float d2;
    [SerializeField]
    private float d3;
    [SerializeField]
    private float d4;
    [SerializeField]
    private float d5;
    [SerializeField]
    private float DecTime = 0.1f;
    [SerializeField]
    private bool canDecide;
    private void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("p1");
        player = gameObjects[0];
        canDecide=true;

    }
    private void Update() {
        distance = Mathf.Abs(player.transform.position.x - transform.position.x);
        if(canDecide)
        {
            Decide(distance);
            StartCoroutine(DecisionTime(DecTime));
        }
        
    }
    private void Decide(float dist)
    {
        if(dist >d1)
        {
            pb.moveInput = new Vector2(-0.9f, -0.9f);
        }
        else if(dist <d2)
        {
            pb.moveInput = new Vector2(0f, 0f);
            Random_attack();
        }
        else if(dist <d3)
        {
            Random_attack();
        }
    }
    private void Random_attack()
    {
        int attackID = Random.Range(0, 4);
        if(attackID == 1)
        {
            cc.AI_Jab();
        }
        if(attackID == 2)
        {
            cc.AI_Right_Hook();
        }
        if(attackID == 3)
        {
            cc.AI_Uppercut();
        }
    }
    private IEnumerator Combo( int combo, float delay)
    {
        if(combo == 1)
        {
            cc.AI_Uppercut();
            yield return new WaitForSeconds(delay);
            cc.AI_Right_Hook();
            yield return new WaitForSeconds(delay);
            cc.AI_Right_Hook();

        }
        else if(combo == 2)
        {
            cc.AI_Jab();
            yield return new WaitForSeconds(delay);
            cc.AI_Uppercut();
            yield return new WaitForSeconds(delay);
            cc.AI_Right_Hook();
        }
        else if (combo ==3)
        {
            cc.AI_Jab();
            yield return new WaitForSeconds(delay);
            cc.AI_Jab();
            yield return new WaitForSeconds(delay);
            cc.AI_Right_Hook();
        }
    }
    private IEnumerator DecisionTime(float decisiontime)
    {
        canDecide=false;
        yield return new WaitForSeconds(decisiontime);
        canDecide=true;
    }

}

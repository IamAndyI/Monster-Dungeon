using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwipeEffect : MonoBehaviour
{
    RaycastHit2D[] swipeHit;
    float spinRadius = 3;
    Boss myBossScript;

    // Start is called before the first frame update
    void Start()
    {
        
        myBossScript = FindObjectOfType<Boss>();
    }

    void Update()
    {
        swipeHit = Physics2D.CircleCastAll(transform.position, spinRadius, Vector2.zero);
        Destroy(gameObject, 1);
    }

    public void SwipeAttack()
    {
        if (swipeHit != null)
        {
            for (int i = 0; i < swipeHit.Length; i++)
            {
                if (swipeHit[i].collider != null && swipeHit[i].collider.gameObject.tag == "Enemy")
                {
                    swipeHit[i].collider.gameObject.GetComponent<EnemyMovement>().LoseHealth(myBossScript.powerValue);
                }
            }
        }
    }
}

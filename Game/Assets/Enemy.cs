using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    

    public float health;
    public float maxHealth = 10f;
    public float damage = 1f;

    Animator animator;
    public StatusBarBehavior healthBar;
    public GameObject floatingPoints;

    private void Start()
    {
        animator = GetComponent<Animator>();
        health = maxHealth;
        healthBar.SetHealth(health, maxHealth);
    }

    //damage handler
    public void TakeDamage(float damageAmount) 
    {
        health -= damageAmount;
        healthBar.SetHealth(health, maxHealth);
        //floating hit text
        GameObject hitPoints = Instantiate(floatingPoints, transform.position, Quaternion.identity) as GameObject;
        hitPoints.transform.GetChild(0).GetComponent<TextMesh>().text = "" + damageAmount + "[" + health + "]";
        print(health);
        if(health <= 0)
        {
            Defeated();
        }
    }
    public void Defeated()
    {
        animator.SetTrigger("defeated");
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    //attack handler
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            var playerComponent = other.GetComponent<PlayerController>();
            if(playerComponent != null)
            {
                playerComponent.TakeDamage(damage);
            }
        }
    }

}

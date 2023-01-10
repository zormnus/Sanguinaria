using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    private Animator m_animator;

    void Start ()
    {
        m_animator = GetComponent<Animator>();
    }

    // public void Death(){
    //     if (health <= 0){
    //         m_animator.SetTrigger("Death");
    //     }
    // }
    
    public void TakeDamage(int damage){        
        if (health > 0){
            health -= damage;
            m_animator.SetTrigger("Hurt");
        }
        if (health <= 0){
            m_animator.SetTrigger("Death");
        }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float JumpForce;
    private float moveInput;

    private Rigidbody2D rb;

    private bool FacingRight = true;

    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public Animator anim;

    //
    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;
    public int damage;
    //

    private void Start(){
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isRecharged = true;
    }

    private void FixedUpdate() {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(FacingRight == false && moveInput > 0){
            Flip();
        }
        else if(FacingRight == true && moveInput <0){
            Flip();
        }

        if (!isAttacking && moveInput != 0) State = States.Run;
    }

    private void Update(){
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if(isGrounded == true && Input.GetButtonDown("Jump")){
            rb.velocity = Vector2.up * JumpForce;
        }

        if (!isAttacking && moveInput == 0) State = States.Idle;  
        if (!isAttacking && !isGrounded) State = States.Jump;

        if (Input.GetButtonDown("Fire1")){
            Attack();
        }

    }


    private void Flip(){
        FacingRight = !FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator AttackAnimation(){
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

     private IEnumerator AttackCoolDown(){
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }   

    private void Attack(){
        if(isGrounded && isRecharged){

            State = States.Attack;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }

    private void OnAttack(){
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < enemies.Length; i++){
            enemies[i].GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private States State{
         get { return (States)anim.GetInteger("state"); }
         set { anim.SetInteger("state", (int)value); }
    }

    public enum States{
        Idle,
        Run,
        Jump,
        Attack
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}

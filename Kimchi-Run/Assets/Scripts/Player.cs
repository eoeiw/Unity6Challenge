using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float JumpForce;

    [Header("Reference")]
    public Rigidbody2D PlayerRigidBody;

    private bool isGrounded = true;
    public Animator PlayerAnimator;

    public BoxCollider2D PlayerCollider;

    private bool isInvincible = false;

    private int isCheat = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&isGrounded)
        {
            PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            PlayerAnimator.SetInteger("State", 1);
        }

    }
    public void ActivateInvincibility(){ /// 치트모드 함수
        isInvincible = true;
        isCheat = 1;
    }

    public void KillPlayer(){
        PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
        PlayerCollider.enabled = false;
        PlayerAnimator.enabled = false;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground"){
            if (!isGrounded)
            {
                PlayerAnimator.SetInteger("State", 2);
            }
            isGrounded = true;
            // ���� �� Platform �ݶ��̴��� Player �ݶ��̴� �浹 �� isGrounded bool�� true. 
        }
    }

    void Hit(){
        GameManager.Instance.Lives -= 1;
    }

    void Heal(){
        GameManager.Instance.Lives = Mathf.Min(3, GameManager.Instance.Lives + 1); // Mathf.Min 메소드를 사용하여 lives + 1 변수와 3을 비교하여 작은 값을 반환
    }

    void StartInvincible(){
        isInvincible = true;
        Invoke("StopInvincible", 5f); // Invoke 메소드를 사용하여 StopInvincible 메소드를 5초 뒤에 호출
    }

    void StopInvincible(){
        isInvincible = false;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Enemy"){
            if(!isInvincible){
                Destroy(collider.gameObject);
                Hit();
            }
        }
        else if(collider.gameObject.tag == "Food"){
            Destroy(collider.gameObject);
            Heal();
        }
        else if(collider.gameObject.tag == "Golden"){
            if(isCheat == 0){
                Destroy(collider.gameObject);
                CancelInvoke("StopInvincible");
                StartInvincible();
            }
            else{
            Destroy(collider.gameObject);
            }
        }
    }
}

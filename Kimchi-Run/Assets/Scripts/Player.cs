using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

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

    public AudioClip[] soundEffects;
    // 0 : Jump
    public AudioSource[] audioSources;

    [Header("Change Color")]
    public SpriteRenderer spriteRenderer; // 플레이어의 SpriteRenderer
    private Color originalColor; // 원래 색상을 저장할 변수



    void Awake() {
        originalColor = spriteRenderer.color;
    }
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space)&&isGrounded) || Input.touchCount > 0 && isGrounded)
        {
            if(Input.touchCount > 0){
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began){
                    PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
                    PlayEffect(0);
                    isGrounded = false;
                    PlayerAnimator.SetInteger("State", 1);
                }
            }
            PlayerRigidBody.AddForceY(JumpForce, ForceMode2D.Impulse);
            PlayEffect(0);
            isGrounded = false;
            PlayerAnimator.SetInteger("State", 1);
        }

    }
    public void ActivateInvincibility(){ /// 치트모드 함수
        isInvincible = true;
        isCheat = 1;
        spriteRenderer.color = new Color(1, 1, 1, 0.7f);
    }

    public void PlayEffect(int soundIndex)
    {
        audioSources[soundIndex].Play();
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
        }
    }

    void Hit(){
        GameManager.Instance.Lives -= 1;
        spriteRenderer.color = Color.red;
        Invoke("ResetColor", 0.2f);
    }

    void Heal(){
        GameManager.Instance.Lives = Mathf.Min(3, GameManager.Instance.Lives + 1); // Mathf.Min 메소드를 사용하여 lives + 1 변수와 3을 비교하여 작은 값을 반환
    }

    void StartInvincible()
    {
        isInvincible = true;
        //StartCoroutine(StartChangeColor());
        Invoke("StopInvincible", 100f); // 무적 상태 5초 지속
    }

    void StopInvincible(){
        isInvincible = false;
    }

    IEnumerator StartChangeColor()
    {
    float blinkDuration = 5f; // 총 깜빡이는 시간
    float firstPhaseDuration = 4f; // 첫 번째 단계의 시간
    float elapsedTime = 0f;

    while (elapsedTime < blinkDuration)
    {
        if (elapsedTime < firstPhaseDuration)
        {
            // 처음 4초 동안 유지되는 투명도 변화
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(firstPhaseDuration - elapsedTime);
            elapsedTime = firstPhaseDuration;
        }
        else
        {
            // 마지막 1초 동안 깜빡이는 효과
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1, 1, 1, 0.9f);
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.2f;
        }
    }

    // 무적 상태가 종료되면 원래 색상으로 복귀
    ResetColor();
    }
    private void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }


    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Enemy"){
            if(!isInvincible){
                Destroy(collider.gameObject);
                PlayEffect(2);
                Hit();
            }
        }
        else if(collider.gameObject.tag == "Food"){
            Destroy(collider.gameObject);
            PlayEffect(1);
            Heal();
        }
        else if(collider.gameObject.tag == "Golden"){
            if(isCheat == 0){
                Destroy(collider.gameObject);
                PlayEffect(1);
                CancelInvoke("StopInvincible");
                StartInvincible();
            }
            else{
            Destroy(collider.gameObject);
            }
        }
    }
}

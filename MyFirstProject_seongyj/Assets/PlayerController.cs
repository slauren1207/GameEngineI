using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5.0f;

    [Header("점프 설정")]
    public float jumpForce = 10.0f;

    private Animator animator; //애니메이터 flicker 방지

    private Rigidbody2D rb;

    private SpriteRenderer sprite; //왼오 뒤집기

    private bool isGrounded = false;
    private int score = 0;  // 점수 추가

    // 리스폰용 시작 위치 - 새로 추가!
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // 게임 시작 시 위치를 저장 - 새로 추가!
        startPosition = transform.position;
        Debug.Log("시작 위치 저장: " + startPosition);

        sprite.flipX = false;
    }

    void Update()
    {
        // 좌우 이동 뒤집기 포함!
        float moveX = 0f;
        //if (Input.GetKey(KeyCode.A)) moveX = -1f;
        //if (Input.GetKey(KeyCode.D)) moveX = 1f;
        if (Input.GetKey(KeyCode.A)) { moveX = -1f; sprite.flipX = true; }
        if (Input.GetKey(KeyCode.D)) { moveX = 1f; sprite.flipX = false; }

        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // flipX 처리 (불필요한 변경 방지)
        //if (moveX < 0 && !sprite.flipX) sprite.flipX = true;
        //else if (moveX > 0 && sprite.flipX) sprite.flipX = false;

        // Animator Run/Idle 전환
        animator.SetFloat("Speed", Mathf.Abs(moveX));


        // 점프  & 바닥 충돌 감지시 실행 가능
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("Jump", true);
        }
    }

    // 바닥 충돌 감지 (Collision)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);

        }

        // 장애물 충돌 감지 - 새로 추가!
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("⚠️ 장애물 충돌! 시작 지점으로 돌아갑니다.");

            // 장애물 충돌 시 생명 감소로 변경!
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("⚠️ 장애물 충돌! 생명 -1");
                // GameManager 찾아서 생명 감소
                GameManager gameManager = FindObjectOfType<GameManager>();

                if (gameManager != null)
                {
                    gameManager.TakeDamage(1);  // 생명 1 감소
                }

                // 짧은 무적 시간 (0.5초 후 원래 위치로)
                transform.position = startPosition;
                rb.velocity = Vector2.zero;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 코인 수집 (기존)
        if (other.CompareTag("Coin"))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddScore(10);
            }
            Destroy(other.gameObject);
        }
        // 골 도달 - 새로 추가!
        if (other.CompareTag("Goal"))
        {
            Debug.Log("🎉 Goal Reached!");
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameClear();  // 게임 클리어 함수 호출
            }
        }
    }
}

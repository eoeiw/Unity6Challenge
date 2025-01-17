using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Scroll speed of the background.")]
    public float scrollSpeed = 1f;

    [Header("References")]
    public GameObject background1; // 첫 번째 배경 스프라이트
    public GameObject background2; // 두 번째 배경 스프라이트

    private Vector3 startPos1;
    private Vector3 startPos2;

    // Start is called before the first frame update
    void Start()
    {
        // 배경의 초기 위치를 저장
        startPos1 = background1.transform.position;
        startPos2 = background2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 배경을 좌측으로 계속 이동시키기
        background1.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        background2.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // 첫 번째 배경이 화면을 벗어나면 두 번째 배경의 뒤로 위치를 변경
        if (background1.transform.position.x <= -background1.GetComponent<SpriteRenderer>().bounds.size.x)
        {
            background1.transform.position = startPos2 + new Vector3(background1.GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
        }

        // 두 번째 배경이 화면을 벗어나면 첫 번째 배경의 뒤로 위치를 변경
        if (background2.transform.position.x <= -background2.GetComponent<SpriteRenderer>().bounds.size.x)
        {
            background2.transform.position = startPos1 + new Vector3(background2.GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
        }
    }
}

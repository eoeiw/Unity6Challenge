using UnityEngine;

public class Hearts : MonoBehaviour
{
    public Sprite OnHeart; // OnHeart 스프라이트를 저장하기 위한 변수
    public Sprite OffHeart; // OffHeart 스프라이트를 저장하기 위한 변수
    public SpriteRenderer SpriteRenderer; // SpriteRenderer 컴포넌트를 저장하기 위한 변수
    public int LiveNumber;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.Lives >= LiveNumber)
        {
            SpriteRenderer.sprite = OnHeart;
        }
        else
        {
            SpriteRenderer.sprite = OffHeart;
        }
    }
}

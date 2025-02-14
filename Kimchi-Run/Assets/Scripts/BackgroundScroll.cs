using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How fast should the texture scroll?")]

    public float scrollSpeed;
    public float objectVariable = 1;

    [Header("References")]

    public MeshRenderer meshRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.mainTextureOffset += 
        new Vector2(scrollSpeed * (GameManager.Instance.CalculateGameSpeed() / objectVariable) * Time.deltaTime, 0); // GameManager 클래스의 CalculateGameSpeed 메소드를 사용하여 게임 속도를 계산
        // Time.deltaTime : 1프레임이 몇 초인지 알아내는 놈. 2프레임이라면 0.5, 4프레임이라면 0.25.
        // fps가 달라져도 일정한 순서를 유지하게 해줌
    }
}

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public enum GameState{
    Intro,
    Playing,
    Dead
}
public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance; // GameManager 클래스의 인스턴스를 저장하기 위한 변수
    public GameState State = GameState.Intro; // 게임 상태를 저장하기 위한 변수

    public int Lives = 3;

    [Header("Reference")]
    public GameObject IntroUI; // IntroUI 게임 오브젝트를 참조하기 위한 변수
    public GameObject EnemySpawner; // EnemySpawner 게임 오브젝트를 참조하기 위한 변수
    public GameObject FoodSpawner; // FoodSpawner 게임 오브젝트를 참조하기 위한 변수
    public GameObject GoldSpawner; // GoldSpawner 게임 오브젝트를 참조하기 위한 변수
    public Player PlayerScript; // Player 스크립트를 참조하기 위한 변수
    public GameObject DeadUI; // DeadUI 게임 오브젝트를 참조하기 위한 변수

    public TMP_Text scoreText; // scoreText 텍스트를 참조하기 위한 변수

    public float PlayStartTime;

    public List<KeyCode> cheatCode = new List<KeyCode> { 
        KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, 
        KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow, 
        KeyCode.B, KeyCode.A 
    }; // 치트 코드를 저장하기 위한 리스트 // ↑↑↓↓←→←→BA 유명하자나~

    private Queue<KeyCode> inputQueue = new Queue<KeyCode>(); // 입력을 저장하기 위한 큐
    int isCheat = 0;



    [Header("Change Sky")]
    public GameObject Sunset; // 에디터에서 연결할 대상
    public GameObject Night; // 에디터에서 연결할 대상
    public GameObject nightBuildings; // 에디터에서 연결할 대상
    public GameObject builngs2; // 에디터에서 연결할 대상


    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }
    void Start()
    {
        IntroUI.SetActive(true); // IntroUI 게임 오브젝트를 활성화
    }

    float CalculateScore(){
        return Time.time - PlayStartTime;
    }

    void SaveHighScore(){
        int score = Mathf.FloorToInt(CalculateScore());
        int currenthighScore = PlayerPrefs.GetInt("HighScore"); // PlayerPrefs 클래스의 GetFloat 메소드를 사용하여 HighScore 키에 저장된 값을 반환
        if(score > currenthighScore && isCheat == 0){ // 현재 점수가 하이 스코어보다 높고 치트 코드가 입력되지 않았을 때
            PlayerPrefs.SetInt("HighScore", score); // PlayerPrefs 클래스의 SetFloat 메소드를 사용하여 HighScore 키에 score 변수를 저장
            PlayerPrefs.Save(); // PlayerPrefs 클래스의 Save 메소드를 사용하여 저장
        }
    }

    int GetHighScore(){
        return PlayerPrefs.GetInt("HighScore", 0); // PlayerPrefs 클래스의 GetFloat 메소드를 사용하여 HighScore 키에 저장된 값을 반환
    }

    public float CalculateGameSpeed(){ // 게임 속도를 계산하는 메소드
        if(State != GameState.Playing){
            return 4f; // 게임 상태가 Playing이 아니면 5의 속도로 고정
        }
        float speed = 5f + (0.5f * Mathf.FloorToInt(CalculateScore() / 10f)); // speed 변수에 7 + (0.5 * (CalculateScore() / 10))을 저장 // 10초마다 0.5씩 증가
        return Mathf.Min(speed, 30f); // Mathf.Min 메소드를 사용하여 speed 변수와 30을 비교하여 작은 값을 반환 // 최대 속도를 30으로 제한
    }

    // Update is called once per frame
    void Update()
    {
        if(State == GameState.Intro){
            if (Input.GetKeyDown(KeyCode.Space))
            {
                State = GameState.Playing; // 게임 상태를 Playing으로 변경
                IntroUI.SetActive(false); // IntroUI 게임 오브젝트를 비활성화
                EnemySpawner.SetActive(true); // EnemySpawner 게임 오브젝트를 활성화
                FoodSpawner.SetActive(true); // FoodSpawner 게임 오브젝트를 활성화 
                GoldSpawner.SetActive(true); // GoldSpawner 게임 오브젝트를 활성화 
                PlayStartTime = Time.time;
            }

            // 치트 코드 감지
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    ProcessCheatCode(key);
                }
            }
        }


        if(State == GameState.Playing){
            if(isCheat == 0){
                scoreText.text = "Score : " + Mathf.FloorToInt(CalculateScore()); // 스코어 표시
            }
            else if(isCheat == 1){
                scoreText.text = "Mode : Cheat \nScore : " + Mathf.FloorToInt(CalculateScore()); // 스코어 표시
            }

            if (Mathf.FloorToInt(CalculateScore()) == 100) //100을 초과하는 시점은 매우 짧을 수 있음. 오류 발생시 ==으로 바꿔볼 것
            {
                Sunset.SetActive(true); // 그냥 바로 나타나게 설정
            }
            else if(Mathf.FloorToInt(CalculateScore()) == 200){ // Night는 200점부터.
                Night.SetActive(true);  
                nightBuildings.SetActive(true);
                builngs2.SetActive(false);
            }
        }
        else if(State == GameState.Dead){
            scoreText.text = "High Score: " + GetHighScore();
        }
        
        if(State == GameState.Playing && Lives <= 0){
            PlayerScript.KillPlayer();
            EnemySpawner.SetActive(false); 
            FoodSpawner.SetActive(false); 
            GoldSpawner.SetActive(false);
            DeadUI.SetActive(true);
            State = GameState.Dead;
            SaveHighScore(); // 하이 스코어 저장 추가
        }
        if(State == GameState.Dead && Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene("main"); // main 씬을 로드
        }
    }

    private bool IsCheatCodeEntered() // 치트 코드가 입력되었는지 확인하는 메소드
    {
        if (inputQueue.Count != cheatCode.Count) return false; // 큐의 크기와 치트 코드의 크기가 다르면 false 반환

        int index = 0;
        foreach (KeyCode key in inputQueue) // 큐의 모든 요소에 대해 반복
        {
            if (key != cheatCode[index]) return false;
            index++;
        }
        return true;
    }

    void ProcessCheatCode(KeyCode key)
    {
        // 입력된 키를 큐에 저장
        inputQueue.Enqueue(key);

        // 큐의 크기가 치트 코드보다 크면 가장 오래된 키 제거
        if (inputQueue.Count > cheatCode.Count)
        {
            inputQueue.Dequeue();
        }

        // 입력된 키와 치트 코드 비교
        if (IsCheatCodeEntered())
        {
            PlayerScript.ActivateInvincibility();
            State = GameState.Playing;
            IntroUI.SetActive(false); // IntroUI 게임 오브젝트를 비활성화
            EnemySpawner.SetActive(true); // EnemySpawner 게임 오브젝트를 활성화
            FoodSpawner.SetActive(true); // FoodSpawner 게임 오브젝트를 활성화 
            GoldSpawner.SetActive(true); // GoldSpawner 게임 오브젝트를 활성화 
            PlayStartTime = Time.time;
            isCheat = 1;
        }
    }
}

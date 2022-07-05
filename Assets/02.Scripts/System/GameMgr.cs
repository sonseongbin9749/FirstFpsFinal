using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMgr : MonoBehaviour
{

    

    // ���� �ؽ�Ʈ ���� ����
    public TMP_Text scoreText;

    // ���� ���� ����
    private int totalScore = 0;

    // ���� ������ ���� ����
    public GameObject monster;

    // ���� ���� ����
    public static float createTime = 3.0f;
    private float realcurrenttime = 0f;

    public GameObject player;

    // ���� �⿬�� ��ġ ���� List
    public List<Transform> points = new List<Transform>();

    // ���͸� �̸� �����ؼ� ������ List
    public List<GameObject> monsterPool = new List<GameObject>();

    // ������Ʈ Ǯ�� ������ ���� �ִ� ����
    public int maxMonsters = 150;

    // ������ ���� ���θ� �����ϴ� ��� ����
    private bool isGameOver;

    public float currenttime = 0f;

    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if(isGameOver)
            {
                CancelInvoke("CreateMonster");
                    
            }
        }
    }

    private static GameMgr instance;

    public static GameMgr GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<GameMgr>();

            if( instance == null )
            {
                GameObject container = new GameObject("GameMgr");
                instance = container.AddComponent<GameMgr>();
            }
        }
        return instance;
    }

    void Awake()
    {
        if (instance == null)
            instance = this;

        //DontDestroyOnLoad(this.gameObject);     ? 
        
    }

    void Start()
    {
        
        // ���ھ� ���� ���
        DisplayScore(0);

        // ���� ������Ʈ Ǯ ����
        CreateMonsterPool();

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        //spawnPointGroup?.GetComponentsInChildren<Transform>(points);

        //Transform[] pointArray = spawnPointGroup.GetComponentsInChildren<Transform>(true);

        foreach(Transform item in spawnPointGroup)
        {
            points.Add(item);
        }

        // ���� �ð� �������� ȣ��
        
    }

    private void Update()
    {
        
        realcurrenttime += Time.deltaTime;

        if (createTime < realcurrenttime)
        {

            int idx = Random.Range(0, points.Count);

            //Instantiate(monster, points[idx].position, points[idx].rotation);

            GameObject _monster = GetMonsterInPool();

            _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

            _monster?.SetActive(true);
            realcurrenttime = 0f;
        }

        if(isGameOver)
        {
            
            currenttime += Time.deltaTime;
            if (currenttime > 5)
            {
                DayText.day = 0;
                ItemText.scoreValue = 3;
                SceneManager.LoadScene("StartMenu");
            }
            
        }
        
    }

    void CreateMonster()
    {
        int idx = Random.Range(0, points.Count);

        //Instantiate(monster, points[idx].position, points[idx].rotation);

        GameObject _monster = GetMonsterInPool();

        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        _monster?.SetActive(true);
    }

    void CreateMonsterPool()
    {
        for(int i=0; i<maxMonsters; ++i)
        {
            // ���� ����
            var _monster = Instantiate<GameObject>(monster);

            // ���� �̸� ����
            _monster.name = $"Monster_{i:00}";

            // ���� ��Ȱ��ȭ
            _monster.SetActive(false);

            monsterPool.Add(_monster);
        }
    }

    public GameObject GetMonsterInPool()
    {
        
            foreach (var _monster in monsterPool)
            {
                if (_monster.activeSelf == false)
                    return _monster;
            }

            return null;
        
    }

    public void DisplayScore(int score)
    {
        totalScore += score;
        scoreText.text = $"<color=#00ff00>SCORE : </color> <color=#ff0000>{totalScore:#,##0}</color>";
    }
}

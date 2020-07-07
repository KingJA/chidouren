using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;

    public bool isSuperPacman = false;

    public List<GameObject> pacdots = new List<GameObject>();
    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> {0, 1, 2, 3};

    public Text remainText;
    public Text nowText;
    public Text scoreText;

    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;

    public static GameManager Instance {
        get { return _instance; }
    }

    private void Awake(){
        _instance = this;
        Screen.SetResolution(1024,768,false);
        int tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++) {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }

        foreach (Transform t in GameObject.Find("Maze").transform) {
            pacdots.Add(t.gameObject);
        }

        pacdotNum = GameObject.Find("Maze").transform.childCount;
    }

    private void Update(){
        if (nowEat==pacdotNum&&pacman.GetComponent<PacmanMove>().enabled!=false) {
            gamePanel.SetActive(false);
            Instantiate(winPrefab);
            StopAllCoroutines();
            setGameState(false);
        }

        if (nowEat==pacdotNum) {
            if (Input.anyKeyDown) {
                SceneManager.LoadScene(0);
            }
        }
        if (gamePanel.activeInHierarchy) {
            remainText.text = "Remain:\n" + (pacdotNum - nowEat);
            nowText.text = "Eaten:\n" + nowEat;
            scoreText.text = "Score:\n" + score;
        }
    }

    //随机生成一个超级豆子
    private void createSuperDot(){
        //防止出现协程异步和主线程的同时操作index
        if (pacdots.Count < 5) {
            return;
        }

        int superDotIndex = Random.Range(0, pacdots.Count);
        pacdots[superDotIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdots[superDotIndex].GetComponent<Pacdot>().isSuperPacdot = true;
    }

    public void eatDot(GameObject gameObject){
        nowEat++;
        score += 100;
        pacdots.Remove(gameObject);
    }

    public void eatSuperDot(){
        score += 200;
        Invoke("createSuperDot", 10f);
        isSuperPacman = true;
        freezeEnemy();
        StartCoroutine(recoverEnemy());
    }

    /**
     * 协程 延迟调用
     */
    IEnumerator recoverEnemy(){
        //停顿3秒后才执行后面逻辑
        yield return new WaitForSeconds(3f);
        disFreezeEnemy();
        isSuperPacman = false;
    }

    private void Start(){
        setGameState(false);
    }

    /**
     * 冻结敌人状态，停止，透明度0.7
     */
    private void freezeEnemy(){
        // enabled = false Update方法不执行
        blinky.GetComponent<GhostMove>().enabled = false;
        clyde.GetComponent<GhostMove>().enabled = false;
        inky.GetComponent<GhostMove>().enabled = false;
        pinky.GetComponent<GhostMove>().enabled = false;

        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }

    /**
     * 解冻敌人状态
     */
    private void disFreezeEnemy(){
        // enabled = false Update方法不执行
        blinky.GetComponent<GhostMove>().enabled = true;
        clyde.GetComponent<GhostMove>().enabled = true;
        inky.GetComponent<GhostMove>().enabled = true;
        pinky.GetComponent<GhostMove>().enabled = true;

        blinky.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject gameoverPrefab;
    public GameObject winPrefab;
    public GameObject startCountDownPrefab;
    public AudioClip startClip;

    public void startGame(){
        StartCoroutine(playStartCountDown());
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0, 0, -5));
        startPanel.SetActive(false);
    }

    IEnumerator playStartCountDown(){
        GameObject go = Instantiate(startCountDownPrefab);
        yield return new WaitForSeconds(4f);
        Destroy(go);
        setGameState(true);
        Invoke("createSuperDot", 10f);
        gamePanel.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void exitGame(){
        Application.Quit();
    }

    private void setGameState(bool enable){
        pacman.GetComponent<PacmanMove>().enabled = enable;
        blinky.GetComponent<GhostMove>().enabled = enable;
        clyde.GetComponent<GhostMove>().enabled = enable;
        inky.GetComponent<GhostMove>().enabled = enable;
        pinky.GetComponent<GhostMove>().enabled = enable;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostMove : MonoBehaviour {
    public GameObject[] wayPointsGo;
    private List<Vector3> wayPoints = new List<Vector3>();
    public float speed = 0.2f;
    private int index = 0;
    private Vector3 startPos;

    private void Start(){
        startPos = transform.position + new Vector3(0, 3, 0);
        initPath();
    }

    private void initPath(){
        wayPoints.Clear();
        foreach (Transform t in wayPointsGo[Random.Range(0, wayPointsGo.Length)].transform) {
            wayPoints.Add(t.position);
        }
        wayPoints.Insert(0,startPos);
        wayPoints.Add(startPos);
    }

    private void FixedUpdate(){
        if (transform.position != wayPoints[index]) {
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index], speed);
            //通过刚体来设置物体的位置
            GetComponent<Rigidbody2D>().MovePosition(temp);
        }
        else {
            index++;
            if (index >= wayPoints.Count) {
                index = 0;
                initPath();
            }
        }

        Vector2 dir = wayPoints[index] - transform.position;
        //将移动方向设置给动画状态机
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.name == "Pacman") {
            Destroy(other.gameObject);
        }
    }
}
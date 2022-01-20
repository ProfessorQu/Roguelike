using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public GameObject player;
    Rigidbody2D playerRB;

    public Transform[] startingPositions;
    public GameObject[] rooms;
    /*
        index 0 --> LR
        index 1 --> LRB
        index 2 --> LRT
        index 3 --> LRTB
    */

    public GameObject startingPlatform;

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.0f;

    public float minX, maxX;
    public float minY;
    [HideInInspector] public bool stopGeneration = false;

    public LayerMask whatIsRoom;

    private int downCounter = 0;

    private void Start() {
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.useTriggers = true;
        contactFilter2D.SetLayerMask(whatIsRoom);

        int startPosIdx = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[startPosIdx].position;

        Instantiate(rooms[0], transform.position, Quaternion.identity);

        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y + player.transform.localScale.y * 2);
        player.transform.position = playerPosition;

        playerRB = player.GetComponent<Rigidbody2D>();

        Instantiate(startingPlatform, transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update() {
        if (timeBtwRoom <= 0 && !stopGeneration) {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else {
            timeBtwRoom -= Time.deltaTime;
        }

        if (!stopGeneration) {
            playerRB.velocity = new Vector2(0, 0);
        }
    }

    private void Move() {
        if (direction == 1 || direction == 2) { // MOVE RIGHT
            if (transform.position.x < maxX) {
                downCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3) {
                    direction = 2;
                } else if (direction == 4) {
                    direction = 5;
                }
            }
            else {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4) { // MOVE LEFT
            if (transform.position.x > minX) {
                downCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;
                
                direction = Random.Range(3, 6);

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);
            }
            else {
                direction = 5;
            }
        }
        else if (direction == 5) { // MOVE DOWN
            downCounter++;

            if (transform.position.y > minY) {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
                RoomType roomType = roomDetection.GetComponent<RoomType>();

                if (roomType.type != 1 && roomType.type != 3) {
                    if (downCounter >= 2) {
                        roomType.DestroyRoom();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else {
                        roomType.DestroyRoom();

                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2) {
                            randBottomRoom = 1;
                        }

                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);
                
                direction = Random.Range(1, 6);
            } else {
                stopGeneration = true;
            }
        }
    }
}

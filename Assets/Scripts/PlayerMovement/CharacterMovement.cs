using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController characterController;
    private GameObject joystick;
    private GameObject playerCanvas;
    private RectTransform joystickRtf;
    private Vector2 stickPos;
    private Vector2 moveDir;
    private Vector3 finalVect;
    [SerializeField] private float moveSpeed = 2f;
    private Quaternion rotation = Quaternion.identity;
    private Animator animator;
    private float gravity = -4.5f;

    // Start is called before the first frame update
    void Start()
    {
        playerCanvas = GameObject.Find("PlayerCanvas");
        joystick = GameObject.Find("Joystick");
        joystickRtf = joystick.GetComponent<RectTransform>();
        Vector3 stick = joystickRtf.position;
        stickPos = new Vector2(stick.x, stick.y);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        finalVect = new Vector3(0, gravity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCanvas.activeSelf)
        {
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = touch.position;
            float distance = Vector2.Distance(touchPos, stickPos);
            if (distance < 700)
            {
                if (distance > 200)
                {
                    Vector2 normalizedVect = touchPos - stickPos;
                    normalizedVect.Normalize();
                    normalizedVect *= 200;
                    normalizedVect += stickPos;
                    joystickRtf.position = normalizedVect;
                }
                else
                {
                    joystickRtf.position = touchPos;
                }
                moveDir = new Vector2(joystickRtf.position.x - stickPos.x, joystickRtf.position.y - stickPos.y);
                moveDir.Normalize();
                Vector3 moveVect = new Vector3(moveDir.x, 0, moveDir.y);
                finalVect.x = moveDir.x;
                finalVect.z = moveDir.y;
                if (moveDir.x != 0 || moveDir.y != 0)
                {
                    rotation = Quaternion.LookRotation(moveVect, Vector3.up);
                    transform.rotation = rotation;
                }
                animator.SetBool("isMoving", true);
            }
            else
            {
                finalVect.x = 0;
                finalVect.z = 0;
                joystickRtf.position = stickPos;
                animator.SetBool("isMoving", false);
            }
        }
        else
        {
            finalVect.x = 0;
            finalVect.z = 0;
            joystickRtf.position = stickPos;
            animator.SetBool("isMoving", false);
        }
        characterController.Move(finalVect * (moveSpeed * Time.deltaTime));
    }
}

using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SwipeInput : MonoBehaviour
{

    // If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
    public const float MAX_SWIPE_TIME = 0.5f;

    public const float MIN_SWIPE_DISTANCE = 0.17f;

    public static bool swipedRight = false;
    public static bool swipedLeft = false;
    public static bool swipedUp = false;
    public static bool swipedDown = false;

    public bool debugWithArrowKeys = true;

    private int counter = 0;
    private int maxRayDistance;
    private int currentDistance;
    private int maxDistance;
    private AudioSource source;
    private RaycastHit hit;
    private bool isGrounded;
    private float distToTheGround;

    [SerializeField] private LevelGenerator generator;
    [SerializeField] private Text score;
    [SerializeField] private Text coins;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private float volumeScale;


    private Vector2 startPos;
    private float startTime;

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToTheGround + 0.01f);
    }

    private bool IsTubeOnTheWay(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out hit, maxRayDistance))
        {
            if (hit.collider.tag == "Tube")
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        swipedRight = false;
        swipedLeft = false;
        swipedUp = false;
        swipedDown = false;

        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                startTime = Time.time;
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (Time.time - startTime > MAX_SWIPE_TIME)
                    return;

                Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                if (swipe.magnitude < MIN_SWIPE_DISTANCE)
                    return;

                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                {
                    if (swipe.x > 0)
                    {
                        if (!IsTubeOnTheWay(Vector3.right))
                        {
                            transform.DOJump(Vector3.right, 0.25f, 1, 0.25f, false).SetRelative();
                            source.PlayOneShot(jumpSound, volumeScale);
                            counter++;
                        }

                        swipedRight = true;
                    }
                    else
                    {
                        if (!IsTubeOnTheWay(Vector3.left))
                        {
                            transform.DOJump(Vector3.left, 0.25f, 1, 0.25f, false).SetRelative();
                            source.PlayOneShot(jumpSound, volumeScale);
                            counter++;
                        }

                        swipedLeft = true;
                    }
                }
                else
                {
                    if (swipe.y > 0)
                    {
                        if (!IsTubeOnTheWay(Vector3.forward))
                        {
                            if (currentDistance < maxDistance)
                            {
                                score.text = maxDistance.ToString();
                            }
                            else
                            {
                                int scoreValue = PlayerPrefs.GetInt("Score", 0) + 1;
                                PlayerPrefs.SetInt("Score", scoreValue);
                                score.text = scoreValue.ToString();
                            }

                            transform.DOJump(Vector3.forward, 0.25f, 1, 0.25f, false).SetRelative();
                            source.PlayOneShot(jumpSound, volumeScale);
                            generator.SpawnTerrainPrefab(false, transform.position);

                            counter++;
                        }

                        swipedUp = true;
                    }
                    else
                    {
                        if (!IsTubeOnTheWay(Vector3.back))
                        {
                            transform.DOJump(Vector3.back, 0.25f, 1, 0.25f, false).SetRelative();
                            source.PlayOneShot(jumpSound, volumeScale);
                            counter++;
                        }

                        swipedDown = true;
                    }
                }
            }
        }

        if (debugWithArrowKeys)
        {
            swipedDown = swipedDown || Input.GetKeyDown(KeyCode.DownArrow);
            swipedUp = swipedUp || Input.GetKeyDown(KeyCode.UpArrow);
            swipedRight = swipedRight || Input.GetKeyDown(KeyCode.RightArrow);
            swipedLeft = swipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);
        }
    }
}
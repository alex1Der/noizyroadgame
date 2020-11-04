using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Constants
    private const float maxSwipeTime = 0.5f;
    private const float minSwipeDistance = 0.17f;
    #endregion
    #region Private Static
    private static bool swipedRight = false;
    private static bool swipedLeft = false;
    private static bool swipedUp = false;
    private static bool swipedDown = false;
    #endregion
    #region Private
    private bool debugWithArrowKeys;
    private bool isGrounded;
    private bool couldBeSwipe;
    private int counter;
    private int amountOfStepsBeforeCheck;
    private int startPosition;
    private int currentDistance;
    private int maxDistance;
    private int maxRayDistance;
    private int resultBoardDelay;
    private int plainMoveDelay;
    private int gameOverBoardMoveValue;
    private int highscoreBoardMoveValue;
    private int backgroundMoveValue;
    private int savedVolume;
    private int startPoint;
    private int endPoint;
    private AudioSource source;
    private RaycastHit hit;
    private Vector3 normalPlayerScale;
    private Vector3 beganPhasePlayerScale;
    private Vector3 plainOffset;
    private Vector2 startPos;
    private float startTime;
    private float distanceToTheGround;
    private float minDistanceToTheGroundOffset;
    private float xPlainOffset;
    private float yPlainOffset;
    private float zPlainOffset;
    private float jumpPower;
    private float jumpDuration;
    private float zPrejumpScale;
    private float rotationValue;
    #endregion
    #region Private Serialized Fields
    [SerializeField] private LevelGenerator generator;
    [SerializeField] private Text coins;
    [SerializeField] private Text score;
    [SerializeField] private Text highScore;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private GameObject plain;
    [SerializeField] private GameObject resultBoard;
    [SerializeField] private GameObject startBoard;
    [SerializeField] private GameObject highscoreBoard;
    [SerializeField] private GameObject gameOverBoard;
    [SerializeField] private GameObject blueBackground;
    #endregion
    public float volumeScale;

    private void OnEnable()
    {
        #region Initializations
        debugWithArrowKeys = true;
        jumpPower = 0.25f;
        jumpDuration = 0.25f;
        xPlainOffset = 15.0f;
        yPlainOffset = 15.0f;
        zPlainOffset = 150.0f;
        minDistanceToTheGroundOffset = 0.01f;
        zPrejumpScale = 40.0f;
        rotationValue = 90.0f;
        currentDistance = 0;
        startPoint = -11;
        endPoint = 11;
        counter = 0;
        maxDistance = 0;
        maxRayDistance = 1;
        amountOfStepsBeforeCheck = 5;
        resultBoardDelay = 2;
        plainMoveDelay = 5;
        gameOverBoardMoveValue = 120;
        highscoreBoardMoveValue = 180;
        backgroundMoveValue = 130;
        savedVolume = PlayerPrefs.GetInt("Volume", 1);
        volumeScale = savedVolume == 1 ? 0.05f : 0.0f;
        score.text = PlayerPrefs.GetInt("Score", 0).ToString();
        highScore.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        startPosition = (int)transform.position.z;
        source = GetComponent<AudioSource>();
        distanceToTheGround = GetComponent<BoxCollider>().bounds.extents.y;
        normalPlayerScale = transform.localScale;
        beganPhasePlayerScale = new Vector3(transform.localScale.x, transform.localScale.y, zPrejumpScale);
        plainOffset = new Vector3(plain.transform.position.x - xPlainOffset, plain.transform.position.y - yPlainOffset, plain.transform.position.z - zPlainOffset);
        #endregion
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("Score", 0);
    }

    private void Update()
    {
        currentDistance = (int)transform.position.z - startPosition;
        if (currentDistance > maxDistance)
        {
            maxDistance = currentDistance;
        }

        SwipeControl();

        //if (transform.position.x <= endPoint || transform.position.x >= startPoint)
        //{
        //    plain.SetActive(true);
        //    plain.transform.DOMove(plainOffset, plainMoveDelay);
        //    ObjectPoolScript.Despawn(gameObject);
        //    //ShowResultCanvas();
        //}
    }

    private void CorrectPlayerPosition()
    {
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z));
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToTheGround + minDistanceToTheGroundOffset);
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

    private void ShowResultCanvas()
    {
        resultBoard.SetActive(true);
        gameOverBoard.transform.DOMoveY(gameOverBoardMoveValue, resultBoardDelay);
        highscoreBoard.transform.DOMoveY(highscoreBoardMoveValue, resultBoardDelay);
        blueBackground.transform.DOMoveY(backgroundMoveValue, resultBoardDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Water" || collision.collider.tag == "Car")
        {
            plain.SetActive(true);
            plain.transform.DOMove(plainOffset, plainMoveDelay);
            ObjectPoolScript.Despawn(gameObject);
            ShowResultCanvas();
        }
        else if (collision.collider.tag == "Coin")
        {
            int coinsAmount = PlayerPrefs.GetInt("Coins", 0) + 1;
            PlayerPrefs.SetInt("Coins", coinsAmount);
            coins.text = coinsAmount.ToString();

            source.PlayOneShot(coinSound, volumeScale);

            ObjectPoolScript.Despawn(collision.collider.gameObject);
        }
        else if (collision.collider.tag == "Desk")
        {
            transform.parent = collision.collider.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Desk")
        {
            transform.parent = null;
        }
    }

    private void SwipeControl()
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
                if(startBoard.activeSelf)
                    startBoard.SetActive(false);
                transform.localScale = beganPhasePlayerScale;
                startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                startTime = Time.time;
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (Time.time - startTime > maxSwipeTime)
                {
                    transform.localScale = normalPlayerScale;

                    return;
                }

                Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                if (swipe.magnitude < minSwipeDistance)
                {
                    return;
                }

                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                {
                    if (swipe.x > 0)
                    {
                        if (!IsTubeOnTheWay(Vector3.right))
                        {
                            transform.localScale = normalPlayerScale;
                            transform.DOJump(Vector3.right, jumpPower, 1, jumpDuration, false).SetRelative();
                            source.PlayOneShot(jumpSound, volumeScale);

                            counter++;
                        }

                        swipedRight = true;
                    }
                    else
                    {
                        if (!IsTubeOnTheWay(Vector3.left))
                        {
                            transform.localScale = normalPlayerScale;
                            transform.DOJump(Vector3.left, jumpPower, 1, jumpDuration, false).SetRelative();
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

                            transform.localScale = normalPlayerScale;
                            transform.DOJump(Vector3.forward, jumpPower, 1, jumpDuration, false).SetRelative();
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
                            transform.localScale = normalPlayerScale;
                            transform.DOJump(Vector3.back, jumpPower, 1, jumpDuration, false).SetRelative();
                            source.PlayOneShot(jumpSound, volumeScale);

                            counter++;
                        }

                        swipedDown = true;
                    }
                }
            }
        }

        if (counter % amountOfStepsBeforeCheck == 0 && IsGrounded() && gameObject != null)
        {
            CorrectPlayerPosition();
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
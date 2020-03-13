using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ 
    public static GameManager Instance;

    public GameObject ScoreText, SpeedText;

    public GameObject playerPrefab, groundForwardPrefab, groundForwardHolePrefab, groundLeftPrefab, groundRightPrefab;

    public GameObject GameOverPanel; 

    private Ground[] grounds = new Ground[10];

    private int groundIndex = 0;

    private Text scoreText, speedText;

    void Start ()
    {
        Instance = this;
        scoreText = ScoreText.GetComponent<Text>();
        speedText = SpeedText.GetComponent<Text>();
        Instantiate(playerPrefab, transform.position + Vector3.up, transform.rotation, transform);
        SpawnGround(groundForwardPrefab, this.transform);

        for(int i = 1; i < grounds.Length - 1; i++)
        {
            SpawnGround(GetRandomGround(), grounds[groundIndex].NextGround);
        }
    }

    public void SpawnGround()
    {
        SpawnGround(GetRandomGround(), grounds[groundIndex].NextGround);
    }

    void SpawnGround(GameObject p, Transform t)
    {
        groundIndex++;

        if(groundIndex > grounds.Length - 1)
        {
            groundIndex = 0;
        }

        if (grounds[groundIndex] != null)
        {
            grounds[groundIndex].DestroyGround();
        }

        grounds[groundIndex] = (Instantiate(p, t.position, t.rotation, this.transform).GetComponent<Ground>());
    }

    GameObject GetRandomGround()
    {
        GroundType randomGroundType = (GroundType)Random.Range(0, 3);
        
        if(randomGroundType == GroundType.Left || randomGroundType == GroundType.Right)
        {
            if(ExistsTwice(randomGroundType))
            {
                if (Random.Range(0, 2) == 0)
                {
                    return groundForwardPrefab;
                }
                else
                {
                    if(randomGroundType == GroundType.Left)
                    {
                        if (!ExistsTwice(GroundType.Right))
                        {
                            return groundRightPrefab;
                        }
                    }
                    else
                    {
                        if (!ExistsTwice(GroundType.Left))
                        {
                            return groundLeftPrefab;
                        }
                    }

                    return groundForwardPrefab;
                }
            }

            return (randomGroundType == GroundType.Left) ? groundLeftPrefab : groundRightPrefab;
        }

        if(Random.Range(0, 100) >= 70.0f)
        {
            return groundForwardHolePrefab;
        }

        return groundForwardPrefab;
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score + " Beans";
    }

    public void SetSpeedText(float speed)
    {
        speedText.text = speed + " beans/s";
    }

    bool ExistsTwice(GroundType type)
    {
        int count = 0;

        foreach(Ground g in grounds)
        {
            if (g != null)
            {
                if(g.GroundType == type)
                {
                    count++;
                }
            }
        }

        return (count >= 2);
    }

    public void LeftPressed()
    {
        if (Player.Instance != null)
        {
            Player.Instance.horizontalInput.x = -1.0f;
        }
    }

    public void RightPressed()
    {
        if(Player.Instance != null)
        {
            Player.Instance.horizontalInput.x = 1.0f;
        }
    }

    public void LeftReleased()
    {
        if (Player.Instance != null)
        {
            Player.Instance.horizontalInput.x = 0.0f;
        }
    }

    public void RightReleased()
    {
        if (Player.Instance != null)
        {
            Player.Instance.horizontalInput.x = 0.0f;
        }
    }

    public void JumpClicked()
    {
        if(Player.Instance != null)
        {
            Player.Instance.Jump();
        }
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void ReloadLevel()
    {
        GameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
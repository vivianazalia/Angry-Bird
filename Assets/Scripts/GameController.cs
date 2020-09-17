using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SlingShooter slingShooter;
    public TrailController trailController;
    
    public List<Bird> birds;

    public List<Enemy> enemies;

    private Bird shotBird;
    public BoxCollider2D tapCollider;

    public bool isGameEnded = false;
    public bool isFailed = false;
    private Scene currentActiveScene;

    [Header("UI Settings")]
    [SerializeField] private GameObject panelLevel;
    [SerializeField] private GameObject restartBtn;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject levelClear;
    [SerializeField] private GameObject levelFailed;

    void Start()
    {
        currentActiveScene = SceneManager.GetActiveScene();
        
        for(int i = 0; i < birds.Count; i++)
        {
            birds[i].OnBirdDestroyed += ChangeBird;
            birds[i].OnBirdShot += AssignTrail;
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        tapCollider.enabled = false;
        slingShooter.InitiateBird(birds[0]);
        shotBird = birds[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    public void ChangeBird()
    {
        tapCollider.enabled = false;

        if (birds.Count == 1 && enemies.Count > 0)
        { 
            isFailed = true;
        }
        
        if (isGameEnded)
        {
            panelLevel.SetActive(true);
            levelClear.SetActive(true);
            restartBtn.SetActive(true);
            nextBtn.SetActive(true);
            return;
        }

        if (isFailed)
        {
            panelLevel.SetActive(true);
            levelFailed.SetActive(true);
            restartBtn.SetActive(true);
            return;
        }

        birds.RemoveAt(0);

        if(birds.Count > 0)
        {
            slingShooter.InitiateBird(birds[0]);
            shotBird = birds[0];
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].gameObject == destroyedEnemy)
            {
                enemies.RemoveAt(i);
                break;
            }
        }

        if(enemies.Count == 0)
        {
            isGameEnded = true;
        }
    }

    void AssignTrail(Bird bird)
    {
        trailController.SetBird(bird);
        StartCoroutine(trailController.SpawnTrail());
        tapCollider.enabled = true;
    }

    void OnMouseUp()
    {
        if(shotBird != null)
        {
            shotBird.OnTap();
        }
    }

    

    void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Nextlevel(int indexNextScene)
    {
        LoadScene(indexNextScene);
    }

    public void Restart()
    {
        SceneManager.LoadScene(currentActiveScene.name);
    }
}

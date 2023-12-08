using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Pathfinding))]
public class GameManager : MonoBehaviour
{
    public GameManager instance = null;
    public GameObject door;
    private Joystick joystick;

    public float startTimer;
    public bool start;
    public bool end = false;

    private GridManager gridManager;
    private Pathfinding pathfinding;
    private Character character;

    public List<Enemy> enemies = new List<Enemy>();
    private List<Enemy> enemiesToRemove = new List<Enemy>();

    public delegate void Death(Enemy enemy);
    private Death death;

    public GameObject deathUI;
    public GameObject menu;
    public GameObject winpanel;

    public bool debugging = true;

    private Dictionary<string, List<TileIndex>> enemyPos = new Dictionary<string, List<TileIndex>>
    {
        { 
            "Swordsman",
            new List<TileIndex>()
            {
                new TileIndex(5,15),
                new TileIndex(1,18),
                new TileIndex(9,18)
            }
        },
        {
            "Wizard",
            new List<TileIndex>()
            {
                new TileIndex(9,23),
                new TileIndex(1,23)
            }
        },
        {
            "Dragon",
            new List<TileIndex>()
            {
                new TileIndex(5,38)
            }
        }
    };

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        gridManager = FindObjectOfType<GridManager>();
        character = FindObjectOfType<Character>();
        pathfinding = GetComponent<Pathfinding>();
        joystick = FindObjectOfType<Joystick>();
    }
    private void Start()
    {
        UpdateCharacterVariables();
        SpawnEnemies(enemyPos);
    }

    private void FixedUpdate()
    {
        Debug.unityLogger.logEnabled = debugging;
 
        CheckWinCondition();

        if (!end)
        {
            if (startTimer < 0)
            {
                start = true;

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].active = true;
                }
            }

            if (!start)
            {
                startTimer -= Time.deltaTime;
            }
            else
            {
                UpdateCharacterVariables();
                UpdateEnemyVariables();

                for (int i = 0; i < enemiesToRemove.Count; i++)
                {
                    enemies.Remove(enemiesToRemove[i]);
                    Destroy(enemiesToRemove[i].gameObject);
                }

                enemiesToRemove.Clear();
            }
        }
    }

    #region "Private Functions"

    private void SpawnEnemies(Dictionary<string, List<TileIndex>> dictionary)
    {
        foreach (KeyValuePair<string, List<TileIndex>> i in dictionary)
        {
            for (int x = 0; x < i.Value.Count; x++)
            {
                GameObject enemyObj = Instantiate(Resources.Load("Enemies/" + i.Key)) as GameObject;
                enemyObj.transform.position = gridManager.GetTileByIndex(i.Value[x]).transform.position;

                Enemy enemy = enemyObj.GetComponent<Enemy>();

                enemy.Initalize();

                enemy.SetPosition(gridManager.PosToTile(enemy.transform.position));
                enemy.SetRange(gridManager);
                enemy.SetChararcterPosition(character.position);
                enemy.SetGameManager(this);
                enemy.SetDeadRange(gridManager);

                enemy.deathEvent.AddListener(OnEnemyDeath);

                enemies.Add(enemy);
            }
        }
    }

    private void UpdateCharacterVariables()
    {
        character.SetPosition(gridManager.PosToTile(character.transform.position));
        character.SetRange(gridManager.GetRange(character.position, character.GetRange()));
    }

    private void UpdateEnemyVariables()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetPosition(gridManager.PosToTile(enemies[i].transform.position));
            enemies[i].SetChararcterPosition(character.position);
            enemies[i].SetRange(gridManager);
            enemies[i].SetFOV(gridManager);
            enemies[i].SetDeadRange(gridManager);

            enemies[i].Tick();
        }
    }

    public Enemy GetNearestEnemy()
    {
        Enemy enemy = null;
        float dis = Mathf.Infinity;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemy == null) enemy = enemies[i];

            float distance = gridManager.Distance(enemies[i].GetPosition(), character.position);

            if(distance < dis)
            {
                dis = distance;
                enemy = enemies[i];
            }

        }
        if (dis > character.GetRange())
            return null;
        else 
            return enemy;
    }

    public List<Tile> GetPathToSafeTile(List<Tile> deadZone, Tile position,int range, bool canFly)
    {
        return pathfinding.GetPathToSafeTile(deadZone, position, character.position, range, canFly);
    }

    private void CheckWinCondition()
    {
        if(enemies.Count == 0)
        {
            end = true;
            door.SetActive(false);

            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

            for (int i = 0; i < coins.Length; i++)
            {
                coins[i].GetComponent<Coin>().active = true;
            }
        }
    }


    private void OnEnemyDeath(Enemy enemy)
    {
        enemiesToRemove.Add(enemy);
        enemy.deathEvent.RemoveListener(OnEnemyDeath);
    }

    #endregion
    #region "Public Functions"

    public List<Tile> GetMoveablePath(Tile origin)
    {
        return pathfinding.GetPathRunable(origin, character.position);
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    public void Return()
    {
        menu.SetActive(false);
        joystick.active = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Win()
    {
        winpanel.SetActive(true);
        joystick.active = false;
    }

    public void Menu()
    {
        menu.SetActive(true);
        joystick.active = false;
    }

    public void DeathUI()
    {
        deathUI.SetActive(true);
        joystick.active = false;
        Time.timeScale = 0;
    }

    public List<Tile> FilterByAttackingRange(List<Tile> waypoints, float range)
    {
        return pathfinding.FilterWaypointsByAttackRange(waypoints,character.position, range);
    }

    public List<Tile> GetFlyablePath(Tile origin)
    {
        return pathfinding.GetPathFlyable(origin, character.position);
    }

    #endregion
    #region "Debug Functions"

    private void DebugCreateEnemy(string prefab, TileIndex position)
    {
        GameObject enemyObj = Instantiate(Resources.Load("Enemies/" + prefab)) as GameObject;
         
        if(enemyObj == null)
        {
            Debug.LogError("Отсутствует префаб персонажа");
            return;
        }

        enemyObj.transform.position = gridManager.GetTileByIndex(position).transform.position;

        Enemy enemy = enemyObj.GetComponent<Enemy>();

        enemy.Initalize();

        enemy.SetPosition(gridManager.PosToTile(enemy.transform.position));
        enemy.SetRange(gridManager);
        enemy.SetFOV(gridManager);
        enemy.SetChararcterPosition(character.position);
        enemy.SetDeadRange(gridManager);
        enemy.SetGameManager(this);

        enemy.deathEvent.AddListener(OnEnemyDeath);

        enemies.Add(enemy);
    }

    #endregion
}

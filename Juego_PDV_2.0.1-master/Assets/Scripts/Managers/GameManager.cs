using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject youDied;
    [SerializeField]
    private GameObject youWon;
    [SerializeField]
    private MusicManager musicManager;

    private int stage;

    private static SceneManage instance;
    public static SceneManage MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneManage>();
            }

            return instance;
        }
    }

    public int MyStage { get => stage; set => stage = value; }

    private NPC currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        youDied.SetActive(false);
        youWon.SetActive(false);
        stage = 1;
        musicManager.playTrack(stage);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.MyHealth.MyCurrentValue <= 0)
        {
            Player.MyInstance.transform.position = new Vector3(-100, 0, 0);
            youDied.SetActive(true);
            youDied.GetComponent<Image>().color = Color.white;
            musicManager.playTrack(0);
        }
        if (stage == 4)
        {
            youWon.SetActive(true);
            youWon.GetComponent<Image>().color = Color.white;
            Time.timeScale = 0;
        }
        ClickTarget();
        
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) //Left click
        {
            Debug.Log("Works");
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            //En hit se guarda la ubicacion que cliqueo el usuario
            //Layer 9 = 2^9 = 512
            //En la capa 9 se guardan los enemigos (o todo lo que pueda recibir click izquierdo)
            if (hit.collider != null) //Toco algo
            {
                Debug.Log(hit.collider);
                if (currentTarget != null && hit.collider.GetComponent<NPC>() == currentTarget)
                {
                    Debug.Log("CurrentTraget is the same");
                    if (player.SpellType != null)
                    {
                        player.CastSpell(player.SpellType);
                        Debug.Log("Shooting to target");
                    }
                    else
                    {
                        Debug.Log("Not selected spell");
                    }
                }
                currentTarget = hit.collider.GetComponent<NPC>();
                player.MyTarget = currentTarget.Select();

                UIManager.MyInstance.ShowTargetFrame(currentTarget);
            }
            else //Deselect
            {
                UIManager.MyInstance.HideTargetFrame();

                if (currentTarget != null)
                {
                    currentTarget.DeSelect();
                }

                currentTarget = null;
                player.MyTarget = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                currentTarget = hit.collider.GetComponent<NPC>();
                if (!currentTarget.IsAlive)
                {
                    foreach (Item item in currentTarget.items)
                    {
                        InventoryScript.MyInstance.AddItem(item);
                    }
                    player.TakeXP(currentTarget.MyXpPoints);
                    currentTarget.OnCharacterRemoved();
                }
            }
        }


    }

    public void SaveAction()
    {
        PlayerData data = new PlayerData();
        data.position = player.transform.position;
        data.maxHealth = player.MyHealth.MyMaxValue;
        data.health = player.MyHealth.MyCurrentValue;
        data.level = player.MyLevel;
        data.currentStage = stage;

        string dataString = JsonUtility.ToJson(data);
        GameManager.Save(dataString);
        Debug.Log("Save Action");
        Debug.Log("Data string: " + dataString);
    }

    public void LoadAction()
    {
        string dataString = GameManager.Load();
        PlayerData data = JsonUtility.FromJson<PlayerData>(dataString);
        MyInstance.LoadFirstLevel(data);
        musicManager.playTrack(data.currentStage);
    }

    public static void Save(string jsonString, string filename = "save.json", string storePath = null)
    {
        if (storePath == null)
        {
            storePath = Application.dataPath + "/saves";
        }
        if (!Directory.Exists(storePath))
        {
            Directory.CreateDirectory(storePath);
        }
        string fullPath = storePath + "/" + filename;
        File.WriteAllText(fullPath, jsonString);
    }

    public static string Load(string filename = "save.json", string storePath = null)
    {
        if (storePath == null)
        {
            storePath = Application.dataPath + "/saves";
        }
        string fullPath = storePath + "/" + filename;
        string loaded = null;
        if (File.Exists(fullPath))
        {
            loaded = File.ReadAllText(fullPath);
        }
        return loaded;
    }
}

public class PlayerData
{
    public Stack<Item> items;
    public Vector3 position;
    public float health;
    public float maxHealth;
    public int level;
    public int currentStage;
}
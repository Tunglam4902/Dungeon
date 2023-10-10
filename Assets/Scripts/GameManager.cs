using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake(){
        if (GameManager.instance != null){
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(FloatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Tài nguyên (Ressources)
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapons weapon;
    public FloatingTextManager FloatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnim;
    public GameObject hud;
    public GameObject menu; 

    // Logic
    public int coins;
    public int experiences;

    // Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration){
        FloatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }
    // Upgrade Weapon
    public bool TryUpgradeWeapon(){
        // Is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        if (coins >= weaponPrices[weapon.weaponLevel]){
            coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }

    // Hitpoint Bar
    public void OnHitpointChange(){
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }
    // Experience system
    public int GetCurrentLevel(){
        int r = 0;
        int add = 0;
        
        while (experiences >= add){
            add += xpTable[r];
            r++;

            if (r == xpTable.Count)
                return r;
        }

        return r;
    }
    public int GetXpToLevel(int level){
        int r = 0;
        int xp = 0;

        while (r < level){
            xp += xpTable[r];
            r++;
        }
        return xp;
    }
    public void GrantXp(int xp){
        int currLevel = GetCurrentLevel();
        experiences += xp;
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }
    public void OnLevelUp(){
        Debug.Log("Level up!");
        player.OnLevelUp();
    }

    // On Scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode){
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Death Menu and Restart
    public void Restart(){
        deathMenuAnim.SetTrigger("Hide");
        GameManager.instance.SaveState();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        player.Restart();
    }

    //Save state
    public void SaveState(){
        string s = "";

        s += "0" + "|";
        s += coins.ToString() + "|";
        s += experiences.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }
    public void LoadState(Scene s, LoadSceneMode mode){
        SceneManager.sceneLoaded -= LoadState;
        if (!PlayerPrefs.HasKey("SaveState"))
            return;
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Thay đổi skin
        coins = int.Parse(data[1]);

        // Experiences
        experiences = int.Parse(data[2]);
        player.SetLevel(GetCurrentLevel());
        // Thay đổi level vũ khí
        weapon.SetWeaponLevel(int.Parse(data[3]));


        Debug.Log("LoadState");

    }   
}

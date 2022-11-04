using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    // public GameObject SettingsMenu;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent = 7;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;

    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
    TMP_Text gameOverText;
    private void Start()
    {
        FindObjectOfType<AudioManager>().PlayAudio("CarAmbience");

        // setup gameover panel
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();

        //belakang
        for(int z=backDistance; z<=0; z++)
        {
            CreateTerrain(grass, z);
        }

        for(int z=1; z<=frontDistance; z++)
        {
            var prefab = GetNextRandomTerrainPrefab(z);

            //instantiative blocknya
            CreateTerrain(prefab, z);
        }
        player.SetUp(backDistance, extent);
    }

    private int playerLastMaxTravel;
    private void Update()
    {
        // cek player
        if(player.IsDie && gameOverPanel.activeInHierarchy==false)
            ShowGameOverPanel();
        
        // infinite terrain system
        if(player.MaxTravel == playerLastMaxTravel)
            return;

        playerLastMaxTravel = player.MaxTravel;

        // membuat blok depan
        var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel+frontDistance);
        CreateTerrain(randTbPrefab, player.MaxTravel+frontDistance);

        // menghapus blok belakang
        var lastTB = map[player.MaxTravel-1+backDistance];

        map.Remove(player.MaxTravel-1+backDistance);
        Destroy(lastTB.gameObject);

        player.SetUp(player.MaxTravel+backDistance, extent);
    }

    void ShowGameOverPanel()
    {
        gameOverText.text = "Your Score: " + player.MaxTravel;
        gameOverPanel.SetActive(true);
        FindObjectOfType<AudioManager>().PlayAudio("Win");
    }

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
        Debug.Log(map[zPos] is Road);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos-1];
        for(int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if(map[nextPos-distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }
        if(isUniform)
        {
            if(tbRef is Grass)
                return road;
            else
                return grass;
        }
        // penentuan terain block dengan probabilitas 50%
        return Random.value > 0.5f ? road : grass;
    }

    public void BackToMainMenu()
    {
        SceneLoader.Load("MainMenu");
        FindObjectOfType<AudioManager>().StopAudio("CarAmbience");
    }

    public void Replay()
    {
        SceneLoader.ReloadLevel();
    }
}

using UnityEngine;
using System.IO;

public class LevelTransition : MonoBehaviour
{
    public bool setOnLoad = true;
    public string nextLevelName;

    public TextAsset data;
    StreamReader sr;

    private void Start()
    {
        if (setOnLoad) LoadPlayerData();
    }

    public void LoadPlayerData()
    {

    }

    public void SavePlayer(GameObject player)
    {
        player.transform.position.ToString();
        player.transform.rotation.ToString();
        player.GetComponent<Inventory>().activeIndex.ToString();
    }

    public void Transition()
    {
        LoadLogic.LoadSceneByName(nextLevelName);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SavePlayer(other.gameObject);
            Transition();
        }
    }
}

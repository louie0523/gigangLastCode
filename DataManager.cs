using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public int Name;
    public Sprite Icon;
    public int num;
    public float Weight;
    public float LifeTime;
}

[System.Serializable]
public class Part
{
    public int Name;
    [TextArea]
    public string Des;
    public Sprite Icon;
    public Color Color;
    public int Upgrade;
    public int UpgradePrice;
    public List<DoubleList> Result = new List<DoubleList>();
    
}

[System.Serializable]
public class DoubleList
{
    public List<float> result = new List<float>();
}

[System.Serializable]
public class Rank
{
    public string Name;
    public int Socre;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public List<Rank> ranks = new List<Rank>();

    public GameObject SliderCanvas;
    public Slider SfxSlider;
    public Slider BgmSlider;

    public float ClearTime;
    public int Score;
    public int Coin;
    public int Stage;
    public int HpUp;
    public int SpeedUp;
    public int DefUp;
    public int InvenUp;
    public int BombUp;
    public int HpPrice;
    public int SpeedPrice;
    public int DefPriece;
    public int InvenPrice;
    public int BombPrice;

    public int MaxPart = 2;

    public List<int>CurrentPart = new List<int>();
    public List<float> PartCool = new List<float>();


    public List<Item> ItemData = new List<Item>();
    public List<Part> Part = new List<Part>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void DataSave()
    {
        PlayerPrefs.SetFloat("ClearTime", ClearTime);
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.SetInt("Coin", Coin);
        PlayerPrefs.SetInt("Stage", Stage);
        PlayerPrefs.SetInt("HpUp", HpUp);
        PlayerPrefs.SetInt("SpeedUp", SpeedUp);
        PlayerPrefs.SetInt("DefUp", DefUp);
        PlayerPrefs.SetInt("InvenUp", InvenUp);
        PlayerPrefs.SetInt("BombUp", BombUp);
        PlayerPrefs.SetInt("MaxPart", MaxPart);

        for(int i = 0; i < GameManager.Instance.Inven.Count; i++)
        {
            PlayerPrefs.SetInt("Inven" + i, GameManager.Instance.Inven[i].num);
        }

        for (int i = 0; i < CurrentPart.Count; i++)
        {
            PlayerPrefs.SetInt("CurrentPart" + i, CurrentPart[i]);
        }

        for (int i = 0; i < Part.Count; i++)
        {
            PlayerPrefs.SetInt("Part" + i, Part[i].Upgrade);
        }

        for(int i = 0; i < ranks.Count; i++)
        {
            PlayerPrefs.SetString("RankName" + i, ranks[i].Name);
            PlayerPrefs.SetInt("RankScore" + i, ranks[i].Socre);
        }

        PlayerPrefs.Save();

        RankSet();
    }

    public void RankSet()
    {
        ranks = ranks.OrderByDescending(_ => _.Socre).ToList();
    }

    public void LoadData()
    {
        ClearTime = PlayerPrefs.GetFloat("ClearTime");
        Score = PlayerPrefs.GetInt("Score");
        Coin = PlayerPrefs.GetInt("Coin");
        Stage = PlayerPrefs.GetInt("Stage");
        HpUp = PlayerPrefs.GetInt("HpUp");
        SpeedUp = PlayerPrefs.GetInt("SpeedUp");
        DefUp = PlayerPrefs.GetInt("DefUp");
        InvenUp = PlayerPrefs.GetInt("InvenUp");
        BombUp = PlayerPrefs.GetInt("BombUp");
        MaxPart = PlayerPrefs.GetInt("MaxPart");
        
        for(int i = 0; i < 3 + InvenUp; i++)
        {
            if (PlayerPrefs.HasKey("Inven" + i))
                GameManager.Instance.Inven.Add(ItemData[PlayerPrefs.GetInt("Inven" + i)]);
        }

        for (int i = 0; i < MaxPart; i++)
        {
            if (PlayerPrefs.HasKey("CurrentPart" + i))
                CurrentPart[i] = PlayerPrefs.GetInt("CurrentPart" + i);
        }

        for (int i = 0; i < Part.Count; i++)
        {
            Part[i].Upgrade = PlayerPrefs.GetInt("Part" + i);
        }

        for (int i = 0; i < 5; i++)
        {
            if (!PlayerPrefs.HasKey("RankName" + i))
                break;

            ranks[i].Name = PlayerPrefs.GetString("RankName" + i);
            ranks[i].Socre =  PlayerPrefs.GetInt("RankScore" + i);
        }

        PlayerPrefs.Save();
    }

    
    private void Start()
    {
        //SfxSlider.value = PlayerPrefs.GetFloat("Sfx", SfxSlider.value);
        //BgmSlider.value = PlayerPrefs.GetFloat("Bgm", BgmSlider.value);

        //SfxSlider.onValueChanged.AddListener(VolumeSet);
        //BgmSlider.onValueChanged.AddListener(VolumeSet);

        RankSet();
    }

    void VolumeSet(float value)
    {
        PlayerPrefs.SetFloat("Sfx", SfxSlider.value);
        PlayerPrefs.SetFloat("Bgm", BgmSlider.value);

        PlayerPrefs.Save();
    }


    public float GetResult(int partsNum, int num)
    {
        return Part[partsNum].Result[num].result[Part[partsNum].Upgrade];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{


    List<GameObject> skinList = new List<GameObject>();
    public Sprite[] spList;
    bool canPress = true;//bug olmasını engellemek için bir kontrol
    Sprite sp;
    public int count;

    [Header("Set fortune speed")]
    public float fortuneSpeed;
    void Start()
    {
        sp = GameObject.FindGameObjectWithTag("SkinButton").transform.GetChild(1).GetComponent<Image>().sprite;//butona tekrar tıklandığında soru işaretini geri getirmek için soru işaretini değişkende tuttum.
    }
    public void unlockBtn()
    {
        if (canPress)
        {
            storeSkinButtons();
            StartCoroutine(randomSelection(fortuneSpeed));
            canPress = false;
        }
        else
        {
            Debug.Log("Wait for done !!");
        }

    }
    void storeSkinButtons()
    {
        //skinbutton tag'ine sahip kartlar listeye ekleniyor.
        foreach (GameObject skin in GameObject.FindGameObjectsWithTag("SkinButton"))
        {
            skinList.Add(skin);
        }
        count = skinList.Count;
        for (int i = 0; i < skinList.Count; i++)
        {
            skinList[i].GetComponent<Image>().color = Color.gray;
            skinList[i].GetComponent<Button>().interactable = false;
            skinList[i].transform.GetChild(1).GetComponent<Image>().sprite = sp;
        }
    }
    void active(GameObject g)
    {
        g.GetComponent<Image>().color = Color.green;
        g.GetComponent<Button>().interactable = true;
    }
    void passive(GameObject g)
    {
        g.GetComponent<Image>().color = Color.gray;
        g.GetComponent<Button>().interactable = false;
    }
    public IEnumerator randomSelection(float _fortuneSpeed)
    {

        for (int i = 0; i < count; i++)
        {
            int rnd = Random.Range(0, skinList.Count); //kart sayısı aralığında rastgele bir sayı 
            if (i == count - 1)//eğer son karta geldiysek o kartı gösterelim
            {
                active(skinList[rnd]);
                string str = skinList[rnd].name.Substring(skinList[rnd].name.Length - 1);
                skinList[rnd].transform.GetChild(1).GetComponent<Image>().sprite = spList[int.Parse(str) - 1];//burada resource klasörü olmadığı için spriteları kartlara eşitlemem gerekiyordu. ben de kart namelerin son hanesindeki rakam ile listeye eklediğim spriteları eşleştirdim.
                canPress = true;
                skinList[rnd].tag = "Player";//açılan karta player tag'ı verdim. şablonu bozup yeni bir tag eklemek istemedim varolanlardan kullandım. tag ile listeye ekleme yaptığım için tag'i değiştirirsem onu listeye almayacaktır.
                skinList.Clear();
                Debug.Log(skinList.Count);
            }
            else //eğer son kartta değilsek karıştırmaya devam
            {
                active(skinList[rnd]);
                yield return new WaitForSeconds(_fortuneSpeed);
                passive(skinList[rnd]);
                skinList.RemoveAt(rnd);//üzerinden geçtiğimiz kartı listeden çıkartalım bir daha üzerine gelmemek  için
            }
            _fortuneSpeed += 0.05f;
        }
    }
}

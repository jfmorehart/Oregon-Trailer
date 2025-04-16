using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HuntManager : MonoBehaviour
{
    [SerializeField]
    private GameObject huntScene;
    public static HuntManager instance;

    public TMP_Text threeStarTimeText;
    public TMP_Text twoStarTimeText;

    public TMP_Text timeEarnedText;
    public GameObject oneStar, twoStar, ThreeStar;

    bool goToGarageScreenOnComplete = false;
    int starsFromLevel = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public void displayHunt(bool goToGarageScreenOnComplete, float timeEarned, float twoStarTime, float threeStarTime, int starsEarned)
    {

        huntScene.SetActive(true);
        //one star is active automatically
        oneStar.SetActive(true);
        if (starsEarned == 2)
            twoStar.SetActive(true);
        if (starsEarned == 3)
            ThreeStar.SetActive(true);
        this.goToGarageScreenOnComplete = goToGarageScreenOnComplete;
        starsFromLevel = starsEarned;
        timeEarnedText.text = (Math.Floor(timeEarned / 60) % 60).ToString("00") + ":" + Convert.ToInt32(timeEarned % 60).ToString("00") ; //TimeSpan.FromSeconds(timeEarned).ToString("hh':'mm':'ss");//string.Format("{0:t}", TimeSpan.FromSeconds(timeEarned));

        twoStarTimeText.text = (Math.Floor(twoStarTime / 60) % 60).ToString("00") + ":" + Convert.ToInt32(twoStarTime % 60).ToString("00");//string.Format("Two Star Time: {0:t}", twoStarTime );
        threeStarTimeText.text = (Math.Floor(threeStarTime / 60) % 60).ToString("00") + ":" + Convert.ToInt32(threeStarTime % 60).ToString("00");//string.Format("Three Star Time: {0:t}", threeStarTime);


        //hideHunt();
    }
    public void hideHunt()
    {
        huntScene.SetActive(false);


        oneStar.SetActive(false);
        twoStar.SetActive(false);
        ThreeStar.SetActive(false);


        timeEarnedText.text = "";
        twoStarTimeText.text = "";
        threeStarTimeText.text = "";

        //communicate with map manager to display the map
        MapManager.instance.nodeActivityDone(goToGarageScreenOnComplete, starsFromLevel);

    }

}

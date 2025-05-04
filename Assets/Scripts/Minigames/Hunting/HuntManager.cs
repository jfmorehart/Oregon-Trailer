using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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

    //instantiate from this prefab
    public StarEarnedVFX starEarnedVFX_Prefab;
    public GameObject starResourceUIObj;
    public void createStarVFX(GameObject star)
    {
        //instantiate it under the mom
        GameObject starVfx = Instantiate(starEarnedVFX_Prefab.gameObject, star.transform);
        starVfx.GetComponent<StarEarnedVFX>().startPos = star.transform.position;
        starVfx.GetComponent<StarEarnedVFX>().endPos = starResourceUIObj.transform.position;
        Instantiate(new GameObject("Start"+ star.transform.parent.name), starVfx.GetComponent<StarEarnedVFX>().startPos, quaternion.identity);
        Instantiate(new GameObject("End " + star.transform.parent.name), starVfx.GetComponent<StarEarnedVFX>().endPos, quaternion.identity);

    }


    public void displayHunt(bool goToGarageScreenOnComplete, float timeEarned, float twoStarTime, float threeStarTime, int starsEarned)
    {

        huntScene.SetActive(true);
        //one star is active automatically
        Debug.Log("Stars earned " + starsEarned);
        oneStar.SetActive(true);
        // create vfx here
        createStarVFX(oneStar);
        if (starsEarned == 2)
        {
            twoStar.SetActive(true);   
            createStarVFX(twoStar);
                //create vfx here
        }
        if (starsEarned == 3)
        {
            ThreeStar.SetActive(true);
            twoStar.SetActive(true);//both should be set active
            createStarVFX(twoStar);
            createStarVFX(ThreeStar);
            // create vfx here
        }
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SkillMatic : MonoBehaviour
{
	public Character.ScalableSkill myskill;

	public Slider sl;
	public TMP_Text value;


	private void Update()
	{
		value.text = sl.value.ToString();
	}

	public void Plus() {
		if(SkillPicker.points > 0 && sl.value < 10) {
			sl.value++;
			SkillPicker.points--;
		}
    }
	public void Minus()
	{
		if (sl.value > 1)
		{
			sl.value--;
			SkillPicker.points++;
		}
	}
}

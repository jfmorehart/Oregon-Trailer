using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.SceneManagement;

public class SkillPicker : MonoBehaviour
{
	public Character characterPicking;

	public static int points = 10;

	public UI_SkillMatic[] skills;

	public CharacterBase gatorheadBase;

	public TMP_Text ptext;

	private void Start()
	{
		if (CurrentGame.activeParty == null) {
			Character gator = new Character(gatorheadBase);
			CurrentGame.NewParty(gator);
			characterPicking = gator;
		}
		Load();
	}
	private void Update()
	{
		ptext.text = "Points Remaining:" + points.ToString();
	}
	private void LoadSkills()
	{
		characterPicking = CurrentGame.activeParty.members[0];
		points = 13;
		for (int i = 0; i < skills.Length; i++)
		{
			skills[i].sl.value = characterPicking.playerScalableSkills[(int)skills[i].myskill];
			//characterPicking.SetScalableSkill(skills[i].myskill, Mathf.RoundToInt(skills[i].sl.value));
			points -= Mathf.RoundToInt(skills[i].sl.value);
			Debug.Log(skills[i].sl.value);
		}
	}
	private void SaveSkills()
	{
		for(int i = 0; i < skills.Length; i++) {
			characterPicking.SetScalableSkill(skills[i].myskill, Mathf.RoundToInt(skills[i].sl.value));
			Debug.Log(characterPicking.SkillCheck(skills[i].myskill));
		}
	}

	public void Save() {
		SaveSkills();
		CurrentGame.activeParty = new Party(characterPicking);
		string destination = Application.persistentDataPath + "/unsecure_save.oregon";
		FileStream file;

		if (File.Exists(destination)) file = File.OpenWrite(destination);
		else file = File.Create(destination);

		Party data = CurrentGame.activeParty;
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file, data);
		file.Close();
	}

	public void Load() {
		string destination = Application.persistentDataPath + "/unsecure_save.oregon";
		FileStream file = null;

		if (File.Exists(destination))
		{
			file = File.OpenRead(destination);
		}
		else {
			Debug.LogError("no save file found");
		}

		BinaryFormatter bf = new BinaryFormatter();
		Party data = (Party) bf.Deserialize(file);
		file.Close();

		CurrentGame.activeParty = data;
		LoadSkills();
	}
	public void Exit() {
		Save();
		SceneManager.LoadScene(0);
    }
}

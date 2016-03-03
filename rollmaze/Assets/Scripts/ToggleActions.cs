using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ToggleActions : MonoBehaviour
{
	private GameObject currentSelectedToggle;

	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			if (currentSelectedToggle == null)
			{
				currentSelectedToggle = EventSystem.current.firstSelectedGameObject;
			}
		}
		else
		{
			currentSelectedToggle = EventSystem.current.currentSelectedGameObject;
		}

		EventSystem.current.SetSelectedGameObject(currentSelectedToggle);
	}

	public void OnClick00()
	{
        MainMenuManager.Instance.LevelSelected("_Scene_Tutorial_levle0");
		//SceneManager.LoadScene("_Scene_Tutorial_levle0");
	}

	public void OnClick01()
	{
        MainMenuManager.Instance.LevelSelected("_Scene_Tutorial_levle0");
		//SceneManager.LoadScene("_Scene_Tutorial_levle1");
	}

	public void OnClick02()
	{
        MainMenuManager.Instance.LevelSelected("_Scene_Tutorial_levle0");
		//SceneManager.LoadScene("_Scene_Tutorial_levle2");
	}

	public void OnClick03()
	{
        MainMenuManager.Instance.LevelSelected("_Scene_Tutorial_levle0");
		//SceneManager.LoadScene("_Scene_Tutorial_levle3");
	}
}

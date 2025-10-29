using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class InputEditor : EditorWindow
{
	// ----------------------------------------------------------------------------------------------------------------------------------------------------------
	[MenuItem("Oyedoyin/Fixed Wing/Miscellaneous/Setup Input", false, 4900)]
	public static void ShowWindow()
	{
		GetWindow<InputEditor>("Input Configuration");
	}


	Color backgroundColor;[HideInInspector] public string currentTab;
	Color silantroColor;[HideInInspector] public int toolbarTab;
	Vector2 scrollPosition = Vector2.zero;
	// ----------------------------------------------------------------------------------------------------------------------------------------------------------
	void OnGUI()
	{
		silantroColor = new Color(1, 0.4f, 0);
		backgroundColor = GUI.backgroundColor;
		toolbarTab = GUILayout.Toolbar(toolbarTab, new string[] { "Configure", "View Layout" });


		//..................................TABS
		switch (toolbarTab)
		{
			case 0: currentTab = "Configure"; break;
			case 1: currentTab = "View Layout"; break;
		}


		//..................................ACTIONS
		switch (currentTab)
		{
			case "Configure":
				//.....NAME
				GUILayout.Space(5f);
				GUI.color = silantroColor;
				EditorGUILayout.HelpBox("Input Configuration", MessageType.None);
				GUI.color = backgroundColor;
				GUILayout.Space(5f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox("Your input manager will be reconfigured to include the input keys needed by the aircraft, the layout is shown in the next toolbar", MessageType.Info);
				GUILayout.Space(3f);
				EditorGUILayout.HelpBox("The current Input manager asset will be backed up to '/Silantro Simulator/Fixed Wing/Data/Input/User/' so you can copy it back at any time (Incase you need to)", MessageType.Warning);
				GUI.color = backgroundColor;
				GUILayout.Space(5f);



				GUILayout.Space(20f);
				GUI.color = silantroColor;
				if (GUILayout.Button("Continue"))
				{
					InitializeQuick();
				}
				break;


			case "View Layout":
				//.....NAME
				GUILayout.Space(5f);
				GUI.color = Color.white;
				EditorGUILayout.HelpBox("Note: This is not editable..Just an overview of the input keys", MessageType.Info);
				GUI.color = backgroundColor;
				
				
				scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));
				var inputManager = AssetDatabase.LoadAllAssetsAtPath("Assets/Silantro Simulator/Fixed Wing/Scripts/Editor/Input/InputManager.asset")[0];
				
				SerializedObject obj = new SerializedObject(inputManager);
				SerializedProperty axisArray = obj.FindProperty("m_Axes");
				if (axisArray.arraySize == 0) Debug.Log("No Axes");
				else { EditorGUILayout.PropertyField(axisArray); }
				GUILayout.Space(100f);
				GUILayout.EndScrollView();


				// ------------------------------------- Check Input Config
				break;
		}
	}

	public static void InitializeQuick()
	{
		string sourcePath = Application.dataPath + "/Silantro Simulator/Fixed Wing/Scripts/Editor/Input/SilantroInput.dat";
		string destPath = Application.dataPath + "/../ProjectSettings/InputManager.asset";
		string defaultPath = Application.dataPath + "/Silantro Simulator/Fixed Wing/Scripts/Editor/Input/SilantroDefault.dat";
		string backupPath = Application.dataPath + "/Silantro Simulator/Fixed Wing/Data/Input/User/InputManager.asset";



		if (!File.Exists(sourcePath))
		{
			Debug.LogError("Source File missing....Please Reimport file");
		}
		if (!File.Exists(destPath))
		{
			Debug.LogError("Destination input manager missing");
		}
		if (File.Exists(destPath) && File.Exists(sourcePath))
		{
			File.Copy(defaultPath, destPath, true);
			File.Copy(sourcePath, destPath, true);
			File.Copy(sourcePath, backupPath, true);
			AssetDatabase.Refresh();
			Debug.Log("Your old Input manager asset has been backed up to " + backupPath);
			Debug.Log("Input Setup Successful!");
		}
	}
}

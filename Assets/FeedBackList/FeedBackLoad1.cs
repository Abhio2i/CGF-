using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class FeedBackLoad1 : MonoBehaviour
{

    public MissionSave missionSave;
    public string FolderPath = "CGF_Mission_Feedback";
    public string Selectpath = "D:/CGF_Mission_Feedback/";
    public GameObject FeedBackButton;
    public Transform FeedBackpanelA;
    public Transform FeedBackpanelB;
    public GameObject feedbackpanelA;
    public GameObject feedbackpanelB;
    public GameObject ButtonPrefeb;
    public Dropdown SortDropdownA;
    public Dropdown SortDropdownB;
    public GameObject FeedBackSearchA;
    public GameObject FeedBackSearchB;
    public Button FeedBackSearchbuttonA;
    public InputField searchInputA;
    public InputField searchInputB;
    //public DatePicker startDatePickerA;
    //public DatePicker startDatePickerB;
    private DateTime previousStartDateA;
    private DateTime previousStartDateB;
    public GameObject searchFieldA;
    public GameObject SearchFieldB;
    public GameObject DatePickerA;
    public GameObject DatePickerB;
    public MissionSave DataPath;

    public int poolSize = 30;


    public List<GameObject> buttonpoolA = new List<GameObject>();
    public List<GameObject> buttonpoolB = new List<GameObject>();



    public void searchButtonA()
    {
        FeedBackSearchA.SetActive(true);
      
    }
    void Listpooling(List<GameObject> buttonpool,Transform panel )
    {
        for (int i = 0; i < poolSize; i++)
        {

            GameObject button = Instantiate(ButtonPrefeb);
            button.SetActive(false); // Initially inactive
            button.transform.SetParent(panel, false);
            buttonpool.Add(button);

        }
    }
    void OnSearchValueChanged(string searchText, List<GameObject> buttonpool, Transform panel, string path)
    {
        // Saare folders le lenge directory se
        string[] folders = Directory.GetDirectories(path);

        // Agar kuch searchText diya gaya hai, to uske basis par filter karenge
        if (!string.IsNullOrEmpty(searchText))
        {
            folders = folders.Where(folder => Path.GetFileName(folder).ToLower().Contains(searchText.ToLower())).ToArray();
        }

        // Filtered folders ko show karenge
        ShowFolders(folders, buttonpool, panel);
    }
    public void FeedBackA()
    {
        string path = missionSave.DatabasePath + FolderPath;
        string[] folders = Directory.GetDirectories(path);
        ShowFolders(folders, buttonpoolA, FeedBackpanelA);

    }
    private void Start()
    {
        string path = missionSave.DatabasePath+ FolderPath;

        Selectpath = path+"/";
        PlayerPrefs.SetInt("PlayFeedBack", 0);
        Listpooling(buttonpoolA, FeedBackpanelA);
        Listpooling(buttonpoolB, FeedBackpanelB);
        setdropDown(SortDropdownA);
        setdropDown(SortDropdownB);
        FeedBackSearchA.SetActive(false);
        // Adding listeners to the dropdowns for sorting
        SortDropdownA.onValueChanged.AddListener(delegate { OnDropdownValueChanged(SortDropdownA, buttonpoolA, FeedBackpanelA,path); });
        SortDropdownB.onValueChanged.AddListener(delegate { OnDropdownValueChanged(SortDropdownB, buttonpoolB, FeedBackpanelB,Selectpath); });
        // Add listeners for input fields
        searchInputA.onValueChanged.AddListener(text => OnSearchValueChanged(text, buttonpoolA, FeedBackpanelA, path));
        searchInputB.onValueChanged.AddListener(text => OnSearchValueChanged(text, buttonpoolB, FeedBackpanelB, Selectpath));
          
        //set searchfield and DatePicker (false)
        DatePickerA.SetActive(false);
        DatePickerB.SetActive(false);   
        searchFieldA.SetActive(false);
        SearchFieldB.SetActive(false);  


    } 
    

    private void Update()
    {
        // Check for DatePicker A changes
        //if (startDatePickerA.SelectedDate != previousStartDateA)
        {
            //previousStartDateA = startDatePickerA.SelectedDate;
            //OnDateChanged(startDatePickerA.SelectedDate, buttonpoolA, FeedBackpanelA, path);
        }

        // Check for DatePicker B changes
        //if (startDatePickerB.SelectedDate != previousStartDateB)
        {
            //previousStartDateB = startDatePickerB.SelectedDate;
            //OnDateChanged(startDatePickerB.SelectedDate, buttonpoolB, FeedBackpanelB, Selectpath);
        }
    }  

    public void searchbuttonA()
    {
        if (searchFieldA.activeInHierarchy)
        {

            searchFieldA.SetActive(false);
        }
        else if (!searchFieldA.activeInHierarchy){
            searchFieldA.SetActive(true);
        }
    

    }
    public void searchbuttonB()
    {
        if (SearchFieldB.activeInHierarchy)
        {

            SearchFieldB.SetActive(false);
        }
        else if (!SearchFieldB.activeInHierarchy)
        {
            SearchFieldB.SetActive(true);
        }

    }
    public void CalenderbutonA (){

        if (DatePickerA.activeInHierarchy)
        {
            DatePickerA.SetActive(false);
        }
        else if (!DatePickerA.activeInHierarchy) {
            DatePickerA.SetActive(true);
        }
    
    
    }

    public void CalenderbutonB()
    {

        if (DatePickerB.activeInHierarchy)
        {
            DatePickerB.SetActive(false);
        }
        else if (!DatePickerB.activeInHierarchy)
        {
            DatePickerB.SetActive(true);
        }


    }
    void OnDropdownValueChanged(Dropdown dropdown, List<GameObject> buttonpool, Transform panel, string path)
    {

        string[] folders = Directory.GetDirectories(path);
        folders = SortFolders(folders, dropdown.value);
        ShowFolders(folders, buttonpool, panel);


    }
   void OnSearchValueChanged(string searchText, List<GameObject> buttonpool, Transform panel, string path, string startDateText, string endDateText)
    {
        // Get all folders from the directory
        string[] folders = Directory.GetDirectories(path);

        // Search by name
        if (!string.IsNullOrEmpty(searchText))
        {
            folders = folders.Where(folder => Path.GetFileName(folder).ToLower().Contains(searchText.ToLower())).ToArray();
        }

        // Parse dates from user input if provided
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;

        if (DateTime.TryParse(startDateText, out DateTime parsedStartDate))
        {
            startDate = parsedStartDate;
        }

        if (DateTime.TryParse(endDateText, out DateTime parsedEndDate))
        {
            endDate = parsedEndDate;
        }

        // Search by date range
        folders = folders.Where(folder =>
        {
            DateTime lastWriteTime = Directory.GetLastWriteTime(folder);
            return lastWriteTime >= startDate && lastWriteTime <= endDate;
        }).ToArray();

        // Show the filtered folders
        ShowFolders(folders, buttonpool, panel);
    }
    void OnDateChanged(DateTime startDate, List<GameObject> buttonpool, Transform panel, string path)
    {
        // Get all folders from the directory
        string[] folders = Directory.GetDirectories(path);

        // Filter by start date
        folders = folders.Where(folder =>
        {
            DateTime lastWriteTime = Directory.GetLastWriteTime(folder);
            return lastWriteTime >= startDate;
        }).ToArray();

        // Show the filtered folders
        ShowFolders(folders, buttonpool, panel);
    }

    private string[] SortFolders(string[] folders, int sortOption)
    {
        switch (sortOption)
        {
            case 0: // Sort by Name
                return folders.OrderBy(folder => Path.GetFileName(folder)).ToArray();
            case 1: // Sort by Name
                return folders.OrderByDescending(folder => Path.GetFileName(folder)).ToArray();
            case 2://sort by Date
                return folders.OrderBy(folder => Directory.GetLastWriteTime(folder)).ToArray();
            case 3://sort by Datedescending
                return folders.OrderByDescending(folder => Directory.GetLastWriteTime(folder)).ToArray();
            default:
                return folders;
        }
    }
   
    void setdropDown(Dropdown sortDropDown)
    {

        // Populate the dropdown options
        sortDropDown.ClearOptions();
        sortDropDown.AddOptions(new System.Collections.Generic.List<string> { "Sort by NameA", "Sort by NameD", "Sort by Date/TimeA","Sort by Date/TimeD"});
        // Add listener to handle sorting when dropdown value changes
       

    }


    void ShowFolders(string[] folders, List<GameObject> buttonpool,Transform panel)
    {
        // Fetch all files from the specified folder path
      
        string[] GetFilesA = folders;


        // Loop over the file list and assign the pooled buttons
        for (int i = 0; i < GetFilesA.Length; i++)
        {
            DateTime LastModified = Directory.GetLastWriteTime(GetFilesA[i]);
            // If there are more files than the pool size, we reuse buttons
            if (i < buttonpoolA.Count)
            {
                buttonpool[i].SetActive(true); // Activate the button from the pool
                buttonpool[i].GetComponentInChildren<Text>().text = Path.GetFileName(GetFilesA[i]); // Set file name to button text
                buttonpool[i].transform.GetChild(1).GetComponent<Text>().text = LastModified.ToString("dd-MM-yy");
                int index = i; // Capturing index for the listener

                // Adding listener for when the button is clicked (prints file path)
                string currentFile = GetFilesA[i];
                buttonpool[i].GetComponent<Button>().onClick.RemoveAllListeners();
                buttonpool[i].GetComponent<Button>().onClick.AddListener(() => OnFileButtonClick(currentFile));
            }
        }
        // Deactivate extra buttons if there are more buttons than files
        for (int j = GetFilesA.Length; j < buttonpool.Count; j++)
        {
            buttonpool[j].SetActive(false);

        }
        // Method to handle file button clicks
        void OnFileButtonClick(string filePath)
        {
            Debug.Log("File clicked: " + filePath);
            // Hide the root folder panel and show the subfolder panel
              

            //SceneManager.LoadScene(filePath);   
            Selectpath = filePath;
            feedbackpanelA.gameObject.SetActive(false);
            feedbackpanelB.gameObject.SetActive(true);

            // Show the subfolders in the new panel
            Showsubfolders(Directory.GetDirectories(filePath), buttonpoolB, FeedBackpanelB);
            
            

        }
        void Showsubfolders(string[] subfolder,List<GameObject>buttonpool,Transform panel) {

            for (int i = 0; i < subfolder.Length; i++)
            {
                if (i < buttonpool.Count)
                {
                    DateTime LastModified = Directory.GetLastWriteTime(subfolder[i]);
                    buttonpool[i].SetActive(true);
                    buttonpool[i].GetComponentInChildren<Text>().text = Path.GetFileName(subfolder[i]);
                    buttonpool[i].transform.GetChild(1).GetComponent<Text>().text = LastModified.ToString("dd-MM-yy");
                    string currentfolder = subfolder[i];
                    //Listner for subfolderbutton to load scene

                    buttonpool[i].GetComponent<Button>().onClick.AddListener(() => LoadSceneOnButtonclick(currentfolder));

                }

                // Extra buttons ko deactivate karna
                for (int j = subfolder.Length; j < buttonpool.Count; j++)
                {
                    buttonpool[j].SetActive(false);
                }
            }
            void LoadSceneOnButtonclick(string folderPath) {

                string[] folder = Directory.GetDirectories(folderPath.Replace("\\","/"));
                if(folder.Length == 0)
                {

                    return;
                }
                string name = Path.GetFileName(folder[0]);
                string path = Path.GetDirectoryName(folder[0]);
                missionSave.path = path.Replace("\\", "/");
                missionSave.LoadMission(name);
                PlayerPrefs.SetInt("PlayFeedBack", 1);
                PlayerPrefs.SetString("FeedBackPath", path);
                string sceneName = "Main_Land_AtarangARDC";
                // Load the specified scene
                SceneManager.LoadScene(sceneName);  
            }




        }
    }







}

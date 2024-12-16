using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Story = Ink.Runtime.Story;

public class InkStory : MonoBehaviour
{
    [SerializeField] private TextAsset inkFile;

    private Story inkStory;

    [SerializeField] private TextMeshProUGUI textStory;

    [SerializeField] private CanvasGroup panelDialog;
    [SerializeField] private Button buttonContinue;

    [SerializeField] private CanvasGroup panelChoices;
    [SerializeField] private Transform parentButtons;
    [SerializeField] private GameObject prefabButtons;

    
    
    
    void Start()
    {
        buttonContinue.onClick.AddListener(GetNextStoryStep);
        
        
        
        
       inkStory = new Story(inkFile.text);
         
        panelDialog.ShowCanvasGroup();
        panelChoices.HideCanvasGroup();
         
         GetNextStoryStep();
         
         buttonContinue.onClick.AddListener(GetNextStoryStep);

         inkStory = new Story(inkFile.text);
         GetNextStoryStep();


    }

    

    
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) 
        {
            GetNextStoryStep();
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SelectChoice(0);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SelectChoice(1);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SelectChoice(2);
        }
      
    }

    
    
    void GetNextStoryStep()
    {
        if (inkStory.canContinue)
        {
            textStory.text = inkStory.Continue();
            Debug.Log(inkStory.state.currentChoices.Count > 0);
            
            
        }
        else if(inkStory.currentChoices.Count > 0)
        {
            panelChoices.ShowCanvasGroup();
            panelDialog.HideCanvasGroup();

            foreach (Transform button in parentButtons)
            {
                Destroy(button.gameObject);
            }
            
            
            for (int i = 0; i < inkStory.currentChoices.Count; i++)
            {
                Choice choice = inkStory.currentChoices[i];
                //s += "Choice " + i + ": " + choice.text + "\n";
                int j = i;
                GameObject buttonGO = Instantiate(prefabButtons, parentButtons);
                buttonGO.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
                buttonGO.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SelectChoice(j);
                });
            }
            
            
        }
        
        
    }

    void SelectChoice(int index)
    {
        panelChoices.HideCanvasGroup();
        panelDialog.ShowCanvasGroup();
        
        inkStory.ChooseChoiceIndex(index);
        GetNextStoryStep();
    }
}
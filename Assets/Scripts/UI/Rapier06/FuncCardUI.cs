using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FuncCardUI : MonoBehaviour
{
    [Header("Function Information")]
    [SerializeField] protected ServerExe connectedExe;

    [SerializeField] protected TextMeshProUGUI tabTitle;
    [SerializeField] protected TextMeshProUGUI funcName;
    [SerializeField] protected Image funcLogo;
    [SerializeField] protected TextMeshProUGUI functionDescription;

    [Header("Data Information")]
    [SerializeField] protected TextMeshProUGUI inputTitle;
    [SerializeField] protected Image inputFileIcon;
    [SerializeField] protected TextMeshProUGUI outputTitle;
    [SerializeField] protected Image outputFileIcon;

    [Header("Buttons Information")]
    [SerializeField] protected Button prevButton;
    [SerializeField] protected Button nextButton;
    [SerializeField] protected Button confirmButton;
    [SerializeField] protected Button returnButton;

    [Header("Output")]
    [SerializeField] protected GameObject functionOutput;

    protected List<DiscSlotContent> validContentToApplyTo;
    protected DataDrive relevantPd;
    protected DiscSlotContent selectedContent;

    protected virtual void Start()
    {
        prevButton.onClick.AddListener(SelectPrevious);
        nextButton.onClick.AddListener(SelectNext);
        confirmButton.onClick.AddListener(ConfirmExecution);

        gameObject.name = connectedExe.name;
        tabTitle.text = connectedExe.fileName;
        funcName.text = connectedExe.objectName;
        funcLogo.sprite = connectedExe.functionImage;
        functionDescription.text = connectedExe.functionFlavourText;

        gameObject.SetActive(false);
    }
    public virtual void OpenFuncUI(List<DiscSlotContent> validContent, DataDrive pd = null)
    {
        validContentToApplyTo = validContent;
        relevantPd = pd;

        SetSelectedFile(validContentToApplyTo[0]);
        if(validContentToApplyTo.Count <= 1)
        {
            prevButton.interactable = false;
            nextButton.interactable = false;
        } else
        {
            prevButton.interactable = true;
            nextButton.interactable = true;
        }

        gameObject.SetActive(true);
    }

    protected void SetSelectedFile(DiscSlotContent content)
    {
        selectedContent = content;
        inputTitle.text = ServerHubUI.Instance.GetFormattedDataSlotName(selectedContent);
        inputFileIcon.sprite = ServerHubUI.Instance.GetFormattedDataSlotIcon(selectedContent);
    }

    protected virtual void SelectNext()
    {
        int i = validContentToApplyTo.IndexOf(selectedContent);
        i++;
        if (i >= validContentToApplyTo.Count) i = 0;
        SetSelectedFile(validContentToApplyTo[i]);
    }

    protected virtual void SelectPrevious()
    {
        int i = validContentToApplyTo.IndexOf(selectedContent);
        i--;
        if (i < 0) i = validContentToApplyTo.Count - 1;
        SetSelectedFile(validContentToApplyTo[i]);
    }

    protected virtual void ConfirmExecution()
    {
        gameObject.SetActive(false);
        functionOutput.SetActive(true);
        returnButton.interactable = true;
    }
}

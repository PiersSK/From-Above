using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FuncCardUI : MonoBehaviour
{
    [Header("Function Information")]
    [SerializeField] private ServerExe connectedExe;

    [SerializeField] private TextMeshProUGUI tabTitle;
    [SerializeField] private TextMeshProUGUI funcName;
    [SerializeField] private Image funcLogo;
    [SerializeField] private TextMeshProUGUI functionDescription;

    [Header("Data Information")]
    [SerializeField] private TextMeshProUGUI inputTitle;
    [SerializeField] private Image inputFileIcon;
    [SerializeField] protected TextMeshProUGUI outputTitle;
    [SerializeField] protected Image outputFileIcon;

    [Header("Buttons Information")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] protected Button confirmButton;

    [Header("Output")]
    [SerializeField] protected GameObject functionOutput;

    private List<DiscSlotContent> validContentToApplyTo;
    private DataDrive relevantPd;
    protected DiscSlotContent selectedContent;

    private void Start()
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
    public void OpenFuncUI(List<DiscSlotContent> validContent, DataDrive pd = null)
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

    private void SetSelectedFile(DiscSlotContent content)
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
    }
}

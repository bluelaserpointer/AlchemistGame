using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ChemicalSummonManager : MonoBehaviour
{
    public static string Version => "alpha1.2.0";
    public static Canvas MainCanvas
    {
        get;
        private set;
    }

    //inspector
    [SerializeField]
    Canvas mainCanvas;

    //data
    protected void Init()
    {
        MainCanvas = mainCanvas;
    }
    /// <summary>
    /// 进入战斗(按钮事件参照用)
    /// </summary>
    /// <param name="match"></param>
    public void GotoMatch(Match match)
    {
        PlayerSave.StartMatch(match);
    }
    public void GotoMenu()
    {
        SceneManager.LoadScene("Title");
    }
    public void GotoMap()
    {
        SceneManager.LoadScene("Map");
    }
    public void StartEvent(Event newEvent)
    {
        PlayerSave.StartEvent(newEvent);
    }
    public void ProgressEvent()
    {
        PlayerSave.ProgressActiveEvent();
    }
}

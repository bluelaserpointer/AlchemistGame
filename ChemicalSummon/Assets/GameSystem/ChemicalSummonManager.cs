using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ChemicalSummonManager : MonoBehaviour
{
    public static string Version => "alpha1.2.3";
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
    public static bool CurrentSceneIsMatch => SceneManager.GetActiveScene().name.Equals("Match");
    public static bool CurrentSceneIsMap => SceneManager.GetActiveScene().name.Equals("Map");
    public static bool CurrentSceneIsTitle => SceneManager.GetActiveScene().name.Equals("Title");
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

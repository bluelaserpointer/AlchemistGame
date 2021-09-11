using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ChemicalSummonManager : MonoBehaviour
{
    public static string Version => "alpha3.0.0";
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
        DynamicGI.UpdateEnvironment();
    }
    public static bool CurrentSceneIsMatch => SceneManager.GetActiveScene().name.Equals("Match");
    public static bool CurrentSceneIsMap => SceneManager.GetActiveScene().name.Equals("World");
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
    public void GotoWorld()
    {
        SceneManager.LoadScene("World");
    }
    public void StartEvent(Event newEvent)
    {
        PlayerSave.StartEvent(newEvent);
    }
    public void ProgressEvent()
    {
        PlayerSave.ProgressActiveEvent();
    }
    public static TranslatableSentenceSO LoadTranslatableSentence(string name)
    {
        TranslatableSentenceSO sentence = Resources.Load<TranslatableSentenceSO>("TranslatableSentence/" + name);
        if (sentence == null)
            Debug.LogWarning("Cannot find TranslatableSentence ScriptableObject by name: " + name);
        return sentence;
    }
    public static void UpdateAllSentence()
    {
        foreach (var substance in Substance.GetAll())
        {
            foreach (var ability in substance.abilities)
                ability.InitDescription();
        }
    }
}

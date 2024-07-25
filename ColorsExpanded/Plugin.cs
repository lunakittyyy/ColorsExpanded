using BepInEx;
using GorillaNetworking;
using Jerald;
using System.Text;
using UnityEngine;
[assembly: AutoRegister]
namespace ColorsExpanded
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Awake()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }
    }

    [AutoRegister]
    public class ColorsPage : Page
    {
        public override string PageName => "CLRSXP";
        public string redTyped;
        public string greenTyped;
        public string blueTyped;
        public string currentLine = "RED";
        public ColorsPage()
        {
            base.OnKeyPressed += (key) =>
            {
                switch (key.Binding)
                {
                    case GorillaKeyboardBindings.option1:
                        currentLine = "RED";
                        break;
                    case GorillaKeyboardBindings.option2:
                        currentLine = "GREEN";
                        break;
                    case GorillaKeyboardBindings.option3:
                        currentLine = "BLUE";
                        break;
                    case GorillaKeyboardBindings.enter:
                        PlayerPrefs.SetFloat("redValue", float.Parse(redTyped));
                        PlayerPrefs.SetFloat("greenValue", float.Parse(greenTyped));
                        PlayerPrefs.SetFloat("blueValue", float.Parse(blueTyped));
                        Debug.Log($"R:{float.Parse(redTyped)} G:{float.Parse(greenTyped)}, B:{float.Parse(blueTyped)}");
                        GorillaTagger.Instance.UpdateColor(float.Parse(redTyped), float.Parse(greenTyped), float.Parse(blueTyped));
                        PlayerPrefs.Save();
                        break;

                    case GorillaKeyboardBindings.delete:
                        switch (currentLine)
                        {
                            case "RED":
                                redTyped = redTyped.Remove(redTyped.Length - 1);
                                break;
                            case "GREEN":
                                greenTyped = greenTyped.Remove(greenTyped.Length - 1);
                                break;
                            case "BLUE":
                                blueTyped = blueTyped.Remove(blueTyped.Length - 1);
                                break;
                        }
                        break;
                    default:
                        switch (currentLine)
                        {
                            case "RED":
                                redTyped += key.characterString;
                                break;
                            case "GREEN":
                                greenTyped += key.characterString;
                                break;
                            case "BLUE":
                                blueTyped += key.characterString;
                                break;
                        }
                        break;
                }
                UpdateContent();
            };
        }

        public override StringBuilder GetPageContent()
        {
            var sb = new StringBuilder();
            sb.AppendLine("OPTION 1 FOR RED");
            sb.AppendLine("OPTION 2 FOR GREEN");
            sb.AppendLine("OPTION 3 FOR BLUE");
            sb.AppendLine("ENTER TO SET COLOR");
            sb.AppendLine($"R: {redTyped}");
            sb.AppendLine($"G: {greenTyped}");
            sb.AppendLine($"B: {blueTyped}");
            return sb;
        }
    }
}

using System.Reflection;
using MegaCrit.Sts2.Core.Timeline;
using TheWildfire.TheWildfireCode.Epochs;

namespace TheWildfire.TheWildfireCode.Patches.EpochPatches;

public class EpochRegistration
{
        internal static readonly Type[] WildfireEpochTypes =
    [
        typeof(Wildfire1Epoch),
        typeof(Wildfire2Epoch),
        typeof(Wildfire3Epoch),
        typeof(Wildfire4Epoch),
        typeof(Wildfire5Epoch),
        typeof(Wildfire6Epoch),
        typeof(Wildfire7Epoch)
    ];

    public static void RegisterEpochs()
    {
        RegisterEpochModels();
        RegisterStoryModel();
    }

    private static void RegisterEpochModels()
    {
        try
        {
            var epochType = typeof(EpochModel);
            var epochDictField = epochType.GetField("_epochTypeDictionary", BindingFlags.Static | BindingFlags.NonPublic);
            var typeToIdField = epochType.GetField("_typeToIdDictionary", BindingFlags.Static | BindingFlags.NonPublic);
            var allIdsField = epochType.GetField("_allEpochIds", BindingFlags.Static | BindingFlags.NonPublic);
            var allEpochsField = epochType.GetField("_allEpochs", BindingFlags.Static | BindingFlags.NonPublic);

            if (epochDictField == null || typeToIdField == null)
            {
                MainFile.Logger.Error("Could not find EpochModel static dictionaries via reflection!");
                return;
            }

            var epochDict = (Dictionary<string, Type>)epochDictField.GetValue(null)!;
            var typeToId = (Dictionary<Type, string>)typeToIdField.GetValue(null)!;
            var allEpochs = (List<Type>)allEpochsField!.GetValue(null);

            foreach (var type in WildfireEpochTypes)
            {
                var epoch = (EpochModel)Activator.CreateInstance(type)!;
                string id = epoch.Id;

                if (epochDict.ContainsKey(id))
                {
                    MainFile.Logger.Info($"Epoch {id} already registered, skipping.");
                    continue;
                }

                epochDict[id] = type;
                typeToId[type] = id;
                allEpochs!.Add(type);
                MainFile.Logger.Info($"Registered epoch: {id}");
            }

            allIdsField?.SetValue(null, null);
            MainFile.Logger.Info($"Registered {WildfireEpochTypes.Length} Wildfire epochs successfully.");
        }
        catch (Exception ex)
        {
            MainFile.Logger.Error($"Failed to register Wildfire epochs: {ex}");
        }
    }

    private static void RegisterStoryModel()
    {
        try
        {
            var storyType = typeof(StoryModel);
            var storyDictField = storyType.GetField("_storyTypeDictionary", BindingFlags.Static | BindingFlags.NonPublic);

            if (storyDictField == null)
            {
                MainFile.Logger.Error("Could not find StoryModel._storyTypeDictionary via reflection!");
                return;
            }

            var storyDict = (Dictionary<string, Type>)storyDictField.GetValue(null)!;
            var story = new WildfireStory();
            var idProp = storyType.GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic)!;
            string storyId = (string)idProp.GetValue(story)!;

            if (storyDict.ContainsKey(storyId))
            {
                MainFile.Logger.Info($"Story {storyId} already registered, skipping.");
                return;
            }

            storyDict[storyId] = typeof(WildfireStory);
            MainFile.Logger.Info($"Registered Wildfire story: {storyId}");
        }
        catch (Exception ex)
        {
            MainFile.Logger.Error($"Failed to register Wildfire story: {ex}");
        }
    }
}
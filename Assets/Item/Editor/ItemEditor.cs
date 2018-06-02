using UnityEditor;
using UnityEditor.Callbacks;
using uItem;

public class ItemEditor : EditorWindow
{
    [MenuItem("ItemSystem/NewItem")]
    public static void CreateNewItem()
    {
        AssetUtility.CreateAsset<ItemTemplate>("Items");
    }

    [OnOpenAsset(0)]
    public static bool LoadItem(int instanceId, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceId);
        if (obj is ItemTemplate)
        {
            ItemEditor editor = GetWindow<ItemEditor>();
            editor.Show();
            return true;
        }
        return false;
    }
}
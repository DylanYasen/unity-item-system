using UnityEditor;
using UnityEditor.Callbacks;
using uItem;

public class ItemEditor : EditorWindow
{
    [MenuItem("ItemSystem/NewItem")]
    public static void CreateNewItem()
    {
        AssetUtility.CreateAsset<Item>("Items");
    }

    [OnOpenAsset(0)]
    public static bool LoadItem(int instanceId, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceId);
        if (obj is Item)
        {
            ItemEditor editor = GetWindow<ItemEditor>();
            editor.Show();
            return true;
        }
        return false;
    }
}
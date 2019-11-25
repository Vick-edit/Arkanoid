using UnityEditor;
using UnityEngine.Tilemaps;

namespace ScriptableObjects
{
    /// <summary>
    ///     Тайл для удобной отрисовки уровней, просто помогает сопоставить место в пространстве с тем
    ///     на котором будет создан настоящий тайл кирпичика, то есть нам от него не нужна даже картинка
    /// </summary>
    public class BrickTile : Tile
    { 
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Tiles/BrickTile")]
        public static void CreateBrickTile()
        {
            var path = EditorUtility.SaveFilePanelInProject(
                "Save Brick Tile", 
                "New Brick Tile", 
                "Asset", 
                "Save Brick Tile", 
                "Assets");
            if (path == "") return;
            AssetDatabase.CreateAsset(CreateInstance<BrickTile>(), path);
        }
#endif
    }
}

using UnityEngine;
using UnityEditor;
using System.IO;

namespace Solutions.Utility.Editor
{
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
	public static class ScriptableObjectUtility
	{
		public static T CreateAsset<T> () where T : ScriptableObject
		{
			string path = AssetDatabase.GetAssetPath (Selection.activeObject);
			if (path == "") 
			{
				path = "Assets";
			} 
			else if (Path.GetExtension (path) != "") 
			{
				path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
			}

			return CreateAsset<T>(path + "/New " + typeof(T).ToString() + ".asset");
		}

		public static T CreateAsset<T> (string path) where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T> ();
			
			CreateAsset(path, asset);

			return asset;
		}

		public static void CreateAsset(string path, ScriptableObject asset) 
		{
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path);
			
			//UnityEngine.Debug.Log("Creating scriptable object at " + assetPathAndName);
			
			AssetDatabase.CreateAsset (asset, assetPathAndName);
			
			AssetDatabase.SaveAssets ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = asset;
		}

		/// <summary>
		/// Deletes the asset file.
		/// </summary>
		/// <param name="asset">The asset</param>
		public static void DeleteAsset(ScriptableObject asset)
		{
			string path = AssetDatabase.GetAssetPath(asset);
			MonoBehaviour.DestroyImmediate(asset, allowDestroyingAssets: true);
			FileUtil.DeleteFileOrDirectory(path);
			AssetDatabase.Refresh();
			bool doesStillExist = System.IO.File.Exists(path);
			if(doesStillExist)
			{
				System.IO.File.Delete(path);
			}
		}
	}
}
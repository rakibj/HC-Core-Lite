using UnityEngine;
using UnityEditor;

namespace Rakib
{
	public class ExtendedHotkeys : ScriptableObject
	{
		//Deselect all by pressing Shift + D
		
		
		
		[MenuItem("Rakib/Select Lightneer Bootstrap &b")]
		static void SelectLightneerBootstrap()
		{
			//Assets\Samples\Framework Lite\1.0.2\Bootstrap
			Selection.activeObject=AssetDatabase.LoadMainAssetAtPath("Assets/Samples/Framework Lite/1.0.9/Bootstrap/BootstrapLoaderConfig.asset");
		}

		[MenuItem("Rakib/Select GameConfig %t")]
		static void SelectColorSetup()
		{
			//Assets\_Project\Resources
			Selection.activeObject=AssetDatabase.LoadMainAssetAtPath("Assets/_Project/Resources/GameConfig.asset");
		}
//		
//[MenuItem("Rakib/Take Screenshot &t")]
//		static void TakeScreenshot()
//		{
//			Camera.main.GetComponent<Screenshot>().TakeScreenshot();
//		}
	}
}
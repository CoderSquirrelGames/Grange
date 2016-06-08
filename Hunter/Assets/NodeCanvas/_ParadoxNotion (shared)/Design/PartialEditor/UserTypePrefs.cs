#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace ParadoxNotion.Design{

	/// <summary>
	/// A collection of user prefered types and associated colors (stored in EditorPrefs)
	/// </summary>
    public static class UserTypePrefs {

		private static List<System.Type> _preferedTypes;


		//The default prefered types list to be shown wherever a type is important
		private static string defaultPreferedTypesList{
			get
			{
				var typeList = new List<System.Type>{

					//Primitives
					typeof(string),
					typeof(float),
					typeof(int),
					typeof(bool),

					//Unity structs
					typeof(Vector2),
					typeof(Vector3),
					typeof(Vector4),
					typeof(Quaternion),
					typeof(Bounds),
					typeof(Rect),
					typeof(Color),
					typeof(AnimationCurve),
/*
					//Unity important non Object classes
					typeof(Debug),
					typeof(Application),
					typeof(Mathf),
					typeof(Physics),
					typeof(Physics2D),
					typeof(Input),
					typeof(NavMesh),
					typeof(NavMeshHit),
					typeof(RaycastHit),
					typeof(RaycastHit2D),
					typeof(PlayerPrefs),
					typeof(Random),
					typeof(Time),
*/
					//Unity Component Objects
					typeof(Object),
					typeof(GameObject),
					typeof(Component),
					typeof(Transform),
					typeof(Animation),
					typeof(Animator),
					typeof(Rigidbody),
					typeof(Rigidbody2D),
					typeof(Collider),
					typeof(Collider2D),
					typeof(NavMeshAgent),
					typeof(CharacterController),
					typeof(AudioSource),
					typeof(Camera),
					typeof(Light),
					typeof(Renderer),
					typeof(SpriteRenderer),

					//UGUI
					#if UNITY_4_6
					typeof(RectTransform),
					typeof(UnityEngine.UI.Image),
					typeof(UnityEngine.UI.Button),
					typeof(UnityEngine.UI.Text),
					typeof(UnityEngine.UI.Slider),
					typeof(Canvas),
					#endif

					//Unity Asset Objects
					typeof(Material),
					typeof(Texture2D),
					typeof(Sprite),
					typeof(AudioClip),
					typeof(AnimationClip)
				};

				return string.Join("|", typeList.Select(t => t.FullName).ToArray() );
			}
		}

		///Get the prefered types set by the user
		public static List<System.Type> GetPreferedTypesList(System.Type baseType){

			if (_preferedTypes == null){
				_preferedTypes = new List<System.Type>();
				foreach(var s in EditorPrefs.GetString("ParadoxNotion.PreferedTypes", defaultPreferedTypesList).Split('|')){
					try { _preferedTypes.Add( ReflectionTools.GetType(s) ); }
					catch {Debug.Log(string.Format("Type '{0}' not found. It will be excluded", s));}
				}
			}

			return _preferedTypes.Where(t => baseType.IsAssignableFrom(t) && !t.IsGenericType ).OrderBy(t => t.Name).ToList();
		}

		///Set the prefered types list for the user
		public static void SetPreferedTypesList(List<System.Type> types){
			_preferedTypes = types;
			var joined = string.Join("|", types.Select(t => t.FullName).ToArray() );
			EditorPrefs.SetString("ParadoxNotion.PreferedTypes", joined);
		}

		///Reset the prefered types to the default ones
		public static void ResetTypeConfiguration(){
			EditorPrefs.SetString("ParadoxNotion.PreferedTypes", defaultPreferedTypesList);
			_preferedTypes = null;
		}

		//A Type to color lookup table
		private static readonly Dictionary<System.Type, Color> typeColors = new Dictionary<System.Type, Color>()
		{
			{typeof(bool),               new Color(1,0.6f,0.6f)},
			{typeof(float),              new Color(0.6f,0.6f,1)},
			{typeof(int),                new Color(0.6f,1,0.6f)},
			{typeof(string),             new Color(0.5f,0.5f,0.5f)},
			{typeof(Vector2),            new Color(1,0.6f,1)},
			{typeof(Vector3),            new Color(1,0.6f,1)},
			{typeof(UnityEngine.Object), Color.grey}
		};

		///Get the color preference for a type
		public static Color GetTypeColor(System.Type type){
			if (!EditorGUIUtility.isProSkin)
				return Color.white;
			if (type == null)
				return Color.red;
			if (typeColors.ContainsKey(type))
				return typeColors[type];

			foreach (var pair in typeColors){
				if (pair.Key.RTIsAssignableFrom(type))
					return typeColors[type] = pair.Value;
				if (typeof(System.Collections.IList).IsAssignableFrom(type)){
					if (type.IsArray){
						return typeColors[type] = GetTypeColor( type.GetElementType() );
					} else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)){
						return typeColors[type] = GetTypeColor( type.GetGenericArguments()[0] );
					}
				}
			}
			return typeColors[type] = new Color(1,1,1,0.8f);
		}

		///Get the hex color preference for a type
		public static string GetTypeHexColor(System.Type type){
			return ColorToHex(GetTypeColor(type));
		}

		static string ColorToHex(Color32 color){
			return ("#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2")).ToLower();
		}
		 
		static Color HexToColor(string hex){
			var r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			var g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			var b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b, 255);
		}
	}
}

#endif
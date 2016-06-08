﻿#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace ParadoxNotion.Design{

    /// <summary>
    /// ContextMenus, basicaly reflection ones
    /// </summary>

	partial class EditorUtils {

		///Get a selection menu of types deriving base type
		public static GenericMenu GetTypeSelectionMenu(Type baseType, Action<Type> callback, GenericMenu menu = null, string subCategory = null){

			if (menu == null)
				menu = new GenericMenu();

			if (subCategory != null)
				subCategory = subCategory + "/";

			GenericMenu.MenuFunction2 Selected = delegate(object selectedType){
				callback((Type)selectedType);
			};

			var scriptInfos = GetScriptInfosOfType(baseType);

			foreach (var info in scriptInfos.Where(info => string.IsNullOrEmpty(info.category))) {
			    menu.AddItem(new GUIContent(subCategory + info.name), false, Selected, info.type);
			}

			menu.AddSeparator("/");

			foreach (var info in scriptInfos.Where(info => !string.IsNullOrEmpty(info.category))) {
			    menu.AddItem(new GUIContent(subCategory + info.category + "/" + info.name), false, Selected, info.type);
			}

			return menu;
		}


		public static GenericMenu GetPreferedTypesSelectionMenu(Type type, Action<Type> callback, bool showInterfaces = true, GenericMenu menu = null, string subCategory = null){
			if (menu == null)
				menu = new GenericMenu();

			if (subCategory != null)
				subCategory = subCategory + "/";

			GenericMenu.MenuFunction2 Selected = delegate(object t){
				callback((Type)t);
			};							

			var listTypes = new Dictionary<Type, string>();
			foreach (var t in UserTypePrefs.GetPreferedTypesList(typeof(object))){
				if (type.IsAssignableFrom(t) || (t.IsInterface && showInterfaces) ){
					var nsString = string.IsNullOrEmpty(t.Namespace)? "No Namespace/" : (t.Namespace.Replace(".","/") + "/") ;
					var finalString = nsString + t.FriendlyName();
					menu.AddItem(new GUIContent(finalString), false, Selected, t);
					listTypes.Add( typeof(List<>).MakeGenericType(new Type[]{t}), finalString );
				}
			}

			menu.AddSeparator("/");
			foreach(var tPair in listTypes){
				menu.AddItem(new GUIContent("List<T>/" + tPair.Value), false, Selected, tPair.Key);
			}

			return menu;
		}

		//yeah this is very special but....
		public static void ShowPreferedTypesSelectionMenu(Type type, Action<Type> callback, bool showInterfaces = true){
			GetPreferedTypesSelectionMenu(type, callback, showInterfaces).ShowAsContext();
			Event.current.Use();
		}

		///Get a GenericMenu for field selection in a type
		public static GenericMenu GetFieldSelectionMenu(Type type, Type fieldType, Action<FieldInfo> callback, GenericMenu menu = null, string subMenu = null){
			
			if (menu == null)
				menu = new GenericMenu();

			if (subMenu != null)
				subMenu = subMenu + "/";

			GenericMenu.MenuFunction2 Selected = delegate(object selectedField){
				callback((FieldInfo)selectedField);
			};

			var itemAdded = false;
			foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public).Where(field => fieldType.IsAssignableFrom(field.FieldType))) {
			    menu.AddItem(new GUIContent( subMenu + type.FriendlyName() + "/" + field.Name), false, Selected, field);
			    itemAdded = true;
			}

			if (!itemAdded)
				menu.AddDisabledItem(new GUIContent(subMenu + type.FriendlyName()));

			return menu;
		}


		///Get a GenericMenu for properties of a type optionaly specifying mustRead & mustWrite
		public static GenericMenu GetPropertySelectionMenu(Type type, Type propType, Action<PropertyInfo> callback, bool mustRead = true, bool mustWrite = true, GenericMenu menu = null, string subMenu = null){
			
			if (menu == null)
				menu = new GenericMenu();

			if (subMenu != null)
				subMenu = subMenu + "/";

			GenericMenu.MenuFunction2 Selected = delegate(object selectedProperty){
				callback((PropertyInfo)selectedProperty);
			};

			var itemAdded = false;
			foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)){
				
				if (!prop.CanRead && mustRead)
					continue;

				if (!prop.CanWrite && mustWrite)
					continue;

				if (propType.IsAssignableFrom(prop.PropertyType)){
					menu.AddItem( new GUIContent( string.Format("{0}/{1} : {2}", subMenu + type.FriendlyName(), prop.Name, prop.PropertyType.FriendlyName())), false, Selected, prop );
					itemAdded = true;
				}
			}

			if (!itemAdded)
				menu.AddDisabledItem(new GUIContent(subMenu + type.FriendlyName()));

			return menu;
		}

		///Get a GenericMenu for method or property get/set methods selection in a type
		public static GenericMenu GetMethodSelectionMenu(Type type, Type returnType, Type acceptedParamsType, System.Action<MethodInfo> callback, int maxParameters, bool propertiesOnly, bool excludeVoid = false, GenericMenu menu = null, string subMenu = null){

			if (menu == null)
				menu = new GenericMenu();

			if (subMenu != null)
				subMenu = subMenu + "/";

			GenericMenu.MenuFunction2 Selected = delegate(object selectedMethod){
				callback((MethodInfo)selectedMethod);
			};

			var itemAdded = false;
			foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)){

				if (propertiesOnly != method.IsSpecialName)
					continue;

				if (method.IsGenericMethod)
					continue;

				if (!returnType.IsAssignableFrom(method.ReturnType))
					continue;

				if (method.ReturnType == typeof(void) && excludeVoid)
					continue;

				var parameters = method.GetParameters();
				if (parameters.Length > maxParameters && maxParameters != -1)
					continue;

				var isAssignable = true;
				if (parameters.Length > 0){
					if ( parameters.Any(param => !acceptedParamsType.IsAssignableFrom(param.ParameterType)) ) {
					    isAssignable = false;
					}
				}

				if (!isAssignable)
					continue;

				menu.AddItem(new GUIContent( subMenu + type.FriendlyName() + "/" + method.SignatureName()), false, Selected, method);
				itemAdded = true;
			}
			
			if (!itemAdded)
				menu.AddDisabledItem(new GUIContent(subMenu + type.FriendlyName()) );

			return menu;
		}

		///Get a menu for static methods in the list of types
		public static GenericMenu GetStaticMethodSelectionMenu(Type type, Action<MethodInfo> callback, GenericMenu menu = null, string subMenu = null){

			if (menu == null)
				menu = new GenericMenu();

			if (subMenu != null)
				subMenu = subMenu + "/";

			GenericMenu.MenuFunction2 Selected = delegate(object selectedMethod){
				callback((MethodInfo)selectedMethod);
			};			

			foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
				menu.AddItem(new GUIContent(subMenu + type.Name + "/" + method.SignatureName()), false, Selected, method);

			return menu;
		}

		///Get a GenericMenu for Events of the type
		public static GenericMenu GetEventSelectionMenu(Type type, Action<EventInfo> callback, GenericMenu menu = null, string subMenu = null){

			if (menu == null)
				menu = new GenericMenu();
			
			if (subMenu != null)
				subMenu = subMenu + "/";
			
			GenericMenu.MenuFunction2 Selected = delegate(object selectedEvent){
				callback((EventInfo)selectedEvent);
			};
			
			var itemAdded = false;
			foreach (var e in type.GetEvents(BindingFlags.Instance | BindingFlags.Public)){

				var m = e.EventHandlerType.GetMethod("Invoke");
				if ( (m.GetParameters().Length == 0 && m.ReturnType == typeof(void) ) || e.EventHandlerType == typeof(System.EventHandler) ){
					menu.AddItem(new GUIContent(subMenu + type.FriendlyName() + "/" + e.Name), false, Selected, e);
					itemAdded = true;
				}
			}

			if (!itemAdded)
				menu.AddDisabledItem(new GUIContent(subMenu + type.FriendlyName()) );
			
			return menu;
		}


		///Shows a GenericMenu for fields of all components of a game object
		public static void ShowGameObjectFieldSelectionMenu(GameObject go, Type fieldType, System.Action<FieldInfo> callback){
			var menu = new GenericMenu();
			foreach (var comp in go.GetComponents(typeof(Component)).Where(c => c.hideFlags == 0) )
				menu = GetFieldSelectionMenu(comp.GetType(), fieldType, callback, menu);
			menu.ShowAsContext();
			Event.current.Use();
		}

		///Shows a GenericMenu for properties of all components of a game object
		public static void ShowGameObjectPropertySelectionMenu(GameObject go, Type propType, Action<PropertyInfo> callback, bool mustRead = true, bool mustWrite = true){
			var menu = new GenericMenu();
			foreach (var comp in go.GetComponents(typeof(Component)).Where(c => c.hideFlags == 0) )
				menu = GetPropertySelectionMenu(comp.GetType(), propType, callback, mustRead, mustWrite, menu);
			menu.ShowAsContext();
			Event.current.Use();
		}

		///Shows a GenericMenu for methods of all components of a game object
		public static void ShowGameObjectMethodSelectionMenu(GameObject go, Type returnType, Type paramsType, System.Action<MethodInfo> callback, int maxParameters, bool propertiesOnly, bool excludeVoid = false){
			var menu = new GenericMenu();
			foreach (var comp in go.GetComponents(typeof(Component)).Where(c => c.hideFlags == 0) )
				menu = GetMethodSelectionMenu(comp.GetType(), returnType, paramsType, callback, maxParameters, propertiesOnly, excludeVoid, menu);
			menu.ShowAsContext();
			Event.current.Use();
		}


		///Show an Event selection menu for all components on a game object
		public static void ShowGameObjectEventSelectionMenu(GameObject go, System.Action<EventInfo> callback){
			var menu = new GenericMenu();
			foreach(var comp in go.GetComponents(typeof(Component)).Where(c => c.hideFlags == 0) )
				menu = GetEventSelectionMenu(comp.GetType(), callback, menu);
			menu.ShowAsContext();
			Event.current.Use();
		}



		//A generic menu selection
		public static void ShowMenu<T>(List<T> options, Action<T> callback){
			
			GenericMenu.MenuFunction2 Selected = delegate(object selection){
				callback((T)selection);
			};

			var menu = new GenericMenu();
			foreach (var element in options)
				menu.AddItem(new GUIContent(element.ToString()), false, Selected, element );
			menu.ShowAsContext();
			Event.current.Use();
		}

	}
}

#endif
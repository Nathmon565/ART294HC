using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AudioController))]
public class AudioControllerEditor : Editor {
	private SerializedProperty prop;
	private ReorderableList list;

	private void OnEnable() {
		prop = serializedObject.FindProperty("sounds");
		list = new ReorderableList(serializedObject, prop, true, true, true, true);

		//Change header
		list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Sound Sources"); };

		list.drawElementCallback = (Rect rect, int index, bool isactive, bool isfocused) => {
			SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;

			SerializedProperty elementName = element.FindPropertyRelative("name");
			string elementTitle = string.IsNullOrEmpty(elementName.stringValue) ? "UNNAMED SOUND" : elementName.stringValue;

			//Draw the list item as a property field, just like Unity does internally.
			EditorGUI.PropertyField(position:new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:element, label: new GUIContent(elementTitle), includeChildren: true);
		};

		list.elementHeightCallback = (int index) => {
			float propertyHeight = EditorGUI.GetPropertyHeight(list.serializedProperty.GetArrayElementAtIndex(index), true);
			float spacing = EditorGUIUtility.singleLineHeight / 2;
			return propertyHeight + spacing;
		};

		list.onAddCallback = (ReorderableList l) => {
			int index = l.serializedProperty.arraySize;
			l.serializedProperty.arraySize++;
			l.index = index;
			SerializedProperty element = l.serializedProperty.GetArrayElementAtIndex(index);
		};

		//Confirm deleting a sound
		list.onRemoveCallback = (ReorderableList l) => {
			if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this sound?", "Yes", "No")) {
				ReorderableList.defaultBehaviours.DoRemoveButton(l);
			}
		};
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}
}

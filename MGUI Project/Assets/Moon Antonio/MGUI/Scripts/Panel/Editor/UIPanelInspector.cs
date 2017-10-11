//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIPanelInspector.cs (11/10/2017)												\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Inspector de UIPanel										\\
// Fecha Mod:		11/10/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if odin

#endif
#endregion

namespace MoonAntonio.MGUI
{
#if odin
	public class UIPanelInspector : MonoBehaviour 
	{

	}
#else
	[CanEditMultipleObjects, CustomEditor(typeof(MoonAntonio.MGUI.UIPanel))]
	public class UIPanelInspector : Editor 
	{
		#region Variables
		private SerializedProperty panelIDProperty;
		private SerializedProperty panelCustomIDProperty;
		private SerializedProperty estadoInicialProperty;
		private SerializedProperty estadoActualProperty;
		private SerializedProperty keyEscapeProperty;
		private SerializedProperty transicionProperty;
		private SerializedProperty tipoTransicionProperty;
		private SerializedProperty duracionTransicionProperty;
		private SerializedProperty onTransicionInitProperty;
		private SerializedProperty onTransicionCompletadaProperty;
		#endregion

		#region Init
		protected virtual void OnEnable()
		{
			this.panelIDProperty = this.serializedObject.FindProperty("panelID");
			this.panelCustomIDProperty = this.serializedObject.FindProperty("panelCustomID");
			this.estadoInicialProperty = this.serializedObject.FindProperty("estadoInicial");
			this.estadoActualProperty = this.serializedObject.FindProperty("estadoActual");
			this.keyEscapeProperty = this.serializedObject.FindProperty("keyEscape");
			this.transicionProperty = this.serializedObject.FindProperty("transicion");
			this.tipoTransicionProperty = this.serializedObject.FindProperty("tipoTransicion");
			this.duracionTransicionProperty = this.serializedObject.FindProperty("duracionTransicion");
			this.onTransicionInitProperty = this.serializedObject.FindProperty("OnTransicionInit");
			this.onTransicionCompletadaProperty = this.serializedObject.FindProperty("OnTransicionCompletada");
		}
		#endregion

		#region GUI
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.Separator();
			this.DrawPropiedadesGenerales();
			EditorGUILayout.Separator();
			this.DrawPropiedadesTransicion();
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(this.onTransicionInitProperty, new GUIContent("On Transicion Inicia"), true);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(this.onTransicionCompletadaProperty, new GUIContent("On Transicion Completa"), true);

			serializedObject.ApplyModifiedProperties();
		}

		protected void DrawPropiedadesGenerales()
		{
			UIPanelID id = (UIPanelID)this.panelIDProperty.enumValueIndex;
			UIPanel.EstadoPanel est = (UIPanel.EstadoPanel)this.estadoActualProperty.enumValueIndex;

			EditorGUILayout.LabelField("Propiedades Generales", EditorStyles.boldLabel);
			EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);

			EditorGUILayout.PropertyField(this.panelIDProperty, new GUIContent("ID"));
			if (id == UIPanelID.Custom)
			{
				EditorGUILayout.PropertyField(this.panelCustomIDProperty, new GUIContent("Custom ID"));
			}
			EditorGUILayout.PropertyField(this.estadoInicialProperty, new GUIContent("Estado Inicial"));
			EditorGUILayout.PropertyField(this.keyEscapeProperty, new GUIContent("Key Escape"));

			EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
		}

		protected void DrawPropiedadesTransicion()
		{
			EditorGUILayout.LabelField("Propiedades Transicion", EditorStyles.boldLabel);
			EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);

			EditorGUILayout.PropertyField(this.transicionProperty, new GUIContent("Transicion"));

			UIPanel.TransicionPanel transition = (UIPanel.TransicionPanel)this.transicionProperty.enumValueIndex;

			if (transition == UIPanel.TransicionPanel.Transicion)
			{
				EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
				EditorGUILayout.PropertyField(this.tipoTransicionProperty, new GUIContent("Tipo"));
				EditorGUILayout.PropertyField(this.duracionTransicionProperty, new GUIContent("Duracion"));
				EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
			}

			EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
		}
		#endregion



	}
#endif
}
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
using UnityEditor;
#endregion

namespace MoonAntonio.MGUI
{
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

		UIPanel mpanel;
		#endregion

		#region Init
		public virtual void OnEnable()
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

			mpanel = (UIPanel)target;
		}
		#endregion

		#region GUI
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawPropiedadesHeader();

			GUILayout.Space(20);

			DrawPropiedadesGenerales();

			GUILayout.Space(20);

			DrawPropiedadesTransicion();

			GUILayout.Space(20);

			DrawEventos();

			serializedObject.ApplyModifiedProperties();
		}

		public void DrawPropiedadesHeader()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Space(5);
			GUILayout.BeginHorizontal();
			GUILayout.Space(10);

			EditorGUILayout.LabelField("Panel General de " + mpanel.ID, EditorStyles.boldLabel);

			GUILayout.Space(10);
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
			GUILayout.EndVertical();
			GUILayout.Space(20);

			EditorGUILayout.LabelField("Info Panel", EditorStyles.boldLabel);

			GUILayout.BeginHorizontal();
			EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
			GUILayout.Label("Estado Actual");
			if (mpanel.estadoActual == UIPanel.EstadoPanel.Mostrado)
			{
				GUILayout.BeginHorizontal();
				GUI.backgroundColor = Color.green;
				GUILayout.Button("Mostrado");
				GUI.backgroundColor = Color.red;
				GUILayout.Button("Ocultado");
				GUILayout.EndHorizontal();
			}
			else
			{
				GUILayout.BeginHorizontal();
				GUI.backgroundColor = Color.red;
				GUILayout.Button("Mostrado");
				GUI.backgroundColor = Color.green;
				GUILayout.Button("Ocultado");
				GUILayout.EndHorizontal();
			}
			GUI.backgroundColor = Color.white;
			EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
			GUILayout.EndHorizontal();
		}

		public void DrawPropiedadesGenerales()
		{
			UIPanelID id = (UIPanelID)this.panelIDProperty.enumValueIndex;
			EditorGUILayout.LabelField("Propiedades Generales", EditorStyles.boldLabel);

			GUILayout.BeginVertical();
			EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);

			EditorGUILayout.PropertyField(this.panelIDProperty, new GUIContent("ID"));
			if (id == UIPanelID.Custom)
			{
				EditorGUILayout.PropertyField(this.panelCustomIDProperty, new GUIContent("Custom ID"));
			}
			EditorGUILayout.PropertyField(this.estadoInicialProperty, new GUIContent("Estado Inicial"));
			EditorGUILayout.PropertyField(this.keyEscapeProperty, new GUIContent("Key Escape"));

			EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
			GUILayout.EndVertical();
		}

		public void DrawPropiedadesTransicion()
		{
			EditorGUILayout.LabelField("Propiedades Transicion", EditorStyles.boldLabel);

			GUILayout.BeginVertical();
			EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);

			EditorGUILayout.PropertyField(this.transicionProperty, new GUIContent("Transicion"));

			UIPanel.TransicionPanel transition = (UIPanel.TransicionPanel)this.transicionProperty.enumValueIndex;

			if (transition == UIPanel.TransicionPanel.Transicion)
			{
				EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
				EditorGUILayout.PropertyField(this.tipoTransicionProperty, new GUIContent("Tipo Transicion"));
				EditorGUILayout.PropertyField(this.duracionTransicionProperty, new GUIContent("Duracion"));
				EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
			}

			EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);
			GUILayout.EndVertical();
		}

		public void DrawEventos()
		{
			EditorGUILayout.LabelField("Eventos", EditorStyles.boldLabel);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(this.onTransicionInitProperty, new GUIContent("On Transicion Inicia"), true);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(this.onTransicionCompletadaProperty, new GUIContent("On Transicion Completa"), true);
		}
		#endregion
	}
}
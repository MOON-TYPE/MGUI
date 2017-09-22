//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIButtonExtendidoInspector.cs (22/09/2017)									\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Inspector de UIButtonExtendido								\\
// Fecha Mod:		22/09/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
#endregion

namespace MoonAntonio.MGUI.UI
{
	/// <summary>
	/// <para>Inspector de UIButtonExtendido</para>
	/// </summary>
	[CanEditMultipleObjects, CustomEditor(typeof(UIButtonExtendido), true)]
	public class UIButtonExtendidoInspector : ButtonEditor
	{
		#region GUI
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onCambiaEstado"), new GUIContent("On Cambia Estado"), true);
			serializedObject.ApplyModifiedProperties();
		}
		#endregion
	}
}
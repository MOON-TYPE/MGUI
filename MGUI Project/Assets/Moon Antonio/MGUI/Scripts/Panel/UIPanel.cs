//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIPanel.cs (11/10/2017)														\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:																	\\
// Fecha Mod:		11/10/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using MoonAntonio.Tweens;
#endregion

namespace MoonAntonio.MGUI
{
	[DisallowMultipleComponent, ExecuteInEditMode, AddComponentMenu("MGUI/Panel", 58), RequireComponent(typeof(CanvasGroup))]
	public class UIPanel : MonoBehaviour, IEventSystemHandler, ISelectHandler, IPointerDownHandler
	{
		#region Enums
		/// <summary>
		/// <para>Transicion del panel.</para>
		/// </summary>
		public enum Transicion
		{
			Instantanea,
			Transicion
		}

		/// <summary>
		/// <para>Estado visual del panel.</para>
		/// </summary>
		public enum Estado
		{
			Mostrado,
			Ocultado
		}

		/// <summary>
		/// <para>Tecla de escape del panel.</para>
		/// </summary>
		public enum KeyEscape
		{
			Ninguno,
			Ocultar,
			Toggle
		}
		#endregion
	}
}
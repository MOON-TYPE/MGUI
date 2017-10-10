//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIPanel.cs (11/10/2017)														\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Control del panel de la UI									\\
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
		#region Variables Privadas
		/// <summary>
		/// <para>Control del tween.</para>
		/// </summary>
		[NonSerialized] private readonly TweenRunner<FloatTween> tweenRun;					// Control del tween
		#endregion

		#region Constructor Interno
		/// <summary>
		/// <para>[Unity] Llamada por Unity antes de la deserializacion, no debe ser llamado.</para>
		/// </summary>
		protected UIPanel()
		{
			// Si no hay tween, inicializar uno.
			if (this.tweenRun == null) this.tweenRun = new TweenRunner<FloatTween>();

			// Iniciar el tween
			this.tweenRun.Init(this);
		}
		#endregion

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
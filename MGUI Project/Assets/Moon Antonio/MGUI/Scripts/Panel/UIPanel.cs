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
		[NonSerialized] private readonly TweenRunner<FloatTween> tweenRun;                  // Control del tween
		/// <summary>
		/// <para>Canvas group del panel.</para>
		/// </summary>
		private CanvasGroup canvasGroup;                                                    // Canvas group del panel
		/// <summary>
		/// <para>Estado inicial del panel.</para>
		/// </summary>
		private Estado estadoInicial = Estado.Ocultado;                                     // Estado inicial del panel
		/// <summary>
		/// <para>Estado actual del panel.</para>
		/// </summary>
		private Estado estadoActual = Estado.Ocultado;										// Estado actual del panel
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

		#region Inicializadores
		/// <summary>
		/// <para>Cargador de <see cref="UIPanel"/>.</para>
		/// </summary>
		protected virtual void Awake()// Cargador de UIPanel
		{
			// Obtener el canvas group
			this.canvasGroup = this.gameObject.GetComponent<CanvasGroup>();

			// Transicion al estado inicial
			if (Application.isPlaying) this.AplicarEstadoInicial(this.estadoInicial);
		}
		#endregion

		#region Metodos Internos
		/// <summary>
		/// <para>Aplica el estado inicial.</para>
		/// </summary>
		/// <param name="estado">El nuevo estado de transicion.</param>
		protected virtual void AplicarEstadoInicial(Estado estado)// Aplica el estado inicial
		{
			// Obtener el nuevo alpha
			float targetAlpha = (estado == Estado.Mostrado) ? 1f : 0f;

			// Fija el alpha del canvas group
			this.SetAlphaCanvas(targetAlpha);

			// Guardar el estado
			this.estadoActual = estado;

			// Si estamos realizando la transicion para mostrar, habilitar el blocksRaycasts
			if (estado == Estado.Mostrado)
			{
				this.canvasGroup.blocksRaycasts = true;
				this.canvasGroup.interactable = true;
			}
		}

		/// <summary>
		/// <para>Selecciona el alpha del canvas group.</para>
		/// </summary>
		/// <param name="alpha">Alpha.</param>
		protected void SetAlphaCanvas(float alpha)// Selecciona el alpha del canvas group
		{
			// Si no hay canvas group, salimos
			if (this.canvasGroup == null) return;

			// Fijar el alpha
			this.canvasGroup.alpha = alpha;

			// Si el alpha es 0, desabilitar el block raycasts
			if (alpha == 0f)
			{
				this.canvasGroup.blocksRaycasts = false;
				this.canvasGroup.interactable = false;
			}
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
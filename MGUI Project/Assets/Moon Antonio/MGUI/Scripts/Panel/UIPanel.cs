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
	/// <summary>
	/// <para>Control del panel de la UI</para>
	/// </summary>
	[DisallowMultipleComponent, ExecuteInEditMode, AddComponentMenu("MGUI/Panel", 58), RequireComponent(typeof(CanvasGroup))]
	public class UIPanel : MonoBehaviour, IEventSystemHandler
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
		private Estado estadoActual = Estado.Ocultado;                                      // Estado actual del panel
		/// <summary>
		/// <para>Id del panel.</para>
		/// </summary>
		[SerializeField] private UIPanelID panelID = UIPanelID.Ninguno;						// Id del panel
		/// <summary>
		/// <para>Custom id del panel.</para>
		/// </summary>
		[SerializeField] private int panelCustomID = 0;                                     // Custom id del panel
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Obtiene o establece el id del panel.</para>
		/// </summary>
		public UIPanelID ID
		{
			get { return this.panelID; }
			set { this.panelID = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece el id custom del panel.</para>
		/// </summary>
		public int CustomID
		{
			get { return this.panelCustomID; }
			set { this.panelCustomID = value; }
		}
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

		/// <summary>
		/// <para>Inicializador de <see cref="UIPanel"/>.</para>
		/// </summary>
		protected virtual void Start()// Inicializador de UIPanel
		{
			// Asignar nuevo custom id
			if (this.CustomID == 0) this.CustomID = UIPanel.NextCustomIDLibre;
		}
		#endregion

		#region API
		#region Propiedades
		/// <summary>
		/// <para>Obtiene el siguiente ID custom no utilizado para un panel.</para>
		/// </summary>
		public static int NextCustomIDLibre
		{
			get
			{
				// Obtiene los paneles
				List<UIPanel> paneles = UIPanel.GetPaneles();

				if (GetPaneles().Count > 0)
				{
					// Ordenar los paneles por id
					paneles.Sort(UIPanel.SortIDPanel);

					// Devolver el id del ultimo panel mas uno
					return paneles[paneles.Count - 1].CustomID + 1;
				}

				// No paneles, return 0
				return 0;
			}
		}
		#endregion

		#region Metodos Estaticos
		/// <summary>
		/// <para>Obtener todos los paneles de la escena (Incluyendo inactivos).</para>
		/// </summary>
		public static List<UIPanel> GetPaneles()// Obtener todos los paneles de la escena (Incluyendo inactivos)
		{
			// Paneles de la escena
			List<UIPanel> paneles = new List<UIPanel>();

			UIPanel[] ps = Resources.FindObjectsOfTypeAll<UIPanel>();

			foreach (UIPanel p in ps)
			{
				// Compruebe si el panel esta activo en la jerarquia
				if (p.gameObject.activeInHierarchy) paneles.Add(p);
			}

			return paneles;
		}
		#endregion

		#region Funcionalidad
		/// <summary>
		/// <para>Ordena por id</para>
		/// </summary>
		/// <param name="panel">Un panel.</param>
		/// <param name="panelSiguiente">Otro panel.</param>
		/// <returns></returns>
		public static int SortIDPanel(UIPanel panel, UIPanel panelSiguiente)// Ordena por id
		{
			return panel.CustomID.CompareTo(panelSiguiente.CustomID);
		}
		#endregion
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
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
		/// <para>Determina si el panel tiene el foco.</para>
		/// </summary>
		protected bool isFocus = false;														// Determina si el panel tiene el foco
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
		private EstadoPanel estadoInicial = EstadoPanel.Ocultado;                           // Estado inicial del panel
		/// <summary>
		/// <para>Estado actual del panel.</para>
		/// </summary>
		private EstadoPanel estadoActual = EstadoPanel.Ocultado;                            // Estado actual del panel
		/// <summary>
		/// <para>Tecla de escape.</para>
		/// </summary>
		[SerializeField] private KeyEscapePanel keyEscape = KeyEscapePanel.Ocultar;			// Tecla de escape
		/// <summary>
		/// <para>Id del panel.</para>
		/// </summary>
		[SerializeField] private UIPanelID panelID = UIPanelID.Ninguno;						// Id del panel
		/// <summary>
		/// <para>Custom id del panel.</para>
		/// </summary>
		[SerializeField] private int panelCustomID = 0;                                     // Custom id del panel
		/// <summary>
		/// <para>Transicion del panel.</para>
		/// </summary>
		[SerializeField] private TransicionPanel transicion = TransicionPanel.Instantanea;	// Transicion del panel
		/// <summary>
		/// <para>Tipo de transicion.</para>
		/// </summary>
		[SerializeField] private TweenEasing tipoTransicion = TweenEasing.InOutQuint;		// Tipo de transicion
		/// <summary>
		/// <para>Duracion de la transicion del panel.</para>
		/// </summary>
		[SerializeField] private float duracionTransicion = 0.1f;							// Duracion de la transicion del panel
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

		/// <summary>
		/// <para>Obtiene o establece la tecla de escape.</para>
		/// </summary>
		public KeyEscapePanel KeyEscape
		{
			get { return this.keyEscape; }
			set { this.keyEscape = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece la transicion.</para>
		/// </summary>
		public TransicionPanel Transicion
		{
			get { return this.transicion; }
			set { this.transicion = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece el tipo de transicion del panel.</para>
		/// </summary>
		public TweenEasing TipoTransicion
		{
			get { return this.tipoTransicion; }
			set { this.tipoTransicion = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece la duracion de la transicion.</para>
		/// </summary>
		public float DuracionTransicion
		{
			get { return this.duracionTransicion; }
			set { this.duracionTransicion = value; }
		}

		/// <summary>
		/// <para>Obtiene un valor que indica si este panel es visible.</para>
		/// </summary>
		public bool IsVisible
		{
			get { return (this.canvasGroup != null && this.canvasGroup.alpha > 0f) ? true : false; }
		}

		/// <summary>
		/// <para>Obtiene un valor que indica si este panel se esta mostrando.</para>
		/// </summary>
		public bool IsOpen
		{
			get { return (this.estadoActual == EstadoPanel.Mostrado); }
		}

		/// <summary>
		/// <para>Obtiene un valor que indica si esta instancia esta enfocada.</para>
		/// </summary>
		public bool IsFocus
		{
			get { return this.isFocus; }
		}
		#endregion

		#region Clases Eventos
		[Serializable] public class TransicionInitEvent : UnityEvent<UIPanel, EstadoPanel, bool> { }
		[Serializable] public class TransicionCompletadoEvent : UnityEvent<UIPanel, EstadoPanel> { }
		#endregion

		#region Eventos
		/// <summary>
		/// <para>Cuando la transicion se inicial.</para>
		/// </summary>
		public TransicionInitEvent OnTransicionInit = new TransicionInitEvent();
		/// <summary>
		/// <para>Cuando la transicion se termina.</para>
		/// </summary>
		public TransicionCompletadoEvent OnTransicionCompletada = new TransicionCompletadoEvent();
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

#if UNITY_EDITOR
		/// <summary>
		/// <para>Cuando termina de carga el script o se cambia un valor en el inspector.</para>
		/// </summary>
		protected virtual void OnValidate()// Cuando termina de carga el script o se cambia un valor en el inspector
		{
			this.duracionTransicion = Mathf.Max(this.duracionTransicion, 0f);

			// Asegurarse de tener un manager
			if (this.keyEscape != KeyEscapePanel.Ninguno)
			{
				UIPanelManager manager = Component.FindObjectOfType<UIPanelManager>();

				// Instanciar el manager si no esta
				if (manager == null)
				{
					GameObject newObj = new GameObject("MGUI Manager");
					newObj.AddComponent<UIPanelManager>();
					newObj.transform.SetAsFirstSibling();
				}
			}

			// Aplicar estado inicial
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = (this.estadoInicial == EstadoPanel.Ocultado) ? 0f : 1f;
			}
		}
#endif
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

		#region Metodos
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

		/// <summary>
		/// <para>Muestra el panel.</para>
		/// </summary>
		public virtual void Mostrar()// Muestra el panel
		{
			this.Mostrar(false);
		}

		/// <summary>
		/// <para>Muestra el panel.</para>
		/// </summary>
		/// <param name="instantaneo">True si se quiere instantaneo..</param>
		public virtual void Mostrar(bool instantaneo)// Muestra el panel
		{
			// TODO : Comprobar si no esta activo
			//if (!this.IsActive()) return;

			// Focus al panel
			//this.Focus();

			// Comprueba si el panel esta mostrado
			if (this.estadoActual == EstadoPanel.Mostrado) return;

			// Transicion
			this.TransicionNuevoEstado(EstadoPanel.Mostrado, instantaneo);
		}

		/// <summary>
		/// <para>Oculta el panel.</para>
		/// </summary>
		public virtual void Ocultar()// Oculta el panel
		{
			this.Ocultar(false);
		}

		/// <summary>
		/// <para>Oculta el panel</para>
		/// </summary>
		/// <param name="instantaneo">True si se quiere instantaneo.</param>
		public virtual void Ocultar(bool instantaneo)// Oculta el panel
		{
			// TODO : Comprobar si no esta activo
			//if (!this.IsActive()) return;

			// Comprueba si el panel esta oculto
			if (this.estadoActual == EstadoPanel.Ocultado) return;

			// Transicion
			this.TransicionNuevoEstado(EstadoPanel.Ocultado, instantaneo);
		}

		/// <summary>
		/// <para>Inicia el tween del alpha.</para>
		/// </summary>
		/// <param name="value">Target alpha.</param>
		/// <param name="duracion">Duracion.</param>
		/// <param name="ignorarTimeScale">Si se quiere ignorar el timeSa.</param>
		public void InitAlphaTween(float value, float duracion, bool ignorarTimeScale)// Inicia el tween del alpha
		{
			// Si no tiene canvasgroup, volver
			if (this.canvasGroup == null) return;

			// Setup
			var floatTween = new FloatTween { Duracion = duracion, FloatInicial = this.canvasGroup.alpha, FloatFinal = value };
			floatTween.AddOnCambioCallback(SetAlphaCanvas);
			floatTween.AddOnFinalizadoCallback(OnTweenCompletado);
			floatTween.IgnorarTimeScale = ignorarTimeScale;
			floatTween.Easing = this.TipoTransicion;

			// Iniciar tween
			this.tweenRun.StartTween(floatTween);
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
		protected virtual void AplicarEstadoInicial(EstadoPanel estado)// Aplica el estado inicial
		{
			// Obtener el nuevo alpha
			float targetAlpha = (estado == EstadoPanel.Mostrado) ? 1f : 0f;

			// Fija el alpha del canvas group
			this.SetAlphaCanvas(targetAlpha);

			// Guardar el estado
			this.estadoActual = estado;

			// Si estamos realizando la transicion para mostrar, habilitar el blocksRaycasts
			if (estado == EstadoPanel.Mostrado)
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

		/// <summary>
		/// <para>Cambia el estado con una transicion.</para>
		/// </summary>
		/// <param name="estado">El nuevo estado.</param>
		/// <param name="instantaneo">Si se quiere instantaneo.</param>
		protected virtual void TransicionNuevoEstado(EstadoPanel estado, bool instantaneo)// Cambia el estado con una transicion
		{
			// Obtiene el alpha
			float targetAlpha = (estado == EstadoPanel.Mostrado) ? 1f : 0f;

			// Llamamos al evento de iniciar transicion
			if (this.OnTransicionInit != null) this.OnTransicionInit.Invoke(this, estado, (instantaneo || this.Transicion == TransicionPanel.Instantanea));

			// Procesamos la transicion
			if (this.Transicion == TransicionPanel.Transicion)
			{
				float duration = (instantaneo) ? 0f : this.DuracionTransicion;

				// Tween del alpha
				this.InitAlphaTween(targetAlpha, duration, true);
			}
			else
			{
				// Fijamos el alpha
				this.SetAlphaCanvas(targetAlpha);

				// Finalizamos el evento
				if (this.OnTransicionCompletada != null) this.OnTransicionCompletada.Invoke(this, estado);
			}

			// Guardamos el estado
			this.estadoActual = estado;

			// Si estamos realizando la transicion para mostrar, habilitar el blocksRaycasts
			if (estado == EstadoPanel.Mostrado)
			{
				this.canvasGroup.blocksRaycasts = true;
				this.canvasGroup.interactable = true;
			}
		}

		/// <summary>
		/// <para>Cuando el tween se completa.</para>
		/// </summary>
		protected virtual void OnTweenCompletado()// Cuando el tween se completa
		{
			if (this.OnTransicionCompletada != null) this.OnTransicionCompletada.Invoke(this, this.estadoActual);
		}
		#endregion

		#region Enums
		/// <summary>
		/// <para>Transicion del panel.</para>
		/// </summary>
		public enum TransicionPanel
		{
			Instantanea,
			Transicion
		}

		/// <summary>
		/// <para>Estado visual del panel.</para>
		/// </summary>
		public enum EstadoPanel
		{
			Mostrado,
			Ocultado
		}

		/// <summary>
		/// <para>Tecla de escape del panel.</para>
		/// </summary>
		public enum KeyEscapePanel
		{
			Ninguno,
			Ocultar,
			Toggle
		}
		#endregion
	}
}
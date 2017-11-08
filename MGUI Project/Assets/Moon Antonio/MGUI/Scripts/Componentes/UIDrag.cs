//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIDrag.cs (11/10/2017)														\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Control para arrastras un objeto							\\
// Fecha Mod:		11/10/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion

namespace MoonAntonio.MGUI
{
	/// <summary>
	/// <para>Control para arrastras un objeto</para>
	/// </summary>
	[AddComponentMenu("MGUI/UIDrag", 82)]
	public class UIDrag : UIBehaviour
	{
		#region Variables Publicas
		/// <summary>
		/// <para>Objetivo del arrastre.</para>
		/// </summary>
		public RectTransform target;                                                // Objetivo del arrastre
		/// <summary>
		/// <para>Se puede mover en horizontal.</para>
		/// </summary>
		public bool horizontal = true;												// Se puede mover en horizontal
		/// <summary>
		/// <para>Se puede mover en vertical.</para>
		/// </summary>
		public bool vertical = true;												// Se puede mover en vertical
		/// <summary>
		/// <para>Tiene incercia.</para>
		/// </summary>
		public bool inercia = true;													// Tiene incercia
		/// <summary>
		/// <para>Determina si se quiere constrain dentro del canvas.</para>
		/// </summary>
		public bool isConstrainConCanvas = false;									// Determina si se quiere constrain dentro del canvas
		/// <summary>
		/// <para>Determina si que quiere constrain cuando se arrastra.</para>
		/// </summary>
		public bool isConstrainDrag = true;											// Determina si que quiere constrain cuando se arrastra
		/// <summary>
		/// <para>Determina si se quiere constrain con la inercia.</para>
		/// </summary>
		public bool isConstrainInercia = true;										// Determina si se quiere constrain con la inercia
		#endregion

		#region Variables Privadas
		/// <summary>
		/// <para>Canvas de la escena.</para>
		/// </summary>
		private Canvas canvas;														// Canvas de la escena
		/// <summary>
		/// <para>Canvas del Rect Transform.</para>
		/// </summary>
		private RectTransform canvasRectTransform;                                  // Canvas del Rect Transform
		/// <summary>
		/// <para>Velocidad actual del objeto.</para>
		/// </summary>
		private Vector2 velocidad;                                                  // Velocidad actual del objeto
		/// <summary>
		/// <para>Determina si se esta arrastrando.</para>
		/// </summary>
		private bool isDragging;                                                    // Determina si se esta arrastrando
		/// <summary>
		/// <para>Posicion inicial</para>
		/// </summary>
		private Vector2 posicionPuntoInicial = Vector2.zero;						// Posicion inicial
		/// <summary>
		/// <para>Posicion del objetivo.</para>
		/// </summary>
		private Vector2 posicionPuntoTarget = Vector2.zero;							// Posicion del objetivo
		#endregion

		#region Clases Eventos
		[Serializable] public class DragInitEvent : UnityEvent<BaseEventData> { }
		[Serializable] public class DragCompletadoEvent : UnityEvent<BaseEventData> { }
		[Serializable] public class DragEvent : UnityEvent<BaseEventData> { }
		#endregion

		#region Eventos
		/// <summary>
		/// <para>Cuando el arrastre se inicia.</para>
		/// </summary>
		public DragInitEvent onDragInit = new DragInitEvent();
		/// <summary>
		/// The on end drag event.
		/// </summary>
		public DragCompletadoEvent onDragCompletada = new DragCompletadoEvent();
		/// <summary>
		/// The on drag event.
		/// </summary>
		public DragEvent onDrag = new DragEvent();
		#endregion

		#region Inicializadores
		/// <summary>
		/// <para>Cargador de <see cref="UIDrag"/>.</para>
		/// </summary>
		protected override void Awake()// Cargador de UIDrag
		{
			// Base
			base.Awake();

			// Obtener el canvas y el rect transform
			this.canvas = UIUtil.FindInParents<Canvas>((this.target != null) ? this.target.gameObject : this.gameObject);
			if (this.canvas != null) this.canvasRectTransform = this.canvas.transform as RectTransform;
		}
		#endregion

		#region API
		#region Metodos
		/// <summary>
		/// <para>Detiene la inercia.</para>
		/// </summary>
		public void StopMovimiento()// Detiene la inercia
		{
			this.velocidad = Vector2.zero;
		}

		/// <summary>
		/// <para>Cuando empieza el arrastre.</para>
		/// </summary>
		/// <param name="data">Data.</param>
		public void OnBeginDrag(PointerEventData data)// Cuando empieza el arrastre
		{
			if (!this.IsActive()) return;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasRectTransform, data.position, data.pressEventCamera, out this.posicionPuntoInicial);
			this.posicionPuntoTarget = this.target.anchoredPosition;
			this.velocidad = Vector2.zero;
			this.isDragging = true;

			// Invoca el evento
			if (this.onDragInit != null) this.onDragInit.Invoke(data as BaseEventData);
		}

		/// <summary>
		/// <para>Cuando termina el arrastre.</para>
		/// </summary>
		/// <param name="data">Data.</param>
		public void OnEndDrag(PointerEventData data)// Cuando termina el arrastre
		{
			this.isDragging = false;

			if (!this.IsActive()) return;

			// Invoca el evento
			if (this.onDragCompletada != null) this.onDragCompletada.Invoke(data as BaseEventData);
		}

		/// <summary>
		/// <para>Cuando se esta arrastrando.</para>
		/// </summary>
		/// <param name="data">Data.</param>
		public void OnDrag(PointerEventData data)// Cuando se esta arrastrando
		{
			if (!this.IsActive() || this.canvas == null) return;

			Vector2 mousePos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasRectTransform, data.position, data.pressEventCamera, out mousePos);

			if (this.isConstrainConCanvas && this.isConstrainDrag)
			{
				mousePos = this.ClampToCanvas(mousePos);
			}

			Vector2 newPosition = this.posicionPuntoTarget + (mousePos - this.posicionPuntoInicial);

			// Restringir movimiento
			if (!this.horizontal)
			{
				newPosition.x = this.target.anchoredPosition.x;
			}
			if (!this.vertical)
			{
				newPosition.y = this.target.anchoredPosition.y;
			}

			// Aplicar la posicion
			this.target.anchoredPosition = newPosition;

			// Invocar el evento
			if (this.onDrag != null) this.onDrag.Invoke(data as BaseEventData);
		}
		#endregion

		#region Funcionalidad
		/// <summary>
		/// <para>Determina si el objeto esta activo.</para>
		/// </summary>
		/// <returns></returns>
		public override bool IsActive()// Determina si el objeto esta activo
		{
			return base.IsActive() && this.target != null;
		}
		#endregion
		#endregion

		#region Metodos Internos
		/// <summary>
		/// <para>Cuando se cambia el transform del padre.</para>
		/// </summary>
		protected override void OnTransformParentChanged()// Cuando se cambia el transform del padre
		{
			// Base
			base.OnTransformParentChanged();

			// Obtener el canvas y el rect transform
			this.canvas = UIUtil.FindInParents<Canvas>((this.target != null) ? this.target.gameObject : this.gameObject);
			if (this.canvas != null) this.canvasRectTransform = this.canvas.transform as RectTransform;
		}

		/// <summary>
		/// <para>Clamps al canvas.</para>
		/// </summary>
		/// <returns>Canvas.</returns>
		/// <param name="Posicion">Posicion.</param>
		protected Vector2 ClampToCanvas(Vector2 Posicion)// Clamps al canvas
		{
			if (this.canvasRectTransform != null)
			{
				Vector3[] esquinas = new Vector3[4];
				this.canvasRectTransform.GetLocalCorners(esquinas);

				float clampedX = Mathf.Clamp(Posicion.x, esquinas[0].x, esquinas[2].x);
				float clampedY = Mathf.Clamp(Posicion.y, esquinas[3].y, esquinas[1].y);

				return new Vector2(clampedX, clampedY);
			}

			// Default
			return Posicion;
		}
		#endregion
	}
}
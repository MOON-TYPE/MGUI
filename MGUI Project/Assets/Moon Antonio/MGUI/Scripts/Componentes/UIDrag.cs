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
		public bool inercia = true;                                                 // Tiene incercia
		/// <summary>
		/// <para>Ratio de damp</para>
		/// </summary>
		private float dampeningRate = 9f;											// Ratio de damp
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
		private Vector2 posicionPuntoTarget = Vector2.zero;                         // Posicion del objetivo
		/// <summary>
		/// <para>Ultima posicion registrada.</para>
		/// </summary>
		private Vector2 ultimaPos = Vector2.zero;									// Ultima posicion registrada
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

		#region Actualizadores
		/// <summary>
		/// <para>Actualizacion final de <see cref="UIDrag"/>.</para>
		/// </summary>
		protected virtual void LateUpdate()// Actualizacion final de UIDrag
		{
			if (!this.target) return;

			// Captura la velocidad de nuestro arrastre para ser utilizado por la inercia
			if (this.isDragging && this.inercia)
			{
				Vector3 to = (this.target.anchoredPosition - this.ultimaPos) / Time.unscaledDeltaTime;
				this.velocidad = Vector3.Lerp(this.velocidad, to, Time.unscaledDeltaTime * 10f);
			}

			this.ultimaPos = this.target.anchoredPosition;

			// Handle de inercia solo cuando no se arrastra
			if (!this.isDragging && this.velocidad != Vector2.zero)
			{
				Vector2 anchoredPosition = this.target.anchoredPosition;

				// Dampen de inercia
				this.Dampen(ref this.velocidad, this.dampeningRate, Time.unscaledDeltaTime);

				for (int i = 0; i < 2; i++)
				{
					// Calcular la inercia
					if (this.inercia)
					{
						anchoredPosition[i] += this.velocidad[i] * Time.unscaledDeltaTime;
					}
					else
					{
						this.velocidad[i] = 0f;
					}
				}

				if (this.velocidad != Vector2.zero)
				{
					// Restringir movimiento
					if (!this.horizontal)
					{
						anchoredPosition.x = this.target.anchoredPosition.x;
					}
					if (!this.vertical)
					{
						anchoredPosition.y = this.target.anchoredPosition.y;
					}

					// Si el objetivo esta restringido dentro de su canvas
					if (this.isConstrainConCanvas && this.isConstrainInercia && this.canvasRectTransform != null)
					{
						Vector3[] canvasCorners = new Vector3[4];
						this.canvasRectTransform.GetWorldCorners(canvasCorners);

						Vector3[] targetCorners = new Vector3[4];
						this.target.GetWorldCorners(targetCorners);

						// Fuera de la pantalla hacia la izquierda o hacia la derecha
						if (targetCorners[0].x < canvasCorners[0].x || targetCorners[2].x > canvasCorners[2].x)
						{
							anchoredPosition.x = this.target.anchoredPosition.x;
						}

						// Fuera de la pantalla hacia arriba o hacia abajo
						if (targetCorners[3].y < canvasCorners[3].y || targetCorners[1].y > canvasCorners[1].y)
						{
							anchoredPosition.y = this.target.anchoredPosition.y;
						}
					}

					// Aplicar inercia
					if (anchoredPosition != this.target.anchoredPosition)
					{
						this.target.anchoredPosition = anchoredPosition;
					}
				}
			}
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
		/// <para>Dampen.</para>
		/// </summary>
		/// <param name="velocidad">Velocidad.</param>
		/// <param name="fuerza">Fuerza.</param>
		/// <param name="delta">Delta.</param>
		protected Vector3 Dampen(ref Vector2 velocidad, float fuerza, float delta)// Dampen
		{
			if (delta > 1f)
			{
				delta = 1f;
			}

			float dampeningFactor = 1f - fuerza * 0.001f;
			int ms = Mathf.RoundToInt(delta * 1000f);
			float totalDampening = Mathf.Pow(dampeningFactor, ms);
			Vector2 vTotal = velocidad * ((totalDampening - 1f) / Mathf.Log(dampeningFactor));

			velocidad = velocidad * totalDampening;

			return vTotal * 0.06f;
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

		/// <summary>
		/// <para>Clamps a la pantalla.</para>
		/// </summary>
		/// <returns>Pantalla.</returns>
		/// <param name="posicion">Posicion.</param>
		protected Vector2 ClampToScreen(Vector2 posicion)// Clamps a la pantalla
		{
			if (this.canvas != null)
			{
				if (this.canvas.renderMode == RenderMode.ScreenSpaceOverlay || this.canvas.renderMode == RenderMode.ScreenSpaceCamera)
				{
					float clampedX = Mathf.Clamp(posicion.x, 0f, Screen.width);
					float clampedY = Mathf.Clamp(posicion.y, 0f, Screen.height);

					return new Vector2(clampedX, clampedY);
				}
			}

			// Default
			return posicion;
		}
		#endregion
	}
}
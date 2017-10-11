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
		public RectTransform target;												// Objetivo del arrastre
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
		private Vector2 velocidad;													// Velocidad actual del objeto
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
		#endregion
	}
}
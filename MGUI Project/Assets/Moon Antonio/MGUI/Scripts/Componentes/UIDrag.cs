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
		private RectTransform target;												// Objetivo del arrastre
		#endregion

		#region Variables Privadas
		/// <summary>
		/// <para>Canvas de la escena.</para>
		/// </summary>
		private Canvas canvas;														// Canvas de la escena
		/// <summary>
		/// <para>Canvas del Rect Transform.</para>
		/// </summary>
		private RectTransform canvasRectTransform;									// Canvas del Rect Transform
		#endregion

		#region Clases Eventos
		[Serializable] public class BeginDragEvent : UnityEvent<BaseEventData> { }
		[Serializable] public class EndDragEvent : UnityEvent<BaseEventData> { }
		[Serializable] public class DragEvent : UnityEvent<BaseEventData> { }
		#endregion

		#region Inicializadores
		/// <summary>
		/// <para>Cargador de <see cref="UIDrag"/>.</para>
		/// </summary>
		protected override void Awake()// Cargador de UIDrag
		{
			base.Awake();

			// Obtener el canvas y el rect transform
			this.canvas = UIUtil.FindInParents<Canvas>((this.target != null) ? this.target.gameObject : this.gameObject);
			if (this.canvas != null) this.canvasRectTransform = this.canvas.transform as RectTransform;
		}
		#endregion
	}
}
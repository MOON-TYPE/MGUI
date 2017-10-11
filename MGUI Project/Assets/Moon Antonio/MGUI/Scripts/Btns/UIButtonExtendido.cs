//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIButtonExtendido.cs (22/09/2017)											\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Extension del boton de UGUI									\\
// Fecha Mod:		22/09/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
#endregion

namespace MoonAntonio.MGUI.UI
{
	/// <summary>
	/// <para>Extension del boton de UGUI.</para>
	/// </summary>
	[AddComponentMenu("Moon Antonio/MGUI/UI/UIButtonExtendido")]
	public class UIButtonExtendido : Button
	{
		#region Variables Publicas
		/// <summary>
		/// <para>El evento de cambio de estado.</para>
		/// </summary>
		public StateChangeEvent onCambiaEstado = new StateChangeEvent();         // El evento de cambio de estado
		
		#endregion

		#region Eventos
		[Serializable] public class StateChangeEvent : UnityEvent<Estados, bool> { }
		#endregion

		#region Metodos Publicos
		/// <summary>
		/// <para>Transicion del estado Seleccionable al estado seleccionado.</para>
		/// </summary>
		/// <param name="estado">Nuevo estado</param>
		/// <param name="instantaneo">Si es <c>true</c>, sera instantaneo.</param>
		protected override void DoStateTransition(Selectable.SelectionState estado, bool instantaneo)
		{
			base.DoStateTransition(estado, instantaneo);

			// Cambio de estado
			if (this.onCambiaEstado != null) this.onCambiaEstado.Invoke((Estados)estado, instantaneo);
		}

		/// <summary>
		/// <para>Esta funcion se llama cuando el objeto se deshabilita o esta inactivo.</para>
		/// </summary>
		protected override void OnDisable()// Esta funcion se llama cuando el objeto se deshabilita o esta inactivo
		{
			base.OnDisable();

			// Cambio de estado
			if (this.onCambiaEstado != null) this.onCambiaEstado.Invoke(Estados.Desabilitado, true);
		}
		#endregion

		#region Enum
		/// <summary>
		/// <para>Estados del boton.</para>
		/// </summary>
		public enum Estados
		{
			Normal,
			Resaltado,
			Presionado,
			Desabilitado
		}
		#endregion
	}
}
//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIAlwaysOnTop.cs (11/10/2017)												\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Ayudante para manejar el movimiento hacia delante del panel	\\
// Fecha Mod:		11/10/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using System;
#endregion

namespace MoonAntonio.MGUI
{
	/// <summary>
	/// <para>Se usa cuando se utiliza la UIUtil para llevar los objetos hacia delante.</para>
	/// <para>Cuando un objeto se esta adelantando, cualquier objeto con este componente se mantendra en la parte superior en el orden especificado.</para>
	/// </summary>
	[DisallowMultipleComponent, AddComponentMenu("MGUI/Always On Top")]
	public class UIAlwaysOnTop : MonoBehaviour, IComparable
	{
		#region Constantes
		public const int ModalBoxNivel = 99996;
		public const int SelectFieldBlockerNivel = 99997;
		public const int SelectFieldNivel = 99998;
		public const int TooltipNivel = 99999;
		#endregion

		#region Variables Privadas
		/// <summary>
		/// <para>Nivel</para>
		/// </summary>
		[SerializeField] private int nivel = 0;				// Nivel
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Obtiene o fija un nivel.</para>
		/// </summary>
		public int Nivel
		{
			get { return this.nivel; }
			set { this.nivel = value; }
		}
		#endregion

		#region Funcionalidad
		/// <summary>
		/// <para>Compara un objeto</para>
		/// </summary>
		/// <param name="obj">Objeto</param>
		/// <returns></returns>
		public int CompareTo(object obj)// Compara un objeto
		{
			if (obj != null)
			{
				UIAlwaysOnTop comp = obj as UIAlwaysOnTop;

				if (comp != null)
				{
					return this.nivel.CompareTo(comp.nivel);
				}
			}

			return 1;
		}
		#endregion
	}
}
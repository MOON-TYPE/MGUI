//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIUtil.cs (11/10/2017)														\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Clase de apoyo para extensiones globales					\\
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
	/// <para>Clase de apoyo para extensiones globales</para>
	/// </summary>
	public static class UIUtil
	{
		/// <summary>
		/// <para>Mueve un gameobject hacia el frente.</para>
		/// </summary>
		/// <param name="go">Game Object.</param>
		public static void HaciaElFrente(GameObject go)// Mueve un gameobject hacia el frente
		{
			Canvas canvas = UIUtil.FindInParents<Canvas>(go);

			// Si el objeto tiene un canvas padre
			if (canvas != null)
				go.transform.SetParent(canvas.transform, true);

			// Agregarlo el ultimo
			go.transform.SetAsLastSibling();

			// Controlar el componente
			if (canvas != null)
			{
				UIAlwaysOnTop[] alwaysOnTopComponenets = canvas.gameObject.GetComponentsInChildren<UIAlwaysOnTop>();

				if (alwaysOnTopComponenets.Length > 0)
				{
					// Ordenar por orden
					Array.Sort(alwaysOnTopComponenets);

					foreach (UIAlwaysOnTop component in alwaysOnTopComponenets)
					{
						component.transform.SetAsLastSibling();
					}
				}
			}
		}

		/// <summary>
		/// <para>Encuentra el componente en los padres del GameObjects.</para>
		/// </summary>
		public static T FindInParents<T>(GameObject go) where T : Component // Encuentra el componente en los padres del GameObjects
		{
			if (go == null) return null;

			var comp = go.GetComponent<T>();

			if (comp != null) return comp;

			Transform t = go.transform.parent;

			while (t != null && comp == null)
			{
				comp = t.gameObject.GetComponent<T>();
				t = t.parent;
			}

			return comp;
		}
	}
}
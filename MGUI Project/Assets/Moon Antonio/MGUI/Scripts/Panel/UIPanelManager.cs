//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// UIPanelManager.cs (11/10/2017)												\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Manager general del MGUI									\\
// Fecha Mod:		11/10/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using System.Collections.Generic;
#endregion

namespace MoonAntonio.MGUI
{
	/// <summary>
	/// <para>Manager general del MGUI</para>
	/// </summary>
	public class UIPanelManager : MonoBehaviour 
	{
		#region Actualizadores
		/// <summary>
		/// <para>Actualizador de <see cref="UIPanelManager"/>.</para>
		/// </summary>
		protected virtual void Update()// Actualizador de UIPanelManager
		{
			// Comprobar si se ha pulsado la tecla de escape
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				bool isMostrarOtro = true;

				// Obtener la lista de paneles
				List<UIPanel> paneles = UIPanel.GetPaneles();

				// Recorrer lista de paneles
				foreach (UIPanel panel in paneles)
				{
					// Comprobar si tiene tecla de escape
					if (panel.KeyEscape != UIPanel.KeyEscapePanel.Ninguno)
					{
						// Compruebe si el panel debe estar oculto
						if (panel.IsOpen && (panel.KeyEscape == UIPanel.KeyEscapePanel.Ocultar || panel.KeyEscape == UIPanel.KeyEscapePanel.Toggle && panel.IsFocus))
						{
							// Oculta el panel
							panel.Ocultar();

							// No permitir que se muestre un panel despues de cerrar este panel
							isMostrarOtro = false;
						}
					}
				}

				// Comprobar si hay que mostrar otro panel
				if (isMostrarOtro)
				{
					// Recorrer lista de paneles
					foreach (UIPanel panel in paneles)
					{
						// Compruebe si el panel tiene activado la accion de la tecla de escape
						if (!panel.IsOpen && panel.KeyEscape == UIPanel.KeyEscapePanel.Toggle)
						{
							// Muestra el panel
							panel.Mostrar();
						}
					}
				}
			}
		}
		#endregion
	}
}
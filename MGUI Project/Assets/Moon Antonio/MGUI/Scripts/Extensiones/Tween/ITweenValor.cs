//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// ITweenValue.cs (22/09/2017)													\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Interfaz del valor del Tween								\\
// Fecha Mod:		22/09/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

namespace MoonAntonio.Tweens
{
	#region Interfaz
	internal interface ITweenValor
	{
		#region Propiedades
		/// <summary>
		/// <para>Ignorar el tiempo.</para>
		/// </summary>
		bool IgnorarTimeScale { get; }
		/// <summary>
		/// <para>Duracion de transicion.</para>
		/// </summary>
		float Duracion { get; }
		/// <summary>
		/// <para>Tipo de transicion.</para>
		/// </summary>
		TweenEasing Easing { get; }
		#endregion

		#region Metodos
		/// <summary>
		/// <para>Obtiene el valor del tween</para>
		/// </summary>
		/// <param name="valor"></param>
		void TweenValor(float valor);
		/// <summary>
		/// <para>Determina la finalizacion del tween.</para>
		/// </summary>
		void Finalizado();
		#endregion

		#region Funcionalidad
		/// <summary>
		/// <para>Determina si el objetivo es valido.</para>
		/// </summary>
		/// <returns></returns>
		bool TargetValido();
		#endregion
	}
	#endregion
}
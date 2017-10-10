//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// TweenRunner.cs (22/09/2017)													\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Tween runner ejecuta la interpolacion dada					\\
// Fecha Mod:		22/09/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using System.Collections;
#endregion

namespace MoonAntonio.Tweens
{
	/// <summary>
	/// <para>Tween runner ejecuta la interpolacion dada</para>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class TweenRunner<T> where T : struct, ITweenValor
	{
		#region Variables Publicas
		/// <summary>
		/// <para>Corrutina</para>
		/// </summary>
		protected MonoBehaviour corrutina;								// Corrutina
		/// <summary>
		/// <para>Tween</para>
		/// </summary>
		protected IEnumerator tween;									// Tween
		#endregion

		#region API
		/// <summary>
		/// <para>Realiza el tween.</para>
		/// </summary>
		/// <param name="tweenInfo">Tween</param>
		/// <returns></returns>
		private static IEnumerator LogicaTween(T tweenInfo)// Realiza el tween
		{
			// Comprueba si el objetivo es valido
			if (!tweenInfo.TargetValido()) yield break;

			float tiempoElapsed = 0.0f;

			// Mientras que el tiempo no sea mayor a la duracion
			while (tiempoElapsed < tweenInfo.Duracion)
			{
				// Ignorar time scale
				tiempoElapsed += (tweenInfo.IgnorarTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);

				// Obtener el valor
				float valor = TweenEasingHandler.Aplicar(tweenInfo.Easing, tiempoElapsed, 0.0f, 1.0f, tweenInfo.Duracion);
				tweenInfo.TweenValor(valor);

				yield return null;
			}

			// Finalizar tween
			tweenInfo.TweenValor(1.0f);
			tweenInfo.Finalizado();
		}
		#endregion

		#region Metodos Publicos
		/// <summary>
		/// <para>Inicializa la corrutina.</para>
		/// </summary>
		/// <param name="contenedorCorrutina">Contenedor de la corrutina.</param>
		public void Init(MonoBehaviour contenedorCorrutina)// Inicializa la corrutina
		{
			// Obtener la corrutina
			this.corrutina = contenedorCorrutina;
		}

		/// <summary>
		/// <para>Inicia el Tween.</para>
		/// </summary>
		/// <param name="info"></param>
		public void StartTween(T info)// Inicia el Tween
		{
			// Comprobar si hay corrutina
			if (this.corrutina == null)
			{
				Debug.LogWarning("Contenedor de Coroutine no configurado ... Llamar primero a Init.");
				return;
			}

			// Parar el Tween
			this.StopTween();

			// Resetear valor
			if (!this.corrutina.gameObject.activeInHierarchy)
			{
				info.TweenValor(1.0f);
				return;
			}

			// Realizar el Tween
			this.tween = LogicaTween(info);
			this.corrutina.StartCoroutine(this.tween);
		}

		/// <summary>
		/// <para>Detiene el Tween.</para>
		/// </summary>
		public void StopTween()// Detiene el Tween
		{
			// Si hay un tween en ejecucion
			if (this.tween != null)
			{
				// Detener tween
				this.corrutina.StopCoroutine(this.tween);
				this.tween = null;
			}
		}
		#endregion
	}
}

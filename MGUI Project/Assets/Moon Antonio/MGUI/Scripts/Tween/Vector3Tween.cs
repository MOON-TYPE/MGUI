//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// Vector3Tween.cs (22/09/2017)													\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Tween con un Vector3										\\
// Fecha Mod:		22/09/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using UnityEngine.Events;
#endregion

namespace MoonAntonio.Tweens
{
	/// <summary>
	/// <para>Tween con un Vector3</para>
	/// </summary>
	public struct Vector3Tween : ITweenValor
	{
		#region Variables Privadas
		/// <summary>
		/// <para>Vector3 inicial.</para>
		/// </summary>
		private Vector3 vector3Inicial;								// Vector3 inicial
		/// <summary>
		/// <para>Vector 3 final.</para>
		/// </summary>
		private Vector3 vector3Final;								// Vector 3 final
		/// <summary>
		/// <para>Duracion.</para>
		/// </summary>
		private float duracion;										// Duracion
		/// <summary>
		/// <para>Determina si se ignora el timeScale.</para>
		/// </summary>
		private bool ignorarTimeScale;								// Determina si se ignora el timeScale
		/// <summary>
		/// <para>Tipo de easing del tween.</para>
		/// </summary>
		private TweenEasing easing;									// Tipo de easing del tween
		/// <summary>
		/// <para>Evento de tween.</para>
		/// </summary>
		private Vector3TweenCallback objetivo;						// Evento de tween
		/// <summary>
		/// <para>Evento de tween finalizado.</para>
		/// </summary>
		private Vector3TweenFinalizadoCallback finalizado;          // Evento de tween finalizado
		#endregion

		#region Eventos
		public class Vector3TweenCallback : UnityEvent<Vector3> { }
		public class Vector3TweenFinalizadoCallback : UnityEvent { }
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Obtiene o establece el vector3 inicial.</para>
		/// </summary>
		/// <value>El vector 3 inicial.</value>
		public Vector3 Vector3Inicial
		{
			get { return vector3Inicial; }
			set { vector3Inicial = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece el vector3 final.</para>
		/// </summary>
		/// <value>El vector3 final.</value>
		public Vector3 Vector3Final
		{
			get { return vector3Final; }
			set { vector3Final = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece la duracion.</para>
		/// </summary>
		/// <value>La duracion.</value>
		public float Duracion
		{
			get { return duracion; }
			set { duracion = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece un valor que indica si <see cref = "MoonAntonio.MGUI.UI.Tweens.Vector3Tween" /> debe ignorar el timeScale.</para>
		/// </summary>
		/// <value><c>true</c> si se ignora el time scale; sino, <c>false</c>.</value>
		public bool IgnorarTimeScale
		{
			get { return ignorarTimeScale; }
			set { ignorarTimeScale = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece el tipo de easing del Tween.</para>
		/// </summary>
		/// <value>El easing.</value>
		public TweenEasing Easing
		{
			get { return easing; }
			set { easing = value; }
		}
		#endregion

		#region Metodos Publicos
		/// <summary>
		/// <para>Interpola el vector3 basado en el porcentaje.</para>
		/// </summary>
		/// <param name="valor">porcentaje.</param>
		public void TweenValor(float valor)// Interpola el vector3 basado en el porcentaje
		{
			// Si el objetivo no es valido, volver
			if (!this.TargetValido()) return;

			this.objetivo.Invoke(Vector3.Lerp(this.vector3Inicial, this.vector3Final, valor));
		}

		/// <summary>
		/// <para>Activa el evento cuando cambia el tween.</para>
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void AddOnCambioCallback(UnityAction<Vector3> callback)// Activa el evento cuando cambia el tween
		{
			if (objetivo == null) objetivo = new Vector3TweenCallback();

			objetivo.AddListener(callback);
		}

		/// <summary>
		/// <para>Activa el evento cuando finaliza el tween.</para>
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void AddOnFinalizadoCallback(UnityAction callback)// Activa el evento cuando finaliza el tween
		{
			if (finalizado == null) finalizado = new Vector3TweenFinalizadoCallback();

			finalizado.AddListener(callback);
		}

		/// <summary>
		/// <para>Cuando finaliza el tween.</para>
		/// </summary>
		public void Finalizado()// Cuando finaliza el tween
		{
			if (finalizado != null) finalizado.Invoke();
		}
		#endregion

		#region Funcionalidad
		/// <summary>
		/// <para>Obtiene si se ignora el TimeScale.</para>
		/// </summary>
		/// <returns></returns>
		public bool GetIgnorarTimeScale()// Obtiene si se ignora el TimeScale
		{
			return ignorarTimeScale;
		}

		/// <summary>
		/// <para>Obtiene la duracion del tween.</para>
		/// </summary>
		/// <returns></returns>
		public float GetDuracion()// Obtiene la duracion del tween
		{
			return duracion;
		}

		/// <summary>
		/// <para>Comprueba si el target es valido.</para>
		/// </summary>
		/// <returns></returns>
		public bool TargetValido()// Comprueba si el target es valido
		{
			return objetivo != null;
		}
		#endregion
	}
}
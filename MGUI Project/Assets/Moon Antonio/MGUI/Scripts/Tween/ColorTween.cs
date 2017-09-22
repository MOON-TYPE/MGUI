//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// ColorTween.cs (22/09/2017)													\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Tween para el color											\\
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
	/// <para>Tween para el color.</para>
	/// </summary>
	public struct ColorTween : ITweenValor
	{
		#region Variables Privadas
		/// <summary>
		/// <para>Color inicial.</para>
		/// </summary>
		private Color colorInicial;									// Color inicial
		/// <summary>
		/// <para>Color final.</para>
		/// </summary>
		private Color colorFinal;									// Color final
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
		/// <para>Tipo de tween de color.</para>
		/// </summary>
		private ColorTween.TipoColorTween tipoTween;				// Tipo de tween de color
		/// <summary>
		/// <para>Evento de tween.</para>
		/// </summary>
		private ColorTweenCallback objetivo;						// Evento de tween
		/// <summary>
		/// <para>Evento de tween finalizado.</para>
		/// </summary>
		private ColorTweenFinalizadoCallback finalizado;			// Evento de tween finalizado
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Obtiene o establece el color inicial.</para>
		/// </summary>
		/// <value>Color inicial.</value>
		public Color ColorInicial
		{
			get { return colorInicial; }
			set { colorInicial = value; }
		}

		/// <summary>
		/// <para>Obtiene o establece el color final.</para>
		/// </summary>
		/// <value>Color final.</value>
		public Color ColorFinal
		{
			get { return colorFinal; }
			set { colorFinal = value; }
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
		/// <para>Obtiene o establece un valor que indica si <see cref = "MoonAntonio.MGUI.UI.Tweens.ColorTween" /> debe ignorar el timeScale.</para>
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

		/// <summary>
		/// <para>Obtiene o establece el tipo de tween de color.</para>
		/// </summary>
		/// <value>El tipo de tween.</value>
		public ColorTween.TipoColorTween TipoTween
		{
			get { return this.tipoTween; }
			set { this.tipoTween = value; }
		}
		#endregion

		#region Enums
		/// <summary>
		/// <para>Tipo de tween</para>
		/// </summary>
		public enum TipoColorTween
		{
			Todo,
			RGB,
			Alpha
		}
		#endregion

		#region Eventos
		public class ColorTweenCallback : UnityEvent<Color> { }
		public class ColorTweenFinalizadoCallback : UnityEvent { }
		#endregion

		#region Metodos Publicos
		/// <summary>
		/// <para>Interpola el color basado en el porcentaje.</para>
		/// </summary>
		/// <param name="valor">porcentaje.</param>
		public void TweenValor(float valor)// Interpola el color basado en el porcentaje
		{
			// Si el objetivo no es valido, volver
			if (!this.TargetValido()) return;

			Color arg = Color.Lerp(this.colorInicial, this.colorFinal, valor);

			if (this.tipoTween == ColorTween.TipoColorTween.Alpha)
			{
				arg.r = this.colorInicial.r;
				arg.g = this.colorInicial.g;
				arg.b = this.colorInicial.b;
			}
			else
			{
				if (this.tipoTween == ColorTween.TipoColorTween.RGB) arg.a = this.colorInicial.a;
			}

			this.objetivo.Invoke(arg);
		}

		/// <summary>
		/// <para>Activa el evento cuando cambia el tween.</para>
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void AddOnCambioCallback(UnityAction<Color> callback)// Activa el evento cuando cambia el tween
		{
			if (objetivo == null) objetivo = new ColorTweenCallback();

			objetivo.AddListener(callback);
		}

		/// <summary>
		/// <para>Activa el evento cuando finaliza el tween.</para>
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void AddOnFinalizadoCallback(UnityAction callback)// Activa el evento cuando finaliza el tween
		{
			if (finalizado == null) finalizado = new ColorTweenFinalizadoCallback();

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
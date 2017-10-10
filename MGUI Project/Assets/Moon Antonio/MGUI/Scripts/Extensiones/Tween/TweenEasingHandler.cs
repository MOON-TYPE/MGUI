//                                  ┌∩┐(◣_◢)┌∩┐
//																				\\
// TweenEasingHandler.cs (22/09/2017)											\\
// Autor: Antonio Mateo (Moon Antonio) 	antoniomt.moon@gmail.com				\\
// Descripcion:		Aplica el Tween												\\
// Fecha Mod:		22/09/2017													\\
// Ultima Mod:		Version Inicial												\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
#endregion

namespace MoonAntonio.Tweens
{
	/// <summary>
	/// <para>Aplica el Tween.</para>
	/// </summary>
	internal class TweenEasingHandler
	{
		#region API
		/// <summary>
		/// <para>Aplica el tipo de easing especificado, t, b, cyd.</para>
		/// </summary>
		/// <param name="e">Tipo easing.</param>
		/// <param name="t">Tiempo easing.</param>
		/// <param name="b">Valor inicial.</param>
		/// <param name="c">valor final.</param>
		/// <param name="d">Duracion.</param>
		public static float Aplicar(TweenEasing e, float t, float b, float c, float d)
		{
			switch (e)
			{
				case TweenEasing.Swing:
					{
						return -c * (t /= d) * (t - 2f) + b;
					}
				case TweenEasing.InQuad:
					{
						return c * (t /= d) * t + b;
					}
				case TweenEasing.OutQuad:
					{
						return -c * (t /= d) * (t - 2) + b;
					}
				case TweenEasing.InOutQuad:
					{
						if ((t /= d / 2) < 1) return c / 2 * t * t + b;
						return -c / 2 * ((--t) * (t - 2) - 1) + b;
					}
				case TweenEasing.InCubic:
					{
						return c * (t /= d) * t * t + b;
					}
				case TweenEasing.OutCubic:
					{
						return c * ((t = t / d - 1) * t * t + 1) + b;
					}
				case TweenEasing.InOutCubic:
					{
						if ((t /= d / 2) < 1) return c / 2 * t * t * t + b;
						return c / 2 * ((t -= 2) * t * t + 2) + b;
					}
				case TweenEasing.InQuart:
					{
						return c * (t /= d) * t * t * t + b;
					}
				case TweenEasing.OutQuart:
					{
						return -c * ((t = t / d - 1) * t * t * t - 1) + b;
					}
				case TweenEasing.InOutQuart:
					{
						if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
						return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
					}
				case TweenEasing.InQuint:
					{
						return c * (t /= d) * t * t * t * t + b;
					}
				case TweenEasing.OutQuint:
					{
						return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
					}
				case TweenEasing.InOutQuint:
					{
						if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
						return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
					}
				case TweenEasing.InSine:
					{
						return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
					}
				case TweenEasing.OutSine:
					{
						return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
					}
				case TweenEasing.InOutSine:
					{
						return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
					}
				case TweenEasing.InExpo:
					{
						return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
					}
				case TweenEasing.OutExpo:
					{
						return (t == d) ? b + c : c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
					}
				case TweenEasing.InOutExpo:
					{
						if (t == 0) return b;
						if (t == d) return b + c;
						if ((t /= d / 2) < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
						return c / 2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
					}
				case TweenEasing.InCirc:
					{
						return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
					}
				case TweenEasing.OutCirc:
					{
						return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
					}
				case TweenEasing.InOutCirc:
					{
						if ((t /= d / 2) < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
						return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
					}
				case TweenEasing.InBack:
					{
						float s = 1.70158f;
						return c * (t /= d) * t * ((s + 1f) * t - s) + b;
					}
				case TweenEasing.OutBack:
					{
						float s = 1.70158f;
						return c * ((t = t / d - 1f) * t * ((s + 1f) * t + s) + 1f) + b;
					}
				case TweenEasing.InOutBack:
					{
						float s = 1.70158f;
						if ((t /= d / 2f) < 1f) return c / 2f * (t * t * (((s *= (1.525f)) + 1f) * t - s)) + b;
						return c / 2f * ((t -= 2f) * t * (((s *= (1.525f)) + 1f) * t + s) + 2f) + b;
					}
				case TweenEasing.InBounce:
					{
						return c - TweenEasingHandler.Aplicar(TweenEasing.OutBounce, d - t, 0f, c, d) + b;
					}
				case TweenEasing.OutBounce:
					{
						if ((t /= d) < (1f / 2.75f))
						{
							return c * (7.5625f * t * t) + b;
						}
						else if (t < (2f / 2.75f))
						{
							return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
						}
						else if (t < (2.5f / 2.75f))
						{
							return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
						}
						else
						{
							return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
						}
					}
				case TweenEasing.InOutBounce:
					{
						if (t < d / 2f) return TweenEasingHandler.Aplicar(TweenEasing.InBounce, t * 2f, 0f, c, d) * .5f + b;
						return TweenEasingHandler.Aplicar(TweenEasing.OutBounce, t * 2f - d, 0f, c, d) * .5f + c * .5f + b;
					}
				case TweenEasing.InElastic:
					{
						float s = 1.70158f; float p = 0f; float a = c;
						if (t == 0f) return b;
						if ((t /= d) == 1f) return b + c;
						if (p == 0f) p = d * .3f;
						if (a < Mathf.Abs(c)) { a = c; s = p / 4f; }
						else s = p / (2f * Mathf.PI) * Mathf.Asin(c / a);
						if (float.IsNaN(s)) s = 0f;
						return -(a * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - s) * (2f * Mathf.PI) / p)) + b;
					}
				case TweenEasing.OutElastic:
					{
						float s = 1.70158f; float p = 0f; float a = c;
						if (t == 0f) return b; if ((t /= d) == 1f) return b + c; if (p == 0f) p = d * .3f;
						if (a < Mathf.Abs(c)) { a = c; s = p / 4f; }
						else s = p / (2f * Mathf.PI) * Mathf.Asin(c / a);
						if (float.IsNaN(s)) s = 0f;
						return a * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - s) * (2f * Mathf.PI) / p) + c + b;
					}
				case TweenEasing.InOutElastic:
					{
						float s = 1.70158f; float p = 0f; float a = c;
						if (t == 0f) return b; if ((t /= d / 2f) == 2f) return b + c; if (p == 0f) p = d * (.3f * 1.5f);
						if (a < Mathf.Abs(c)) { a = c; s = p / 4f; }
						else s = p / (2f * Mathf.PI) * Mathf.Asin(c / a);
						if (float.IsNaN(s)) s = 0f;
						if (t < 1f) return -.5f * (a * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - s) * (2f * Mathf.PI) / p)) + b;
						return a * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - s) * (2f * Mathf.PI) / p) * .5f + c + b;
					}
				case TweenEasing.Linear:
				default:
					{
						return c * t / d + b;
					}
			}
		}
		#endregion
	}
}